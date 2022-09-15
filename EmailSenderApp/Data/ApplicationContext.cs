using EmailSenderApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace EmailSenderApplication.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Email> Email { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }
    }
}
