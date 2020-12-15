using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using P3_app_plass.Data;
using P3_app_plass.Models;

namespace P3_app_plass.Pages
{
    public class DetailGarageModel : PageModel
    {
        private readonly P3_app_plass.Data.ApplicationDbContext _context;

        public DetailGarageModel(P3_app_plass.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Garage Garage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Garage = await _context.Garages.Include(o => o.Vehicles)
                .Include(g => g.User).FirstOrDefaultAsync(m => m.Id == id);

            if (Garage == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
