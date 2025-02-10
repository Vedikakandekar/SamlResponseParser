using Services.SamlResponseAuth.Utility;

namespace Services.SamlResponseAuth.Services.Contracts
{
    public interface IExceptionMapper
    {
        SamlException GetErrorDetails(string exceptionName);
    }
}
