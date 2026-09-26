using System.ComponentModel;

namespace VanguardCore.Commands;

public static class ConfigCommand
{
    public static void GetConfig(ConfigGetType type, Config config)
    {
        switch (type)
        {
            case ConfigGetType.version: DrawText($"{config.Version}"); break;
            case ConfigGetType.versionName: DrawText($"{config.VersionName}"); break;
            case ConfigGetType.configPath: DrawText($"{config.ConfigPath}"); break;
            case ConfigGetType.databasePath: DrawText($"{config.DataBasePath}"); break;
            case ConfigGetType.tempPath: DrawText($"{config.TempPath}"); break;
        }
    }
    public static void SetConfig(ConfigSetType type, string newConfigValue, Config config)
    {
        Config newConfig = new()
        {
            ConfigPath = config.ConfigPath,
            DataBasePath = config.DataBasePath,
            TempPath = config.TempPath
        };

        switch (type)
        {
            case ConfigSetType.configPath: newConfig.ConfigPath = newConfigValue; break;
            case ConfigSetType.databasePath: newConfig.DataBasePath = newConfigValue; break;
            case ConfigSetType.tempPath: newConfig.TempPath = newConfigValue; break;
        }
        DataServices.Conf.SetConfig(config, newConfig);
    }
}

public enum ConfigGetType
{
    version,
    versionName,
    configPath,
    databasePath,
    tempPath
}
public enum ConfigSetType
{
    configPath,
    databasePath,
    tempPath
}