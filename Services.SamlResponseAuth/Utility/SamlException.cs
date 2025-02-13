namespace Services.SamlResponseAuth.Utility
{
    public class SamlException : Exception
    {
        public string? Name { get; }
        public int HttpStatus { get; }
        public string Description { get; }
        public string? Exception { get; }

        public SamlException(string name, int httpStatus, string description, string? ex = null)
            : base($"SAML Error: {description}")
        {
            HttpStatus = httpStatus;
            Name = name;
            Description = description;
            Exception = ex;
        }
    }
}
