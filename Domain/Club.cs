namespace Domain
{
    public class Club : DomainBase
    {
        public string Name { get; set; }

        public string City { get; set; }

        // navigation properties
        public virtual ICollection<League>? Leagues { get; set;}

        public virtual ICollection<Player>? Players { get; set;}
    }
}
