using System.IO;
using MessagePack;
using static VanguardCore.Validators.UserValidators;

namespace VanguardCore.DataServices;

public static class Import
{
    public static ImportResult Users(
        List<User>? users,
        Config gConfig,
        ConflictMode mode,
        bool isLockAcquired = false)
    {
        using var dbLock = new DatabaseLock(gConfig.ConfigPath);

        if (!isLockAcquired)
        {
            if (!dbLock.Acquire())
            {
                DrawError("ERROR: la base de datos está bloqueada.", Color.Red);
            }
        }

        if (users == null || users.Count == 0)
            return ImportResult.InvalidUser;

        // Validar todos los usuarios antes de comenzar a modificar datos.
        foreach (User user in users)
        {
            if (!ValidateUser(user))
                return ImportResult.InvalidUser;
        }

        List<IndexEntry>? index = Query.Index(gConfig);

        // Si no existe índice, creamos uno nuevo en memoria.
        index ??= [];

        string tempUsersPath = Path.Combine(gConfig.TempPath, Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempUsersPath);

        foreach (User user in users)
        {
            int existingIndex = index.FindIndex(e => e.Id == user.Id);

            // ---------------------------------------------------------
            // USUARIO NUEVO
            // ---------------------------------------------------------
            if (existingIndex == -1)
            {
                IndexEntry newEntry = new()
                {
                    PrimaryRole = user.PrimaryRole,
                    Id = user.Id,
                    Version = user.Version,
                    AvatarImage = user.AvatarImage
                };

                string userPath = Path.Combine(
                    tempUsersPath,
                    newEntry.Path);

                try
                {
                    byte[] serializedUser =
                        MessagePackSerializer.Serialize(user);

                    string? directory = Path.GetDirectoryName(userPath);

                    if (!string.IsNullOrEmpty(directory))
                        Directory.CreateDirectory(directory);

                    File.WriteAllBytes(userPath, serializedUser);
                }
                catch (DirectoryNotFoundException)
                {
                    return ImportResult.DirectoryNotFoundException;
                }
                catch (UnauthorizedAccessException)
                {
                    return ImportResult.UnauthorizedAccessException;
                }
                catch (IOException)
                {
                    return ImportResult.IOException;
                }
                catch
                {
                    return ImportResult.DefaultException;
                }

                index.Add(newEntry);
                continue;
            }

            // ---------------------------------------------------------
            // USUARIO EXISTENTE
            // ---------------------------------------------------------
            IndexEntry existingEntry = index[existingIndex];

            IndexEntry updatedEntry;
            User? userToSerialize = user;

            switch (mode)
            {
                case ConflictMode.Skip:
                    continue;

                case ConflictMode.Fail:
                    return ImportResult.Failed;

                case ConflictMode.OverWrite:
                    updatedEntry = new IndexEntry
                    {
                        PrimaryRole = user.PrimaryRole,
                        Id = user.Id,
                        Version = user.Version,
                        AvatarImage = user.AvatarImage,
                        Path = existingEntry.Path
                    };
                    break;

                case ConflictMode.CombineKeepingNew:
                    updatedEntry = new IndexEntry
                    {
                        PrimaryRole = user.PrimaryRole,
                        Id = existingEntry.Id,
                        Version = Math.Max(
                            existingEntry.Version,
                            user.Version) + 1,
                        AvatarImage =
                            user.AvatarImage ?? existingEntry.AvatarImage,
                        Path = existingEntry.Path
                    };

                    userToSerialize =
                        Query.LoadUserById(user.Id, gConfig);

                    if (userToSerialize == null)
                    {
                        userToSerialize = user;
                        break;
                    }

                    userToSerialize.Age = user.Age;

                    userToSerialize.AvatarImage =
                        user.AvatarImage ?? existingEntry.AvatarImage;

                    userToSerialize.Fandoms =
                        [.. userToSerialize.Fandoms.Union(user.Fandoms)];

                    userToSerialize.LastStreakVerification =
                        user.LastStreakVerification ??
                        userToSerialize.LastStreakVerification;

                    if (user.PendActions != null)
                    {
                        userToSerialize.PendActions ??= [];

                        userToSerialize.PendActions =
                            [.. userToSerialize.PendActions
                            .Union(user.PendActions)];
                    }

                    userToSerialize.PrimaryRole = user.PrimaryRole;

                    userToSerialize.Pronouns =
                        [.. userToSerialize.Pronouns.Union(user.Pronouns)];

                    if (user.Roles != null)
                    {
                        userToSerialize.Roles ??= [];

                        userToSerialize.Roles =
                            [.. userToSerialize.Roles.Union(user.Roles)];
                    }

                    userToSerialize.Streak = user.Streak;
                    userToSerialize.Type = user.Type;

                    userToSerialize.Version =
                        Math.Max(
                            existingEntry.Version,
                            user.Version) + 1;

                    if (user.WantedRoles != null)
                    {
                        userToSerialize.WantedRoles ??= [];

                        userToSerialize.WantedRoles =
                            [.. userToSerialize.WantedRoles
                            .Union(user.WantedRoles)];
                    }

                    break;

                case ConflictMode.CombineKeepingOriginal:
                    updatedEntry = new IndexEntry
                    {
                        PrimaryRole = existingEntry.PrimaryRole,
                        Id = existingEntry.Id,
                        Version = Math.Max(
                            existingEntry.Version,
                            user.Version) + 1,
                        AvatarImage =
                            existingEntry.AvatarImage ??
                            user.AvatarImage,
                        Path = existingEntry.Path
                    };

                    userToSerialize =
                        Query.LoadUserById(user.Id, gConfig);

                    if (userToSerialize == null)
                    {
                        userToSerialize = user;
                        break;
                    }

                    userToSerialize.Fandoms =
                        [.. userToSerialize.Fandoms.Union(user.Fandoms)];

                    if (user.PendActions != null)
                    {
                        userToSerialize.PendActions ??= [];

                        userToSerialize.PendActions =
                            [.. userToSerialize.PendActions
                            .Union(user.PendActions)];
                    }

                    userToSerialize.Pronouns =
                        [.. userToSerialize.Pronouns.Union(user.Pronouns)];

                    if (user.Roles != null)
                    {
                        userToSerialize.Roles ??= [];

                        userToSerialize.Roles =
                            [.. userToSerialize.Roles.Union(user.Roles)];
                    }

                    userToSerialize.Version =
                        Math.Max(
                            existingEntry.Version,
                            user.Version) + 1;

                    if (user.WantedRoles != null)
                    {
                        userToSerialize.WantedRoles ??= [];

                        userToSerialize.WantedRoles =
                            [.. userToSerialize.WantedRoles
                            .Union(user.WantedRoles)];
                    }

                    break;

                default:
                    return ImportResult.DefaultException;
            }

            // ---------------------------------------------------------
            // ACTUALIZAR ÍNDICE EN MEMORIA
            // ---------------------------------------------------------
            index[existingIndex] = updatedEntry;

            // ---------------------------------------------------------
            // GUARDAR USUARIO
            // ---------------------------------------------------------
            string path = Path.Combine(
                tempUsersPath,
                existingEntry.Path);

            try
            {
                byte[] serializedUser =
                MessagePackSerializer.Serialize(userToSerialize);

                File.WriteAllBytes(path, serializedUser);
            }
            catch (DirectoryNotFoundException)
            {
                return ImportResult.DirectoryNotFoundException;
            }
            catch (UnauthorizedAccessException)
            {
                return ImportResult.UnauthorizedAccessException;
            }
            catch (IOException)
            {
                return ImportResult.IOException;
            }
            catch
            {
                return ImportResult.DefaultException;
            }
        }

        foreach (string archivoDestino in Directory.GetFiles(gConfig.DataBasePath))
        {
            string nombreArchivo = Path.GetFileName(archivoDestino);
            string rutaEnTemp = Path.Combine(tempUsersPath, nombreArchivo);

            if (!File.Exists(rutaEnTemp))
            {
                File.Copy(archivoDestino, rutaEnTemp);
            }
        }

        // 2. Intercambio atómico
        string directorioPadre = Path.GetDirectoryName(Path.GetFullPath(gConfig.DataBasePath))!;
        string rutaBackup = Path.Combine(directorioPadre, $"_db_backup_{Guid.NewGuid():N}");

        try
        {
            // 1. Renombrar la carpeta actual a backup
            MoverDirectorioSeguro(gConfig.DataBasePath, rutaBackup);

            // 2. Publicar la carpeta temporal como la nueva carpeta de base de datos
            MoverDirectorioSeguro(tempUsersPath, gConfig.DataBasePath);

            // 3. Eliminar el respaldo de la base de datos vieja
            Directory.Delete(rutaBackup, recursive: true);
        }
        catch
        {
            // Si la carpeta original no existe y quedó el backup, intentamos restaurar
            if (!Directory.Exists(gConfig.DataBasePath) && Directory.Exists(rutaBackup))
            {
                MoverDirectorioSeguro(rutaBackup, gConfig.DataBasePath);
            }
            throw;
        }

        // -------------------------------------------------------------
        // ESCRIBIR EL ÍNDICE UNA SOLA VEZ
        // -------------------------------------------------------------
        return OverWriteIndex(index, gConfig);
    }
    private static void MoverDirectorioSeguro(string origen, string destino)
    {
        try
        {
            // Intento rápido de renombrado a nivel de SO
            Directory.Move(origen, destino);
        }
        catch (IOException)
        {
            // Fallback cuando se intenta mover entre volúmenes/discos distintos (Invalid cross-device link)
            CopiarDirectorioRecursivo(origen, destino);
            Directory.Delete(origen, recursive: true);
        }
    }

