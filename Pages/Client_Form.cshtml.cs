using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RealEstatePipeline.Model;
using System.Threading.Tasks;

namespace RealEstatePipeline.Pages
{
    public class Client_FormModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Client_FormModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Client_Info ClientInfo { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Return to the page to display validation errors
            }

            _context.RealEstateInquiries.Add(ClientInfo);
            await _context.SaveChangesAsync(); // Save the new client info to the database

            return RedirectToPage("./Success"); // Redirect to a success page or another relevant page
        }
    }
}
