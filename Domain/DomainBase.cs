namespace Domain
{
    public class DomainBase
    {
        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; } = DateTime.UtcNow;
    }
}
