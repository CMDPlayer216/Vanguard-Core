# 1. Definir la ruta de instalación local (ej. C:\Users\Nombre\AppData\Local\UserDB)
$installDir = "$env:LOCALAPPDATA\VanguardCore"
$exePath = "$installDir\VanguardCore.exe"

# 2. Crear el directorio si no existe
if (-not (Test-Path $installDir)) {
    New-Item -ItemType Directory -Path $installDir | Out-Null
}

# 3. Descargar el ejecutable desde la Release de GitHub
$downloadUrl = "https://github.com/CMDPlayer216/Vanguard-Core/latest/download/vanguard-core-win-x64.exe"
Write-Host "Descargando Vanguard-Core..." -ForegroundColor Cyan
Invoke-WebRequest -Uri $downloadUrl -OutFile $exePath

# 4. Agregar la carpeta al PATH del usuario (para poder usar 'userdb' desde cualquier terminal)
$userPath = [Environment]::GetEnvironmentVariable("Path", "User")
if ($userPath -notlike "*$installDir*") {
    [Environment]::SetEnvironmentVariable("Path", "$userPath;$installDir", "User")
    Write-Host "Se agregó UserDB al PATH del sistema." -ForegroundColor Green
}

Write-Host "¡Instalación completada! Abre una nueva terminal y escribe 'VanguardCore'." -ForegroundColor Green