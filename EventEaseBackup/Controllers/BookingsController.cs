using Event_Ease_2026_Ntsika_Nkonki.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class BookingsController : Controller
{
    private readonly ApplicationDbContext _context;

    public BookingsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Bookings
    public async Task<IActionResult> Index(string searchString)
    {
        ViewData["CurrentFilter"] = searchString;

        var bookings = _context.Bookings
            .Include(b => b.Event)
            .Include(b => b.Venue)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            bookings = bookings.Where(b =>
                b.BookingId.ToString().Contains(searchString) ||
                b.Event.Name.Contains(searchString));
        }

        return View(await bookings.ToListAsync());
    }

    // GET: Bookings/Create
    public IActionResult Create()
    {
        ViewBag.VenueId = new SelectList(_context.Venues, "VenueId", "Name");
        ViewBag.EventId = new SelectList(_context.Events, "EventId", "Name");
        return View();
    }

    // POST: Bookings/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Booking booking)
    {
        if (ModelState.IsValid)
        {
            // Capacity check
            var venue = await _context.Venues.FindAsync(booking.VenueId);
            if (venue != null)
            {
                var bookedSeats = _context.Bookings
                    .Where(b => b.VenueId == booking.VenueId)
                    .Sum(b => b.SeatsBooked);

                if (bookedSeats + booking.SeatsBooked > venue.Capacity)
                {
                    ModelState.AddModelError("SeatsBooked", "Booking exceeds venue capacity.");
                    ViewBag.VenueId = new SelectList(_context.Venues, "VenueId", "Name", booking.VenueId);
                    ViewBag.EventId = new SelectList(_context.Events, "EventId", "Name", booking.EventId);
                    return View(booking);
                }
            }

            // Double booking check (same venue + same date/time)
            var existingBooking = _context.Bookings
                .FirstOrDefault(b => b.VenueId == booking.VenueId && b.BookingDate == booking.BookingDate);

            if (existingBooking != null)
            {
                ModelState.AddModelError("BookingDate", "This venue is already booked for that date/time.");
                ViewBag.VenueId = new SelectList(_context.Venues, "VenueId", "Name", booking.VenueId);
                ViewBag.EventId = new SelectList(_context.Events, "EventId", "Name", booking.EventId);
                return View(booking);
            }

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            TempData["NewBookingId"] = booking.BookingId;
            return RedirectToAction(nameof(Index));
        }

        ViewBag.VenueId = new SelectList(_context.Venues, "VenueId", "Name", booking.VenueId);
        ViewBag.EventId = new SelectList(_context.Events, "EventId", "Name", booking.EventId);
        return View(booking);
    }
}

