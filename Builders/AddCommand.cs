using System.CommandLine;
using System.ComponentModel;

namespace VanguardCore.Builders;

public static class AddCommand
{
    public static Command Build(Config gConfig)
    {
        
        var command = new Command("add", "Añadir un usuario a la base de datos");

        var primaryRoleArgument = new Argument<string>("primary-role") { Arity = ArgumentArity.ExactlyOne };
        var ageArgument = new Argument<int>("age") { Arity = ArgumentArity.ExactlyOne };
        var pronounsArgument = new Argument<string>("pronouns") { Arity = ArgumentArity.ExactlyOne };
        var fandomsArgument = new Argument<string>("fandoms") { Arity = ArgumentArity.ExactlyOne };
        var additionalRolesArgument = new Option<string>("-r", "--additional-roles");
        var wantedRolesArgument = new Option<string>("-w", "--wanted-roles");
        var typeArgument = new Option<UserType>("-t", "--type");
        var imageArgument = new Option<string>("-i", "--image");

        ageArgument.Description = "La edad del usuario que quieres registrar";
        primaryRoleArgument.Description = "El rol principal del usuario que quieres agregar";
        pronounsArgument.Description = "Pronombres del usuario que quieres agregar (separados por |)";
        fandomsArgument.Description = "Fandoms del usuario que quieres agregar (separados por |)";
        wantedRolesArgument.Description = "Roles buscados por el usuario que quieres agregar (separados por |)";
        additionalRolesArgument.Description = "Roles adicionales del usuario que quieres agregar (separados por |)";
        typeArgument.Description = "Tipo de usuario a agregar";
        typeArgument.DefaultValueFactory = _ => UserType.Member;
        imageArgument.Description = "Ruta de la imágen del usuario";

        command.Add(primaryRoleArgument);
        command.Add(ageArgument);
        command.Add(pronounsArgument);
        command.Add(fandomsArgument);
        command.Add(wantedRolesArgument);
        command.Add(additionalRolesArgument);
        command.Add(typeArgument);
        command.Add(imageArgument);

        command.SetAction(parseResult =>
        {
            string? primaryRole = parseResult.GetValue(primaryRoleArgument);
            string? roles = parseResult.GetValue(additionalRolesArgument);
            string? pronouns = parseResult.GetValue(pronounsArgument);
            string? fandoms = parseResult.GetValue(fandomsArgument);
            int age = parseResult.GetValue(ageArgument);
            string? wantedRoles = parseResult.GetValue(wantedRolesArgument);
            UserType type = parseResult.GetValue(typeArgument);
            string? imagePath = parseResult.GetValue(imageArgument);
            Commands.AddCommand.Run(primaryRole, roles, wantedRoles, pronouns, age, type, imagePath, fandoms, gConfig);
        });

        return command;
    }
}
