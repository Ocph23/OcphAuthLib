using Microsoft.EntityFrameworkCore;
using OcphAuthServer.Datas;
using System.Text;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SharedModel.Models;

namespace OcphAuthServer.Services
{
    public class UserManager<T> : IUserManager where T : OcphAuthContext
    {

        private readonly T _dbcontext;
        public UserManager(T context)
        {
            _dbcontext = context;
        }

        public Task<bool> AddToRoles(User user, string roleName)
        {
            try
            {
                var role = _dbcontext.Roles.FirstOrDefault(x => x.Name.ToLower() == roleName.ToLower());
                if (role == null)
                    throw new SystemException("Role Not Found !");
                var userDB = _dbcontext.Users.FirstOrDefault(x => x.Id == user.Id);
                if (userDB != null)
                {
                    userDB.UserRoles.Add(new UserRole { Role = role });
                    _dbcontext.SaveChanges();
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
            catch (DbUpdateException)
            {
                return Task.FromResult(false);
            }
        }

        public Task<bool> CheckPasswordAsync(User user, string password)
        {
            try
            {
                var userDB = _dbcontext.Users.FirstOrDefault(x => x.Id == user.Id);
                if (userDB != null && userDB.Password.Equals(password))
                    return Task.FromResult(true);
                return Task.FromResult(false);
            }
            catch (DbUpdateException)
            {
                return Task.FromResult(false);
            }
        }

        public Task<User> FindUserByEmail(string email)
        {
            try
            {
                var userDB = _dbcontext.Users.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
                return Task.FromResult(userDB);
            }
            catch (DbUpdateException)
            {
                return null;
            }
        }

        public Task<User> FindUserById(int id)
        {
            try
            {
                var userDB = _dbcontext.Users.FirstOrDefault(x => x.Id == id);
                return Task.FromResult(userDB);
            }
            catch (DbUpdateException)
            {
                return null;
            }
        }

        public Task<User> FindUserByUserName(string userName)
        {
            try
            {
                var userDB = _dbcontext.Users.FirstOrDefault(x => x.UserName == userName);
                return Task.FromResult(userDB);
            }
            catch (DbUpdateException)
            {
                return null;
            }
        }
        private string GeneratePasswordHash(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new SystemException("Password Requeired !");

#pragma warning disable SYSLIB0021 // Type or member is obsolete
            using var md5 = MD5CryptoServiceProvider.Create();
#pragma warning restore SYSLIB0021 // Type or member is obsolete

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));

            //get hash result after compute it  
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            byte[] result = md5.Hash;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            StringBuilder strBuilder = new();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return strBuilder.ToString();
        }


        public async Task<LoginResponse> Login(LoginRequest request)
        {
            try
            {

                var user = await FindUserByUserName(request.UserName);
                if (user != null && await CheckPasswordAsync(user, request.Password))
                {
                    var token = await GenerateJwtToken(user);
                    return new LoginResponse(user.UserName, user.Email, token);
                }

                throw new UnauthorizedAccessException($"Your Not Have Accout !");

            }
            catch (System.Exception ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }
        }

        public Task<RegisterResult> Register(User user, string password = "")
        {
            try
            {
                user.Password = password;
                _dbcontext.Users.Add(user);
                _dbcontext.SaveChanges();
                var token = GenerateJwtToken(user);
                return Task.FromResult(new RegisterResult(user.Id, user.UserName, user.Email, token.Result));
            }
            catch (DbUpdateException ex)
            {
                var errors = new List<string>();
                errors.Add(ex.Message);
                throw new OcphAuthException(ex.Message, null, errors);
            }
        }


        private Task<string> GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            if (user.UserRoles != null)
            {
                foreach (var userRole in user.UserRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));
                }

            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthorizationConstants.JWT_SECRET_KEY));

            var token = new JwtSecurityToken(
                issuer: AuthorizationConstants.JWT_ValidIssuer,
                audience: AuthorizationConstants.JWT_ValidAudience,
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return Task.FromResult(tokenHandler.WriteToken(token));
        }



    }



}
