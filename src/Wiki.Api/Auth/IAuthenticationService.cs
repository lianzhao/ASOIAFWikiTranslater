namespace Wiki.Api.Auth
{
    using System.Threading.Tasks;

    public interface IAuthenticationService
    {
        Task LoginAsync(string userName, string password);
    }
}