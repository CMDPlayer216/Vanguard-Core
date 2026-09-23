using System.CommandLine;
namespace VanguardCore.Builders;

public static class ConsultCommandBuilder
{
    public static Command Build(Config gConfig)
    {
        var command = new Command("consult", "Permite consultar la información de un usuario");

        var idArgument = new Argument<string>("id")
        {
            Arity = ArgumentArity.ExactlyOne,
            Description = "ID del usuario que quieres consultar"
        };

        command.Add(idArgument);

        command.SetAction(p =>
        {
            string? id = p.GetValue(idArgument);
            if (id == null)
            {
                DrawText("ID inválido.");
                return;
            }
            Commands.ConsultCommand.Run(id, gConfig);
        });

        return command;
    }
}
