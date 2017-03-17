using Microsoft.EntityFrameworkCore;

namespace CMWebAPI.Models
{
    public partial class CMContext : DbContext
    {
        public CMContext(DbContextOptions<CMContext> options) 
            : base (options)
        {

        }
        public DbSet<CMApplication> CMApplication { get; set; }
        public DbSet<ApplicationFromDBView> ApplicationFromDBView { get; set; }        
    }
}
