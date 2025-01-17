using PersonalExpenseTracker.Model;
using PersonalExpenseTracker.Model.Dto;

namespace PersonalExpenseTracker.Services.Interface
{
    public interface ITransaction
    {
        List<Transaction> GetAllTransaction();

        Transaction TransactionGetById(Guid Id);

        Task<bool> AddTransaction(CreateTransactionDto createTransaction);

        void ActiveDeactive(Guid Id, bool isActive);

        Task<List<Transaction>> HighestTransaction();

        Task<Decimal> CurrentBalance();

        Task<List<Transaction>> SearchUser(FilterDto filter);

        Task UpdateTransaction(UpdateTransactionDto updateTransactionDto);
    }
}