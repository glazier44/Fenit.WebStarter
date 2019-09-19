using Fenit.Toolbox.UserManager.Enum;
using Fenit.Toolbox.UserManager.Model;
using Newtonsoft.Json;

namespace Fenit.Toolbox.UserManager.ViewModel
{
    public class UserData
    {
        public UserData(IUser user)
        {
            Id = user.Id;
            UserName = $"{user.Imie} {user.Nazwisko}";
            UserRole = (UserRole) user.Rola;
            Login = user.Login;
            IsIsAuthenticated = true;
        }

        public UserData()
        {
            IsIsAuthenticated = false;
        }

        public UserData(string json)
        {
            if (string.IsNullOrEmpty(json))
                return;

            var u = JsonConvert.DeserializeObject<UserData>(json);
            Id = u.Id;
            UserName = u.UserName;
            UserRole = u.UserRole;
            Login = u.Login;
            IsIsAuthenticated = true;
        }

        public bool IsIsAuthenticated { get; private set; }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Login { get; set; }

        public UserRole UserRole { get; set; }

        public bool IsAdmin => UserRole <= UserRole.Admin;


        public bool IsRoot => UserRole <= UserRole.Root;

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}