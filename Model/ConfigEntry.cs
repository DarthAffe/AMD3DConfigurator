using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AMD3DConfigurator.Helper;

namespace AMD3DConfigurator.Model;

public class ConfigEntry : AbstractBindable
{
    #region Properties & Fields

    private string _name;
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private string? _exe;
    public string? Exe
    {
        get => _exe;
        set => SetProperty(ref _exe, value);
    }

    private CoreType? _origCoreType;
    private CoreType? _coreType;
    public CoreType? CoreType
    {
        get => _coreType;
        set
        {
            if (SetProperty(ref _coreType, value))
                OnPropertyChanged(nameof(IsValueChanged));
        }
    }

    private string? _exePath;
    public string? ExePath
    {
        get => _exePath;
        set => SetProperty(ref _exePath, value);
    }

    public bool IsValid => (Exe != null) && (CoreType != null);

    public ImageSource? Image
    {
        get
        {
            if (ExePath == null) return null;

            using Icon? icon = IconHelper.ExtractIcon(ExePath) ?? Icon.ExtractAssociatedIcon(ExePath);
            if (icon == null) return null;

            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle,
                                                                              new Int32Rect(0, 0, icon.Width, icon.Height),
                                                                              BitmapSizeOptions.FromEmptyOptions());
        }
    }

    public bool IsValueChanged => _origCoreType != CoreType;

    private bool _isToDelete;
    public bool IsToDelete
    {
        get => _isToDelete;
        set => SetProperty(ref _isToDelete, value);
    }

    private bool _isAdded;
    public bool IsAdded
    {
        get => _isAdded;
        set => SetProperty(ref _isAdded, value);
    }

    #endregion

    #region Constructors

    public ConfigEntry(string name, CoreType? coreType, string? exe, string? exePath)
    {
        this._name = name;
        this.CoreType = this._origCoreType = coreType;
        this.Exe = exe;
        this.ExePath = exePath;
    }

    #endregion
}