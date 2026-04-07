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

        public async Task<IdentityResponse> RegisterWorker(RegisterWorker registerWorker, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(registerWorker.email);

            if (user is not null)
            {
                return new IdentityResponse
                {
                    isSuccess = false,
                    message = "emial is alredy useed",
                    createdAt = DateTime.UtcNow,
                };
            }

            if (registerWorker.serviceId <= 0)
            {
                return new IdentityResponse
                {
                    isSuccess = false,
                    message = "select service",
                    createdAt = DateTime.UtcNow,
                };
            }


            var imageName = "";
            try
            {
                if (registerWorker.nationalIdImage is not null && registerWorker.nationalIdImage.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var extension = Path.GetExtension(registerWorker.nationalIdImage.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        return new IdentityResponse
                        {
                            isSuccess = false,
                            message = "Invalid image format",
                            createdAt = DateTime.UtcNow
                        };
                    }
                    imageName = Guid.NewGuid().ToString() + Path.GetExtension(registerWorker.nationalIdImage.FileName);
                    var rootePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    var folderPath = Path.Combine(rootePath, "WorkerImages");

                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    var imagePath = Path.Combine(folderPath, imageName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await registerWorker.nationalIdImage.CopyToAsync(stream, cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception error {ex.Message}");

                return new IdentityResponse
                {
                    isSuccess = false,
                    message = ex.Message,
                    createdAt = DateTime.UtcNow
                };
            }

            var adduser = new ApplicationUser
            {
                UserName = registerWorker.email,
                Email = registerWorker.email,
                PhoneNumber = registerWorker.phoneNumber,
                fullName=registerWorker.Name
            };

            var result = await _userManager.CreateAsync(adduser, registerWorker.password);

            if (!result.Succeeded)
            {
                return new IdentityResponse
                {
                    isSuccess = false,
                    message = string.Join(",", result.Errors.Select(e => e.Description)),
                    createdAt = DateTime.UtcNow
                };
            }

            await _userManager.AddToRoleAsync(adduser, Rl.Worker);

           
            //var addworker = new Admin.Model.Worker
            //{
            //    userId = adduser.Id,
            //    serviceId = registerWorker.serviceId,
            //    Location = registerWorker.location,
            //    NationalIdImage = imageName,
            //};

            //await _unitOfWork.WorkerReposatory.CreateAsync(addworker, cancellationToken);
            await _unitOfWork.CommitAsync();

            return new IdentityResponse
            {
                isSuccess = true,
                message = "Worker registered successfully",
                createdAt = DateTime.UtcNow,
            };
        }
        //public IdentityResponse RegisterUser()
        //{

        //}
    }
}
