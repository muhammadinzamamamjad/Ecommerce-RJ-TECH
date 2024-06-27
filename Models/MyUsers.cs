using Microsoft.AspNetCore.Identity;

namespace RJTECH_Authentication_.Models
{
    public class MyUsers: IdentityUser
    {
        public string FullName { get; set; }
    }
}
