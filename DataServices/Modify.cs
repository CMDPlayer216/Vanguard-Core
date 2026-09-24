using MessagePack;
using static VanguardCore.Validators.UserValidators;
namespace VanguardCore.DataServices;

public static class Modify
{
    public static ModifyResult User(ModifyingUser changes, Config gConfig)
    {
        using var dbLock = new DatabaseLock(gConfig.ConfigPath);
        if (!dbLock.Acquire())
        {
            DrawText("ERROR: la base de datos está bloqueada.", Color.Red);
            
        }
        if (!ValidateModifications(changes)) return ModifyResult.InvalidModificationsException;
        List<IndexEntry>? Index = Query.Index(gConfig);
        if (Index == null || Index.Count == 0) return ModifyResult.UserDoNotExistException;
        IndexEntry? oldEntry = null;
        IndexEntry? newEntry = null;
        foreach (IndexEntry entry in Index)
        {
            if (entry.Id == changes.Id)
            {
                oldEntry = entry;
                break;
            }
        }
        if (oldEntry == null) return ModifyResult.UserDoNotExistException;
        newEntry = oldEntry;
        User? user = Query.LoadUserByFileName(oldEntry.Path, gConfig);
        if (user == null) return ModifyResult.CorruptUserException;

        List<string> newPronouns = [.. user.Pronouns];
        if (changes.RemovePronouns != null)
        {
            foreach (string pronoun in user.Pronouns)
            {
                if (changes.RemovePronouns.Contains(pronoun)) newPronouns.Remove(pronoun);
            }
        }
        if (changes.AddPronouns != null) newPronouns.AddRange(changes.AddPronouns);
        user.Pronouns = newPronouns;

        List<string>? newRoles = user.Roles;
        if (changes.AddRoles != null || changes.RemoveRoles != null)
        {
            newRoles ??= [];
            if (changes.RemoveRoles != null && user.Roles != null)
            {
                foreach (string rol in user.Roles)
                {
                    if (changes.RemoveRoles.Contains(rol)) newRoles.Remove(rol);
                }
            }
            if (changes.AddRoles != null) newRoles.AddRange(changes.AddRoles);
            user.Roles = newRoles;
        }

        List<string>? newWantedRoles = user.WantedRoles;

        if (changes.AddWantedRoles != null || changes.RemoveWantedRoles != null)
        {
            newWantedRoles ??= [];
            if (changes.RemoveWantedRoles != null && user.WantedRoles != null)
            {
                foreach (string rol in user.WantedRoles)
                {
                    if (changes.RemoveWantedRoles.Contains(rol)) newWantedRoles.Remove(rol);
                }
            }
            if (changes.AddWantedRoles != null) newWantedRoles.AddRange(changes.AddWantedRoles);
            user.WantedRoles = newWantedRoles;

        }
        if (changes.Age != null) user.Age = changes.Age.Value;
        if (changes.AvatarImage != null)
        {
            user.AvatarImage = changes.AvatarImage;
            newEntry.AvatarImage = changes.AvatarImage;
        }
        if (changes.LastStreakVerification != null) user.LastStreakVerification = changes.LastStreakVerification;

        List<UserAction>? newPendActions = user.PendActions;

        if (changes.AddPendActions != null || changes.RemovePendActions != null)
        {
            newPendActions ??= [];
            if (changes.RemovePendActions != null && user.PendActions != null)
            {
                foreach (UserAction action in user.PendActions)
                {
                    if (changes.RemovePendActions.Contains(action)) newPendActions.Remove(action);
                }
            }
            if (changes.AddPendActions != null) newPendActions.AddRange(changes.AddPendActions);
            user.PendActions = newPendActions;
        }

        if (changes.PrimaryRole != null)
        {
            user.PrimaryRole = changes.PrimaryRole;
            newEntry.PrimaryRole = user.PrimaryRole;
        }
        if (changes.Streak != null) user.Streak = changes.Streak.Value;
        if (changes.Type != null) user.Type = changes.Type.Value;

        newEntry.Version++;
        user.Version++;

        Index.Remove(oldEntry);
        Index.Add(newEntry);

        string userPath = Path.Combine(gConfig.DataBasePath, newEntry.Path);
        byte[] serializedUser = MessagePackSerializer.Serialize(user);
        try
        {
            File.WriteAllBytes(userPath, serializedUser);
        }
        catch (DirectoryNotFoundException)
        {
            return ModifyResult.DirectoryNotFoundException;
        }
        catch (UnauthorizedAccessException)
        {
            return ModifyResult.UnauthorizedAccessException;
        }
        catch (IOException)
        {
            return ModifyResult.IOException;
        }
        catch
        {
            return ModifyResult.DefaultException;
        }

        WriteResult result = Write.OverWriteIndex(gConfig, Index);

        return result switch
        {
            WriteResult.DirectoryNotFoundException => ModifyResult.DirectoryNotFoundException,
            WriteResult.UnauthorizedAccessException => ModifyResult.UnauthorizedAccessException,
            WriteResult.IOException => ModifyResult.IOException,
            WriteResult.DefaultException => ModifyResult.DefaultException,
            WriteResult.Success => ModifyResult.Success,
            _ => ModifyResult.DefaultException,
        };
    }
}

public enum ModifyResult
{
    DataBaseIsLockedException,
    ImposibleModificationException,
    DirectoryNotFoundException,
    DefaultException,
    VersionAlreadyExistsException,
    InvalidUserException,
    EmptyEntryException,
    EntryAlradyExists,
    Success,
    UnauthorizedAccessException,
    IOException,
    InvalidModificationsException,
    UserDoNotExistException,
    CorruptUserException
}