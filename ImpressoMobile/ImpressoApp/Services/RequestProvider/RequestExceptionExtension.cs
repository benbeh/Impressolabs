using System;
using System.Net;
using System.Net.Http;
namespace ImpressoApp.Services.RequestProvider
{
    public class RequestExceptionExtension : HttpRequestException
    {
        public HttpStatusCode HttpCode { get; }
        public RequestExceptionExtension(HttpStatusCode code) : this(code, null, null)
        {
        }

        public RequestExceptionExtension(HttpStatusCode code, string message) : this(code, message, null)
        {
        }

        public RequestExceptionExtension(HttpStatusCode code, string message, Exception inner) : base(message,
            inner)
        {
            HttpCode = code;
        }
    }
}
