using Blazored.Toast.Services;
using PersonalExpenseTracker.Abstraction;
using PersonalExpenseTracker.Base;
using PersonalExpenseTracker.Model;
using PersonalExpenseTracker.Model.Dto;
using PersonalExpenseTracker.Model.Exception;
using PersonalExpenseTracker.Services.Interface;

namespace PersonalExpenseTracker.Services
{
    public class DebtService : UserBase<Debt>, IDebt
    {
        private List<Debt> _debtList;

        private readonly ITransaction _transaction;
        private readonly IToastService _toastService;
        public DebtService(ITransaction transaction,IToastService toastService) : base("Debt.json")
        {
            _debtList = LoadItems();
            _transaction = transaction;
            _toastService = toastService;
        }

        public void ActiveDeactive(Guid Id)
        {
            UpdateItem(t => t.Id == Id, t =>
            {
                t.IsActive = false;
            });
        }

        public async Task AddDebt(CreateDebtDto debt)
        {
            try
            {
                var debtModel = new Debt()
                {
                    Id = Guid.NewGuid(),
                    DebtSource = debt.DebtSource,
                    DebtAmount = debt.DebtAmount,
                    DebtDate = DateTime.Now,
                    IsActive = true,
                    IsCleard = false,
                    DueDate = debt.DueDate, 
                    TagId = debt.TagId,
                };

                _debtList.Add(debtModel);
                SaveItems(_debtList);

                var transaction = new CreateTransactionDto
                {
                    Title = $"debt Add : {debt.DebtSource}",
                    TransactionAmount = debt.DebtAmount,
                    TransactionDate = DateTime.Now,
                    TransactionType = (int)TransactionType.Debt,
                    TagId = debt.TagId,
                    Remarks = "Add Debts "
                };

                await _transaction.AddTransaction(transaction);
            }
            catch (Exception ex) 
            {
                throw new NotFoundException("some this is wrong");
            }
        }

        public List<Debt> GetAllDebt()
        {
           return _debtList.Where(t => t.IsActive).ToList();
        }

        public Debt GetById(Guid id)
        {
            return _debtList.FirstOrDefault(d => d.Id == id);
        }

        public async Task UpdateDebt(UpdateDebtDto debt)
        {
            UpdateItem(t => t.Id == debt.Id, t =>
            {
                t.DebtSource = debt.DebtSource;
                t.DebtDate = debt.DebtDate;
                t.DebtAmount = debt.DebtAmount;
            });

            var transaction = new UpdateTransactionDto
            {
                Title = $"debt Add : {debt.DebtSource}",
                TransactionAmount = debt.DebtAmount,
                TransactionDate = DateTime.Now,
                TransactionType = (int)TransactionType.Debt,
                Remarks = "Add Debts "
            };

            await _transaction.UpdateTransaction(transaction);
        }

        public async Task<bool> RepayDebt(RepayDebtDto debt)
        {
            if (await _transaction.CurrentBalance() >= debt.DebtAmount)
            {
                UpdateItem(t => t.Id == debt.Id, t =>
                {
                    t.IsCleard = true;
                });

                var transaction = new CreateTransactionDto
                {
                    Title = $"debt Repaid : {debt.DebtSource}",
                    TransactionAmount = debt.DebtAmount,
                    TransactionDate = DateTime.Now,
                    TransactionType = (int)TransactionType.DebtRepaid,
                    Remarks = "Debts Repaid"
                };

                await _transaction.AddTransaction(transaction);
            }
            else
            {
                return false;
            }
            return true;
        }

        public async Task<List<Debt>> RemainingDebt()
        {
           var pending = GetAllDebt();

            var pendingDebts = pending.Where(d => !d.IsCleard);

            return pendingDebts.OrderBy(t => t.DebtDate).Take(5).ToList();
        }
    }
}
