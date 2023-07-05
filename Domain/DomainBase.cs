namespace Domain
{
    public class DomainBase
    {
        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public DateTime DateModified { get; set; } = DateTime.UtcNow;
    }
}
