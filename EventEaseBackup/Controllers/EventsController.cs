using Event_Ease_2026_Ntsika_Nkonki.Models;
using Event_Ease_2026_Ntsika_Nkonki.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class EventsController : Controller
{
    private readonly ApplicationDbContext _context;

    public EventsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Events (with filtering)
    public async Task<IActionResult> Index(int? eventTypeId, DateTime? startDate, DateTime? endDate, bool? onlyAvailable)
    {
        ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "Name");

        var events = _context.Events
            .Include(e => e.Venue)
            .Include(e => e.EventType)
            .Include(e => e.Bookings)
            .AsQueryable();

        if (eventTypeId.HasValue)
            events = events.Where(e => e.EventTypeId == eventTypeId);

        if (startDate.HasValue)
            events = events.Where(e => e.Date >= startDate);

        if (endDate.HasValue)
            events = events.Where(e => e.Date <= endDate);

        if (onlyAvailable.HasValue && onlyAvailable.Value)
            events = events.Where(e => e.Venue.Capacity > e.Bookings.Sum(b => b.SeatsBooked));

        return View(await events.ToListAsync());
    }

    // GET: Events/Create
    [HttpGet]
    public IActionResult Create()
    {
        ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "Name");
        ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name");
        return View();
    }

    // POST: Events/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Event @event, IFormFile imageFile, [FromServices] BlobService blobService)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                using var stream = imageFile.OpenReadStream();
                @event.ImageUrl = await blobService.UploadFileAsync(stream, imageFile.FileName);
            }

            _context.Events.Add(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "Name", @event.VenueId);
        ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name", @event.EventTypeId);
        return View(@event);
    }

    // GET: Events/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var @event = await _context.Events
            .Include(e => e.Venue)
            .Include(e => e.EventType)
            .FirstOrDefaultAsync(m => m.EventId == id);

        if (@event == null) return NotFound();

        return View(@event);
    }

    // GET: Events/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var @event = await _context.Events.FindAsync(id);
        if (@event == null) return NotFound();

        ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "Name", @event.VenueId);
        ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name", @event.EventTypeId);
        return View(@event);
    }

    // POST: Events/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Event @event, IFormFile imageFile, [FromServices] BlobService blobService)
    {
        if (id != @event.EventId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    using var stream = imageFile.OpenReadStream();
                    @event.ImageUrl = await blobService.UploadFileAsync(stream, imageFile.FileName);
                }

                _context.Update(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Events.Any(e => e.EventId == @event.EventId))
                    return NotFound();
                else
                    throw;
            }
        }

        ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "Name", @event.VenueId);
        ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name", @event.EventTypeId);
        return View(@event);
    }

    // GET: Events/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var @event = await _context.Events
            .Include(e => e.Venue)
            .Include(e => e.EventType)
            .FirstOrDefaultAsync(m => m.EventId == id);

        if (@event == null) return NotFound();

        return View(@event);
    }

    // POST: Events/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var @event = await _context.Events
            .Include(e => e.Bookings)
            .FirstOrDefaultAsync(e => e.EventId == id);

        if (@event == null) return NotFound();

        // Restrict deletion if event has active bookings
        if (@event.Bookings.Any())
        {
            ModelState.AddModelError("", "Cannot delete an event with active bookings.");
            return View(@event);
        }

        _context.Events.Remove(@event);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
