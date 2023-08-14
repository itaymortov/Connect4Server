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
    public class DeleteModel : PageModel
    {
        private readonly ServerSideConnect4.Data.QContext _context;

        public DeleteModel(ServerSideConnect4.Data.QContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.GameDatas == null)
            {
                return NotFound();
            }
            var gamedata = await _context.GameDatas.FindAsync(id);

            if (gamedata != null)
            {
                GameData = gamedata;
                _context.GameDatas.Remove(GameData);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
