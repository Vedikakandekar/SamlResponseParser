using Microsoft.AspNetCore.Mvc;
using Services.SamlResponseAuth.Models;
using Services.SamlResponseAuth.Models.DTO;
using Services.SamlResponseAuth.Services.Contracts;


namespace Services.SamlResponseAuth.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class SamlAuthController : ControllerBase
    {
        private readonly ISamlAuthService _samlAuthService;

        public SamlAuthController(ISamlAuthService samlAuthService)
        {
            _samlAuthService = samlAuthService;
        }

        [HttpPost("ParseSamlResponse")]
        public IActionResult ParseSamlResponse([FromForm] string SAMLResponse)
        {
            try
            {
                string base64decoded = _samlAuthService.DecodeSaml(SAMLResponse);
                Subject subject = _samlAuthService.ParseSaml(base64decoded);
                return Ok(subject);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("GetAttributeValue")]
        public IActionResult GetAttributeValue([FromForm] AttributeDTO AttributeDTO)
        {
            try
            {
                string base64decoded = _samlAuthService.DecodeSaml(AttributeDTO.SAMLResponse);
                string AttributeValue = _samlAuthService.GetUserAttributeValue(AttributeDTO.attributeName, base64decoded);
                return Ok(AttributeValue);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}