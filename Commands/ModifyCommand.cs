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
                    DrawText("Los datos introducidos son inválidos.", Color.Red);
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
            case ModifyResult.UserDoNotExistException:
                {
                    DrawText("El usuario no existe.", Color.Red);
                    return;
                }
            case ModifyResult.CorruptUserException:
                {
                    DrawText("Usuario corrupto, considere eliminarlo.");
                    return;
                }
            case ModifyResult.DirectoryNotFoundException:
                {
                    DrawText("Directorio de base de datos no existe, reconstruyendo...");
                    Validators.FileSystemValidators.AllFileSystem(gConfig);
                    return;
                }
            case ModifyResult.UnauthorizedAccessException:
                {
                    DrawText("El sistema operativo denegó el acceso al directorio.");
                    return;
                }
            case ModifyResult.IOException:
                {
                    DrawText("Error de archivos desconocido.");
                    return;
                }
            case ModifyResult.Success:
                {
                    DrawText("Hecho!");
                    return;
                }
            default:
                {
                    DrawText("Error desconocido");
                    break;
                }
        }
    }
}
