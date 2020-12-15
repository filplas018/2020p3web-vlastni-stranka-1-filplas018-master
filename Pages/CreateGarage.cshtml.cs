using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class CreateGarageModel : PageModel
    {
        private readonly P3_app_plass.Data.ApplicationDbContext _context;

        public CreateGarageModel(P3_app_plass.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IActionResult OnGet()
        {
            
            
            return Page();
        }

        [BindProperty]
        public string Gname { get; set; }
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var Garage = new Garage();
            var UserId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            Garage.UserId = UserId;
            Garage.Name = Gname;
            _context.Garages.Add(Garage);
            await _context.SaveChangesAsync();

            return RedirectToPage("./ListGarage");
        }
    }
}
