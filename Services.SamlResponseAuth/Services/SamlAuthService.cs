using Services.SamlResponseAuth.Models;
using Services.SamlResponseAuth.Services.Contracts;
using System.Xml;
using System.Xml.XPath;

namespace Services.SamlResponseAuth.Services
{
    public class SamlAuthService : ISamlAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SamlAuthService> _logger;

        public SamlAuthService(IConfiguration configuration, ILogger<SamlAuthService> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger;
        }
        public string DecodeSaml(string SAMLResponse)
        {
            try
            {
                if (string.IsNullOrEmpty(SAMLResponse))
                {
                    _logger.LogError("SAML Response cannot be null or empty.");
                    throw new ArgumentException("SAML Response cannot be null or empty.");
                }
                byte[] decodedBytes = Convert.FromBase64String(SAMLResponse);
                return System.Text.Encoding.UTF8.GetString(decodedBytes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while decoding the SAML Response.");
                throw new ArgumentException("An error occurred while decoding the SAML Response.", ex);
            }
        }

        public XmlDocument LoadXmlDocument(string SamlData)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                SamlData = SamlData.Replace(@"\", "");
                xDoc.LoadXml(SamlData);
                _logger.LogInformation("Saml Loaded.");
                return xDoc;
            }
            catch (XmlException ex)
            {
                _logger.LogError(ex, "Failed to load SAML XML document. Ensure the SAML data is well-formed XML.");
                throw new ArgumentException("Failed to load SAML XML document. Ensure the SAML data is well-formed XML.", ex);
            }
        }

        public Subject ParseSaml(string SamlResponse)
        {
            try
            {
                XmlDocument xDoc = LoadXmlDocument(SamlResponse);
                Subject subject = new Subject();
                if (!SamlResponse.Contains(_configuration["SAML:V1Namespace"] ?? string.Empty) && !SamlResponse.Contains(_configuration["SAML:V2Namespace"] ?? string.Empty))
                {
                    return subject;
                }
                XmlNamespaceManager xMan = new XmlNamespaceManager(xDoc.NameTable);
                xMan.AddNamespace("saml", _configuration["SAML:V1Namespace"] ?? string.Empty);
                xMan.AddNamespace("saml2", _configuration["SAML:V2Namespace"] ?? string.Empty);
                subject.NotBefore = xDoc.SelectSingleNode(_configuration["SAML:NotBefore"] ?? string.Empty, xMan)?.Value;
                subject.NotOnOrAfter = xDoc.SelectSingleNode(_configuration["SAML:NotOnOrAfter"] ?? string.Empty, xMan)?.Value ?? string.Empty;
                subject.NameID = xDoc.SelectSingleNode(_configuration["SAML:V1NameId"] ?? string.Empty, xMan)?.InnerText ?? xDoc.SelectSingleNode(_configuration["SAML:V2NameId"] ?? string.Empty, xMan)?.InnerText ?? string.Empty;
                subject.Issuer = xDoc.SelectSingleNode(_configuration["SAML:V1Issuer"] ?? string.Empty, xMan)?.Value ?? xDoc.SelectSingleNode(_configuration["SAML:V2Issuer"] ?? string.Empty, xMan)?.InnerText ?? string.Empty;
                subject.Email = xDoc.SelectSingleNode($"{_configuration["SAML:V1AttributeName"]}\"saml_email\"]/saml:AttributeValue", xMan)?.InnerText ?? xDoc.SelectSingleNode($"{_configuration["SAML:V2AttributeName"]}\"saml_email\"]/saml2:AttributeValue", xMan)?.InnerText ?? string.Empty;
                return subject;
            }
            catch (XmlException ex)
            {
                _logger.LogError(ex, "Error processing SAML data. Ensure it is valid XML.");
                throw new InvalidOperationException("Error processing SAML data. Ensure it is valid XML.", ex);
            }
            catch (XPathException ex)
            {
                _logger.LogError(ex, $"Invalid XPath expression:");
                throw new InvalidOperationException($"Invalid XPath expression:", ex);
            }
        }

        public string GetUserAttributeValue(string AttributeName, string SamlResponse)
        {
            if (string.IsNullOrWhiteSpace(AttributeName) || string.IsNullOrWhiteSpace(SamlResponse))
            {
                _logger.LogError("Arguments cannot be null or empty.");
                throw new ArgumentException("Arguments cannot be null or empty.");
            }
            try
            {
                XmlDocument xDoc = LoadXmlDocument(SamlResponse);
                if (!SamlResponse.Contains(_configuration["SAML:V1Namespace"] ?? string.Empty) && !SamlResponse.Contains(_configuration["SAML:V2Namespace"] ?? string.Empty))
                {
                    return string.Empty;
                }
                XmlNamespaceManager xMan = new XmlNamespaceManager(xDoc.NameTable);
                xMan.AddNamespace("saml", _configuration["SAML:V1Namespace"] ?? string.Empty);
                xMan.AddNamespace("saml2", _configuration["SAML:V2Namespace"] ?? string.Empty);
                return xDoc.SelectSingleNode($"{_configuration["SAML:V1AttributeName"]}\"{AttributeName}\"]/saml:AttributeValue", xMan)?.InnerText ?? xDoc.SelectSingleNode($"{_configuration["SAML:V2AttributeName"]}\"{AttributeName}\"]/saml2:AttributeValue", xMan)?.InnerText ?? string.Empty;
            }
            catch (XmlException ex)
            {
                _logger.LogError(ex, "Error processing SAML data. Ensure it is valid XML.");
                throw new InvalidOperationException("Error processing SAML data. Ensure it is valid XML.", ex);
            }
            catch (XPathException ex)
            {
                _logger.LogError(ex, $"Invalid XPath expression:");
                throw new InvalidOperationException($"Invalid XPath expression:", ex);
            }
        }
    }
}