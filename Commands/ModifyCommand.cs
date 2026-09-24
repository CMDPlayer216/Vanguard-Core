using VanguardCore.DataServices;

namespace VanguardCore.Commands;

public static class ModifyCommand
{
    public static void Run(ModifyingUser changes, Config gConfig)
    {
        string? input = TakeInput("Seguro que quieres hacer estos cambios? (s/N) > ", Color.Yellow);
        if (input?.Equals("s", StringComparison.OrdinalIgnoreCase) is not true) return;

        ModifyResult result = Modify.User(changes, gConfig);

        switch (result)
        {
            case ModifyResult.InvalidModificationsException:
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
                    return;
                }
            case ModifyResult.UserDoNotExistException:
                {
                    DrawError("El usuario no existe.", Color.Red);
                    return;
                }
            case ModifyResult.CorruptUserException:
                {
                    DrawError("Usuario corrupto, considere eliminarlo.");
                    return;
                }
            case ModifyResult.DirectoryNotFoundException:
                {
                    DrawError("Directorio de base de datos no existe, reconstruyendo...");
                    Validators.FileSystemValidators.AllFileSystem(gConfig);
                    return;
                }
            case ModifyResult.UnauthorizedAccessException:
                {
                    DrawError("El sistema operativo denegó el acceso al directorio.");
                    return;
                }
            case ModifyResult.IOException:
                {
                    DrawError("Error de archivos desconocido.");
                    return;
                }
            case ModifyResult.Success:
                {
                    DrawText("Hecho!");
                    return;
                }
            default:
                {
                    DrawError("Error desconocido");
                    break;
                }
        }
    }
}
