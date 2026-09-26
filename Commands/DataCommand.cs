using VanguardCore.DataServices;

namespace VanguardCore.Commands;

public static class DataCommand
{
    public static class Import
    {
        public static void User(Config gConfig, string path, ConflictMode mode)
        {
            path = ExpandPath(path);
            User? user = Query.LoadUserByPath(path);
            if (user == null)
            {
                DrawError("El usuario está corrupto.", Color.Red);
                return;
            }
            ImportResult result = DataServices.Import.User(user, gConfig, mode);

            switch (result)
            {
                case ImportResult.Combined:
                    {
                        DrawText($"Datos combinados exitosamente, usa \"vanguardb consult {user.Id}\" para más información", Color.Green);
                        break;
                    }
                case ImportResult.Skipped:
                    {
                        DrawError("El usuario ya existe, omitido.", Color.Red);
                        break;
                    }
                case ImportResult.Failed:
                    {
                        DrawError("El usuario ya existe, omitido.", Color.Red);
                        break;
                    }
                case ImportResult.OverWrited:
                    {
                        DrawText($"El usuario ha sido sobreescrito, usa \"vanguardb consult {user.Id}\" para más información.");
                        break;
                    }
                case ImportResult.InvalidUser:
                    {
                        DrawError("Los datos del usuario son inválidos.", Color.Red);
                        break;
                    }
                case ImportResult.DirectoryNotFoundException:
                    {
                        Validators.FileSystemValidators.AllFileSystem(gConfig);
                        DrawError("Error: Directorio de base de datos no encontrado, seerá regenerado automáticamente, intenta otra vez", Color.Red);
                        break;
                    }
                case ImportResult.UnauthorizedAccessException:
                    {
                        DrawError("Error: El sistema operativo denegó el acceso.", Color.Red);
                        break;
                    }
                case ImportResult.IOException:
                    {
                        DrawError("Error de sistema de archivos.", Color.Red);
                        break;
                    }
                case ImportResult.DefaultException:
                    {
                        DrawError("Error desconocido.", Color.Red);
                        break;
                    }
                case ImportResult.Success:
                    {
                        DrawText($"Usuario importado con éxito, usa \"vanguardb consult {user.Id}\" para más información");
                        break;
                    }
            }
        }
        public static void DataBase(Config gConfig, string path, ConflictMode mode)
        {
            path = ExpandPath(path);
            List<User>? users = Pack.UnpackDataBase(path);
            if (users == null || users.Count == 0)
            {
                DrawError("ERROR: este archivo no contiene usuarios válidos.", Color.Red);
                Environment.Exit(1);
            }

            ImportResult result = DataServices.Import.Users(users, gConfig, mode, true);

            switch (result)
            {
                case ImportResult.Combined:
                    {
                        DrawText("Datos combinados exitosamente, usa \"vanguardb consult\" para más información con las siguientes IDs:", Color.Green);
                        foreach (User? user in users)
                        {
                            DrawText($"  - {user.Id}");
                        }
                        break;
                    }
                case ImportResult.Skipped:
                    {
                        DrawError("El usuario ya existe, omitido.", Color.Red);
                        break;
                    }
                case ImportResult.Failed:
                    {
                        DrawError("Un usuario ya existe, importación fallida.", Color.Red);
                        break;
                    }
                case ImportResult.OverWrited:
                    {
                        DrawText("El usuario ha sido sobreescrito, usa \"vanguardb consult \" para más información.");
                        break;
                    }
                case ImportResult.InvalidUser:
                    {
                        DrawError("Los datos de un usuario son inválidos.", Color.Red);
                        break;
                    }
                case ImportResult.DirectoryNotFoundException:
                    {
                        Validators.FileSystemValidators.AllFileSystem(gConfig);
                        DrawError("Error: Directorio de base de datos no encontrado, seerá regenerado automáticamente, intenta otra vez", Color.Red);
                        break;
                    }
                case ImportResult.UnauthorizedAccessException:
                    {
                        DrawError("Error: El sistema operativo denegó el acceso.", Color.Red);
                        break;
                    }
                case ImportResult.IOException:
                    {
                        DrawError("Error de sistema de archivos.", Color.Red);
                        break;
                    }
                case ImportResult.DefaultException:
                    {
                        DrawError("Error desconocido.", Color.Red);
                        break;
                    }
                case ImportResult.Success:
                    {
                        DrawText($"Usuarios importados con éxito, usa \"vanguardb consult\" para más información con las siguientes IDs:", Color.Green);
                        foreach (User? user in users)
                        {
                            DrawText($"  - {user.Id}");
                        }
                        break;
                    }
            }
        }
    }
    public static class Export
    {
        public static void User(string Id, string destPath, Config gConfig)
        {
            destPath = ExpandPath(destPath);

            ExportResult result = DataServices.Export.User(Id, destPath, gConfig);

            switch (result)
            {
                case ExportResult.ThereIsNotUsersException:
                    DrawError("Error: No hay usuarios registrados.", Color.Red);
                    break;
                case ExportResult.UserNotFoundException:
                    DrawError("Error: Usuario no encontrado", Color.Red);
                    break;
                case ExportResult.SourceUnaccesibleException:
                    DrawError("Error: Usuario inaccesible. El archvio existe? Otro proceso usándolo?", Color.Red);
                    break;
                case ExportResult.DestinyUnaccesibleException:
                    DrawError("Error: El sistema denegó el acceso al archivo.", Color.Red);
                    break;
                case ExportResult.IOException:
                    DrawError("Error de sistema de archivos.", Color.Red);
                    break;
                case ExportResult.DefaultException:
                    DrawError("Error desconocido.", Color.Red);
                    break;
                case ExportResult.Sucess:
                    DrawText("Hecho!", Color.Green);
                    break;
            }
        }
        public static void DataBase(string destPath, Config gConfig)
        {
            destPath = ExpandPath(destPath);

            ExportResult result = DataServices.Export.DataBase(destPath, gConfig);

            switch (result)
            {
                case ExportResult.ThereIsNotUsersException:
                    DrawError("Error: No hay usuarios registrados.", Color.Red);
                    break;
                case ExportResult.DestinyUnaccesibleException:
                    DrawError("Error: El sistema denegó el acceso al archivo. Existe el directorio? Tienes permiso de modificarlo?", Color.Red);
                    break;
                case ExportResult.IOException:
                    DrawError("Error de sistema de archivos.", Color.Red);
                    break;
                case ExportResult.DefaultException:
                    DrawError("Error desconocido.", Color.Red);
                    break;
                case ExportResult.Sucess:
                    DrawText("Hecho!", Color.Green);
                    break;
            }
        }
    }
    private static string ExpandPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return path;

        path = path.Trim();

        // ~ = home del usuario
        if (path == "~" || path.StartsWith("~/") || path.StartsWith("~\\"))
        {
            var home = Environment.GetFolderPath(
                Environment.SpecialFolder.UserProfile);

            path = Path.Combine(
                home,
                path.Length > 1
                    ? path.Substring(2)
                    : string.Empty);
        }

        // Resuelve . y ..
        return Path.GetFullPath(path);
    }
}
