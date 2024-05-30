using Core.Contracts;
using Core.Entities;
using LightNovelPub.Models;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace LightNovelPub.Controllers
{
    public class ChaptersController : Controller
    {
        private readonly IUnitOfWork _uow;

        [BindProperty]
        public ChapterPage PageModel { get; set; } = new();

        public ChaptersController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IActionResult> Index(int id)
        {
            var novel = await _uow.Novels.GetByIdAsync(id, includeProperties: "Chapters");
            return View(novel);
        }

        public async Task<IActionResult> Chapter(int id)
        {
            var chapter = await _uow.Chapters.GetByIdAsync(id, includeProperties: "Novel");
            var novel = await _uow.Novels.GetByIdAsync(chapter.NovelId, includeProperties: "Chapters");


            PageModel.Chapter = chapter;
            PageModel.Novel = novel;
            PageModel.NovelId = novel.Id;

            novel.Chapters = novel.Chapters.OrderBy(c => c.Id).ToList();
            var currentIndex = novel.Chapters.IndexOf(chapter);

            if (currentIndex + 1 < novel.Chapters.Count)
            {
                PageModel.NextChapterId = novel.Chapters.ElementAt(currentIndex + 1).Id;
            }
            else
            {
                PageModel.DisableNextBtn = true;
            }
            if (currentIndex - 1 >= 0)
            {
                PageModel.PrevChapterId = novel.Chapters.ElementAt(currentIndex - 1).Id;
            }
            else
            {
                PageModel.DisablePrevBtn = true;
            }


            return View(PageModel);
        }

        public IActionResult NavigateChapters(string action)
        {
            switch (action)
            {
                case "Next":
                    return RedirectToAction("Chapter", new { id = PageModel.NextChapterId });
                case "Previous":
                    return RedirectToAction("Chapter", new { id = PageModel.PrevChapterId });
                case "Home":
                    return RedirectToAction("Index",controllerName:"Novels", new { id = PageModel.NovelId });
            }
            return View();
        }
    }
}
