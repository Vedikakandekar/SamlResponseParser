using System.Xml;
using System.Xml.XPath;

namespace Services.SamlResponseAuth.Utility
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled Exception: {ExceptionType} - {Message}", ex.GetType().Name, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            SamlException samlException;
            samlException = exception as SamlException ?? MapExceptionToSamlException(exception);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = samlException.HttpStatus;
            var response = new { samlException.HttpStatus,samlException.Name, samlException.Description,samlException.Exception };
            return context.Response.WriteAsJsonAsync(response);
        }

        private SamlException MapExceptionToSamlException(Exception exception)
        {
            var errorDetails = exception switch
            {
                ArgumentNullException => new SamlException("ArgumentNullException", 400, "A required argument was null.", exception.Message),
                FormatException => new SamlException("InvalidSamlFormatException", 400, " The SAML response contains invalid or malformed XML", exception.Message),
                KeyNotFoundException => new SamlException("KeyNotFoundException", 404, "The requested resource was not found.", exception.Message),
                XPathException => new SamlException("ParsingException", 400, "Invalid XPath expression encountered while parsing SAML.", exception.Message),
                XmlException => new SamlException("KeyNotFoundException", 404, "An error occurred while parsing the SAML response.", exception.Message),
                _ => new SamlException(exception.GetType().Name, 500, "An unexpected error occurred.", exception.Message)
            };

            return errorDetails;
        }
    }
}
