using PersonalExpenseTracker.Abstraction;
using PersonalExpenseTracker.Model;
using PersonalExpenseTracker.Model.Dto;
using PersonalExpenseTracker.Model.Exception;
using PersonalExpenseTracker.Services.Interface;
using PersonalExpenseTracker.Base;
using Blazored.Toast.Services;
using Microsoft.JSInterop;

namespace PersonalExpenseTracker.Services
{
    public class TransactionService : UserBase<Transaction>, ITransaction
    {
        private List<Transaction> _transactions;
        private readonly IToastService _toastService;
        private readonly IJSRuntime JS;

        public TransactionService(IToastService toastService,IJSRuntime jS)  : base("Transaction.json")
        {
            _transactions = LoadItems();
            _toastService = toastService;
            JS = jS;
        }

        public void ActiveDeactive(Guid Id, bool isActive)
        {
            var success = UpdateItem(t => t.Id == Id, t => t.IsActive = isActive);
        }

        public async Task<bool> AddTransaction(CreateTransactionDto createTransaction)
        {
            try
            {
                if (createTransaction.TransactionType == (int)TransactionType.Debit && await CurrentBalance() < createTransaction.TransactionAmount)
                {
                    return false;
                }

                var modelTransaction = new Transaction()
                {
                    Id = Guid.NewGuid(),
                    Title = createTransaction.Title,
                    TransactionAmount = createTransaction.TransactionAmount,
                    TransactionDate = DateTime.Now,
                    IsActive = true,
                    Remarks = createTransaction.Remarks,
                    TagId = createTransaction.TagId,
                    TransactionType = createTransaction.TransactionType,
                };

                _transactions.Add(modelTransaction);

                SaveItems(_transactions);
                return true;
            }
            catch (Exception ex)
            {
                throw new NotFoundException("some this is wrong");
                return false;
            }
        }

        public async Task<Decimal> CurrentBalance()
        {
            var transaction = GetAllTransaction();

            var totalCredit = transaction.Where(t => t.TransactionType == 4)           
                             .Sum(t => t.TransactionAmount);

            var totalDebit = transaction.Where(t => t.TransactionType == 5)
                            .Sum(t => t.TransactionAmount);

            var totalDebt = transaction.Where(t => t.TransactionType == 6).Sum(t => t.TransactionAmount);

            var totalDebtRepaid = transaction.Where(t => t.TransactionType == 7).Sum(t => t.TransactionAmount);

            var sumofTransaction = totalCredit - totalDebit;

            var currentBalance = sumofTransaction + totalDebt - totalDebtRepaid;

            return currentBalance;
        }

        public List<Transaction> GetAllTransaction()
        {
           return _transactions.Where(t => t.IsActive).OrderByDescending(t => t.Id).ToList();
        }

        public async Task<List<Transaction>> HighestTransaction()
        {
            var transaction =  GetAllTransaction();
            return transaction.OrderByDescending(t => t.TransactionAmount).Take(5).ToList();
        }

        public async Task<List<Transaction>> SearchUser(FilterDto filterDto)
        {
            try
            {
                var query = _transactions.AsQueryable();

                if (!string.IsNullOrWhiteSpace(filterDto.Title))
                {
                    query = query.Where(t => t.Title != null &&
                                    t.Title.Contains(filterDto.Title, StringComparison.OrdinalIgnoreCase));
                }

                if (filterDto.TransactionDate.HasValue)
                {
                    query = query.Where(t => t.TransactionDate.HasValue && t.TransactionDate.Value.Date == filterDto.TransactionDate.Value.Date);
                }

                if(filterDto.StartDate.HasValue && filterDto.EndDate.HasValue)
                {
                    query = query.Where(t => t.TransactionDate.HasValue && t.TransactionDate.Value.Date >= filterDto.StartDate.Value.Date &&
                                        t.TransactionDate.Value.Date <= filterDto.EndDate.Value.Date);
                }

                var result = query.ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new NotFoundException("An error occurred while searching for the user.");
            }
        }

        public Transaction TransactionGetById(Guid Id)
        {
            return _transactions.FirstOrDefault(t => t.Id == Id);
        }

        public async Task UpdateTransaction(UpdateTransactionDto updateTransactionDto)
        {
            UpdateItem(t => t.Id == updateTransactionDto.Id, t =>
            {
                t.Title = updateTransactionDto.Title;
                t.TransactionAmount = updateTransactionDto.TransactionAmount;
                t.TransactionDate = updateTransactionDto.TransactionDate;
                t.TransactionType = updateTransactionDto.TransactionType;
                t.Remarks = updateTransactionDto.Remarks;
            });
        }
    }
}
