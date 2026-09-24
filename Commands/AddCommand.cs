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
            DrawError("Los datos introducidos son inválidos.", Color.Red);
            DrawError($"primaryRol: {primaryRole}, pronouns {pronouns}, age: {age}");
            DrawError("");
            DrawError("  --> Caracteres válidos para IDs: ", newLine: false);
            DrawError(Validators.UserValidators.IdValidChars, Color.Green);
            DrawError("  --> Caracteres válidos para otros textos: ", newLine: false);
            DrawError(Validators.UserValidators.OtherValidChars, Color.Green);
            DrawError($"  --> Edad mínima válida: {Validators.UserValidators.MinValidAge}");
            DrawError($"  --> Racha mínima válida: {Validators.UserValidators.MinValidStreak}");
            DrawError($"  --> Formato de fecha: yyyy-MM-dd");
            return;
        }
        byte[]? image = null;
        if (imagePath != null)
        {
            if (!File.Exists(imagePath))
            {
                DrawError("Esa imágen no existe.", Color.Red);
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
                    DrawError("Los datos introducidos son inválidos.", Color.Red);
                    DrawError("");
                    DrawError("  --> Caracteres válidos para IDs: ", newLine: false);
                    DrawError(Validators.UserValidators.IdValidChars, Color.Green);
                    DrawError("  --> Caracteres válidos para otros textos: ", newLine: false);
                    DrawError(Validators.UserValidators.OtherValidChars, Color.Green);
                    DrawError($"  --> Edad mínima válida: {Validators.UserValidators.MinValidAge}");
                    DrawError($"  --> Racha mínima válida: {Validators.UserValidators.MinValidStreak}");
                    DrawError($"  --> Formato de fecha: yyyy-MM-dd");
                    break;
                }
            case WriteResult.DirectoryNotFoundException:
                {
                    DrawError("Error: Directorio de base de datos no encontrado, regenerando...", Color.Red);
                    Validators.FileSystemValidators.AllFileSystem(gConfig);
                    break;
                }
            case WriteResult.UnauthorizedAccessException:
                {
                    DrawError("Error: El sistema operativo denegó el acceso al sistema de archivos.", Color.Red);
                    break;
                }
            case WriteResult.IOException:
                {
                    DrawError("Error desconocido en el sistema de archivos.", Color.Red);
                    break;
                }
            case WriteResult.DefaultException:
                {
                    DrawError("Error desconocido.", Color.Red);
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
