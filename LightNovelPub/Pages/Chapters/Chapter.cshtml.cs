using Core.Contracts;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LightNovelPub.Pages.Chapters
{
    public class ChapterModel : PageModel
    {
        private readonly IUnitOfWork _uow;

        public Chapter Chapter { get; set; } = new();

        [BindProperty]
        public int NovelId { get; set; }
        [BindProperty]
        public int NextChapterId { get; set; }

        [BindProperty]
        public int PrevChapterId { get; set; }

        public bool DisableNextBtn { get; set; } = false;
        public bool DisablePrevBtn { get; set; } = false;


        public ChapterModel(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task OnGetAsync(int id)
        {
            var chapter = await _uow.Chapters.GetByIdAsync(id,includeProperties:"Novel");

            if (chapter != null)
            {
                Chapter = chapter;

                var novel = await _uow.Novels.GetNovelByChapterId(chapter.Id);
                NovelId = chapter.NovelId;

                if (novel != null)
                {
                    novel.Chapters = novel.Chapters.OrderBy(c => c.Id).ToList();
                    var currentIndex = novel.Chapters.IndexOf(chapter);

                    if (currentIndex + 1 < novel.Chapters.Count)
                    {
                        NextChapterId = novel.Chapters.ElementAt(currentIndex + 1).Id;
                    }
                    else
                    {
                        DisableNextBtn = true;
                    }
                    if(currentIndex - 1 >= 0)
                    {
                        PrevChapterId = novel.Chapters.ElementAt(currentIndex-1).Id;
                    }
                    else
                    {
                        DisablePrevBtn = true;
                    }
                }
               
            }
        }

        public IActionResult OnPost(string action)
        {
            switch (action)
            {
                case "Next":
                    return RedirectToPage("/Chapters/Chapter", new { id = NextChapterId});
                case "Previous":
                    return RedirectToPage("/Chapters/Chapter", new { id = PrevChapterId });
                case "Home":
                    return RedirectToPage("/Novels/Index", new { id = NovelId });
            }

            return Page();
        }
    }
}
