using System;
using System.ComponentModel;

namespace Fenit.Toolbox.Core.Extension
{
    public static class EnumExtension
    {
        public static string ToName(this System.Enum value)
        {
            var attribute = value.GetAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static T GetAttribute<T>(this System.Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return (T) attributes[0];
        }
    }
}