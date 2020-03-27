using System.Collections.Generic;
using document_api.V1.Domain;

namespace document_api.V1.Gateways
{
    public interface ITransactionsGateway
    {
        List<Transaction> GetTransactionsByPropertyRef(string propertyRef);
    }
}
