using YamlDotNet.Serialization;
namespace VanguardCore.Models;

public class Config
{
    [YamlIgnore]
    public int Version { get; } = 9;
    [YamlIgnore]
    public string VersionName { get; } = "0.2.6 RELEASE CANDIDATE 5";
    [YamlIgnore]
    public string ConfigPath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "vanguardb");
    [YamlMember(Alias = "data_base_path")]
    public string DataBasePath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "vanguardb");
    [YamlMember(Alias = "temp_path")]
    public string TempPath { get; set; } = Path.GetTempPath();
}