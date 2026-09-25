using System.CommandLine;
using VanguardCore.Commands;
namespace VanguardCore.Builders;

public static class ConfigCommand
{
    public static Command Build(Config gConfig)
    {
        var command = new Command("config") { Description = "Operaciones de configuración" };
        var getCommand = new Command("get") { Description = "Obtener el valor de una configuración actual" };
        var setCommand = new Command("set") { Description = "Establecer el valor actual de una configuración" };

        var typeGetArgument = new Argument<ConfigGetType>("config-type") { Description = "Atributo a consultar", Arity = ArgumentArity.ExactlyOne };
        var typeSetArgument = new Argument<ConfigSetType>("config-type") { Description = "Argumento a establecer", Arity = ArgumentArity.ExactlyOne };

        var newValueArgument = new Argument<string>("value") { Description = "Nuevo valor", Arity = ArgumentArity.ExactlyOne };

        getCommand.Add(typeGetArgument);
        setCommand.Add(typeSetArgument);
        setCommand.Add(newValueArgument);

        getCommand.SetAction(p =>
        {
            ConfigGetType type = p.GetValue(typeGetArgument);
            Commands.ConfigCommand.GetConfig(type, gConfig);
        });
        setCommand.SetAction(p =>
        {
            ConfigSetType type = p.GetValue(typeSetArgument);
            string? newValue = p.GetValue(newValueArgument);

            if (newValue == null) return;

            Commands.ConfigCommand.SetConfig(type, newValue, gConfig);
        });

        command.Add(getCommand);
        command.Add(setCommand);

        return command;
    }
}
