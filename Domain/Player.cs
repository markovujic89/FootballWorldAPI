namespace Domain
{
    public class Player : DomainBase
    {        
        public int Age { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public Guid? ClubId { get; set; }

        public string Position { get; set; }

        public virtual Club? Club { get; set; }
    }
}
