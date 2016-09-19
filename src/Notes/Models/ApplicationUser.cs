using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Notes.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            IncludeFinished = false;
            SortingField = "FinishDate";
            SortingAscending = true;
            ThemeSelection = 1;
        }

        public Boolean IncludeFinished { get; set; }
        public String SortingField { get; set; }
        public Boolean SortingAscending { get; set; }
        public int ThemeSelection { get; set; }
    }
}