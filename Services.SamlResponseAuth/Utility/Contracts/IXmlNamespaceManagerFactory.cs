using System.Xml;

namespace Services.SamlResponseAuth.Utility.Contracts
{
    public interface IXmlNamespaceManagerFactory
    {
        XmlNamespaceManager CreateNamespaceManager(XmlDocument xmlDocument);
    }
}
