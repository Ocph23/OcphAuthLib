using OcphAuthBlazorView.Models;
using SharedModel.Models;

namespace OcphAuthBlazorView.Services
{
    public interface IAccountService
    {
        public Task<bool> Login(string userName, string password);
        public Task<bool> Logout();
        public Task<bool> Register(RegisterRequest model);
    }
}
