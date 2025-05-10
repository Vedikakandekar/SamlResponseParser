using Microsoft.AspNetCore.Mvc;
using Services.SamlResponseAuth.Models;
using Services.SamlResponseAuth.Services;
using Services.SamlResponseAuth.Services.Contracts;
using Services.SamlResponseAuth.Utility;


namespace Services.SamlResponseAuth.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class SamlAuthController : ControllerBase
    {
        private readonly ISamlAuthService _samlAuthService;
        private readonly ILogger<SamlAuthService> _logger;

        public SamlAuthController(ISamlAuthService samlAuthService, ILogger<SamlAuthService> logger)
        {
            _samlAuthService = samlAuthService;
            _logger = logger;
        }

        [HttpPost("ParseSamlResponse")]
        public IActionResult ParseSamlResponse([FromForm] string SAMLResponse)
        {
               string base64decoded = _samlAuthService.DecodeSaml(SAMLResponse) ?? string.Empty;
                Subject subject = _samlAuthService.ParseSaml(base64decoded)!;
                return StatusCode(200,subject);
        }
       
    }
}