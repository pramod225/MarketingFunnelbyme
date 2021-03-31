using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Security.Principal;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;

namespace YuktiSolutions.MarketingFunnel.Models.Database
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var userId = userIdentity.GetUserId();
                var user = await context.Users.FirstOrDefaultAsync(x => x.Id.Equals(userId, StringComparison.CurrentCultureIgnoreCase));

                if (String.IsNullOrEmpty(user.DisplayName) == false)
                {
                    userIdentity.AddClaim(new Claim("Name", user.DisplayName));
                }
                else
                {
                    userIdentity.AddClaim(new Claim("Name", user.Email));

                }
            }
            // Add custom user claims here
            return userIdentity;
        }
        [StringLength(256, ErrorMessage = "Display name cannot be more than {1} characters long.")]
        public String DisplayName { get; set; }
        public Boolean IsActive { get; set; }
        public String SelectedLanguage { get; set; }

    }

    public static class IdentityExtensions
    {
        public static string GetDisplayName(this IIdentity identity)
        {
            var displayName = ((ClaimsIdentity)identity).FindFirst("Name");
            if (displayName == null)
            {
                return "";
            }
            return String.Format("{0}", displayName.Value);
        }
    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public virtual DbSet<AnalyticsCredential> AnalyticsCredentials { get; set; }
    }
}