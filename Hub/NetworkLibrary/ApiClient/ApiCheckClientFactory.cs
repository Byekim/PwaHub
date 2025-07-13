using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLibrary.ApiClient
{
    public class ApiCheckClientFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILoggerFactory _loggerFactory;

        public ApiCheckClientFactory(IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory)
        {
            _httpClientFactory = httpClientFactory;
            _loggerFactory = loggerFactory;
        }

        public ApiCheckClient<T> Create<T>()
        {
            var logger = _loggerFactory.CreateLogger<ApiCheckClient<T>>();
            var httpClient = _httpClientFactory.CreateClient();
            return new ApiCheckClient<T>(httpClient, logger);
        }
    }

}
