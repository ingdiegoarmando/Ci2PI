using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;

namespace Ci2.PI.ServicioWeb.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("db_ci2pi_owner");

            var configuracionTabUsuario = modelBuilder.Entity<ApplicationUser>().ToTable("TabUsuario");
            configuracionTabUsuario.Property(p => p.Id).HasColumnName("Ci2UsuarioId");
            configuracionTabUsuario.Property(p => p.UserName).HasColumnName("Ci2NombreUsuario");
            configuracionTabUsuario.Property(p => p.Email).HasColumnName("Ci2CorreoElectronico");
            configuracionTabUsuario.Property(p => p.EmailConfirmed).HasColumnName("Ci2CorreoElectronicoConfirmado");
            configuracionTabUsuario.Property(p => p.PasswordHash).HasColumnName("Ci2PasswordHash");
            configuracionTabUsuario.Property(p => p.SecurityStamp).HasColumnName("Ci2SecurityStamp");
            configuracionTabUsuario.Property(p => p.PhoneNumber).HasColumnName("Ci2NumeroTelefonico");
            configuracionTabUsuario.Property(p => p.PhoneNumberConfirmed).HasColumnName("Ci2NumeroTelefonicoConfirmado");
            configuracionTabUsuario.Property(p => p.TwoFactorEnabled).HasColumnName("Ci2AutenticacionDosEtapasActivado");
            configuracionTabUsuario.Property(p => p.LockoutEndDateUtc).HasColumnName("Ci2FechaBloqueoUtc");
            configuracionTabUsuario.Property(p => p.LockoutEnabled).HasColumnName("Ci2BloqueoActivado");
            configuracionTabUsuario.Property(p => p.AccessFailedCount).HasColumnName("Ci2NumeroAccesosFallidos");

            var configuracionTabUsuarioClaim = modelBuilder.Entity<IdentityUserClaim>().ToTable("TabUsuarioClaim");
            configuracionTabUsuarioClaim.Property(p => p.Id).HasColumnName("Ci2UsuarioClaimId");
            configuracionTabUsuarioClaim.Property(p => p.UserId).HasColumnName("Ci2UsuarioId");
            configuracionTabUsuarioClaim.Property(p => p.ClaimType).HasColumnName("Ci2TipoClaim");
            configuracionTabUsuarioClaim.Property(p => p.ClaimValue).HasColumnName("Ci2ValorClaim");

            var configuracionTabUsuarioLogin = modelBuilder.Entity<IdentityUserLogin>().ToTable("TabUsuarioLogin");
            configuracionTabUsuarioLogin.Property(p => p.UserId).HasColumnName("Ci2UsuarioId");
            configuracionTabUsuarioLogin.Property(p => p.LoginProvider).HasColumnName("Ci2ProvedorLogin");
            configuracionTabUsuarioLogin.Property(p => p.ProviderKey).HasColumnName("Ci2llaveProvedor");


            var configuracionTabUsuarioRol = modelBuilder.Entity<IdentityUserRole>().ToTable("TabUsuarioRol");
            configuracionTabUsuarioRol.Property(p => p.RoleId).HasColumnName("Ci2RolId");
            configuracionTabUsuarioRol.Property(p => p.UserId).HasColumnName("Ci2UsuarioId");

            var configuracionTabRol = modelBuilder.Entity<IdentityRole>().ToTable("TabRol");
            configuracionTabRol.Property(p => p.Id).HasColumnName("LeyRolId");
            configuracionTabRol.Property(p => p.Name).HasColumnName("LeyNombre");


        }
    }
}