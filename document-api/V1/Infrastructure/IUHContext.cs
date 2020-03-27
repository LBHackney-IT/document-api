using Microsoft.EntityFrameworkCore;
using document_api.V1.Domain;

namespace document_api.V1.Infrastructure
{
    public interface IUHContext
    {
        DbSet<UhTransaction> UTransactions { get; set; }
    }
}
