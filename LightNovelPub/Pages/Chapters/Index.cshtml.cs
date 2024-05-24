using Core.Contracts;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LightNovelPub.Pages.Novels.NewFolder
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _uow;
        public Novel Novel { get; set; } = new();

        public IndexModel(IUnitOfWork uow)
        {
                _uow = uow;
        }
        public async Task OnGetAsync(int id)
        {
            var novel = await _uow.Novels.GetByIdAsync(id, includeProperties: "Chapters");

            if(novel != null)
            {
                Novel = novel;
            }
        }
    }
}
