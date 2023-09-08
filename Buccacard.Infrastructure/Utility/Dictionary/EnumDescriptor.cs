using System;

namespace HermesApp.Infrastructure.Dictionary
{
    public class EnumDisplayNameAttribute: Attribute
    {
        public string DisplayName { get; set; }
    }

    public static class EnumExtensions
    {
        public static string DisplayName(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            var attribute = Attribute.GetCustomAttribute(field, typeof(EnumDisplayNameAttribute)) as EnumDisplayNameAttribute;

            return attribute == null ? value.ToString() : attribute.DisplayName;
        }
    }
}
