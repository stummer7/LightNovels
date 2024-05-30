using Core.Contracts;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LightNovelPub.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _uow;
        public List<Novel> Novels { get; set; } = new();

        public HomeController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
        public async Task<IActionResult> Index()
        {
            Novels = await _uow.Novels.GetAllNovelsAsync();
            return View(Novels);
        }
    }
}
