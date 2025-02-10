namespace Services.SamlResponseAuth.Models
{
    public class Subject
    {
        public string? Issuer { get; set; }
        public string? NotBefore { get; set; }
        public string? NotOnOrAfter { get; set; }
        public string? Email { get; set; }
        public Dictionary<string, string>? UserAttributes { get; set; }
    }
}
