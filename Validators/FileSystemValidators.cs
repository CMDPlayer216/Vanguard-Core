namespace VanguardCore.Validators;

public static class FileSystemValidators
{
    public static void AllFileSystem(Config gConfig)
    {
        if (!Directory.Exists(gConfig.ConfigPath)) Directory.CreateDirectory(gConfig.ConfigPath);
        if (!Directory.Exists(gConfig.DataBasePath)) Directory.CreateDirectory(gConfig.DataBasePath);
        if (!Directory.Exists(gConfig.TempPath)) Directory.CreateDirectory(gConfig.TempPath);
    }
}
