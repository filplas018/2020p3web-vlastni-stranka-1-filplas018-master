using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using P3_app_plass.Data;
using P3_app_plass.Models;

namespace P3_app_plass.Pages
{
    [Authorize]
    public class EditGarageModel : PageModel
    {
        private readonly P3_app_plass.Data.ApplicationDbContext _context;

        public EditGarageModel(P3_app_plass.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Garage Garage { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Garages.Add(Garage);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
