using System;
using System.Diagnostics;
using System.IO;

namespace VanguardCore.DataServices;

public class DatabaseLock : IDisposable
{
    private readonly string _lockFilePath;
    private bool _isLocked = false;

    public DatabaseLock(string dbDirectory)
    {
        _lockFilePath = Path.Combine(dbDirectory, "vanguarddb.lock");

        // Suscribirse a las interrupciones del sistema
        Console.CancelKeyPress += OnCancelKeyPress;
        AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
    }

    public bool Acquire()
    {
        try
        {
            // Intentar crear el archivo de forma atómica
            using (FileStream fs = File.Open(_lockFilePath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            using (StreamWriter writer = new(fs))
            {
                writer.Write(Environment.ProcessId.ToString());
            }

            _isLocked = true;
            return true;
        }
        catch (IOException)
        {
            // El archivo YA EXISTE.
            // Si está huérfano, se elimina y se reintenta adquirir.
            if (IsStaleLock())
            {
                DeleteLockFile();
                return Acquire(); // Reintentar adquisición
            }

            // Está bloqueado por otro proceso activo
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error] No se pudo crear el archivo lock: {ex.Message}");
            return false;
        }
    }

    public void Release()
    {
        if (_isLocked)
        {
            DeleteLockFile();
            _isLocked = false;
        }
    }

    private bool IsStaleLock()
    {
        try
        {
            if (File.Exists(_lockFilePath))
            {
                string content = File.ReadAllText(_lockFilePath).Trim();
                if (int.TryParse(content, out int pid))
                {
                    // Si el proceso guardado NO está corriendo, el lock es huérfano
                    Process.GetProcessById(pid);
                    return false; // El proceso sigue vivo
                }
            }
        }
        catch (ArgumentException)
        {
            // GetProcessById lanza ArgumentException si el PID ya no existe
            return true; // Lock huérfano
        }
        catch
        {
            // Ante cualquier error de lectura/acceso, se asume que no se puede considerar huérfano
        }

        return false;
    }

    private void DeleteLockFile()
    {
        try
        {
            if (File.Exists(_lockFilePath))
            {
                File.Delete(_lockFilePath);
            }
        }
        catch { }
    }

    private void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
    {
        Release();
    }

    private void OnProcessExit(object? sender, EventArgs e)
    {
        Release();
    }

    public void Dispose()
    {
        Release();
        Console.CancelKeyPress -= OnCancelKeyPress;
        AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
    }
}