    private static void CopiarDirectorioRecursivo(string origen, string destino)
    {
        Directory.CreateDirectory(destino);

        foreach (string archivo in Directory.GetFiles(origen))
        {
            string archivoDestino = Path.Combine(destino, Path.GetFileName(archivo));
            File.Copy(archivo, archivoDestino, overwrite: true);
        }

        foreach (string subDirectorio in Directory.GetDirectories(origen))
        {
            string subDirectorioDestino = Path.Combine(destino, Path.GetFileName(subDirectorio));
            CopiarDirectorioRecursivo(subDirectorio, subDirectorioDestino);
        }
    }
    public static ImportResult User(User user, Config gConfig, ConflictMode mode)
    {
        using var dbLock = new DatabaseLock(gConfig.ConfigPath);
        if (!dbLock.Acquire())
        {
            DrawError("ERROR: la base de datos está bloqueada.", Color.Red);
            return ImportResult.Failed; // Salir si el lock falla
        }

        if (!ValidateUser(user))
            return ImportResult.InvalidUser;

        List<IndexEntry>? index = Query.Index(gConfig);
        if (index == null)
        {
            return MapWriteResult(Write.AddUser(user, gConfig, true));
        }

        // Buscar el elemento en el índice sin modificar la colección durante la iteración
        int existingIndex = index.FindIndex(e => e.Id == user.Id);

        if (existingIndex == -1)
        {
            // El usuario no existe previamente en el índice
            return MapWriteResult(Write.AddUser(user, gConfig, true));
        }

        IndexEntry existingEntry = index[existingIndex];
        IndexEntry updatedEntry;
        User? userToSerialize = user;

        switch (mode)
        {
            case ConflictMode.Skip:
                return ImportResult.Skipped;

            case ConflictMode.Fail:
                return ImportResult.Failed;

            case ConflictMode.OverWrite:
                updatedEntry = new IndexEntry
                {
                    PrimaryRole = user.PrimaryRole,
                    Id = user.Id,
                    Version = user.Version,
                    AvatarImage = user.AvatarImage,
                    Path = existingEntry.Path
                };
                break;

            case ConflictMode.CombineKeepingNew:
                updatedEntry = new IndexEntry
                {
                    PrimaryRole = user.PrimaryRole,
                    Id = existingEntry.Id,
                    Version = Math.Max(existingEntry.Version, user.Version) + 1,
                    AvatarImage = user.AvatarImage ?? existingEntry.AvatarImage,
                    Path = existingEntry.Path
                };
                userToSerialize = Query.LoadUserById(user.Id, gConfig);
                if (userToSerialize == null)
                {
                    userToSerialize = user;
                    break;
                }
                userToSerialize.Age = user.Age;
                userToSerialize.AvatarImage = user.AvatarImage ?? existingEntry.AvatarImage;
                userToSerialize.Fandoms = [.. userToSerialize.Fandoms.Union(user.Fandoms)];
                userToSerialize.LastStreakVerification = user.LastStreakVerification ?? userToSerialize.LastStreakVerification;
                if (user.PendActions != null)
                {
                    userToSerialize.PendActions ??= [];
                    userToSerialize.PendActions = [.. userToSerialize.PendActions.Union(user.PendActions)];
                }
                userToSerialize.PrimaryRole = user.PrimaryRole;
                userToSerialize.Pronouns = [.. userToSerialize.Pronouns.Union(user.Pronouns)];
                if (user.Roles != null)
                {
                    userToSerialize.Roles ??= [];
                    userToSerialize.Roles = [.. userToSerialize.Roles.Union(user.Roles)];
                }
                userToSerialize.Streak = user.Streak;
                userToSerialize.Type = user.Type;
                userToSerialize.Version = Math.Max(existingEntry.Version, user.Version) + 1;
                if (user.WantedRoles != null)
                {
                    userToSerialize.WantedRoles ??= [];
                    userToSerialize.WantedRoles = [.. userToSerialize.WantedRoles.Union(user.WantedRoles)];
                }
                break;

            case ConflictMode.CombineKeepingOriginal:
                updatedEntry = new IndexEntry
                {
                    PrimaryRole = existingEntry.PrimaryRole,
                    Id = existingEntry.Id,
                    Version = Math.Max(existingEntry.Version, user.Version) + 1,
                    AvatarImage = existingEntry.AvatarImage ?? user.AvatarImage,
                    Path = existingEntry.Path
                };
                userToSerialize = Query.LoadUserById(user.Id, gConfig);
                if (userToSerialize == null)
                {
                    userToSerialize = user;
                    break;
                }
                userToSerialize.Fandoms = [.. userToSerialize.Fandoms.Union(user.Fandoms)];
                if (user.PendActions != null)
                {
                    userToSerialize.PendActions ??= [];
                    userToSerialize.PendActions = [.. userToSerialize.PendActions.Union(user.PendActions)];
                }
                userToSerialize.Pronouns = [.. userToSerialize.Pronouns.Union(user.Pronouns)];
                if (user.Roles != null)
                {
                    userToSerialize.Roles ??= [];
                    userToSerialize.Roles = [.. userToSerialize.Roles.Union(user.Roles)];
                }
                userToSerialize.Version = Math.Max(existingEntry.Version, user.Version) + 1;
                if (user.WantedRoles != null)
                {
                    userToSerialize.WantedRoles ??= [];
                    userToSerialize.WantedRoles = [.. userToSerialize.WantedRoles.Union(user.WantedRoles)];
                }
                break;

            default:
                return ImportResult.DefaultException;
        }

        // Reemplazar la entrada en la lista sin romper la iteración
        index[existingIndex] = updatedEntry;

        // Guardar usuario en disco
        string userPath = Path.Combine(gConfig.DataBasePath, existingEntry.Path);
        try
        {
            byte[] serializedUser = MessagePackSerializer.Serialize(userToSerialize);
            File.WriteAllBytes(userPath, serializedUser);
        }
        catch (DirectoryNotFoundException) { return ImportResult.DirectoryNotFoundException; }
        catch (UnauthorizedAccessException) { return ImportResult.UnauthorizedAccessException; }
        catch (IOException) { return ImportResult.IOException; }
        catch { return ImportResult.DefaultException; }

        // Actualizar el índice en disco
        return OverWriteIndex(index, gConfig);
    }

