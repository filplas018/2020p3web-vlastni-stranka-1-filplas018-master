using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using P3_app_plass.Models;

namespace P3_app_plass.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly P3_app_plass.Data.ApplicationDbContext _context;
        public List<Vehicle> Vehs { get; set; }
        public Vehicle Rveh {get;set;}

        public IndexModel(ILogger<IndexModel> logger, P3_app_plass.Data.ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void OnGet()       
        {
            Random random = new Random();
            Vehs = _context.Vehicles.ToList();
            int index = random.Next(Vehs.Count);
            
            Rveh = Vehs[index];
        }
    }
}
