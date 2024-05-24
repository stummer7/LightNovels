using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Persistence;
using Core.Contracts;

namespace LightNovelPub.Pages.Novels
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _uow;

        public IndexModel(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IList<Novel> Novel { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_uow.Novels != null)
            {
                Novel = await _uow.Novels.GetAllNovelsAsync();
            }
        }
    }
}
