using System.CommandLine;
using System.Security.Cryptography;

namespace VanguardCore.Builders;

public static class SearchCommand
{
    public static Command Build(Config gConfig)
    {
        
        var command = new Command("search", "Buscar un usuario en la base de datos");

        var idOption = new Option<string>("-u", "--user-id") { Description = "ID de usuario" };
        var primaryRolOption = new Option<string>("-p", "--primary-rol") { Description = "Rol principal asignado" };
        var fastSearchOption = new Option<bool>("-f", "--fast") { Description = "Acelera las búsquedas por nombre e ID" };
        var rolOption = new Option<string>("-r", "--rol") { Description = "Rol asignado" };
        var wantedRolOption = new Option<string>("-w", "--wanted-rol") { Description = "Rol buscado" };
        var pronounOption = new Option<string>("-P", "--pronoun") { Description = "Pronombre asignado" };
        var ageOption = new Option<int?>("-a", "--age") { Description = "Edad asignada" };
        var minAgeOption = new Option<int?>("--min-age") { Description = "Rango mínimo de edad" };
        var maxAgeOption = new Option<int?>("--max-age") { Description = "Rango máximo de edad" };
        var creationTimeOption = new Option<DateTime?>("-d", "--date-created") { Description = "Fecha de registro" };
        var minCreationTimeOption = new Option<DateTime?>("--min-date-created") { Description = "Fecha mínima de registro" };
        var maxCreationTimeOption = new Option<DateTime?>("--max-date-created") { Description = "Fecha máxima de registro" };
        var lastStreakTimeOption = new Option<DateOnly?>("-l", "--last-streak-date") { Description = "Fecha de registro" };
        var minLastStreakTimeOption = new Option<DateOnly?>("--min-last-streak-date") { Description = "Fecha mínima de verificación de racha" };
        var maxLastStreakOption = new Option<DateOnly?>("--max-last-streak-date") { Description = "Fecha máxima de verificación de racha" };
        var typeOption = new Option<UserType?>("-t", "--type") { Description = "Tipo de usuario" };
        var fandomOption = new Option<string>("-F", "--fandom") { Description = "Fandom asignado" };
        var rawOption = new Option<bool>("--raw") { Description = "Imprime la salida en JSON" };

        ageOption.DefaultValueFactory = null;
        minAgeOption.DefaultValueFactory = null;
        maxAgeOption.DefaultValueFactory = null;
        creationTimeOption.DefaultValueFactory = null;
        minCreationTimeOption.DefaultValueFactory = null;
        maxCreationTimeOption.DefaultValueFactory = null;
        lastStreakTimeOption.DefaultValueFactory = null;
        maxLastStreakOption.DefaultValueFactory = null;
        typeOption.DefaultValueFactory = null;

        command.Add(idOption);
        command.Add(primaryRolOption);
        command.Add(fastSearchOption);
        command.Add(rolOption);
        command.Add(wantedRolOption);
        command.Add(pronounOption);
        command.Add(ageOption);
        command.Add(minAgeOption);
        command.Add(maxAgeOption);
        command.Add(creationTimeOption);
        command.Add(minCreationTimeOption);
        command.Add(maxCreationTimeOption);
        command.Add(lastStreakTimeOption);
        command.Add(minLastStreakTimeOption);
        command.Add(maxLastStreakOption);
        command.Add(typeOption);
        command.Add(fandomOption);
        command.Add(rawOption);
        command.Add(rawOption);

        command.SetAction(p =>
        {
            string? primaryRole = p.GetValue(primaryRolOption);
            string? id = p.GetValue(idOption);
            bool fastSearch = p.GetValue(fastSearchOption);
            string? rol = p.GetValue(rolOption);
            string? wantedRol = p.GetValue(wantedRolOption);
            string? pronoun = p.GetValue(pronounOption);
            int? age = p.GetValue(ageOption);
            int? minAge = p.GetValue(minAgeOption);
            int? maxAge = p.GetValue(maxAgeOption);
            DateTime? creationTime = p.GetValue(creationTimeOption);
            DateTime? minCreationTime = p.GetValue(minCreationTimeOption);
            DateTime? maxCreationTime = p.GetValue(maxCreationTimeOption);
            DateOnly? lastStreak = p.GetValue(lastStreakTimeOption);
            DateOnly? minLastStreak = p.GetValue(minLastStreakTimeOption);
            DateOnly? maxLastStreak = p.GetValue(maxLastStreakOption);
            UserType? type = p.GetValue(typeOption);
            string? fandom = p.GetValue(fandomOption);
            bool raw = p.GetValue(rawOption);

            SearchFilters filters = new()
            {
                PrimaryRole = primaryRole,
                Id = id,
                FastSearch = fastSearch,
                Rol = rol,
                WantedRol = wantedRol,
                Pronoun = pronoun,
                Age = age,
                MinAge = minAge,
                MaxAge = maxAge,
                CreationTime = creationTime,
                MinCreationTime = minCreationTime,
                MaxCreationTime = maxCreationTime,
                LastStreakVerification = lastStreak,
                MinLastStreakVerification = minLastStreak,
                MaxLastStreakVerification = maxLastStreak,
                Type = type,
                Fandom = fandom,
                RawOutput = raw
            };
            
            Commands.SearchCommand.Run(filters, gConfig);
        });

        return command;
    }
}
