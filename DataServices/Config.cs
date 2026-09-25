using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
namespace VanguardCore.DataServices;

public static class Conf
{
    public static void SetConfig(Config config, Config newConfig)
    {
        string configDir = config.ConfigPath;
        const string configFileName = "config.yaml";
        config = newConfig;
        var serializer = new SerializerBuilder()
        .WithNamingConvention(UnderscoredNamingConvention.Instance)
        .Build();
        string? yaml = serializer.Serialize(config);
        File.WriteAllText(Path.Combine(configDir, configFileName), yaml);
    }
    public static Config GetConfig()
    {
        Config config = new();
        string configPath = Path.Combine(config.ConfigPath, "config.yaml");
        if (!File.Exists(configPath)) return config;

        string rawConfig = File.ReadAllText(configPath);
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance) // Mapea camel_case/snake_case a C#
            .IgnoreUnmatchedProperties() // Evita fallos si el usuario pone propiedades extra
            .Build();

        return deserializer.Deserialize<Config>(rawConfig);
    }
}
