using System;
using System.Linq;
using System.Xml.Linq;

namespace FMRookieScouter.Interface
{
    public interface IXElementSerializable
    {
        XElement Save();
        void Load(XElement element);
    }

    public static class XElementSerializableExt
    {
        public static bool TryGetAttributeValue(this XElement element, string attributeName, out string value)
        {
            value = element.Attribute(attributeName)?.Value ?? string.Empty;
            return !string.IsNullOrEmpty(value);
        }

        public static bool TryGetAttributeIntValue(this XElement element, string attributeName, out int value)
        {
            value = 0;
            if (!TryGetAttributeValue(element, attributeName, out string str) || !int.TryParse(str, out value))
                return false;

            return true;
        }

        public static bool TryGetAttributeDoubleValue(this XElement element, string attributeName, out double value)
        {
            value = 0;
            if (!TryGetAttributeValue(element, attributeName, out string str) || !double.TryParse(str, out value))
                return false;

            return true;
        }

        public static bool TryGetAttributeEnumValue<TEnum>(this XElement element, string attributeName, out TEnum value)
            where TEnum : struct, IConvertible
        {
            value = default;
            if (!TryGetAttributeValue(element, attributeName, out string str) || !Enum.TryParse(str, out value))
                return false;

            return true;
        }

        public static XElement SaveSpec<T>(this T target)
        {
            var type = typeof(T);
            var element = new XElement(type.Name);

            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(target);
                if (value == null)
                    continue;

                element.Add(new XAttribute(property.Name, value));
            }

            return element;
        }

        public static void LoadSpec<T>(this T target, XElement element)
        {
            var type = typeof(T);
            if (element.Name != type.Name)
                return;

            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                if (!element.TryGetAttributeIntValue(property.Name, out int value))
                    continue;

                property.SetValue(target, value);
            }
        }
    }
}
