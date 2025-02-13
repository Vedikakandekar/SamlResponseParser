using Services.SamlResponseAuth.Models;
using Services.SamlResponseAuth.Services.Contracts;
using System.Globalization;
using System.Xml;
using Services.SamlResponseAuth.Utility;
using Microsoft.Extensions.Options;
using Services.SamlResponseAuth.Utility.Contracts;
namespace Services.SamlResponseAuth.Services
{
    public class SamlAuthService : ISamlAuthService
    {
       
        private readonly SamlXPathSettings _samlPathsettings;

        private readonly IExceptionMapper _exceptionMapper;

        private readonly IXmlDocumentLoader _xmlDocumentLoader;

        private readonly IXmlNamespaceManagerFactory _xmlNamespaceManagerFactory;

        public SamlAuthService(IOptions<SamlXPathSettings> samlPathsettings, IExceptionMapper exceptionMapper, IXmlDocumentLoader xmlDocumentLoader,
        IXmlNamespaceManagerFactory xmlNamespaceManagerFactory)
        {
            _samlPathsettings = samlPathsettings?.Value 
                                ?? throw new ArgumentNullException(nameof(samlPathsettings));
            _exceptionMapper = exceptionMapper 
                                ?? throw new ArgumentNullException(nameof(exceptionMapper));
            _xmlDocumentLoader = xmlDocumentLoader 
                                ?? throw new ArgumentNullException(nameof(xmlDocumentLoader));

            _xmlNamespaceManagerFactory = xmlNamespaceManagerFactory 
                                ?? throw new ArgumentNullException(nameof(xmlNamespaceManagerFactory));
        }

        public string? DecodeSaml(string SAMLResponse)
        {
                if (string.IsNullOrEmpty(SAMLResponse))
                {
                    ThrowSamlException(ExceptionCodes.NullOrEmptyResponse);
                }
                return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(SAMLResponse));
         
        }

        public Subject? ParseSaml(string SamlResponse)
        {
            Console.WriteLine(SamlResponse);

            XmlDocument xDoc = _xmlDocumentLoader.LoadXml(SamlResponse);

            Subject subject = new Subject() { UserAttributes = new Dictionary<string, string>() };
                if (!SamlResponse.Contains(_samlPathsettings.V1Namespace) 
                    && !SamlResponse.Contains(_samlPathsettings.V2Namespace))
                {
                    ThrowSamlException(ExceptionCodes.UnprocessableEntity);
                }
            XmlNamespaceManager xMan = _xmlNamespaceManagerFactory.CreateNamespaceManager(xDoc);


            var status = xDoc.SelectSingleNode(_samlPathsettings.StatusCode, xMan)?.Value;
                if (string.IsNullOrEmpty(status))
                {
                    ThrowSamlException(ExceptionCodes.MissingStatus);
                }
                if (status !=_samlPathsettings.V1StatusSuccess && status !=_samlPathsettings.V2StatusSuccess)
                {
                    ThrowSamlException(status!);
                }
                var Assertion = xDoc.SelectSingleNode(_samlPathsettings.AssertionNode, xMan);
                if(Assertion == null)
                {
                    ThrowSamlException(ExceptionCodes.MissingAssertion);
                }
                subject.NotBefore = xDoc.SelectSingleNode(_samlPathsettings.NotBefore, xMan)?.Value;
                subject.NotOnOrAfter = xDoc.SelectSingleNode(_samlPathsettings.NotOnOrAfter, xMan)?.Value ?? string.Empty;
                subject.Issuer = xDoc.SelectSingleNode(_samlPathsettings.V1Issuer, xMan)?.Value 
                               ?? xDoc.SelectSingleNode(_samlPathsettings.V2Issuer, xMan)?.InnerText 
                               ?? string.Empty;
                if(string.IsNullOrEmpty(subject.Issuer))
                {
                    ThrowSamlException(ExceptionCodes.MissingIssuer);
                }
                if (subject.NotBefore == null || subject.NotOnOrAfter == null)
                {
                    ThrowSamlException(ExceptionCodes.NullConditions);
                }
                ValidateTimestamps(subject.NotBefore!, subject.NotOnOrAfter!);
                XmlNodeList attributeNodes = xDoc.SelectNodes(_samlPathsettings.Attribute, xMan)!;
                if (attributeNodes != null)
                {
                    foreach (XmlNode attributeNode in attributeNodes)
                    {
                        string attributeName = attributeNode.Attributes?[_samlPathsettings.V2AttributeName]?.Value
                                               ?? attributeNode.Attributes?[_samlPathsettings.V1AttributeName]?.Value
                                               ?? string.Empty;

                        string attributeValue = attributeNode.SelectSingleNode(_samlPathsettings.AttributeValue, xMan)?.InnerText 
                                                ?? string.Empty;
                        if (!string.IsNullOrEmpty(attributeName) && !string.IsNullOrEmpty(attributeValue))
                        {
                        if (attributeName == _samlPathsettings.EmailAttributeName)
                        {
                            subject.Email = attributeValue;
                        }
                            subject.UserAttributes[attributeName] = attributeValue;
                        }
                    }
                }
                if(subject.Email == null) {
                    ThrowSamlException(ExceptionCodes.NullEmail);
                }
                return subject;
        }

        private void ValidateTimestamps(string notBefore, string notOnOrAfter)
        {
            var format = _samlPathsettings.DateFormat;
            var currentTime = DateTimeOffset.UtcNow;
            var valid = DateTimeOffset.ParseExact(notBefore, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)
                        <= currentTime &&
                        DateTimeOffset.ParseExact(notOnOrAfter, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)
                        > currentTime;
            if (!valid) { 
                ThrowSamlException(ExceptionCodes.InvalidConditions);
            }
        }

        private void ThrowSamlException(string exceptionName)
        {
            throw _exceptionMapper.GetErrorDetails(exceptionName);
        }

    }
}