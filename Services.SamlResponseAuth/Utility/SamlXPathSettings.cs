namespace Services.SamlResponseAuth.Utility
{
    public class SamlXPathSettings
    {
        public string V1Namespace { get; set; } = string.Empty;
        public string V2Namespace { get; set; } = string.Empty;
        public string V1Protocol { get; set; } = string.Empty;
        public string V2Protocol { get; set; } = string.Empty;
        public string AssertionNode { get; set; } = string.Empty;
        public string StatusCode { get; set; } = string.Empty;
        public string NotBefore { get; set; } = string.Empty;
        public string NotOnOrAfter { get; set; } = string.Empty;
        public string V1Issuer { get; set; } = string.Empty;
        public string V2Issuer { get; set; } = string.Empty;
        public string Attribute { get; set; } = string.Empty;
        public string AttributeValue { get; set; } = string.Empty;
        public string DateFormat { get; set; } = string.Empty;
        public string V1AttributeName { get; set; } = string.Empty;
        public string V2AttributeName { get; set; } = string.Empty;
        public string EmailAttributeName { get; set; } = string.Empty;
        public string V1StatusSuccess { get; set; } = string.Empty;
        public string V2StatusSuccess { get; set; } = string.Empty;
    }
}
