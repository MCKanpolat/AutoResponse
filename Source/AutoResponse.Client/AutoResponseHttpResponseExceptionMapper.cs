﻿// <copyright file="AutoResponseHttpResponseExceptionMapper.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutoResponse.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using AutoResponse.Core.Dtos;
    using AutoResponse.Core.Enums;
    using AutoResponse.Core.Exceptions;
    using AutoResponse.Core.Models;

    /// <summary>
    /// AutoResponse HTTP response exception mapper.
    /// </summary>
    /// <seealso cref="AutoResponse.Client.AutoResponseHttpResponseExceptionMapperBase" />
    public class AutoResponseHttpResponseExceptionMapper : AutoResponseHttpResponseExceptionMapperBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoResponseHttpResponseExceptionMapper"/> class.
        /// </summary>
        public AutoResponseHttpResponseExceptionMapper()
            : base(new AutoResponseHttpResponseFormatter())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoResponseHttpResponseExceptionMapper"/> class.
        /// </summary>
        /// <param name="formatter">The formatter.</param>
        public AutoResponseHttpResponseExceptionMapper(IAutoResponseHttpResponseFormatter formatter)
            : base(formatter)
        {
        }

        /// <inheritdoc />
        protected override void ConfigureMappings(HttpResponseExceptionConfiguration configuration)
        {
            configuration.AddMapping(
                HttpStatusCode.Unauthorized,
                (r, c) => new UnauthenticatedException(
                    c.Formatter.Code(r.Property("code")),
                    c.Formatter.Message(r.Property("message"))));

            configuration.AddMapping(
                HttpStatusCode.Forbidden,
                "AR403",
                (r, c) => new EntityPermissionException(
                    c.Formatter.Code(r.Property("code")),
                    userId: c.Formatter.EntityProperty(r.Property("userId")),
                    entityType: c.Formatter.EntityType(r.Property("resource")),
                    entityId: c.Formatter.EntityProperty(r.Property("resourceId"))));

            configuration.AddMapping(
                HttpStatusCode.Forbidden,
                "AR403C",
                (r, c) => new EntityCreatePermissionException(
                    c.Formatter.Code(r.Property("code")),
                    c.Formatter.EntityProperty(r.Property("userId")),
                    c.Formatter.EntityType(r.Property("resource")),
                    c.Formatter.EntityType(r.Property("resourceId"))));

            configuration.AddMapping(
                (HttpStatusCode)422,
                (r, c) =>
                    {
                        var errorsResponse = r.Property<List<ResourceValidationErrorApiModel>>("errors");
                        var errors = errorsResponse?.Select(e => new ValidationError(
                            resource: c.Formatter.EntityType(e.Resource),
                            field: c.Formatter.EntityProperty(e.Field),
                            code: ToValidationErrorCode(e.Code),
                            errorMessage: c.Formatter.Message(e.Message))) ?? Enumerable.Empty<ValidationError>();

                        var errorDetails = new ValidationErrorDetails(c.Formatter.Message(r.Property("message")), errors);
                        return new EntityValidationException(
                            code: c.Formatter.Code(r.Property("code")),
                            errorDetails: errorDetails);
                    });

            configuration.AddMapping(
                HttpStatusCode.NotFound,
                "AR404",
                (r, c) => new EntityNotFoundException(
                    c.Formatter.Code(r.Property("code")),
                    entityType: c.Formatter.EntityType(r.Property("resource")),
                    entityId: c.Formatter.EntityProperty(r.Property("resourceId"))));

            configuration.AddMapping(
                HttpStatusCode.NotFound,
                "AR404Q",
                (r, c) => new EntityNotFoundQueryException(
                    c.Formatter.Code(r.Property("code")),
                    c.Formatter.EntityType(r.Property("resource")),
                    r.Property<IEnumerable<QueryParameterDto>>("queryParameters").Select(qp => new QueryParameter(qp.Key, qp.Value))));

            configuration.AddMapping(
                HttpStatusCode.InternalServerError,
                (r, c) => new ServiceErrorException(
                    c.Formatter.Code(r.Property("code")),
                    new Exception(c.Formatter.Message(r.Property("message")))));
        }

        /// <inheritdoc />
        protected override Task<Exception> GetDefaultException(
            RequestContent requestContent,
            ResponseContent responseContent,
            HttpResponseExceptionContext context)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("There was an unexpected response.");
            stringBuilder.AppendLine("Response:");
            stringBuilder.AppendLine(responseContent.ToString());
            stringBuilder.AppendLine("Request:");
            stringBuilder.AppendLine(requestContent.ToString());

            var exceptionMessage = context.Formatter.Message(stringBuilder.ToString());
            return Task.FromResult(new Exception(exceptionMessage));
        }

        private static ValidationErrorCode ToValidationErrorCode(string code)
        {
            return Enum.TryParse(code, ignoreCase: true, result: out ValidationErrorCode result) ? result : ValidationErrorCode.None;
        }
    }
}