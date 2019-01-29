using System;
namespace ImpressoApp.Exceptions
{
    public class ServiceAuthenticationException : Exception
    {
        public string Content { get; private set; }

        public ServiceAuthenticationException()
        {
        }

        public ServiceAuthenticationException(string content)
        {
            Content = content;
        }
    }
}
