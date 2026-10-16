namespace prog_part_two.Models
{
    public class Events
    {
        public int EventId { get; set; }
        public string? EventName { get; set; }
        public string? EventType { get; set; }
        public string? EventDate { get; set; }
        public string? EventLocation { get; set; }
        public string? EventDescription { get; set; }
        public string? Distance { get; set; }

        //foreign keys
        public int OrganiserId { get; set; }
        public int CategoryId { get; set; }
    }
}
