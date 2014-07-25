namespace Wiki.Api.Wikia.Auth
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Wiki.Api.Auth;

    class FormAuthenticationService : WebServiceBase, IAuthenticationService
    {
        private readonly string loginUri;

        public FormAuthenticationService(string loginUri)
        {
            this.loginUri = loginUri;
        }

        public FormAuthenticationService(HttpClient client, bool disposeClient, string loginUri)
            : base(client, disposeClient)
        {
            this.loginUri = loginUri;
        }

        public async Task LoginAsync(string userName, string password)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("username", userName);
            dict.Add("password", password);
            dict.Add("keeploggedin", "1");
            var response = await this.client.PostAsync(this.loginUri, new FormUrlEncodedContent(dict));
            response.EnsureSuccessStatusCode();
        }
    }
}