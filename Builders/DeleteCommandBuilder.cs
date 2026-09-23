using System.CommandLine;
namespace VanguardCore.Builders;

public static class DeleteCommandBuilder
{
    public static Command Build(Config gConfig)
    {
        var command = new Command("delete", "Permite eliminar un usuario");

        var idArgument = new Argument<string>("id")
        {
            Arity = ArgumentArity.ExactlyOne,
            Description = "ID del usuario que quieres eliminar"
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
            Commands.DeleteCommand.Run(id, gConfig);
        });

        return command;
    }
}
