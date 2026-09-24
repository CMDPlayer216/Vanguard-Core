using System.CommandLine;
using VanguardCore.DataServices;
namespace VanguardCore.Builders;

public static class ImportCommand
{
    public static Command Build(Config gConfig)
    {
        var command = new Command("import", "Permite importar una base de datos o un usuario");

        var userCommand = new Command("user") { Description = "Importar usuario" };
        var dataBaseCommand = new Command("database") { Description = "Importar base de datos" };

        var pathArgument = new Argument<string>("path")
        {
            Description = "Ruta del archivo a importar",
            Arity = ArgumentArity.ExactlyOne
        };
        var conflictModeArgument = new Argument<ConflictMode>("conflict-mode")
        {
            Description = "Modo de resolución de conflictos",
            DefaultValueFactory = _ => ConflictMode.Skip
        };

        userCommand.Add(pathArgument);
        userCommand.Add(conflictModeArgument);

        dataBaseCommand.Add(pathArgument);
        dataBaseCommand.Add(conflictModeArgument);

        userCommand.SetAction(p =>
        {
            string? path = p.GetValue(pathArgument);
            ConflictMode conflictMode = p.GetValue(conflictModeArgument);
            if (path == null) return;
            Commands.DataCommand.Import.User(gConfig, path, conflictMode);
        });

        dataBaseCommand.SetAction(p =>
        {
            string? path = p.GetValue(pathArgument);
            ConflictMode conflictMode = p.GetValue(conflictModeArgument);
            if (path == null) return;
            Commands.DataCommand.Import.DataBase(gConfig, path, conflictMode);
        });

        command.Add(userCommand);
        command.Add(dataBaseCommand);

        return command;
    }
}