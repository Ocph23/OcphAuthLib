using OcphAuthServer.Datas;
using SharedModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcphAuthServer.Services
{
    public interface IUserManager
    {
        public Task<User> FindUserByEmail(string email);
        public Task<User> FindUserByUserName(string email);
        public Task<User> FindUserById(int email);
        
        public Task<RegisterResult> Register(User email, string password="");
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<bool> AddToRoles(User user, string role);
        public  Task<LoginResponse> Login(LoginRequest request);
    }
}
