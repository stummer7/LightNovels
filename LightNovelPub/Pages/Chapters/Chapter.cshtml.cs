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

        public ChapterModel(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task OnGetAsync(int id)
        {
            var chapter = await _uow.Chapters.GetByIdAsync(id);

            if (chapter != null)
            {
                Chapter = chapter;
            }
        }
    }
}
