using Microsoft.Extensions.Options;
using Services.SamlResponseAuth.Utility.Contracts;
using System.Xml;

namespace Services.SamlResponseAuth.Utility
{
    public class XmlNamespaceManagerFactory : IXmlNamespaceManagerFactory
    {
        private readonly SamlXPathSettings _samlPathsettings;

        public XmlNamespaceManagerFactory(IOptions<SamlXPathSettings> samlPathsettings)
        {
            _samlPathsettings = samlPathsettings?.Value ?? throw new ArgumentNullException(nameof(samlPathsettings));
        }

        public XmlNamespaceManager CreateNamespaceManager(XmlDocument xmlDocument)
        {
            var xMan = new XmlNamespaceManager(xmlDocument.NameTable);
            xMan.AddNamespace("saml", _samlPathsettings.V1Namespace);
            xMan.AddNamespace("saml2", _samlPathsettings.V2Namespace);
            xMan.AddNamespace("samlp", _samlPathsettings.V1Protocol);
            xMan.AddNamespace("samlp2", _samlPathsettings.V2Protocol);
            return xMan;
        }
    }
}