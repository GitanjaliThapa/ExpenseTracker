using PersonalExpenseTracker.Model;
using PersonalExpenseTracker.Base;

namespace PersonalExpenseTracker.Components.Pages
{
    public partial class Dashboard
    {

        #region Oninilization

        protected override async Task OnInitializedAsync()
        {
            transactions = UserTransaction.GetAllTransaction() ?? new List<Transaction>();

            await Highest();
            await CurrentAmount();
            await PendingDebts();
            CalculateData();
        }
        #endregion

        #region Highest Top 5 

        private List<Transaction> transactions { get; set; } = [];
        private List<Transaction> highestTransactions { get; set; } = [];
        private async Task Highest()
        {
            var response = await UserTransaction.HighestTransaction();

            if (response is null)
            {
                return;
            }

            highestTransactions = response;
        }
        #endregion

        #region Current Balance
        private decimal CurrentBalance { get; set; }

        private async Task CurrentAmount()
        {
            var response = await UserTransaction.CurrentBalance();

            if (response <= 0)
            {
                return;
            }

            CurrentBalance = response;
        }
        #endregion

        #region PendingDebts

        private List<Debt>? RemainingDebt { get; set; } = [];
        private async Task PendingDebts()
        {
            var response = await UserDebt.RemainingDebt();

            if (response is null)
            {
                return;
            }

            RemainingDebt = response;
        }
        #endregion

        public int totalTransactions { get; private set; }
        public decimal totalAmount { get; private set; }
        public decimal totalInflows { get; private set; }
        public decimal totalOutflows { get; private set; }
        public decimal totalDebt { get; private set; }
        public decimal remainingDebt { get; private set; }
        public decimal highestInflow { get; private set; }
        public decimal highestOutflow { get; private set; }
        public decimal highestDebt { get; private set; }

        public void CalculateData()
        {
            totalTransactions = transactions.Count;
            totalAmount = transactions.Sum(t => t.TransactionAmount);

            totalInflows = transactions
                .Where(t => t.TransactionType == (int)TransactionType.Credit || t.TransactionType == (int)TransactionType.Debt)
                .Sum(t => t.TransactionAmount);

            totalOutflows = transactions
                .Where(t => t.TransactionType == (int)TransactionType.Debit || t.TransactionType == (int)TransactionType.DebtRepaid)
                .Sum(t => t.TransactionAmount);

            totalDebt = transactions
                .Where(t => t.TransactionType == (int)TransactionType.Debt).Sum(d => d.TransactionAmount);

            remainingDebt = RemainingDebt.Sum(d => d.DebtAmount);

            highestInflow = transactions
                .Where(t => t.TransactionType == (int)TransactionType.Credit || t.TransactionType == (int)TransactionType.Debt)
                .DefaultIfEmpty(new Transaction { TransactionAmount = 0 })
                .Max(t => t.TransactionAmount);
            highestOutflow = transactions
                .Where(t => t.TransactionType == (int)TransactionType.Debit || t.TransactionType == (int)TransactionType.DebtRepaid)
                .DefaultIfEmpty(new Transaction { TransactionAmount = 0 })
                .Max(t => t.TransactionAmount);

            highestDebt = transactions
                .Where(t => t.TransactionType == (int)TransactionType.Debt)
                .DefaultIfEmpty(new Transaction { TransactionAmount = 0 })
                .Max(d => d.TransactionAmount);
        }

        private DateTime? filterFromDate;
        private DateTime? filterToDate;

        private void ApplyDateFilter()
        {
            if (filterFromDate.HasValue && filterToDate.HasValue)
            {
                transactions = transactions
                    .Where(t => t.TransactionDate >= filterFromDate.Value && t.TransactionDate <= filterToDate.Value)
                    .ToList();
            }
            else
            {
                transactions = UserTransaction.GetAllTransaction() ?? new List<Transaction>();
            }
            CalculateData();
        }

        private void ResetDateFilter()
        {
            filterFromDate = null;
            filterToDate = null;
            transactions = UserTransaction.GetAllTransaction() ?? new List<Transaction>();

            CalculateData();
        }
    }
}