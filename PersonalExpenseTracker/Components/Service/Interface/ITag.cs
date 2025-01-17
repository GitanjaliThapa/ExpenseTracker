using PersonalExpenseTracker.Model;
using PersonalExpenseTracker.Model.Dto;

namespace PersonalExpenseTracker.Services.Interface
{
    public interface ITag
    {
        List<Tag> GetAllTag();

        Tag TagGetById(Guid Id);

        Task AddTag(CreateTagDto tag);

        void ActiveDeactive(Guid Id, bool isActive);

        Task UpdateTag(UpdateTagDto tag);

        List<Tag> GetAllTagUseByOther();
    }
}