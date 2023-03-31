using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AMD3DConfigurator.Converter;

[ValueConversion(typeof(object), typeof(Visibility))]
public class NullToCollapsedVisibilityConverter : IValueConverter
{
    #region Methods

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool val = value != null;
        if (!bool.TryParse(parameter?.ToString(), out bool invert))
            invert = false;

        if (invert)
            val = !val;

        return val ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();

    #endregion
}