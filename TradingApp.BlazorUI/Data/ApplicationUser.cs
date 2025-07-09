using Microsoft.AspNetCore.Identity;

namespace TradingApp.BlazorUI.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        //some example code
        public string FavouriteColor { get; set; }
        public DateTime BirthDate { get; set; }
    }

}
