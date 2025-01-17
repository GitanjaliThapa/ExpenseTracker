using PersonalExpenseTracker.Model;
using PersonalExpenseTracker.Model.Dto;

namespace PersonalExpenseTracker.Services.Interface
{
    public interface IDebt
    {
        Task AddDebt(CreateDebtDto debt);

        List<Debt> GetAllDebt();

        Debt GetById(Guid id);

        void ActiveDeactive(Guid Id);

        Task UpdateDebt(UpdateDebtDto debt);

        Task<bool> RepayDebt(RepayDebtDto debt);

        Task<List<Debt>> RemainingDebt();
    }
}