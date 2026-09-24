using System.CommandLine;

namespace VanguardCore;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand("Vanguard Core - CLI de administración de usuarios");
        Config gConfig = new();
        Validators.FileSystemValidators.AllFileSystem(gConfig);

        Command addCommand = Builders.AddCommand.Build(gConfig);
        Command searchCommand = Builders.SearchCommand.Build(gConfig);
        Command consultCommand = Builders.ConsultCommandBuilder.Build(gConfig);
        Command deleteCommand = Builders.DeleteCommandBuilder.Build(gConfig);
        Command modifyCommand = Builders.ModifyCommand.Build(gConfig);
        Command importCommand = Builders.ImportCommand.Build(gConfig);
        Command exportCommand = Builders.ExportCommand.Build(gConfig);

        rootCommand.Add(importCommand);
        rootCommand.Add(addCommand);
        rootCommand.Add(searchCommand);
        rootCommand.Add(consultCommand);
        rootCommand.Add(deleteCommand);
        rootCommand.Add(modifyCommand);
        rootCommand.Add(exportCommand);

        return await rootCommand.Parse(args).InvokeAsync();
    }
}