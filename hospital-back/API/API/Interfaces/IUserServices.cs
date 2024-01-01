using API.ViewModels;

namespace API.Interfaces
{
    public interface IUserServices
    {
        public Task<Result<ViewUserResponseToken>> Login(ViewUserLogin user);
        public Task<Result<ViewUserResponseToken>> CreateUser(ViewUserCreate user);
        public Task<Result<ViewUser>> DeleteUser(ViewUserDelete user);
        public Task<Result<ViewUserResetPassword>> ResetPassword(ViewUserResetPassword user);
        public Task<Result<ViewUserResponseToken>> ValidateToken(ViewUserCreate user);

    }
}
