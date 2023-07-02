namespace Domain
{
    public class League : DomainBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Club>? Clubs { get; set; }
    }
}
