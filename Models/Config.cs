using YamlDotNet.Serialization;
namespace VanguardCore.Models;

public class Config
{
    [YamlIgnore]
    public int Version { get; } = 4;
    [YamlIgnore]
    public string VersionName { get; } = "0.2.2 RELEASE CANDIDATE 1";
    [YamlIgnore]
    public string ConfigPath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "vanguardb");
    [YamlMember(Alias = "DataBasePath")]
    public string DataBasePath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "vanguardb");
    [YamlMember(Alias = "TempPath")]
    public string TempPath { get; set; } = Path.GetTempPath();
}