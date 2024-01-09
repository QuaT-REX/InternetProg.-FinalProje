using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace sorucevap.Models
{
    public class Context:IdentityDbContext<IdentityAppUser , IdentityAppRole , int>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-C5DQJD2\\SQLEXPRESS;Initial Catalog=SoruCevap; TrustServerCertificate=True; Integrated Security=True;");
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Soru> sorus { get; set; }
        public DbSet<Cevap> cevaps { get; set; }
    }
}
