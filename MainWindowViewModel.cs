using System;
using System.Collections.ObjectModel;
using System.IO;
using AMD3DConfigurator.Model;
using System.Linq;
using System.Windows;
using AMD3DConfigurator.Misc;
using Microsoft.Win32;
using AMD3DConfigurator.Helper;

namespace AMD3DConfigurator;

public class MainWindowViewModel : AbstractBindable
{
    #region Properties & Fields

    private ObservableCollection<ConfigEntry> _entries = new();
    public ObservableCollection<ConfigEntry> Entries
    {
        get => _entries;
        set => SetProperty(ref _entries, value);
    }

    private ConfigEntry? _newEntry;
    public ConfigEntry? NewEntry
    {
        get => _newEntry;
        set
        {
            if (SetProperty(ref _newEntry, value))
                AddCommand.RaiseCanExecuteChanged();
        }
    }

    #endregion

    #region Commands

    private ActionCommand? _reloadCommand;
    public ActionCommand ReloadCommand => _reloadCommand ??= new ActionCommand(Reload);

    private ActionCommand? _applyChangesCommand;
    public ActionCommand ApplyChangesCommand => _applyChangesCommand ??= new ActionCommand(ApplyChanges);

    private ActionCommand? _addCommand;
    public ActionCommand AddCommand => _addCommand ??= new ActionCommand(Add, () => NewEntry == null);

    private ActionCommand<ConfigEntry>? _deleteCommand;
    public ActionCommand<ConfigEntry> DeleteCommand => _deleteCommand ??= new ActionCommand<ConfigEntry>(Delete);

    private ActionCommand? _addNewEntryCommand;
    public ActionCommand AddNewEntryCommand => _addNewEntryCommand ??= new ActionCommand(AddNewEntry);

    private ActionCommand? _cancelNewEntryCommand;
    public ActionCommand CancelNewEntryCommand => _cancelNewEntryCommand ??= new ActionCommand(CancelNewEntry);

    #endregion

    #region Constructors

    public MainWindowViewModel()
    {
        Reload();
    }

    #endregion

    #region Methods

    private void Reload()
    {
        if (!RegistryHelper.IsVCacheConfigAvailable())
        {
            Entries = new ObservableCollection<ConfigEntry>();
            MessageBox.Show("The required registry path is not available. Please make sure you're actually using a CPU with this feature and have the latest chipset driver installed.",
                            "Error initializing");
        }
        else
            Entries = new ObservableCollection<ConfigEntry>(RegistryHelper.ListPrograms().OrderBy(x => x.Name));
    }

    private void ApplyChanges()
    {
        foreach (ConfigEntry entry in Entries)
        {
            try
            {
                if (entry.IsToDelete)
                    RegistryHelper.DeleteEntry(entry);
                else if (entry.IsAdded)
                    RegistryHelper.AddEntry(entry);
                else if (entry.IsValueChanged)
                    RegistryHelper.UpdateEntry(entry);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, $"Error updating '{entry.Name}' ...", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        try
        {
            ServiceHelper.RestartAMDService();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, $"Failed to restart AMD-service :(", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        Reload();
    }

    private void Add()
    {
        OpenFileDialog ofd = new()
        {
            Multiselect = false,
            Filter = "Executables (*.exe)|*.exe|All files (*.*)|*.*",
            CheckFileExists = true,
            CheckPathExists = true,
            AddExtension = true,
        };

        if (ofd.ShowDialog() == true)
        {
            string file = ofd.FileName;
            if (!File.Exists(file)) return;

            NewEntry = new ConfigEntry(Path.GetFileNameWithoutExtension(file), CoreType.Cache, Path.GetFileName(file), file) { IsAdded = true };
        }
    }

    private void AddNewEntry()
    {
        if (NewEntry == null) return;

        if (Entries.Any(x => x.Name == NewEntry.Name))
        {
            MessageBox.Show($"An entry with the name '{NewEntry.Name}' already exists.", "Duplicate entry", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        ConfigEntry? existingEntry = Entries.FirstOrDefault(x => x.Exe == NewEntry.Exe);
        if (existingEntry != null)
            if (MessageBox.Show($"An entry for the same executable '{NewEntry.Exe}' already exists with the name '{existingEntry.Name}'.\r\nShould this one be added anyway?",
                               "Duplicate entry", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

        Entries.Add(NewEntry);
        NewEntry = null;
    }

    private void CancelNewEntry() => NewEntry = null;

    private void Delete(ConfigEntry? configEntry)
    {
        if (configEntry == null) return;

        if (configEntry.IsAdded)
            Entries.Remove(configEntry);
        else
            configEntry.IsToDelete = true;
    }

    #endregion
}