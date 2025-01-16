using Services.SamlResponseAuth.Models;
using System.Xml;

namespace Services.SamlResponseAuth.Services.Contracts
{
    public interface ISamlAuthService
    {
        public string DecodeSaml(string SAMLResponse);

        public XmlDocument LoadXmlDocument(string samlData);

        public Subject ParseSaml(string SAMLResponse);

        public string GetUserAttributeValue(string AttributeName, string SamlResponse);

    }
}