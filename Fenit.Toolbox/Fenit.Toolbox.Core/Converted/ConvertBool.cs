
namespace Fenit.Toolbox.Core.Converted
{
    public static class ConvertBool
    {
        public static string ToString(bool b)
        {
            return b ? "Tak" : "Nie";
        }

        public static bool ToBool(string s)
        {
            return s.Equals("Tak");
        }
    }
}

