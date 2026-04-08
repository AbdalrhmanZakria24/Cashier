
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Fixawy.DBSeder
{
    public class DBInitializar : IDBInitializar
    {
        private readonly ApplicationDBContext _dBContext;
        private readonly ILogger<DBInitializar> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public DBInitializar(ApplicationDBContext dBContext,
            ILogger<DBInitializar> logger,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _dBContext = dBContext;
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void Initialize()
        {
            try
            {
                if(_dBContext.Database.GetPendingMigrations().Any())
                {
                    _dBContext.Database.Migrate();
                }

                if(!_roleManager.RoleExistsAsync(Rl.SuperAdmin).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new(Rl.SuperAdmin)).GetAwaiter().GetResult();
                }
                if(!_roleManager.RoleExistsAsync(Rl.BranchManager).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new(Rl.BranchManager)).GetAwaiter().GetResult();
                }
                if(!_roleManager.RoleExistsAsync(Rl.Tentant).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new(Rl.Tentant)).GetAwaiter().GetResult();
                }
                if(!_roleManager.RoleExistsAsync(Rl.Customer).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new(Rl.Customer)).GetAwaiter().GetResult();
                }
                if(!_roleManager.RoleExistsAsync(Rl.Cashier).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new(Rl.Cashier)).GetAwaiter().GetResult();
                }

                var user = new ApplicationUser
                {
                    UserName = "SuperAdmin",
                    fullName = "SuperAdmin",
                    Email = "SuperAdmin@gamil.com",
                    EmailConfirmed = true,
                    Address="Cairo"
                };

                _userManager.CreateAsync(user,"Admin123@").GetAwaiter().GetResult();

                _userManager.AddToRoleAsync(user,Rl.SuperAdmin).GetAwaiter().GetResult();

            }catch (Exception ex)
            {
                _logger.LogError($"Error {ex.Message}");
            }
        }


    }
}
