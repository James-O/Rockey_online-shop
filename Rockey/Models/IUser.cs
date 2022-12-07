using Microsoft.AspNetCore.Identity;

namespace Rockey.Models
{
    public class IUser: IdentityUser
    {
        public string FullName { get;set; }
    }
}
