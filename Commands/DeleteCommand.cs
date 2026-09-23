using VanguardCore.DataServices;
namespace VanguardCore.Commands;

public static class DeleteCommand
{
    public static void Run(string Id, Config gConfig)
    {
        string? input = TakeInput("Seguro que quieres eliminar a este usuario? (s/N) > ");
        if (input?.Equals("s", StringComparison.OrdinalIgnoreCase) is not true) return;

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
                    DrawText("El directorio de la base de datos no existe, regenerando...");
                    Validators.FileSystemValidators.AllFileSystem(gConfig);
                    break;
                }
            case DeleteResult.UnauthorizedAccessException:
                {
                    DrawText("El sistema operativo denegó el acceso al archivo.");
                    break;
                }
            case DeleteResult.IOException:
                {
                    DrawText("Error de IO.");
                    break;
                }
            case DeleteResult.DefaultException:
                {
                    DrawText("Error desconocido.");
                    break;
                }
            case DeleteResult.UserDoNotExistException:
                {
                    DrawText("Ese usuario no existe.");
                    break;
                }
        }
    }
}
