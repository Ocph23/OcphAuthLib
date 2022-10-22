using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OcphAuthServer.Services
{
    public static class OcphAuthDependency
    {
        public static IServiceCollection AddOcphAuth<T>(this IServiceCollection service, string connectionString) where T:OcphAuthContext {

            service.AddDbContext<T>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            service.AddScoped<IUserManager, UserManager<T>>();
            return service;
        }
     
    }
}
