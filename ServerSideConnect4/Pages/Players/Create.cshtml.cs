using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;
using NuGet.Common;
using ServerSideConnect4.Data;
using ServerSideConnect4.Models;

namespace ServerSideConnect4.Pages.Players
{
    public class CreateModel : PageModel
    {
        private readonly ServerSideConnect4.Data.QContext _context;
        public List<SelectListItem> countries { get; set; }


        public CreateModel(ServerSideConnect4.Data.QContext context)
        {
            _context = context;
            this.countries = new List<SelectListItem>
            {
                new SelectListItem { Value = "Israel", Text = "Israel" },
                new SelectListItem { Value = "Mexico", Text = "Mexico" },
                new SelectListItem { Value = "Canada", Text = "Canada" },
                new SelectListItem { Value = "United Kingdom", Text = "United Kingdom" },
                new SelectListItem { Value = "Germany", Text = "Germany" },
                new SelectListItem { Value = "France", Text = "France" },
                new SelectListItem { Value = "Italy", Text = "Italy" },
                new SelectListItem { Value = "Spain", Text = "Spain" },
                new SelectListItem { Value = "Japan", Text = "Japan" },
                new SelectListItem { Value = "China", Text = "China" },
                new SelectListItem { Value = "Australia", Text = "Australia" },
                new SelectListItem { Value = "South Korea", Text = "South Korea" },
                new SelectListItem { Value = "Brazil", Text = "Brazil" },
                new SelectListItem { Value = "Argentina", Text = "Argentina" },
                new SelectListItem { Value = "India", Text = "India" },
                new SelectListItem { Value = "Russia", Text = "Russia" },
                new SelectListItem { Value = "South Africa", Text = "South Africa" },
                new SelectListItem { Value = "Nigeria", Text = "Nigeria" },
                new SelectListItem { Value = "Egypt", Text = "Egypt" },
                new SelectListItem { Value = "USA", Text = "USA" }

            };
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Player Player { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            // Check if the ID already exists in the database
            if (_context.Player.Any(p => p.ID == Player.ID))
            {
                ModelState.AddModelError("ID", "This ID already exists. Please choose a different one.");
                return Page();
            }
            _context.Player.Add(Player);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
