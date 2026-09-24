using System.CommandLine;
using VanguardCore.Models;

namespace VanguardCore.Builders;

public static class ModifyCommand
{
    public static Command Build(Config gConfig)
    {
        var command = new Command("modify", "Modificar un usuario existente");

        var idOption = new Argument<string>(
            "user-id")
        {
            Description = "ID del usuario a modificar",
            Arity = ArgumentArity.ExactlyOne
        };

        var primaryRoleOption = new Option<string?>(
            "-p", "--primary-rol")
        {
            Description = "Nuevo rol principal"
        };

        var addRoleOption = new Option<string[]?>(
            "--add-rol")
        {
            Description = "Roles adicionales que se añadirán"
        };

        var removeRoleOption = new Option<string[]?>(
            "--remove-rol")
        {
            Description = "Roles adicionales que se eliminarán"
        };

        var addWantedRoleOption = new Option<string[]?>(
            "--add-wanted-rol")
        {
            Description = "Roles buscados que se añadirán"
        };

        var removeWantedRoleOption = new Option<string[]?>(
            "--remove-wanted-rol")
        {
            Description = "Roles buscados que se eliminarán"
        };

        var addPronounOption = new Option<string[]?>(
            "--add-pronoun")
        {
            Description = "Pronombres que se añadirán"
        };

        var removePronounOption = new Option<string[]?>(
            "--remove-pronoun")
        {
            Description = "Pronombres que se eliminarán"
        };

        var ageOption = new Option<int?>(
            "-a", "--age")
        {
            Description = "Nueva edad"
        };

        var lastStreakOption = new Option<DateOnly?>(
            "-l", "--last-streak-date")
        {
            Description = "Nueva fecha de última verificación de racha"
        };

        var typeOption = new Option<UserType?>(
            "-t", "--type")
        {
            Description = "Nuevo tipo de usuario"
        };

        var addActionOption = new Option<string[]?>(
            "--add-action")
        {
            Description = "Acción pendiente a añadir: Tipo|Fecha|Razón"
        };

        var removeActionOption = new Option<string[]?>(
            "--remove-action")
        {
            Description = "Acción pendiente a eliminar: Tipo|Fecha|Razón"
        };

        var streakOption = new Option<int?>(
            "-s", "--streak")
        {
            Description = "Nueva racha"
        };

        var avatarOption = new Option<string?>(
            "--avatar")
        {
            Description = "Ruta de la nueva imagen de avatar"
        };

        command.Add(idOption);
        command.Add(primaryRoleOption);
        command.Add(addRoleOption);
        command.Add(removeRoleOption);
        command.Add(addWantedRoleOption);
        command.Add(removeWantedRoleOption);
        command.Add(addPronounOption);
        command.Add(removePronounOption);
        command.Add(ageOption);
        command.Add(lastStreakOption);
        command.Add(typeOption);
        command.Add(addActionOption);
        command.Add(removeActionOption);
        command.Add(streakOption);
        command.Add(avatarOption);

        command.SetAction(p =>
        {
            string id = p.GetValue(idOption)!;

            string? primaryRole =
                p.GetValue(primaryRoleOption);

            string[]? addRoles =
                p.GetValue(addRoleOption);

            string[]? removeRoles =
                p.GetValue(removeRoleOption);

            string[]? addWantedRoles =
                p.GetValue(addWantedRoleOption);

            string[]? removeWantedRoles =
                p.GetValue(removeWantedRoleOption);

            string[]? addPronouns =
                p.GetValue(addPronounOption);

            string[]? removePronouns =
                p.GetValue(removePronounOption);

            int? age =
                p.GetValue(ageOption);

            DateOnly? lastStreak =
                p.GetValue(lastStreakOption);

            UserType? type =
                p.GetValue(typeOption);

            string[]? addActions =
                p.GetValue(addActionOption);

            string[]? removeActions =
                p.GetValue(removeActionOption);

            int? streak =
                p.GetValue(streakOption);

            string? avatarPath =
                p.GetValue(avatarOption);

            ModifyingUser changes = new()
            {
                Id = id,

                PrimaryRole = primaryRole,

                AddRoles = addRoles?.ToList(),
                RemoveRoles = removeRoles?.ToList(),

                AddWantedRoles = addWantedRoles?.ToList(),
                RemoveWantedRoles = removeWantedRoles?.ToList(),

                AddPronouns = addPronouns?.ToList(),
                RemovePronouns = removePronouns?.ToList(),

                Age = age,
                LastStreakVerification = lastStreak,
                Type = type,

                AddPendActions = ParseActions(addActions),
                RemovePendActions = ParseActions(removeActions),

                Streak = streak,

                AvatarImage = LoadAvatar(avatarPath)
            };

            Commands.ModifyCommand.Run(changes, gConfig);
        });

        return command;
    }

    private static List<UserAction>? ParseActions(string[]? actions)
    {
        if (actions == null)
            return null;

        List<UserAction> result = [];

        foreach (string action in actions)
        {
            string[] parts = action.Split(
                '|',
                3,
                StringSplitOptions.None);

            if (parts.Length != 3)
                throw new ArgumentException($"Formato de acción inválido: {action}");

            if (!Enum.TryParse<Models.Action>(
                    parts[0],
                    true,
                    out Models.Action actionType))
            {
                throw new ArgumentException(
                    $"Tipo de acción inválido: {parts[0]}");
            }

            if (!DateTime.TryParse(
                    parts[1],
                    out DateTime actionTime))
            {
                throw new ArgumentException(
                    $"Fecha de acción inválida: {parts[1]}");
            }

            result.Add(new UserAction
            {
                ActionType = actionType,
                ActionTimeTrigger = actionTime,
                Reason = parts[2]
            });
        }

        return result;
    }

    private static byte[]? LoadAvatar(string? path)
    {
        if (path == null)
            return null;

        return File.ReadAllBytes(path);
    }
}