using Services.SamlResponseAuth.Models;
using System.Xml;

namespace Services.SamlResponseAuth.Services.Contracts
{
    public interface ISamlAuthService
    {
        public string? DecodeSaml(string SAMLResponse);

        public Subject? ParseSaml(string SAMLResponse);



    }
}