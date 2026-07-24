using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace PDVnet.ControleCaixa.UI.Converters
{
    public class DashboardConverters
    {

        public class ChartHeightConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is decimal dec) return Math.Max(dec / 500, 20); // ajuste o divisor conforme seus dados
                if (value is double dbl) return Math.Max(dbl / 500, 20);
                return 40;
            }
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
        }

        // Estilo da barra (hoje vs outros dias)
        public class BarStyleConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                bool isToday = value is bool b && b;
                var dict = new ResourceDictionary { Source = new Uri("DashboardStyles.xaml", UriKind.Relative) };
                return isToday
                    ? dict["ChartBarTodayStyle"]
                    : dict["ChartBarStyle"];
            }
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
        }

        // Background do badge por tipo
        public class TipoBackgroundConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var tipo = value?.ToString()?.ToUpper();
                return tipo == "ENTRADA"
                    ? new SolidColorBrush(Color.FromRgb(209, 250, 229))
                    : new SolidColorBrush(Color.FromRgb(254, 226, 226));
            }
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
        }

        // Foreground do badge por tipo
        public class TipoForegroundConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var tipo = value?.ToString()?.ToUpper();
                return tipo == "ENTRADA"
                    ? new SolidColorBrush(Color.FromRgb(16, 185, 129))
                    : new SolidColorBrush(Color.FromRgb(239, 68, 68));
            }
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
        }

        // Estilo do valor (verde/vermelho)
        public class ValorStyleConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var tipo = value?.ToString()?.ToUpper();
                var dict = new ResourceDictionary { Source = new Uri("DashboardStyles.xaml", UriKind.Relative) };
                return tipo == "ENTRADA"
                    ? dict["TransactionValuePositiveStyle"]
                    : dict["TransactionValueNegativeStyle"];
            }
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
        }
    }
}
