using Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LightNovelPub.Controllers
{
    public class NovelsController : Controller
    {
        private readonly IUnitOfWork _uow;

        public NovelsController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IActionResult> Index(int id)
        {
            var novel = await _uow.Novels.GetByIdAsync(id, includeProperties: "Categories");
            return View(novel);
        }
    }
}
