using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace Sufficit.Identity
{
    public class ApiBearerTokenHandler : DelegatingHandler
    {
        private readonly ITokenProvider _token;

        /// <summary>
        /// Testar se o accessor existe, !?
        /// </summary>
        public ApiBearerTokenHandler(ITokenProvider token)
        {
            _token = token;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (ShouldAuthenticate(request))
            {
                if (request.Headers.Authorization == null)
                {
                    // request the access token
                    var accessToken = await _token.GetTokenAsync();

                    if (string.IsNullOrWhiteSpace(accessToken))
                        return new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "access token not available at this time", RequestMessage = request };

                    // set the bearer token to the outgoing request
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                }
            }

            // Proceed calling the inner handler, that will actually send the request
            // to our protected api
            return await base.SendAsync(request, cancellationToken);
        }

        protected virtual bool ShouldAuthenticate(HttpRequestMessage request)
            => true;
    }
}
