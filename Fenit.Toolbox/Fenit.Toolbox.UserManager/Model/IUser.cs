using System;

namespace Fenit.Toolbox.UserManager.Model
{
    public interface IUser
    {
        int Id { get; set; }
        string Imie { get; set; }
        string Nazwisko { get; set; }
        bool IsDeleted { get; set; }
        bool IsBlock { get; set; }
        int Rola { get; set; }
        DateTime AddDate { get; set; }
        DateTime? DeleteDate { get; set; }
        DateTime EditDate { get; set; }
        string Login { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        string Salt { get; set; }
        DateTime? LastPasswordFailureDate { get; set; }
        int PasswordFailureCount { get; set; }
    }
}