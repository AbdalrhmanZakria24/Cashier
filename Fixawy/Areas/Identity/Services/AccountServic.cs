using Microsoft.AspNetCore.Identity;

namespace Fixawy.Areas.Identity.Services
{
    public class AccountServic : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountServic> _logger;

        public AccountServic(IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountServic> logger)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        //public async Task<IdentityResponse> RegisterWorker(, CancellationToken cancellationToken)
        //{
           
        //}

    }
}
