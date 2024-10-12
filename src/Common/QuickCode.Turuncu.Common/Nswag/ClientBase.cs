using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace QuickCode.Turuncu.Common.Nswag
{
    public abstract class ClientBase
    {
        public string BearerToken { get; private set; } = default!;
        public string ApiKey { get; private set; } = default!;
        
        public void SetBearerToken(string token)
        {
            BearerToken = token;
        }

        public void SetApiKey(string apiKey)
        {
            ApiKey = apiKey;
        }
   
        protected async Task<HttpRequestMessage> CreateHttpRequestMessageAsync(CancellationToken cancellationToken)
        {
            var msg = new HttpRequestMessage();

            if (!string.IsNullOrWhiteSpace(BearerToken))
            {
                msg.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);
                BearerToken = string.Empty;
            }
            
            if (!string.IsNullOrWhiteSpace(ApiKey))
            {
                msg.Headers.Add("X-Api-Key", ApiKey);
                BearerToken = string.Empty;
            }

            return await Task.FromResult(msg);
        }
    }
}
