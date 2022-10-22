using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcphAuthServer.Datas
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool EmailComfirm { get; set; }

        public bool IsLock { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }



    }
}
