using System.Collections.Generic;
using System.Linq;
using document_api.V1.Domain;

namespace document_api.V1.Factory
{
    public abstract class AbstractTransactionFactory
    {
        public abstract Transaction FromUhTransaction(UhTransaction transaction);

        public List<Transaction> FromUhTransaction(IEnumerable<UhTransaction> result)
        {
            return result.Select(FromUhTransaction).ToList();
        }
    }
}