    private static ImportResult OverWriteIndex(List<IndexEntry> index, Config gConfig)
    {
        string indexPath = Path.Combine(gConfig.ConfigPath, "index.ivdb");
        string tempIndexPath = Path.Combine(gConfig.ConfigPath, "index.ivdb.tmp");
        string backupIndexPath = Path.Combine(gConfig.ConfigPath, "index.ivdb.bak");
        try
        {
            byte[] serializedIndex = MessagePackSerializer.Serialize(index);
            File.WriteAllBytes(tempIndexPath, serializedIndex);
            File.Copy(indexPath, backupIndexPath, overwrite: true);
            File.Copy(tempIndexPath, indexPath, overwrite: true);
        }
        catch (DirectoryNotFoundException) { return ImportResult.DirectoryNotFoundException; }
        catch (UnauthorizedAccessException) { return ImportResult.UnauthorizedAccessException; }
        catch (IOException) { return ImportResult.IOException; }
        catch { return ImportResult.DefaultException; }

        return ImportResult.Success;
    }

    private static ImportResult MapWriteResult(WriteResult result) => result switch
    {
        WriteResult.DirectoryNotFoundException => ImportResult.DirectoryNotFoundException,
        WriteResult.DefaultException => ImportResult.Failed,
        WriteResult.InvalidUserException => ImportResult.InvalidUser,
        WriteResult.Success => ImportResult.Success,
        WriteResult.UnauthorizedAccessException => ImportResult.UnauthorizedAccessException,
        WriteResult.IOException => ImportResult.IOException,
        _ => ImportResult.DefaultException
    };
}

public enum ConflictMode
{
    CombineKeepingOriginal,
    CombineKeepingNew,
    OverWrite,
    Skip,
    Fail
}

public enum ImportResult
{
    Combined,
    Skipped,
    OverWrited,
    Failed,
    InvalidUser,
    DirectoryNotFoundException,
    UnauthorizedAccessException,
    IOException,
    DefaultException,
    Success
}