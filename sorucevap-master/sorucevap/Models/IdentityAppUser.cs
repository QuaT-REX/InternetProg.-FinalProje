using Microsoft.AspNetCore.Identity;

namespace sorucevap.Models
{
    public class IdentityAppUser:IdentityUser<int>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
