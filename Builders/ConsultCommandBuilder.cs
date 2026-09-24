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
        var rawOption = new Option<bool>("--raw") { Description = "Imprime la salida en JSON" };

        command.Add(idArgument);
        command.Add(rawOption);

        command.SetAction(p =>
        {
            string? id = p.GetValue(idArgument);
            if (id == null)
            {
                DrawText("ID inválido.");
                return;
            }
            Commands.ConsultCommand.Run(id, p.GetValue(rawOption), gConfig);
        });

        return command;
    }
}
