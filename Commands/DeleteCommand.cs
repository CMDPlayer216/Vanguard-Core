using VanguardCore.DataServices;
namespace VanguardCore.Commands;

public static class DeleteCommand
{
    public static void Run(string Id, bool noConfirm, Config gConfig)
    {
        string? input = null;
        if (!noConfirm) input = TakeInput("Seguro que quieres eliminar a este usuario? (s/N) > ");
        if (input?.Equals("s", StringComparison.OrdinalIgnoreCase) is not true && !noConfirm) return;

        DeleteResult result = Delete.User(Id, gConfig);
        switch (result)
        {
            case DeleteResult.Success:
                {
                    DrawText("Hecho!", Color.Green);
                    break;
                }
            case DeleteResult.DirectoryNotFoundException:
                {
                    DrawError("El directorio de la base de datos no existe, regenerando...");
                    Validators.FileSystemValidators.AllFileSystem(gConfig);
                    break;
                }
            case DeleteResult.UnauthorizedAccessException:
                {
                    DrawError("El sistema operativo denegó el acceso al archivo.");
                    break;
                }
            case DeleteResult.IOException:
                {
                    DrawError("Error de IO.");
                    break;
                }
            case DeleteResult.DefaultException:
                {
                    DrawError("Error desconocido.");
                    break;
                }
            case DeleteResult.UserDoNotExistException:
                {
                    DrawError("Ese usuario no existe.");
                    break;
                }
        }
    }
}
