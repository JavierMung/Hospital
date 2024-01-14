using API.ViewModels;

namespace API.Interfaces
{
    public interface IUserServices
    {
        public Task<Result<ViewUserToken>> Login(ViewUserLogin user);
        public Task<Result<ViewUserToken>> CreateUser(ViewUser user);
        public Task<Result<ViewUserToken>> DeleteUser(ViewUserPasswordToken user);
        public Task<Result<ViewUserLogin>> ResetPassword(ViewUserPasswordToken user);
        public Task<Result<ViewUserReqPassword>> RequestResetPassword(ViewUserReqPassword user);
        public Task<Result<ViewUserToken>> ValidateToken(ViewUserToken user);
       // public Task<Result<ViewUserToken>> UpdateUser(ViewUserToken user);

    }
}
