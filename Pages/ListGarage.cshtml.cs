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
    public class ListGarageModel : PageModel
    {
        private readonly P3_app_plass.Data.ApplicationDbContext _context;

        public ListGarageModel(P3_app_plass.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Garage> Garage { get;set; }

        public async Task OnGetAsync()
        {
            //Garage = await _context.Garages.Include(g => g.User).ToListAsync();

            Garage = await _context.Garages.Include(n => n.User).Where(x => x.UserId == _context.Users.Single(x => x.UserName == User.Identity.Name).Id).ToListAsync();
        }
    }
}
