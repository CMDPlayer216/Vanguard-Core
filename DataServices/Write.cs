using MessagePack;
using static VanguardCore.DataServices.Query;
using static VanguardCore.Validators.UserValidators;

namespace VanguardCore.DataServices;

public static class Write
{
    public static WriteResult AddUser(User user, Config gConfig)
    {
        using var dbLock = new DatabaseLock(gConfig.ConfigPath);
        if (!dbLock.Acquire())
        {
            DrawText("ERROR: la base de datos está bloqueada.", Color.Red);
            Environment.Exit(2);
        }
        bool isValidUser = ValidateUser(user);
        if (!isValidUser) return WriteResult.InvalidUserException;
        IndexEntry newEntry = new() { PrimaryRole = user.PrimaryRole, Id = user.Id, AvatarImage = user.AvatarImage, Version = user.Version };
        byte[] serializedUser = MessagePackSerializer.Serialize(user);
        string userPath = Path.Combine(gConfig.DataBasePath, newEntry.Path);
        try
        {
            File.WriteAllBytes(userPath, serializedUser);
        }
        catch (DirectoryNotFoundException)
        {
            return WriteResult.DirectoryNotFoundException;
        }
        catch (UnauthorizedAccessException)
        {
            return WriteResult.UnauthorizedAccessException;
        }
        catch (IOException)
        {
            return WriteResult.IOException;
        }
        catch
        {
            return WriteResult.DefaultException;
        }
        WriteResult result = AddIndexEntry(newEntry, gConfig);
        if (result != WriteResult.Success)
        {
            try
            {
                File.Delete(userPath);
            }
            catch { }

            return result;
        }
        return result;
    }
    private static WriteResult AddIndexEntry(IndexEntry Entry, Config gConfig, List<IndexEntry>? Index = null)
    {
        using var dbLock = new DatabaseLock(gConfig.ConfigPath);
        if (!dbLock.Acquire())
        {
            DrawText("ERROR: la base de datos está bloqueada.", Color.Red);
            Environment.Exit(2);
        }
        Index ??= Query.Index(gConfig);
        Index ??= [];
        foreach (IndexEntry indexEntry in Index)
        {
            if (indexEntry.Id == Entry.Id) return WriteResult.UserAlreadyExistsException;
        }

        Index.Add(Entry);
        string indexPath = Path.Combine(gConfig.ConfigPath, "index.ivdb");
        byte[] serializedIndex = MessagePackSerializer.Serialize(Index);
        try
        {
            File.WriteAllBytes(indexPath, serializedIndex);
        }
        catch (DirectoryNotFoundException)
        {
            return WriteResult.DirectoryNotFoundException;
        }
        catch (UnauthorizedAccessException)
        {
            return WriteResult.UnauthorizedAccessException;
        }
        catch (IOException)
        {
            return WriteResult.IOException;
        }
        catch
        {
            return WriteResult.DefaultException;
        }
        return WriteResult.Success;
    }
    public static WriteResult OverWriteIndex(Config gConfig, List<IndexEntry> Index)
    {
        using var dbLock = new DatabaseLock(gConfig.ConfigPath);
        if (!dbLock.Acquire())
        {
            DrawText("ERROR: la base de datos está bloqueada.", Color.Red);
            Environment.Exit(2);
        }
        string indexPath = Path.Combine(gConfig.ConfigPath, "index.ivdb");
        byte[] serializedIndex = MessagePackSerializer.Serialize(Index);
        try
        {
            File.WriteAllBytes(indexPath, serializedIndex);
        }
        catch (DirectoryNotFoundException)
        {
            return WriteResult.DirectoryNotFoundException;
        }
        catch (UnauthorizedAccessException)
        {
            return WriteResult.UnauthorizedAccessException;
        }
        catch (IOException)
        {
            return WriteResult.IOException;
        }
        catch
        {
            return WriteResult.DefaultException;
        }
        return WriteResult.Success;
    }

}

public enum WriteResult
{
    DataBaseIsLockedException,
    UserAlreadyExistsException,
    DirectoryNotFoundException,
    DefaultException,
    InvalidUserException,
    EmptyEntryException,
    EntryAlradyExists,
    Success,
    UnauthorizedAccessException,
    IOException
}
