using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Events.Data;
using ThAmCo.Events.Models;

namespace ThAmCo.Events.Models
{
    public class EventViewModel
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Title"), Required]
        public string Title { get; set; }

        [Display(Name = "Date"), DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true), Required]
        public DateTime Date { get; set; }

        [Display(Name = "Duration")]
        public TimeSpan? Duration { get; set; }

        [Display(Name = "Type"), Required]
        public string Type { get; set; }

        [Display(Name = "Venue")]
        public string Venue { get; set; }

        [Display(Name = "Reservation Ref.")]
        public string VenueReference { get; set; }

        [Display(Name = "Venue")]
        public ReservationGetDTO VenueData { get; set; }

        [Display(Name = "Total Guests")]
        public int TotalGuests { get; set; }

        [Display(Name = "All Guests")]
        public List<GuestBookingViewModel> AllGuests { get; set; }

        [Display(Name = "Total Staff")]
        public int TotalStaff { get; set; }

        [Display(Name = "All Staff")]
        public List<StaffBookingViewModel> AllStaff { get; set; }

        [Display(Name = "Has First Aider")]
        public string HasFirstAider { get; set; }

        [Display(Name = "Has Enough Staff")]
        public string HasEnoughStaff { get; set; }

        [Display(Name = "Has Food Booked")]
        public string HasFoodBooked { get; set; }

        [Display(Name = "Food Menu")]
        public int? FoodMenu { get; set; }

        [Display(Name = "Food Menu Data")]
        public List<FoodMenuDTO> FoodMenuData { get; set; }

        [Display(Name = "Food Menu Cost")]
        public string TotalFoodMenuCost { get; set; }

        [Display(Name = "Venue Cost")]
        public string TotalVenueCost { get; set; }

        [Display(Name = "Event Cost")]
        public double TotalEventCost { get; set; }
    }
}
