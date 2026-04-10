using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RecruitmentAgencyFinal.Models;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RecruitmentAgencyFinal.Models
{
    // Модель пользователя для Identity
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public System.DateTime RegisteredAt { get; set; }
        public int? CandidateId { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    // Контекст базы данных для Identity
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Candidate> Candidates { get; set; }
    }
}