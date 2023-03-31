using System.Windows;

namespace AMD3DConfigurator.Misc;

public class BindingProxy : Freezable
{
    #region Properties & Fields

    // ReSharper disable once InconsistentNaming
    public static readonly DependencyProperty DataProperty = DependencyProperty.Register(nameof(Data), typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));

    public object Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    #endregion

    #region Methods

    protected override Freezable CreateInstanceCore() => new BindingProxy();

    #endregion
}
