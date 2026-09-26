namespace VanguardCore.Validators;

public static class UserValidators
{
    public const string IdValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-_1234567890";
    public const string OtherValidChars = $"{IdValidChars}üÜáéíóúÁÉÍÓÚçÇ. ";
    public const int MinValidAge = 0;
    public const int MinValidStreak = 0;
    public const int MinValidVersion = 1;

    public static bool ValidateId(string Id)
    {
        if (string.IsNullOrEmpty(Id) || string.IsNullOrWhiteSpace(Id)) return false;
        foreach (char c in Id)
        {
            if (!IdValidChars.Contains(c)) return false;
        }

        return true;
    }

    public static bool ValidateStreak(int streak)
    {
        return streak >= MinValidStreak;
    }
    public static bool ValidateAge(int age)
    {
        return age >= MinValidAge;
    }
    public static bool ValidateUnnecesaryAge(int? age)
    {
        if (age == null) return true;
        return age >= MinValidAge;
    }
    public static bool ValidateUnnecesaryStreak(int? streak)
    {
        if (streak == null) return true;
        return streak >= MinValidAge;
    }
    public static bool ValidateString(string text)
    {
        foreach (char c in text)
        {
            if (!OtherValidChars.Contains(c)) return false;
        }

        return true;
    }
    public static bool ValidateUnnecesaryString(string? text)
    {
        if (text == null) return true;
        foreach (char c in text)
        {
            if (!OtherValidChars.Contains(c)) return false;
        }

        return true;
    }
    public static bool ValidateUnnecesaryStrings(List<string>? strings)
    {
        if (strings == null) return true;
        foreach (string s in strings)
        {
            if (!ValidateUnnecesaryString(s)) return false;
        }
        return true;
    }
    public static bool ValidateStrings(List<string> strings)
    {
        foreach (string s in strings)
        {
            if (!ValidateString(s)) return false;
        }
        return true;
    }
    public static bool ValidateVersion(int version)
    {
        return version >= MinValidVersion;
    }

    public static bool ValidateUser(User user)
    {
        return ValidateAge(user.Age) && ValidateId(user.Id)
            && ValidateStreak(user.Streak) && ValidateString(user.PrimaryRole)
            && ValidateStrings(user.Pronouns) && ValidateUnnecesaryStrings(user.Roles)
            && ValidateUnnecesaryStrings(user.WantedRoles) && ValidateVersion(user.Version)
            && ValidateStrings(user.Fandoms);
    }
    public static bool ValidateModifications(ModifyingUser user)
    {
        return ValidateUnnecesaryAge(user.Age) && ValidateId(user.Id)
            && ValidateUnnecesaryStreak(user.Streak) && ValidateUnnecesaryString(user.PrimaryRole)
            && ValidateUnnecesaryStrings(user.AddPronouns) && ValidateUnnecesaryStrings(user.AddRoles)
            && ValidateUnnecesaryStrings(user.AddWantedRoles) && ValidateUnnecesaryStrings(user.RemoveWantedRoles)
            && ValidateUnnecesaryStrings(user.RemovePronouns) && ValidateUnnecesaryStrings(user.RemoveRoles);
    }
}
