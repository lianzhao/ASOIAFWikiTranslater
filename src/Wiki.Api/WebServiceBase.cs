namespace Wiki.Api
{
    using System;
    using System.Net.Http;

    public class WebServiceBase : IDisposable
    {
        protected readonly HttpClient client;

        private readonly bool disposeClient;

        public WebServiceBase()
            : this(new HttpClient(), true)
        {
        }

        public WebServiceBase(HttpClient client, bool disposeClient)
        {
            this.client = client;
            this.disposeClient = disposeClient;
        }

        public void Dispose()
        {
            if (this.disposeClient)
            {
                this.client.Dispose();
            }
        }
    }
}