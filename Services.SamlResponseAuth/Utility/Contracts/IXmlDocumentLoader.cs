using System.Xml;

namespace Services.SamlResponseAuth.Utility.Contracts
{
    public interface IXmlDocumentLoader
    {
        XmlDocument LoadXml(string xmlContent);
    }

}
