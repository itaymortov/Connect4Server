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
    public class DetailsModel : PageModel
    {
        private readonly ServerSideConnect4.Data.QContext _context;

        public DetailsModel(ServerSideConnect4.Data.QContext context)
        {
            _context = context;
        }

      public GameData GameData { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.GameDatas == null)
            {
                return NotFound();
            }

            var gamedata = await _context.GameDatas.FirstOrDefaultAsync(m => m.ID == id);
            if (gamedata == null)
            {
                return NotFound();
            }
            else 
            {
                GameData = gamedata;
            }
            return Page();
        }
    }
}
