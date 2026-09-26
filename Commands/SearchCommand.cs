using System.Text.Json;
using VanguardCore.DataServices;

namespace VanguardCore.Commands;

public static class SearchCommand
{
    public static void Run(SearchFilters filters, Config gConfig)
    {
        bool isFastSearchAviable =
            filters.Rol == null && filters.WantedRol == null &&
            filters.Pronoun == null && filters.MinAge == null &&
            filters.MaxAge == null && filters.MinCreationTime == null &&
            filters.MaxCreationTime == null && filters.LastStreakVerification == null &&
            filters.Type == null && filters.MinStreak == null &&
            filters.MaxStreak == null && filters.Fandom == null &&
            filters.Age == null && filters.CreationTime == null &&
            filters.Streak == null && filters.MinLastStreakVerification == null &&
            filters.MaxLastStreakVerification == null;

        List<IndexEntry> matches = [];
        List<IndexEntry>? Index = Query.Index(gConfig);
        if (Index == null)
        {
            DrawError("No hay usuarios registrados.", Color.Red);
            return;
        }
        if (isFastSearchAviable && filters.FastSearch)
        {
            foreach (IndexEntry entry in Index)
            {
                if (filters.PrimaryRole != null && entry.PrimaryRole.Equals(filters.PrimaryRole, StringComparison.OrdinalIgnoreCase))
                {
                    matches.Add(entry);
                    continue;
                }
                if (filters.Id != null && entry.Id == filters.Id)
                {
                    matches.Add(entry);
                    continue;
                }
                if (filters.PrimaryRole != null && entry.PrimaryRole.Contains(filters.PrimaryRole, StringComparison.OrdinalIgnoreCase))
                {
                    matches.Add(entry);
                    continue;
                }
                if (filters.Id != null && entry.Id.Contains(filters.Id))
                {
                    matches.Add(entry);
                    continue;
                }
            }
        }
        else
        {
            foreach (IndexEntry entry in Index)
            {
                User? user = Query.LoadUserByFileName(entry.Path, gConfig);
                if (user == null)
                {
                    DrawError($"Usuario inválido encontrado en {entry.Path}");
                    continue;
                }
                // Primero comprobamos números
                if (filters.Type.HasValue && filters.Type == user.Type)
                {
                    matches.Add(entry);
                    break;
                }
                if (filters.Age.HasValue && filters.Age == user.Age)
                {
                    matches.Add(entry);
                    continue;
                }
                if (filters.Streak.HasValue && filters.Streak == user.Streak)
                {
                    matches.Add(entry);
                    continue;
                }
                if (IsInRangeMatch(filters.MinAge, filters.MaxAge, user.Age))
                {
                    matches.Add(entry);
                    continue;
                }
                if (IsInRangeMatch(filters.MinStreak, filters.MaxStreak, user.Streak))
                {
                    matches.Add(entry);
                    continue;
                }
                // Ahora comprobamos fechas
                if (filters.CreationTime.HasValue && filters.CreationTime == user.CreationTime)
                {
                    matches.Add(entry);
                    continue;
                }
                if (filters.LastStreakVerification.HasValue && filters.LastStreakVerification == user.LastStreakVerification)
                {
                    matches.Add(entry);
                    continue;
                }
                if (IsInRangeMatch(filters.MinCreationTime, filters.MaxCreationTime, user.CreationTime))
                {
                    matches.Add(entry);
                    continue;
                }
                if (user.LastStreakVerification.HasValue && IsInRangeMatch(filters.MinLastStreakVerification, filters.MaxLastStreakVerification, user.LastStreakVerification))
                {
                    matches.Add(entry);
                    continue;
                }
                // Ahora cadenas
                if (filters.PrimaryRole != null && user.PrimaryRole.Equals(filters.PrimaryRole, StringComparison.OrdinalIgnoreCase))
                {
                    matches.Add(entry);
                    continue;
                }
                if (filters.Id != null && filters.Id == user.Id)
                {
                    matches.Add(entry);
                    continue;
                }
                if (filters.Id != null && user.Id.Contains(filters.Id))
                {
                    matches.Add(entry);
                    continue;
                }
                if (filters.PrimaryRole != null && user.PrimaryRole.Contains(filters.PrimaryRole, StringComparison.OrdinalIgnoreCase))
                {
                    matches.Add(entry);
                    continue;
                }
                // Ahora listas
                bool match = false;
                if (filters.Pronoun != null)
                {
                    // Exacta
                    foreach (string pronoun in user.Pronouns)
                    {
                        if (pronoun.Equals(filters.Pronoun, StringComparison.OrdinalIgnoreCase))
                        {
                            match = true;
                            break;
                        }
                    }
                    // Parcial
                    if (!match)
                    {
                        foreach (string pronoun in user.Pronouns)
                        {
                            if (pronoun.Contains(filters.Pronoun, StringComparison.OrdinalIgnoreCase))
                            {
                                match = true;
                                break;
                            }
                        }
                    }
                    if (match)
                    {
                        matches.Add(entry);
                        continue;
                    }
                }
                if (filters.Rol != null && user.Roles != null)
                {
                    // Exacta
                    foreach (string rol in user.Roles)
                    {
                        if (rol.Equals(filters.Rol, StringComparison.OrdinalIgnoreCase))
                        {
                            match = true;
                            break;
                        }
                    }
                    // Parcial
                    if (!match)
                    {
                        foreach (string rol in user.Roles)
                        {
                            if (rol.Contains(filters.Rol, StringComparison.OrdinalIgnoreCase))
                            {
                                match = true;
                                break;
                            }
                        }
                    }
                    if (match)
                    {
                        matches.Add(entry);
                        continue;
                    }
                }
                match = false;
                if (filters.WantedRol != null && user.WantedRoles != null)
                {
                    // Exacta
                    foreach (string rol in user.WantedRoles)
                    {
                        if (rol.Equals(filters.WantedRol, StringComparison.OrdinalIgnoreCase))
                        {
                            match = true;
                            break;
                        }
                    }
                    // Parcial
                    if (!match)
                    {
                        foreach (string rol in user.WantedRoles)
                        {
                            if (rol.Contains(filters.WantedRol, StringComparison.OrdinalIgnoreCase))
                            {
                                match = true;
                                break;
                            }
                        }
                    }
                    if (match)
                    {
                        matches.Add(entry);
                        continue;
                    }
                }
                match = false;
                if (filters.Fandom != null)
                {
                    // Exacta
                    foreach (string fandom in user.Fandoms)
                    {
                        if (fandom.Equals(filters.Fandom, StringComparison.OrdinalIgnoreCase))
                        {
                            match = true;
                            break;
                        }
                    }
                    // Parcial
                    if (!match)
                    {
                        foreach (string fandom in user.Fandoms)
                        {
                            if (fandom.Contains(filters.Fandom, StringComparison.OrdinalIgnoreCase))
                            {
                                match = true;
                                break;
                            }
                        }
                    }
                    if (match)
                    {
                        matches.Add(entry);
                        continue;
                    }
                }
            }
        }
        if (matches.Count == 0)
        {
            DrawError("No hay resultados.", Color.Red);
            return;
        }
        if (filters.RawOutput)
        {
            string? entryJson = JsonSerializer.Serialize(matches);
            DrawText(entryJson);
        }
        else
        {

            foreach (IndexEntry result in matches)
            {
                DrawText($"{result.Id}", Color.Green, false);
                DrawText(" | ", Color.Gray, false);
                DrawText($"{result.PrimaryRole}");
            }
            DrawText("ID          ", Color.Yellow, false);
            DrawText(" | ", Color.Gray, false);
            DrawText("Rol principal", Color.Yellow);
        }
    }

    private static bool IsInRangeMatch(int? min, int? max, int value)
    {
        return (min.HasValue || max.HasValue) &&
               (!min.HasValue || value >= min.Value) &&
               (!max.HasValue || value <= max.Value);
    }
    private static bool IsInRangeMatch(DateTime? min, DateTime? max, DateTime value)
    {
        return (min.HasValue || max.HasValue) &&
               (!min.HasValue || value >= min.Value) &&
               (!max.HasValue || value <= max.Value);
    }
    private static bool IsInRangeMatch(DateOnly? min, DateOnly? max, DateOnly? value)
    {
        return (min.HasValue || max.HasValue) &&
               (!min.HasValue || value >= min.Value) &&
               (!max.HasValue || value <= max.Value);
    }
}
