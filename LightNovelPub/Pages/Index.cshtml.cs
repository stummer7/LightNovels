using Core.Contracts;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LightNovelPub.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IUnitOfWork _uow;
        public List<Novel> Novels { get; set; } = new();

        public IndexModel(ILogger<IndexModel> logger, IUnitOfWork uow)
        {
            _logger = logger;
            _uow = uow;
        }

        public async Task OnGetAsync()
        {
            Novel n = new Novel();
            n.Name = "Test123";

            await _uow.Novels.AddAsync(n);
            await _uow.SaveChangesAsync();

            Novels = await _uow.Novels.GetAllNovelsAsync();
        }
    }
}