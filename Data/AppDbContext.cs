using Microsoft.EntityFrameworkCore;
using NgoDonationManagement.Models;

namespace NgoDonationManagement.Data;
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Donation> Donations => Set<Donation>();
}
