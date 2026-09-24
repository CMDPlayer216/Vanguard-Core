using System.Text.Json;
using VanguardCore.DataServices;

namespace VanguardCore.Commands;

public static class ConsultCommand
{
    public static void Run(string Id, bool isRaw, Config gConfig)
    {
        List<IndexEntry>? Index = Query.Index(gConfig);
        if (Index == null)
        {
            DrawError("No hay usuarios registrados.", Color.Red);
            return;
        }
        User? user = null;
        foreach (IndexEntry? entry in Index)
        {
            if (entry.Id == Id)
            {
                user = Query.LoadUserByFileName(entry.Path, gConfig);
                break;
            }
        }
        if (user == null)
        {
            DrawError("El usuario está corrupto o no existe.", Color.Red);
            return;
        }
        if (isRaw)
        {
            string? userSerialized = "";
            userSerialized = JsonSerializer.Serialize(user);
            DrawText(userSerialized);
        }
        else
        {
            DrawText("====== DATOS DE USUARIO ======", Color.Red);
            DrawText($"Rol principal: {user.PrimaryRole}");
            DrawText($"ID: {user.Id}");
            DrawText($"Edad: {user.Age}");
            DrawText($"Fecha de registro: {user.CreationTime}");
            DrawText($"Tipo de usuario: {user.Type}");
            DrawText("Pronombres:");
            foreach (string pronoun in user.Pronouns)
            {
                DrawText($"  - {pronoun}");
            }
            DrawText("Fandoms:");
            foreach (string fandom in user.Fandoms)
            {
                DrawText($"  - {fandom}");
            }
            if (user.Roles != null && user.Roles.Count != 0)
            {
                DrawText("Roles adicionales:");
                foreach (string rol in user.Roles)
                {
                    DrawText($"  - {rol}");
                }
            }
            if (user.WantedRoles != null && user.WantedRoles.Count != 0)
            {
                DrawText("Roles buscados:");
                foreach (string rol in user.WantedRoles)
                {
                    DrawText($"  - {rol}");
                }
            }
            string LastStreakVerificationPlaceholder = user.LastStreakVerification?.ToString() ?? "Nunca";

            DrawText($"Última verificación de racha: {LastStreakVerificationPlaceholder}");
            DrawText($"Racha actual: {user.Streak}");
            if (user.PendActions != null && user.PendActions.Count != 0)
            {
                DrawText("Acciones pendientes:");
                bool isFirstRendered = false;
                foreach (UserAction action in user.PendActions)
                {
                    if (isFirstRendered) DrawText("=======================================");
                    else isFirstRendered = true;
                    DrawText($"  - Tipo: {action.ActionType}");
                    DrawText($"  - Fecha para ejecutar: {action.ActionTimeTrigger}");
                    DrawText($"  - Razón: {action.Reason}");
                }
            }
        }
    }
}

// PrimaryRole, Id