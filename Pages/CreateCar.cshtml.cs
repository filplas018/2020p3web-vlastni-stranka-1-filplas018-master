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
    public class CreateCarModel : PageModel
    {
        private readonly P3_app_plass.Data.ApplicationDbContext _context;
        
        public CreateCarModel(P3_app_plass.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IActionResult OnGet(int? gid)
        {
            if(gid != null)
            {
                Vehicle = new Vehicle();
                Vehicle.GarageId = (int)gid;
            }
            var UserId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            Garages1 = _context.Garages.Where(x => x.UserId == UserId).ToList();
        ViewData["GarageId"] = new SelectList(Garages1, "Id", "Name");
            return Page();
        }
        [BindProperty]
        public List<Garage> Garages1 { get; set; }
        [BindProperty]
        public Vehicle Vehicle { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Vehicles.Add(Vehicle);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
