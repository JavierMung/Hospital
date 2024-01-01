namespace API.ViewModels
{
    public record ViewUser(string Username, string Password, int IdTrabajador, string Email);
    public record ViewUserLogin(string Username, string Password);
    public record ViewUserResetPassword(string Email);
    public record ViewUserDelete(string Token, string Username, string Password);
    public record ViewUserCreate(ViewUser Usuario, string Token);
    public record ViewUserResponseToken(string Username, string Token);

}
