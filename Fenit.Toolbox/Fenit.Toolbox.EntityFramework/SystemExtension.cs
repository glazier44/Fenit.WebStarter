using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace Fenit.Toolbox.EntityFramework
{
    public static class SystemExtension
    {
        public static void CatchDbEntityValidationException(Exception ex, string source)
        {
            var e = ex as DbEntityValidationException;
            if (e != null)
            {
                var result = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    result.Append(
                        $"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");
                    result.Append(Environment.NewLine);
                    result.Append(
                        eve.ValidationErrors.Select(
                            ve => $"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\""));
                    result.Append(Environment.NewLine);
                }

                // LoggerManager.Log(source, result.ToString());
            }
        }
    }
}