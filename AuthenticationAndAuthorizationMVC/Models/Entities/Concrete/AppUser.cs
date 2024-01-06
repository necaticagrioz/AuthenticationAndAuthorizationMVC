using AuthenticationAndAuthorizationMVC.Models.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationAndAuthorizationMVC.Models.Entities.Concrete
{
    public class AppUser : IdentityUser, IBaseEntity
    {
        public string? Occupation { get; set; }

        private DateTime _createdDate = DateTime.Now;
        public DateTime CreatedDate 
        { 
            get => _createdDate;
            set => _createdDate = value;
        }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        private Status _status = Status.Active;
        public Status Status 
        {
            get => _status;
            set => _status = value;
        }
    }
}
