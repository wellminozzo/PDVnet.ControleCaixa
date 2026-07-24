using System;
using System.Globalization;
using System.Windows.Data;

namespace PDVnet.ControleCaixa.UI.Converters;

public class IntToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int intValue && parameter is string param)
        {
            return intValue == int.Parse(param);
        }
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue && boolValue && parameter is string param)
        {
            return int.Parse(param);
        }
        return Binding.DoNothing;
    }
}