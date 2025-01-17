
using PersonalExpenseTracker.Model;
using PersonalExpenseTracker.Model.Dto;
using PersonalExpenseTracker.Base;

namespace PersonalExpenseTracker.Components.Pages
{
    public partial class AddTransaction
    {
        private List<Transaction>? transactions { get; set; }
        public bool isError = false;

        #region OnInitialize
        protected override async Task OnInitializedAsync()
        {
            await GetAllTransaction();
            await GetAllTags();
            StateHasChanged();
        }
        #endregion

        #region GetAllTransaction
        private async Task GetAllTransaction()
        {
            var response = UserTransaction.GetAllTransaction();

            if (response == null)
            {
                return;
            }

            transactions = response;
            filteredTransactions = transactions;
        }
        #endregion

        #region Add Transaction
        private bool IsCreateButtonDisabled =>
        string.IsNullOrEmpty(createTransaction.Title) ||
        string.IsNullOrEmpty(createTransaction.TransactionDate.ToString()) ||
        string.IsNullOrEmpty(createTransaction.TransactionType.ToString()) ||
        string.IsNullOrEmpty(createTransaction.Remarks);

        private bool IsCreateModalOpen { get; set; }
        private bool IsCredit = false;
        private string ModalTitle = "Transaction";
        private string ModalDescription = "Transaction";
        private CreateTransactionDto createTransaction { get; set; } = new();

        private void OpenTransactionRegister(bool isCredit = false)
        {
            isError = false;
            IsCreateModalOpen = true;
            IsCredit = isCredit;

            createTransaction = new CreateTransactionDto
            {
                TransactionType = isCredit ? (int)TransactionType.Credit : (int)TransactionType.Debt
            };

            ModalTitle = isCredit ? "Credit Transaction" : "Debit Transaction";
            ModalDescription = "Add a new" + (isCredit ? "credit" : "debit") + "transaction";

            StateHasChanged();
        }

        private async Task AddRegisterTransaction(bool isclosed)
        {
            if (isclosed)
            {
                IsCreateModalOpen = false;
                return;
            }

            try
            {
                createTransaction.TransactionType = IsCredit ? (int) TransactionType.Credit : (int)TransactionType.Debit;
                var result = await UserTransaction.AddTransaction(createTransaction);

                if (result == false)
                {
                    isError = true;
                    return;
                }
                isError = false;

                IsCreateModalOpen = false;
                StateHasChanged();

            }
            catch (Exception ex)
            {
                throw new Exception("");
            }
        }
        #endregion

        #region GetAll Tags
        private List<Tag>? Tags { get; set; }
        private async Task GetAllTags()
        {
            var response = UserTag.GetAllTagUseByOther();

            if (response is null)
            {
                return;
            }

            Tags = response;

            StateHasChanged();
        }
        #endregion

        #region Delete
        private bool IsDeleteModalOpen { get; set; }

        private Transaction DeleteTransaction { get; set; } = new();

        private async Task OpenDebtDeleteModal(Guid Id)
        {
            var response = UserTransaction.TransactionGetById(Id);

            if (response is null)
            {
                // SnackbarService.ShowSnackbar(response?.Message ?? Constants.Message.ExceptionMessage, Severity.Error, Variant.Outlined);
                return;
            }

            DeleteTransaction = response;

            IsDeleteModalOpen = true;

            StateHasChanged();
        }

        private async Task DeleteTrans(bool isActive)
        {
            try
            {
                UserTransaction.ActiveDeactive(DeleteTransaction.Id, isActive);

                IsDeleteModalOpen = false;
            }
            catch (Exception ex)
            {
                //SnackbarService.ShowSnackbar(ex.Message, Severity.Error, Variant.Outlined);
            }
        }
        #endregion
    }
}