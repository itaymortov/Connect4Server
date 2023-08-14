using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ServerSideConnect4.Data;
using ServerSideConnect4.Models;

namespace ServerSideConnect4.Pages.Game
{
    public class IndexModel : PageModel
    {
        private readonly ServerSideConnect4.Data.QContext _context;

        public IndexModel(ServerSideConnect4.Data.QContext context)
        {
            _context = context;
        }

        public IList<GameData> GameData { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.GameDatas != null)
            {
                GameData = await _context.GameDatas.ToListAsync();
            }
        }
    }
}
