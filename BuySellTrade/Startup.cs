
using BuySellTrade.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BuySellTrade.Startup))]
namespace BuySellTrade
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createAdminRoleAndUser();
        }

        /* Creates the Admin role the first time the application runs
         * The admin username is johndoe@gmail.com
         * The password is P@ssword1
         * Code in this method inspired by https://code.msdn.microsoft.com/ASPNET-MVC-5-Security-And-44cbdb97 
         */   
        private void createAdminRoleAndUser()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));       
            if (!roleManager.RoleExists("Admin"))
            {        
                // Create the Admin role      
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                // Create the Admin user
                var user = new ApplicationUser();
                user.UserName = "johndoe@gmail.com";
                user.Email = "johndoe@gmail.com";
                string userPassword = "P@ssword1";

                var chkUser = UserManager.Create(user, userPassword);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }
        }
    }
}
