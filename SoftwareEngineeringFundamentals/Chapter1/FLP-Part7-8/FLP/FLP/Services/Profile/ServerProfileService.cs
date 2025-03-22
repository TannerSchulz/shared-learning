using FLP.Client.Services.Profile;
using FLP.Data;
using FLP.Shared.Models;

namespace FLP.Services.Profile
{
    internal sealed class ServerProfileService(UnitOfWork _unitOfWork) : IProfileService
    {
        public Task<Status> CreateProfile(ProfileDto profileDto)
        {
            throw new NotImplementedException();
        }

        public async Task<ProfileDto> GetUserProfile(string email = "")
        {
            var result = new ProfileDto();
            try
            {
                var profile = _unitOfWork.Profile.Get(o => o.ApplicationUser.Email == email, includeProperties: "ApplicationUser").First();
                result = new()
                {
                    UserName = profile.ApplicationUser.UserName,
                    DisplayName = profile.DisplayName,
                    Biography = profile.Biography,
                    ProfileImageUrl = profile.ProfileImageUrl
                };
            }
            catch(Exception ex)
            {
                var errorMessage = ex.Message;
            }
            return result;
        }

        public async Task SaveProfile(ProfileDto profileDto, string email = "")
        {
            try
            {
                var profile = _unitOfWork.Profile.Get(o => o.ApplicationUser.Email == email, includeProperties: "ApplicationUser").First();
                profile.DisplayName = profileDto.DisplayName;
                profile.Biography = profileDto.Biography;
                profile.ProfileImageUrl = profileDto.ProfileImageUrl;
                _unitOfWork.Save();
            }
            catch(Exception ex)
            {
                var newProfile = new Data.Entities.Profile
                {
                    ApplicationUser = _unitOfWork.ApplicationUser.Get(o => o.Email == email).First(),
                    DisplayName = profileDto.DisplayName,
                    Biography = profileDto.Biography,
                    ProfileImageUrl = profileDto.ProfileImageUrl
                };
                _unitOfWork.Profile.Insert(newProfile);
                _unitOfWork.Save();
            }
        }
    }
}
