namespace Services.SamlResponseAuth.Utility
{
    public static class SamlErrorMapping
    {
        public static readonly SamlException DefaultSamlError = new SamlException("UnknownException", 400, "Unknown error, Something went wrong ! ");

        public static readonly Dictionary<string, SamlException> ExceptionDictionary = new()
    {
         // v2.0 statuscodes
        { "urn:oasis:names:tc:SAML:2.0:status:Requester", new SamlException("Requester", 400, "The request could not be performed due to an error on the part of the requester.") },
        { "urn:oasis:names:tc:SAML:2.0:status:Responder", new SamlException("Responder", 500, "The request could not be performed due to an error on the part of the SAML responder or authority.") },
        { "urn:oasis:names:tc:SAML:2.0:status:InvalidAttrNameOrValue", new SamlException("InvalidAttrNameOrValue", 400, "Unexpected or invalid content was encountered within a <saml:Attribute> or\r\n<saml:AttributeValue> element.") },
        { "urn:oasis:names:tc:SAML:2.0:status:AuthnFailed", new SamlException("AuthnFailed", 400, "The responding provider was unable to successfully authenticate the principal.") },
        { "urn:oasis:names:tc:SAML:2.0:status:InvalidNameIDPolicy", new SamlException("InvalidNameIDPolicy", 400, "The responding provider cannot or will not support the requested name identifier policy.") },
        { "urn:oasis:names:tc:SAML:2.0:status:RequestDenied", new SamlException("RequestDenied", 403, "The responder is able to process the request but has chosen not to respond.") },
        { "urn:oasis:names:tc:SAML:2.0:status:ResourceNotRecognized", new SamlException("ResourceNotRecognized", 404, "The resource value provided in the request is invalid or unrecognized.") },
        { "urn:oasis:names:tc:SAML:2.0:status:TooManyResponses", new SamlException("TooManyResponses", 500, " The response message would contain more elements than the responder is able to return.") },
        { "urn:oasis:names:tc:SAML:2.0:status:UnknownAttrProfile", new SamlException("UnknownAttrProfile", 400, " An entity that has no knowledge of a particular attribute profile has been presented with an attribute drawn from that profile.") },

        // v1.0 statuscodes
        { "samlp:Requester", new SamlException("Requester", 400, "The request could not be performed due to an error on the part of the requester. ") },
        { "samlp:Responder", new SamlException("Responder", 500, "The request could not be performed due to an error on the part of the responder.") },
        { "samlp:TooManyResponses", new SamlException("TooManyResponses", 500, "The response would contain more elements than the responder will return. ") },
        { "samlp:RequestDenied", new SamlException("RequestDenied", 403, "The responder is able to process the request but has chosen not to respond") },
        { "samlp:ResourceNotRecognized", new SamlException("ResourceNotRecognized", 404, "The responder does not wish to support resource-specific attribute queries, or the resource value provided is invalid or unrecognized") },

        // custom 
        {"NullConditionsException",new SamlException("NullConditionsException", 400, "Bad Request , NotBefore/NotOnOrAfter can't be null") },
        {"NullEmailAttributeException",new SamlException("NullEmailAttributeException", 401, "Unothorized , Email User Attribute can't be null") },
        {"InvalidConditionsException",new SamlException("InvalidConditionsException", 401, "Unothorized , Response is Either early or Delayed .") },
        {"NullOrEmptyResponseException",new SamlException("NullOrEmptyResponseException", 400, "SAML Response cannot be null or empty.") },
        {"MissingStatusException",new SamlException("MissingStatusException", 404, "Invalid Saml Response, Status is missing")},
        {"MissingAssertionException",new SamlException("MissingAssertionException", 404, "Invalid Saml Response, Assertion is missing")},
        {"UnprocessableEntityException",new SamlException("UnprocessableEntityException", 400, "Bad request, Response does not contain appropriate namespaces")},
        {"InvalidStatusException",new SamlException("InvalidStatusException", 404, "Invalid Saml Response Status")},
        {"MissingIssuerException",new SamlException("MissingIssuerException", 404, "Invalid Saml Response, Issuer can't be null or empty")},
        };
    }

    public static class ExceptionCodes
    {
        public const string NullOrEmptyResponse = "NullOrEmptyResponseException";
        public const string MissingAssertion = "MissingAssertionException";
        public const string MissingStatus = "MissingStatusException";
        public const string NullEmail = "NullEmailAttributeException";
        public const string InvalidConditions = "InvalidConditionsException";
        public const string UnauthorizedEmail = "UnauthorizedEmailException";
        public const string UnprocessableEntity = "UnprocessableEntityException";
        public const string NullConditions = "NullConditionsException";
        public const string MissingIssuer = "MissingIssuerException";
    }

}
