using System;
using System.Collections.Generic;
using System.Net.Http;
using BacklogApiBridge.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;

namespace BacklogApiBridge.Extensions
{
    public static class HttpClientFactoryExtensions
    {
        public static THttpClient CreateRestServiceClient<THttpClient>(
            this IHttpClientFactory source, 
            string spaceKey)
        {
            var client = source.CreateClient(spaceKey);
            return RestService.For<THttpClient>(client, new RefitSettings
            {
                UrlParameterFormatter = new BacklogUriParameterFormatter(),
                ContentSerializer = new JsonContentSerializer(new JsonSerializerSettings 
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                })
            });
        }
    }
}
