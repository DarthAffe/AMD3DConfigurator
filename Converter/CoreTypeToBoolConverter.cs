using System;
using System.Globalization;
using System.Windows.Data;
using AMD3DConfigurator.Model;

namespace AMD3DConfigurator.Converter;

[ValueConversion(typeof(CoreType?), typeof(bool))]
internal class CoreTypeToBoolConverter : IValueConverter
{
    #region Methods

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (value as CoreType?) == CoreType.Cache;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => (value as bool?) == true ? CoreType.Cache : CoreType.NonCache;

    #endregion
}