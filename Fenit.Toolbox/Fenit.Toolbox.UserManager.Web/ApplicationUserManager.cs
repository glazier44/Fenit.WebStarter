using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;
using Fenit.Toolbox.Core.Answers;
using Fenit.Toolbox.UserManager.Enum;
using Fenit.Toolbox.UserManager.Helper;
using Fenit.Toolbox.UserManager.Model;
using Fenit.Toolbox.UserManager.ViewModel;

namespace Fenit.Toolbox.UserManager.Web
{
    public class ApplicationUserManager<TEntity, TDbContext>
        where TEntity : class, IUser, new()
        where TDbContext : DbContext, new()
    {
        protected Func<TDbContext> ContextFactory;

        public ApplicationUserManager()
        {
            Func<TDbContext> factory = () => new TDbContext();
            ContextFactory = factory;
        }

        public bool CheckAccess(UserRole role)
        {
            var user = GetUserData();
            var result = role >= user.UserRole;
            return result;
        }

        public Response ChangePasswordUser(IUserRestartPasswordViewModel userModel)
        {
            var result = new Response();
            using (var context = ContextFactory())
            {
                try
                {
                    SafeManager.HashString(userModel.Password, out var hash, out var salt);
                    var user = context.Set<TEntity>().FirstOrDefault(w => w.Id == userModel.Id && !w.IsDeleted);
                    if (user != null)
                    {
                        var hashPassword = SafeManager.GetHashString(userModel.OldPassword, user.Salt);
                        if (user.Password.Equals(hashPassword))
                        {
                            if (userModel.OldPassword.Equals(userModel.Password))
                            {
                                result.AddError("Hasło jest takie samo jak poprzednie");
                            }
                            else
                            {
                                user.Password = hash;
                                user.Salt = salt;
                                user.PasswordFailureCount = 0;
                                context.SaveChanges();
                                result.AddSucces("Hasło zostało zmienione.");
                            }
                        }
                        else
                        {
                            result.AddError("Złe hasło użytkownika");
                        }
                    }
                    else
                    {
                        result.AddError("Brak użytkownika");
                    }
                }
                catch (Exception e)
                {
                    //LoggerManager.Log(e);
                    //transaction.Rollback();
                    result.AddError("Bład zapisu");
                }
            }

            return result;
        }

        public Response ResetPasswordUser(IUserRestartPasswordViewModel userModel)
        {
            var result = new Response();
            using (var context = ContextFactory())
            {
                try
                {
                    var newPassword = SafeManager.CreatePassword(10);
                    SafeManager.HashString(newPassword, out var hash, out var salt);
                    var user = context.Set<TEntity>().FirstOrDefault(w => w.Id == userModel.Id && !w.IsDeleted);
                    if (user != null)
                    {
                        user.Password = hash;
                        user.Salt = salt;
                        user.PasswordFailureCount = 0;
                        context.SaveChanges();
                        result.AddSucces($"Hasło zostało zresetowane. Nowe to: {newPassword}");
                    }
                    else
                    {
                        result.AddError("Brak użytkownika");
                    }
                }
                catch (Exception e)
                {
                    //LoggerManager.Log(e);
                    //transaction.Rollback();
                    result.AddError("Bład zapisu");
                }
            }

            return result;
        }

        public Response DeleteUser(IUserViewModel userModel)
        {
            var result = new Response();
            using (var context = ContextFactory())
            {
                try
                {
                    var user = context.Set<TEntity>().FirstOrDefault(w => w.Id == userModel.Id && !w.IsDeleted);
                    if (user != null)
                    {
                        user.IsDeleted = true;
                        user.DeleteDate = DateTime.Now;

                        context.SaveChanges();

                        result.AddSucces($"Uzytkownik {user.Login} został skasowany");
                    }
                    else
                    {
                        result.AddError("Brak użytkownika");
                    }
                }
                catch (Exception e)
                {
                    //LoggerManager.Log(e);
                    //transaction.Rollback();
                    result.AddError("Bład zapisu");
                }
            }

            return result;
        }

        public Response BlockUser(IUserViewModel userModel)
        {
            var result = new Response();
            using (var context = ContextFactory())
            {
                try
                {
                    var user = context.Set<TEntity>().FirstOrDefault(w => w.Id == userModel.Id && !w.IsDeleted);
                    if (user != null)
                    {
                        user.IsBlock = !userModel.IsActive;
                        user.EditDate = DateTime.Now;
                        user.PasswordFailureCount = 0;
                        context.SaveChanges();

                        result.AddSucces(
                            $"Uzytkownik {user.Login} został {(userModel.IsActive ? "odblokowany" : "zablokowany")}");
                    }
                    else
                    {
                        result.AddError("Brak użytkownika");
                    }
                }
                catch (Exception e)
                {
                    //LoggerManager.Log(e);
                    //transaction.Rollback();
                    result.AddError("Bład zapisu");
                }
            }

            return result;
        }

        public Response EditUser(IUserViewModel userModel)
        {
            var result = new Response();
            using (var context = ContextFactory())
            {
                try
                {
                    if (
                        context.Set<TEntity>()
                            .Any(w => w.Login == userModel.Login && w.Id != userModel.Id && !w.IsDeleted))
                    {
                        result.AddError("Login nie jest unikalny");
                        return result;
                    }


                    var user = context.Set<TEntity>().FirstOrDefault(w => w.Id == userModel.Id && !w.IsDeleted);
                    if (user == null)
                    {
                        SafeManager.HashString(userModel.Password, out var hash, out var salt);

                        user = new TEntity
                        {
                            Nazwisko = userModel.Nazwisko,
                            Imie = userModel.Imie,
                            IsBlock = !userModel.IsActive,
                            Rola = userModel.Rola,
                            EditDate = DateTime.Now,
                            AddDate = DateTime.Now,
                            Email = userModel.Email,
                            Login = userModel.Login,
                            Password = hash,
                            Salt = salt
                        };
                        context.Set<TEntity>().Add(user);
                        result.AddSucces($"Użytkownik {userModel.Login} został dodany.");
                    }
                    else
                    {
                        user.Nazwisko = userModel.Nazwisko;
                        user.Imie = userModel.Imie;
                        user.IsBlock = !userModel.IsActive;
                        user.Rola = userModel.Rola;
                        user.EditDate = DateTime.Now;
                        user.Email = userModel.Email;
                        user.Login = userModel.Login;
                        result.AddSucces($"Użytkownik {userModel.Login} został zedytowany.");
                    }

                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    //LoggerManager.Log(e);
                    //transaction.Rollback();
                    result.AddError("Bład zapisu");
                }
            }

            return result;
        }

        public UserData GetUserData()
        {
            var cookie = HttpContext.Current.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
            if (cookie != null)
            {
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (ticket != null)
                {
                    var userData = new UserData(ticket.UserData);
                    return userData;
                }
            }

            return null;
        }

        public LoginResult SignInManager(string login, string password)
        {
            var result = new LoginResult();
            using (var context = ContextFactory())
            {
                var user = context.Set<TEntity>().FirstOrDefault(w => w.Login == login && !w.IsDeleted);

                if (user != null && !user.IsBlock)
                {
                    if (user.PasswordFailureCount > 5)
                    {
                        result.SignInStatus = SignInStatus.Block;
                        user.IsBlock = true;
                    }
                    else
                    {
                        var hashPassword = SafeManager.GetHashString(password, user.Salt);
                        if (user.Password.Equals(hashPassword))
                        {
                            SetAuthCookie(user);
                            user.PasswordFailureCount = 0;
                            result.SignInStatus = SignInStatus.Ok;
                        }
                        else
                        {
                            user.LastPasswordFailureDate = DateTime.Now;
                            user.PasswordFailureCount += 1;
                            result.SignInStatus = user.PasswordFailureCount > 5
                                ? SignInStatus.Block
                                : SignInStatus.Failure;
                        }
                    }
                }
                else
                {
                    result.SignInStatus = SignInStatus.Failure;
                }

                context.SaveChanges();
            }

            return result;
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        private static void SetAuthCookie(IUser user)
        {
            if (user == null) return;
            var userData = new UserData(user);
            SetAuthCookie(user.Login, true, userData);
        }

        private static void SetAuthCookie(string userName, bool createPersistentCookie, UserData userData)
        {
            var cookie = FormsAuthentication.GetAuthCookie(userName, createPersistentCookie);
            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            var newTicket = new FormsAuthenticationTicket(
                ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration,
                ticket.IsPersistent, userData.ToJson(), ticket.CookiePath
            );

            var encTicket = FormsAuthentication.Encrypt(newTicket);
            cookie.Value = encTicket;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}