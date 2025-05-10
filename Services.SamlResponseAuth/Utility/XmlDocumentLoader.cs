using Services.SamlResponseAuth.Utility.Contracts;
using System.Xml;

namespace Services.SamlResponseAuth.Utility
{
    public class XmlDocumentLoader : IXmlDocumentLoader
    {
        public XmlDocument LoadXml(string xmlContent)
        {
            var xDoc = new XmlDocument();
            xDoc.LoadXml(xmlContent);
            return xDoc;
        }
    }
}
