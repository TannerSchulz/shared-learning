using FLP.Shared.Models;

namespace FLP.Client.Services.UserAccount
{
    public interface IUserAccountService
    {
        Task<Status> Register(RegisterModel model);
    }
}
