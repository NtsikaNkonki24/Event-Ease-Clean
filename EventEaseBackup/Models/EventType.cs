
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Event_Ease_2026_Ntsika_Nkonki.Models
{
    public class EventType
    {
        public int EventTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}

