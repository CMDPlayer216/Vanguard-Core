using System.CommandLine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace VanguardCore;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand("Vanguard Core - CLI de administración de usuarios");
        Config gConfig = DataServices.Conf.GetConfig();
        Validators.FileSystemValidators.AllFileSystem(gConfig);

        Command addCommand = Builders.AddCommand.Build(gConfig);
        Command searchCommand = Builders.SearchCommand.Build(gConfig);
        Command consultCommand = Builders.ConsultCommandBuilder.Build(gConfig);
        Command deleteCommand = Builders.DeleteCommandBuilder.Build(gConfig);
        Command modifyCommand = Builders.ModifyCommand.Build(gConfig);
        Command importCommand = Builders.ImportCommand.Build(gConfig);
        Command exportCommand = Builders.ExportCommand.Build(gConfig);
        Command configCommand = Builders.ConfigCommand.Build(gConfig);

        rootCommand.Add(importCommand);
        rootCommand.Add(addCommand);
        rootCommand.Add(searchCommand);
        rootCommand.Add(consultCommand);
        rootCommand.Add(deleteCommand);
        rootCommand.Add(modifyCommand);
        rootCommand.Add(exportCommand);
        rootCommand.Add(configCommand);

        return await rootCommand.Parse(args).InvokeAsync();
    }
}