using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fixawy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService )
        {
            _categoryService = categoryService;
        }

     
    }
}
