
using System.Collections.Generic;
using System.Linq;
using document_api.V1.Domain;
using document_api.V1.Factory;
using document_api.V1.Infrastructure;

namespace document_api.V1.Gateways
{
    public class TransactionsGateway : ITransactionsGateway
    {
        private readonly IUHContext _uhcontext;
        private readonly TransactionFactory _transactionFactory;

        public TransactionsGateway(IUHContext uhcontext)
        {
            _transactionFactory = new TransactionFactory();
            _uhcontext = uhcontext;
        }

        public List<Transaction> GetTransactionsByPropertyRef(string propertyRef)
        {
            var result = _uhcontext.UTransactions.Where(t => t.PropRef == propertyRef).ToList();
            return _transactionFactory.FromUhTransaction(result);
        }
    }
}
