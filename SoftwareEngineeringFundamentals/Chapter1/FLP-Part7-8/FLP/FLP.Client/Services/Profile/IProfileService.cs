using FLP.Shared.Models;

namespace FLP.Client.Services.Profile
{
    public interface IProfileService
    {
        Task<Status> CreateProfile(ProfileDto profileDto);
        Task SaveProfile(ProfileDto profileDto, string email = "");
        Task<ProfileDto> GetUserProfile(string email = "");
    }
}
