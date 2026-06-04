using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Event_Ease_2026_Ntsika_Nkonki.Models
{
    public class Event
    {
        public int EventId { get; set; }

        [Required(ErrorMessage = "Event name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Event date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Event Date")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Venue is required")]
        public int? VenueId { get; set; }

        [ValidateNever]
        public Venue Venue { get; set; }

        [Required(ErrorMessage = "Event type is required")]
        public int? EventTypeId { get; set; }

        [ValidateNever]
        public EventType EventType { get; set; }

        public string? ImageUrl { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
