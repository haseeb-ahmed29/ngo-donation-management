using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NgoDonationManagement.Data;
using NgoDonationManagement.Models;

namespace NgoDonationManagement.Controllers;
public class DonationsController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index(string? search, string? status)
    {
        var query = db.Donations.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(x => x.Name.Contains(search));
        if (!string.IsNullOrWhiteSpace(status)) query = query.Where(x => x.Status == status);
        ViewBag.Search = search; ViewBag.Status = status;
        return View(await query.OrderByDescending(x => x.CreatedAt).ToListAsync());
    }
    public IActionResult Create() => View(new Donation());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Donation item)
    { if (!ModelState.IsValid) return View(item); db.Donations.Add(item); await db.SaveChangesAsync(); TempData["Notice"] = "Record created successfully."; return RedirectToAction(nameof(Index)); }
    public async Task<IActionResult> Edit(int? id) => id is null ? NotFound() : (await db.Donations.FindAsync(id) is Donation item ? View(item) : NotFound());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Donation item)
    { if (id != item.Id) return NotFound(); if (!ModelState.IsValid) return View(item); db.Update(item); await db.SaveChangesAsync(); TempData["Notice"] = "Record updated successfully."; return RedirectToAction(nameof(Index)); }
    public async Task<IActionResult> Delete(int? id) => id is null ? NotFound() : (await db.Donations.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==id) is Donation item ? View(item) : NotFound());
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id) { var item = await db.Donations.FindAsync(id); if (item is not null) { db.Donations.Remove(item); await db.SaveChangesAsync(); } return RedirectToAction(nameof(Index)); }
}
