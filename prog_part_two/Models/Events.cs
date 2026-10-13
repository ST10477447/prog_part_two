namespace prog_part_two.Models
{
    public class Events
    {
        public int EventId { get; set; }
        public string? EventName { get; set; }
        public int EventDate { get; set; }
        public string? EventLocation { get; set; }
        public string? EventDescription { get; set; }

        //foreign keys
        public int OrganiserId { get; set; }
        public int ParticipantId { get; set; }
    }
}
