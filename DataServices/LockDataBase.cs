using System;
using System.IO;

namespace VanguardCore.DataServices;

public class DatabaseLock : IDisposable
{
    private readonly string _lockFilePath;
    private FileStream? _lockStream;
    private bool _hasAcquiredLock = false;

    public DatabaseLock(string configDirectory)
    {
        _lockFilePath = Path.Combine(configDirectory, "vanguarddb.lock");
    }

    public bool Acquire()
    {
        try
        {
            // Mantenemos el FileStream ABIERTO con FileShare.None.
            // Mientras _lockStream esté abierto, el sistema operativo le prohibirá 
            // a CUALQUIER otro proceso/hilo abrir, escribir o modificar este archivo.
            _lockStream = new FileStream(
                _lockFilePath,
                FileMode.OpenOrCreate,
                FileAccess.ReadWrite,
                FileShare.None,
                4096,
                FileOptions.DeleteOnClose); // Se auto-elimina al cerrar el proceso/stream
            _hasAcquiredLock = true;

            return true;
        }
        catch (IOException)
        {
            // El SO impidió abrir el archivo porque OTRO proceso tiene el FileStream activo.
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error] No se pudo adquirir el lock: {ex.Message}");
            return false;
        }
    }

    public void Release()
    {
        if (_lockStream != null)
        {
            _lockStream.Close();
            _lockStream.Dispose();
            _lockStream = null;
        }

        // Por seguridad, aseguramos borrado si DeleteOnClose falló por el SO
        try
        {
            if (_hasAcquiredLock && File.Exists(_lockFilePath))
            {
                File.Delete(_lockFilePath);
                _hasAcquiredLock = false;
            }
        }
        catch { }
    }

    public void Dispose()
    {
        Release();
    }
}