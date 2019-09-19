namespace Fenit.Toolbox.UserManager.ViewModel
{
    public interface IUserViewModel
    {
        int Id { get; set; }
        string Login { get; set; }
        string Imie { get; set; }
        string Nazwisko { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        string ConfirmPassword { get; set; }
        int Rola { get; set; }
        bool IsActive { get; set; }
    }

    public interface IUserRestartPasswordViewModel
    {
        int Id { get; set; }
        string Password { get; set; }
        string ConfirmPassword { get; set; }
        string OldPassword { get; set; }
    }
}