using Event_Ease_2026_Ntsika_Nkonki.Models;
using Event_Ease_2026_Ntsika_Nkonki.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class VenuesController : Controller
{
    private readonly ApplicationDbContext _context;

    public VenuesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Venues
    public async Task<IActionResult> Index()
{
    try
    {
        var venues = await _context.Venues.ToListAsync();
        return View(venues);
    }
    catch (Exception ex)
    {
        return Content(ex.ToString());
    }
}
    // GET: Venues/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var venue = await _context.Venues.FirstOrDefaultAsync(m => m.VenueId == id);
        if (venue == null) return NotFound();
        return View(venue);
    }

    // GET: Venues/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Venues/Create (with optional image upload)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Venue venue, IFormFile imageFile, [FromServices] BlobService blobService)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                using var stream = imageFile.OpenReadStream();
                venue.ImageUrl = await blobService.UploadFileAsync(stream, imageFile.FileName);
            }

            _context.Add(venue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(venue);
    }

    // GET: Venues/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var venue = await _context.Venues.FindAsync(id);
        if (venue == null) return NotFound();
        return View(venue);
    }

    // POST: Venues/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Venue venue)
    {
        if (id != venue.VenueId) return NotFound();
        if (ModelState.IsValid)
        {
            _context.Update(venue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(venue);
    }

    // GET: Venues/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var venue = await _context.Venues.FirstOrDefaultAsync(m => m.VenueId == id);
        if (venue == null) return NotFound();
        return View(venue);
    }

    // POST: Venues/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var venue = await _context.Venues
            .Include(v => v.Events)
            .Include(v => v.Bookings)
            .FirstOrDefaultAsync(v => v.VenueId == id);

        if (venue == null) return NotFound();

        if (venue.Bookings.Any())
        {
            ModelState.AddModelError("", "Cannot delete a venue with active bookings.");
            return View(venue);
        }

        _context.Venues.Remove(venue);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

}
