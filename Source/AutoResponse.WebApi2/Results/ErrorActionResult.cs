﻿namespace AutoResponse.WebApi2.Results
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;

    using AutoResponse.Core.Models;

    using Humanizer;

    public abstract class ErrorActionResult : IHttpActionResult
    {
        private readonly HttpRequestMessage request;

        private readonly HttpStatusCode statusCode;        

        protected ErrorActionResult(HttpRequestMessage request, HttpStatusCode statusCode)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if ((int)statusCode < 400)
            {
                throw new ArgumentException(nameof(statusCode));
            }            

            this.request = request;
            this.statusCode = statusCode;            
        }

        protected abstract ValidationErrorDetails GetErrorDetails();

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = this.request.CreateResponse(this.statusCode);

            response.Content = new ObjectContent<ErrorDetailsApiModel<ValidationErrorApiModel>>(
                this.MapToErrorDetailsApiModel(this.GetErrorDetails()),
                this.request.GetConfiguration().Formatters.JsonFormatter,
                "application/json");

            return Task.FromResult(response);
        }

        private ErrorDetailsApiModel<ValidationErrorApiModel> MapToErrorDetailsApiModel(ValidationErrorDetails errorDetails)
        {
            var errors = errorDetails.Errors?.Select(this.MapToErrorApiModel);

            return new ErrorDetailsApiModel<ValidationErrorApiModel>
            {
                Message = errorDetails.Message,
                Errors = errors
            };
        }

        private ValidationErrorApiModel MapToErrorApiModel(ValidationError validationError)
        {
            return new ValidationErrorApiModel
            {
                Resource = validationError.Resource,
                Field = validationError.Field,
                Code = validationError.Code.ToString().Kebaberize()
            };            
        }
    }
}