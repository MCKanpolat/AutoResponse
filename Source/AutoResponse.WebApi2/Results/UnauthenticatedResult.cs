﻿namespace AutoResponse.WebApi2.Results
{
    using System.Net.Http;

    using AutoResponse.Core.ApiEvents;

    public class UnauthenticatedResult : AutoResponseResult
    {
        public UnauthenticatedResult(HttpRequestMessage request)
            : base(request, new UnauthenticatedApiEvent())
        {
        }
    }
}