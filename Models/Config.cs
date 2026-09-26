using YamlDotNet.Serialization;
namespace VanguardCore.Models;

public class Config
{
    [YamlIgnore]
    public int Version { get; } = 6;
    [YamlIgnore]
    public string VersionName { get; } = "0.2.3 RELEASE CANDIDATE 2";
    [YamlIgnore]
    public string ConfigPath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "vanguardb");
    [YamlMember(Alias = "DataBasePath")]
    public string DataBasePath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "vanguardb");
    [YamlMember(Alias = "TempPath")]
    public string TempPath { get; set; } = Path.GetTempPath();
}