using System.CommandLine;
namespace VanguardCore.Builders;

public static class ExportCommand
{
    public static Command Build(Config gConfig)
    {
        var command = new Command("export") { Description = "Operaciones de exportación" };

        var userCommand = new Command("user") { Description = "Exportar usuario" };
        var dataBaseCommand = new Command("database") { Description = "Exportar base de datos" };

        var idOption = new Argument<string>("source-id") { Description = "ID del usuario a exportar", Arity = ArgumentArity.ExactlyOne };
        var destOption = new Argument<string>("target-file") { Description = "Archivo de salida", Arity = ArgumentArity.ExactlyOne };

        userCommand.Add(idOption);
        userCommand.Add(destOption);

        dataBaseCommand.Add(destOption);

        userCommand.SetAction(p =>
        {
            string? id = p.GetValue(idOption);
            string? target = p.GetValue(destOption);

            if (id == null || target == null) { DrawError("Error: Datos inválidos."); return; }

            Commands.DataCommand.Export.User(id, target, gConfig);
        });

        dataBaseCommand.SetAction(p =>
        {
            string? target = p.GetValue(destOption);

            if (target == null) { DrawError("Error: Datos inválidos."); return; }

            Commands.DataCommand.Export.DataBase(target, gConfig);
        });

        command.Add(userCommand);
        command.Add(dataBaseCommand);

        return command;
    }
}
