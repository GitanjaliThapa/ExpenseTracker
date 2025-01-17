using PersonalExpenseTracker.Model;
using PersonalExpenseTracker.Model.Dto;

namespace PersonalExpenseTracker.Components.Pages
{
    public partial class AddDebt
    {
        private List<Debt>? debts { get; set; } = new();
        public bool isError = false;

        #region Oninitialize
        protected override async Task OnInitializedAsync()
        {
            await GetAllDebt();
            await GetAllTags();
            StateHasChanged();
        }
        #endregion

        #region GetAllDebt
        private async Task GetAllDebt()
        {
            var response = UserDebt.GetAllDebt();

            if (response is null)
            {
                return;
            }

            debts = response;
            StateHasChanged();
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

        #region AddDebt
        private bool IsCreateButtonDisabled =>
        string.IsNullOrEmpty(CreateDebtDto.DebtSource) ||
        string.IsNullOrEmpty(CreateDebtDto.DebtAmount.ToString());

        private CreateDebtDto CreateDebtDto { get; set; } = new();
        private bool IsCreateModalOpen { get; set; }
        private void OpenDebtRegister()
        {
            IsCreateModalOpen = true;
            CreateDebtDto = new CreateDebtDto();
            StateHasChanged();
        }

        private async Task AddRegisterDebt(bool isclosed)
        {
            if (isclosed)
            {
                IsCreateModalOpen = false;
                return;
            }

            try
            {
                var result = UserDebt.AddDebt(CreateDebtDto);

                if (result is null)
                {
                    return;
                }
                await GetAllDebt();
                IsCreateModalOpen = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                throw new Exception("");
            }
        }
        #endregion

        #region Delete
        private bool IsDeleteModalOpen { get; set; }

        private Debt DeleteDebt { get; set; } = new();

        private async Task OpenDebtDeleteModal(Guid Id)
        {
            var response = UserDebt.GetById(Id);

            if (response is null)
            {
                return;
            }

            DeleteDebt = response;

            IsDeleteModalOpen = true;
            await GetAllTags();
            StateHasChanged();
        }

        private async Task DeleteTag(bool isClosed)
        {
            if (isClosed)
            {
                IsDeleteModalOpen = false;
                return;
            }

            try
            {
                UserDebt.ActiveDeactive(DeleteDebt.Id);
                await GetAllDebt();
                IsDeleteModalOpen = false;
            }
            catch (Exception ex)
            {
                //SnackbarService.ShowSnackbar(ex.Message, Severity.Error, Variant.Outlined);
            }
        }
        #endregion

        #region Update Debts
        private bool IsUpdateModalOpen { get; set; }

        private UpdateDebtDto UpdateDebtDto { get; set; } = new();

        private Debt GetDebtDto { get; set; } = new();

        private bool IsDebtButtonDisabled =>
            string.IsNullOrEmpty(UpdateDebtDto.DebtSource) ||
            string.IsNullOrEmpty(UpdateDebtDto.DueDate.ToString()) ||
            string.IsNullOrEmpty(UpdateDebtDto.DebtAmount.ToString()) ||
            string.IsNullOrEmpty(UpdateDebtDto.Tag?.TagName);

        private async Task OpenUpdateModal(Guid debtId)
        {
            var response = UserDebt.GetById(debtId);

            if (response is null)
            {
                return;
            }

            GetDebtDto = response;

            UpdateDebtDto = new UpdateDebtDto()
            {
                Id = GetDebtDto.Id,
                DebtSource = GetDebtDto.DebtSource,
                DebtDate = GetDebtDto.DebtDate,
                DebtAmount = GetDebtDto.DebtAmount
            };

            OpenCloseEditModal();
            StateHasChanged();
        }

        private void OpenCloseEditModal()
        {
            IsUpdateModalOpen = !IsUpdateModalOpen;

            StateHasChanged();
        }

        private async Task UpdateTag(bool isClosed)
        {
            IsUpdateModalOpen = false;

            try
            {
                var result = UserDebt.UpdateDebt(UpdateDebtDto);
                await GetAllDebt();
                if (result is null)
                {
                    //SnackbarService.ShowSnackbar(result?.Message ?? Constants.Message.ExceptionMessage, Severity.Error, Variant.Outlined);
                    return;
                }
            }
            catch (Exception ex)
            {
                // SnackbarService.ShowSnackbar(ex.Message, Severity.Error, Variant.Outlined);
            }
        }
        #endregion

        #region Repay Debt

        private bool IsDebtRepayModalOpen { get; set; }

        private RepayDebtDto RepayDebtDto { get; set; } = new();

        private async Task OpenRepayDebtModal(Guid debtId)
        {
            isError = false;
            var response = UserDebt.GetById(debtId);

            if (response is null)
            {
                return;
            }

            GetDebtDto = response;

            RepayDebtDto = new RepayDebtDto()
            {
                Id = GetDebtDto.Id,
                DebtSource = GetDebtDto.DebtSource,
                DebtDate = GetDebtDto.DebtDate,
                DebtAmount = GetDebtDto.DebtAmount
            };

            OpenCloseRepayModal();
            StateHasChanged();
        }

        private void OpenCloseRepayModal()
        {
            IsDebtRepayModalOpen = !IsDebtRepayModalOpen;

            StateHasChanged();
        }

        private async Task RepayDebt(bool isClosed)
        {
            IsDebtRepayModalOpen = false;

            try
            {
                var result = await UserDebt.RepayDebt(RepayDebtDto);
                if (result == false)
                {
                    isError = true;
                    return;
                }
                isError = false;

                await GetAllDebt();

                debts.Where(_ => _.Id == RepayDebtDto.Id).FirstOrDefault().IsCleard = true;

                StateHasChanged();
            }
            catch (Exception ex)
            {
            }
        }

        #endregion
    }
}