namespace AutoResponse.Core.Responses
{
    using System.Net;

    using AutoResponse.Core.Dtos;
    using AutoResponse.Core.Extensions;
    using AutoResponse.Core.Models;

    public class ResourceValidationHttpResponse : JsonHttpResponse<ValidationResponseDetailsDto>
    {
        public ResourceValidationHttpResponse(string code, ValidationErrorDetails validationErrorDetails)
            : base(validationErrorDetails.ToDto(code), (HttpStatusCode)422)
        {
        }
    }
}