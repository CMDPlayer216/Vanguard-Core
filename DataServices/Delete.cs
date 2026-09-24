using MessagePack;

namespace VanguardCore.DataServices;

public static class Delete
{
    public static DeleteResult User(string Id, Config gConfig)
    {
        using var dbLock = new DatabaseLock(gConfig.ConfigPath);
        if (!dbLock.Acquire())
        {
            DrawError("ERROR: la base de datos está bloqueada.", Color.Red);
            
        }
        List<IndexEntry>? Index = Query.Index(gConfig);
        if (Index == null || Index.Count == 0) return DeleteResult.UserDoNotExistException;
        foreach (IndexEntry entry in Index)
        {
            if (entry.Id == Id)
            {
                try
                {
                    List<IndexEntry> newIndex = [.. Index];
                    newIndex.Remove(entry);
                    string indexPath = Path.Combine(gConfig.ConfigPath, "index.ivdb");
                    byte[] serializedIndex = MessagePackSerializer.Serialize(newIndex);
                    File.WriteAllBytes(indexPath, serializedIndex);
                    File.Delete(Path.Combine(gConfig.DataBasePath, entry.Path));
                    
                    return DeleteResult.Success;
                }
                catch (DirectoryNotFoundException)
                {
                    return DeleteResult.DirectoryNotFoundException;
                }
                catch (UnauthorizedAccessException)
                {
                    return DeleteResult.UnauthorizedAccessException;
                }
                catch (IOException)
                {
                    return DeleteResult.IOException;
                }
                catch
                {
                    return DeleteResult.DefaultException;
                }
            }
        }

        return DeleteResult.UserDoNotExistException;
    }
}

public enum DeleteResult
{
    DataBaseIsLockedException,
    DirectoryNotFoundException,
    DefaultException,
    Success,
    UnauthorizedAccessException,
    IOException,
    UserDoNotExistException
}