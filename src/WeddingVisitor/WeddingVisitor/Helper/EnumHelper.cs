using System;
using System.ComponentModel;
using System.Reflection;

namespace WeddingVisitor.Helper
{
    public static class EnumHelper
    {
        public static string GetDescription(this object source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());
            var att = (DescriptionAttribute)fi.GetCustomAttribute(typeof(DescriptionAttribute));
            if (att != null)
            {
                return att.Description;
            }
            else
            {
                return source.ToString();
            }
        }

        public static bool TryParseFromDescription<TEnum>(this string description, out TEnum @enum)
            where TEnum : struct, IConvertible
        {
            @enum = default;
            try
            {

                var type = typeof(TEnum).GetType();
                foreach (TEnum value in type.GetEnumValues())
                {
                    if (string.Equals(value.GetDescription(), description))
                    {
                        @enum = value;
                        return true;
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
