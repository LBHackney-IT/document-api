using Microsoft.EntityFrameworkCore;
using document_api.V1.Domain;

namespace document_api.V1.Infrastructure
{
    public class UhContext : DbContext, IUHContext
    {
        public UhContext(DbContextOptions options) : document(options)
        {
        }

        public DbSet<UhTransaction> UTransactions { get; set; }
    }
}
 