namespace RecipeMvc.Soft.Data;

public class DbInitializer
{
    private readonly ApplicationDbContext _context;

    public DbInitializer(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task Initialize()
    {
        _context.Database.EnsureCreated();
        return Task.CompletedTask;
    }
}
