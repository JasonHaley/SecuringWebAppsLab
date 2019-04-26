using Microsoft.EntityFrameworkCore;

namespace SecureWebApp.Db
{
    public class LabDbContext : DbContext
    {
        public LabDbContext(DbContextOptions<LabDbContext> options)
            : base(options)
        {  }

        public DbQuery<AzureGroup> AzureGroups { get; set; }

    }
}
