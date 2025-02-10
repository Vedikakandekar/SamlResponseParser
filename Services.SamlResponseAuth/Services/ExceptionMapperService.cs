using Services.SamlResponseAuth.Services.Contracts;
using Services.SamlResponseAuth.Utility;

namespace Services.SamlResponseAuth.Services
{
    public class ExceptionMapperService :IExceptionMapper
    {
        public SamlException GetErrorDetails(string exceptionName)
        {
            if (SamlErrorMapping.ExceptionDictionary.TryGetValue(exceptionName, out var errorDetails))
                return errorDetails;
            else
                return SamlErrorMapping.DefaultSamlError;
        }
    }
}
