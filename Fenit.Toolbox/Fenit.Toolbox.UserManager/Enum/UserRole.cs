using System;
using System.ComponentModel;

namespace Fenit.Toolbox.UserManager.Enum
{
    [Flags]
    public enum UserRole
    {
        [Description("Root")] Root = 100,
        [Description("Admin")] Admin = 200,
        [Description("Użytkownik")] User = 500,
    }
}
