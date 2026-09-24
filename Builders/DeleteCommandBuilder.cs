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
        var noConfirmOption = new Option<bool>("--noconfirm") { Description = "Omite la confirmación. USAR CON PRECAUCIÓN" };

        command.Add(idArgument);
        command.Add(noConfirmOption);

        command.SetAction(p =>
        {
            string? id = p.GetValue(idArgument);
            if (id == null)
            {
                DrawText("ID inválido.");
                return;
            }
            Commands.DeleteCommand.Run(id, p.GetValue(noConfirmOption), gConfig);
        });

        return command;
    }
}
