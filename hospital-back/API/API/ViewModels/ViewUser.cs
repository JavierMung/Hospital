namespace API.ViewModels
{
    public record ViewUser(string Username, string Password, int IdTrabajador, string Email);
    public record ViewUserLogin(string Username, string Password);
    public record ViewUserPasswordToken(string Token, string Username, string Password);
    public record ViewUserReqPassword(string Email);
    public record ViewUserToken(string Username, string Token);

}
