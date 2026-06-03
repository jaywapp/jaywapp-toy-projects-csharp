using System;
using System.Globalization;
using System.Windows.Data;
using WeddingVisitor.Helper;

namespace WeddingVisitor.Converter
{
    public class EnumDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum @enum)
                return @enum.GetDescription();

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
