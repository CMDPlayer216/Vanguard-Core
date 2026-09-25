using YamlDotNet.Serialization;
namespace VanguardCore.Models;

public class Config
{
    [YamlIgnore]
    public int Version { get; } = 3;
    [YamlIgnore]
    public string VersionName { get; } = "0.2.0 BETA";
    [YamlIgnore]
    public string ConfigPath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "vanguardb");
    [YamlMember(Alias = "DataBasePath")]
    public string DataBasePath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "vanguardb");
    [YamlMember(Alias = "TempPath")]
    public string TempPath { get; set; } = Path.GetTempPath();
}