using VanguardCore.DataServices;

namespace VanguardCore.Commands;

public static class AddCommand
{
    public static void Run(string? primaryRole,
                           string? roles,
                           string? wantedRoles,
                           string? pronouns,
                           int age,
                           UserType type,
                           string? imagePath,
                           string? fandoms,
                           Config gConfig)
    {
        if (primaryRole == null || pronouns == null || fandoms == null)
        {
            DrawText("Los datos introducidos son inválidos.", Color.Red);
            DrawText($"primaryRol: {primaryRole}, pronouns {pronouns}, age: {age}");
            DrawText("");
            DrawText("  --> Caracteres válidos para IDs: ", newLine: false);
            DrawText(Validators.UserValidators.IdValidChars, Color.Green);
            DrawText("  --> Caracteres válidos para otros textos: ", newLine: false);
            DrawText(Validators.UserValidators.OtherValidChars, Color.Green);
            DrawText($"  --> Edad mínima válida: {Validators.UserValidators.MinValidAge}");
            DrawText($"  --> Racha mínima válida: {Validators.UserValidators.MinValidStreak}");
            DrawText($"  --> Formato de fecha: yyyy-MM-dd");
            return;
        }
        byte[]? image = null;
        if (imagePath != null)
        {
            if (!File.Exists(imagePath))
            {
                DrawText("Esa imágen no existe.", Color.Red);
                return;
            }
            image = File.ReadAllBytes(imagePath);
        }
        List<string>? listRoles = null;
        if (roles != null) listRoles = [.. roles.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)];
        List<string>? listWantedRoles = null;
        if (wantedRoles != null) listWantedRoles = [.. wantedRoles.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)];
        User user = new()
        {
            PrimaryRole = primaryRole,
            Roles = listRoles,
            WantedRoles = listWantedRoles,
            Type = type,
            Pronouns = [.. pronouns.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)],
            Age = age,
            Fandoms = [.. fandoms.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)],
            AvatarImage = image
        };
        WriteResult result = Write.AddUser(user, gConfig);

        switch (result)
        {
            case WriteResult.InvalidUserException:
                {
                    DrawText("Los datos introducidos son inválidos.", Color.Red);
                    DrawText("");
                    DrawText("  --> Caracteres válidos para IDs: ", newLine: false);
                    DrawText(Validators.UserValidators.IdValidChars, Color.Green);
                    DrawText("  --> Caracteres válidos para otros textos: ", newLine: false);
                    DrawText(Validators.UserValidators.OtherValidChars, Color.Green);
                    DrawText($"  --> Edad mínima válida: {Validators.UserValidators.MinValidAge}");
                    DrawText($"  --> Racha mínima válida: {Validators.UserValidators.MinValidStreak}");
                    DrawText($"  --> Formato de fecha: yyyy-MM-dd");
                    break;
                }
            case WriteResult.DirectoryNotFoundException:
                {
                    DrawText("Error: Directorio de base de datos no encontrado, regenerando...", Color.Red);
                    Validators.FileSystemValidators.AllFileSystem(gConfig);
                    break;
                }
            case WriteResult.UnauthorizedAccessException:
                {
                    DrawText("Error: El sistema operativo denegó el acceso al sistema de archivos.", Color.Red);
                    break;
                }
            case WriteResult.IOException:
                {
                    DrawText("Error desconocido en el sistema de archivos.", Color.Red);
                    break;
                }
            case WriteResult.DefaultException:
                {
                    DrawText("Error desconocido.", Color.Red);
                    break;
                }
            case WriteResult.Success:
                {
                    DrawText("Usuario agregado con éxito!", Color.Green);
                    break;
                }
        }
    }
}
