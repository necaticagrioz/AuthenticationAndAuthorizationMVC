namespace AuthenticationAndAuthorizationMVC.Models.Entities.Interfaces
{
    public enum Status { Active = 1, Modified, Passive }

    public interface IBaseEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Status Status { get; set; }
    }
}
