using Microsoft.AspNetCore.Identity;
using TaskerFullStackApp.Models;

namespace TaskerFullStackApp.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        //extending IdentityUser with custom properties
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public Guid? ProfilePictureId { get; set; }

        //navigation property within Db for profile picture
        public virtual ImageUpload? ProfilePicture { get; set; } //navigation property for profile picture
    }

}
