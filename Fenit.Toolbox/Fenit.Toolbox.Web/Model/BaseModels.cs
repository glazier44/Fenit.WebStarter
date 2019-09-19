using System;
using System.ComponentModel;

namespace Fenit.Toolbox.Web.Model
{
    public abstract class BaseModel
    {
        public int Id { get; set; }
    }

    public abstract class BaseNameModel : BaseModel
    {
        [DisplayName("Nazwa")] public string Name { get; set; }

        public string NameHtml => string.IsNullOrEmpty(Name) ? string.Empty : Name.Replace(" ", "_");

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? string.Empty : Name;
        }
    }

    public abstract class BaseIsDeletedModel : BaseNameModel
    {
        public bool IsDeleted { get; set; }
    }

    public abstract class BaseDTeModel : BaseIsDeletedModel
    {
        public DateTime CreateDate { get; set; }
    }

    public abstract class BaseCanEditModel : BaseDTeModel
    {
        public bool IsEdited { get; set; }
    }
}