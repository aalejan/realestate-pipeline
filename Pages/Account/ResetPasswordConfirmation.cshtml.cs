using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RealEstatePipeline.Pages.Account
{
    public class ResetPasswordConfirmationModel : PageModel
    {

        public string UserType { get; set; }
        public void OnGet(string userType)
        {
            System.Diagnostics.Debug.WriteLine("UserType: " + userType);
            UserType = userType;
        }
    }
}
