namespace VanguardCore.Models;

public class Config
{
    public int Version { get; } = 1;
    public string VersionName { get; } = "0.0.1 ALPHA";
    public string ConfigPath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "vanguarddb");
    public string DataBasePath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "vanguarddb");
    public string TempPath { get; } = Path.GetTempPath();
}
