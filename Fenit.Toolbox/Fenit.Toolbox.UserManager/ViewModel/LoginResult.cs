

using Fenit.Toolbox.Core.Answers;
using Fenit.Toolbox.UserManager.Enum;

namespace Fenit.Toolbox.UserManager.ViewModel
{
    public class LoginResult : Response
    {
        private SignInStatus _signInStatus;

        public SignInStatus SignInStatus
        {
            get => _signInStatus;
            set
            {
                _signInStatus = value;
                switch (value)
                {
                    case SignInStatus.Block:
                    {
                        Message = "zablokowany";
                        IsError = true;
                        break;
                    }
                    case SignInStatus.Failure:
                    {
                        Message = "Błąd";
                        IsError = true;
                        break;
                    }
                }
            }
        }
    }
}