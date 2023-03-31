using System;
using System.Collections.Generic;
using AMD3DConfigurator.Model;
using Microsoft.Win32;

namespace AMD3DConfigurator.Helper;

public static class RegistryHelper
{
    #region Constants

    private const string KEY_BASE = @"SYSTEM\ControlSet001\Services\amd3dvcache\Preferences\App";

    private const string KEY_TYPE = "Type";
    private const string KEY_ENDS_WITH = "EndsWith";
    private const string KEY_EXE_PATH = "Config_ExePath";

    #endregion

    #region Methods

    public static bool IsVCacheConfigAvailable() => Registry.LocalMachine.OpenSubKey(KEY_BASE) != null;

    public static IEnumerable<ConfigEntry> ListPrograms()
    {
        using RegistryKey? appKey = Registry.LocalMachine.OpenSubKey(KEY_BASE);
        if (appKey == null)
            yield break;

        foreach (string programKeyName in appKey.GetSubKeyNames())
        {
            using RegistryKey? programKey = appKey.OpenSubKey(programKeyName);
            if (programKey == null) continue;

            yield return new ConfigEntry(programKeyName,
                                         Enum.TryParse(programKey.GetValue(KEY_TYPE)?.ToString(), out CoreType coreType) ? coreType : null,
                                         programKey.GetValue(KEY_ENDS_WITH)?.ToString(),
                                         programKey.GetValue(KEY_EXE_PATH)?.ToString());
        }
    }

    public static void AddEntry(ConfigEntry entry)
    {
        using RegistryKey? appKey = Registry.LocalMachine.OpenSubKey(KEY_BASE, true);
        if (appKey == null) return;

        using RegistryKey entryKey = appKey.CreateSubKey(entry.Name, true);

        entryKey.SetValue(KEY_TYPE, (int)(entry.CoreType ?? CoreType.NonCache));
        entryKey.SetValue(KEY_ENDS_WITH, entry.Exe ?? string.Empty);
        entryKey.SetValue(KEY_EXE_PATH, entry.ExePath ?? string.Empty);
    }

    public static void UpdateEntry(ConfigEntry entry)
    {
        using RegistryKey? appKey = Registry.LocalMachine.OpenSubKey(KEY_BASE);
        using RegistryKey? entryKey = appKey?.OpenSubKey(entry.Name, true);

        entryKey?.SetValue(KEY_TYPE, (int)(entry.CoreType ?? CoreType.NonCache));
    }

    public static void DeleteEntry(ConfigEntry entry)
    {
        using RegistryKey? appKey = Registry.LocalMachine.OpenSubKey(KEY_BASE, true);
        appKey?.DeleteSubKeyTree(entry.Name);
    }

    #endregion
}