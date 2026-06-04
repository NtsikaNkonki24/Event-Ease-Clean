using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;


namespace Event_Ease_2026_Ntsika_Nkonki.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        public int EventId { get; set; }

        [ValidateNever]
        public Event Event { get; set; }

        public int VenueId { get; set; }

        [ValidateNever]
        public Venue Venue { get; set; }

        public string CustomerName { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime BookingDate { get; set; }

        public int SeatsBooked { get; set; }
    }
}
