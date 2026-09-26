# Vanguard Core 0.2.4 RELEASE CANDIDATE 3
Interfaz de línea de comandos (CLI) con el objetivo de administrar usuarios en comunidades de roleplay (Teams de Whatsapp, servidores de Discord, etc).

---
## Instalación sencilla
### Prerequisitos
1. Emulador de terminal (Símbolo de sistema, PowerShell, Kitty, Alacrity, etc)
2. Un cerebro

### En Linux:
```bash
git clone https://github.com/CMDPlayer216/Vanguard-Core.git
cd Vanguard-Core
./install.sh
```
### En Windows:
1. Descarga el archivo `install.ps1` desde el código o la última release.
2. Haz doble click en él o:
```PowerShell
.\install.ps1
```
---
## Características principales
### Adición de usuarios
```bash
vanguardb <primary-role> <age> <pronouns> <fandoms> [opciones]
```
Argumentos:
- primary-role: Rol principal del usuario por el que se le reconoce.
- age: Edad del usuario. Tiene que ser un número positivo.
- pronouns: Pronombres del usuario, se separan usando `|`.
- fandoms: Fandoms a los que pertenece el usuario, se separan usando `|`.

Opciones:
- `-w` | `--wanted-roles`: Roles buscados por el usuario que quieres agregar (separados por `|`).
- `-r` | `--additional-roles`: Roles adicionales del usuario que quieres agregar (separados por `|`).
- `-t` | `--type`: Tipo de usuario a agregar (por defecto: `Member`).
- `-i` | `--image`: Ruta a la imágen de avatar del usuario.
- `-?` | `-h` | `--help`: Muestra ayuda e información de uso.

Tipos de miembro admitidos:
- `Administrator`
- `Helper`
- `Member`

Ejemplo de uso:
```bash
vanguardb add "Shigeo Kageyama" 15 "Él|He|Him" "Mob Psycho 100" -w "Reigen Arataka" -i ~/Descargas/Mob.png
```
### Búsqueda en la base de datos
```bash
vanguardb search [opciones]
```
Opciones:
- `-u`|`--user-id`: ID de usuario
- `-p`|`--primary-rol`: Rol principal asignado
- `-f`|`--fast`: Acelera las búsquedas por nombre e ID
- `-r`|`--rol`: Rol asignado
- `-w`|`--wanted-rol`: Rol buscado
- `-P`|`--pronoun`: Pronombre asignado
- `-a`|`--age`: Edad asignada
- `-min-age`: Rango mínimo de edad
- `--max-age`: Rango máximo de edad
- `-d`|`--date-created`: Fecha de registro
- `--min-date-created`: Fecha mínima de registro
- `--max-date-created`: Fecha máxima de registro
- `-l`|`--last-streak-date`: Fecha de registro
- `--min-last-streak-date`: Fecha mínima de verificación de racha
- `--max-last-streak-date`: Fecha máxima de verificación de racha
- `-t`|`--type`: Tipo de usuario
- `-F`|`--fandom`: Fandom asignado
- `-?`|`-h`|`--help`: Mostrar ayuda e información de uso
- `--raw`: Ofrece salida en formato JSON para integración con CLI o scripts

Ejemplo de uso:
```bash
vanguardb search -p shi -f
```
Resultado:
```text
ZZdLarGA93iR | Shigeo Kageyama
ID           | Rol principal
```
O en crudo:
```bash
vanguardb search -p shi -f --raw
```
Resultado:
```text
[{"primaryRole":"Shigeo Kageyama","id":"ZZdLarGA93iR","version":1,"avatarImageBase64":null,"path":"28c0c96f-01c9-4315-bdb7-03e201b555fb.vud"},{"primaryRole":"Shigeo Kageyama","id":"clZnOalPsnV7","version":1,"avatarImageBase64":null,"path":"a955cd42-e14c-4a6f-9000-7023d6a2802b.vud"}]
```
### Modificar un usuario
```bash
vanguardb modify [opciones]
```
Opciones:
* `-u`|`--user-id`: ID del usuario a modificar
* `-p`|`--primary-rol`: Nuevo rol principal
* `--add-rol`: Roles adicionales que se añadirán
* `--remove-rol`: Roles adicionales que se eliminarán
* `--add-wanted-rol`: Roles buscados que se añadirán
* `--remove-wanted-rol`: Roles buscados que se eliminarán
* `--add-pronoun`: Pronombres que se añadirán
* `--remove-pronoun`: Pronombres que se eliminarán
* `-a`|`--age`: Nueva edad
* `-l`|`--last-streak-date`: Nueva fecha de última verificación de racha
* `-t`|`--type`: Nuevo tipo de usuario (`Administrator`, `Helper` o `Member`)
* `--add-action`: Acción pendiente a añadir (`Tipo|Fecha|Razón`)
* `--remove-action`: Acción pendiente a eliminar (`Tipo|Fecha|Razón`)
* `-s`|`--streak`: Nueva racha
* `--avatar`: Ruta de la nueva imagen de avatar
* `-?`|`-h`|`--help`: Mostrar ayuda e información de uso

Ejemplo de uso:
```bash
vanguardb modify -u ZZdLarGA93iR -p "Ritsu Kageyama" --remove-wanted-rol "Arataka Reigen"
```
El programa preguntará si estas seguro de ejecutar esta operación (`s/N`).
### Consultar un usuario
```bash
vanguardb consult <id>
```
Ejemplo de uso:
```bash
vanguardb consult ZZdLarGA93iR
```
Resultado:
```text
====== DATOS DE USUARIO ======
Rol principal: Shigeo Kageyama
ID: ZZdLarGA93iR
Edad: 18
Fecha de registro: 23/9/2026 8:38:23 p. m.
Tipo de usuario: Administrator
Pronombres:
  - Él
  - He1Him
Fandoms:
  - Mob Psycho 100
Roles buscados:
  - Reigen Arataka
Última verificación de racha: Nunca
Racha actual: 0
```
O en crudo:
```bash
vanguardb consult ZZdLarGA93iR --raw
```
Resultado:
```text
{"primaryRole":"Shigeo Kageyama","roles":null,"wantedRoles":null,"id":"ZZdLarGA93iR","version":1,"pronouns":["\u00C9l","He","Him"],"age":18,"creationTime":"2026-09-23T23:25:18.5964779Z","lastStreakVerification":null,"type":2,"pendActions":null,"streak":0,"avatarImageBase64":null,"fandoms":["Mob Psycho 100"]}
```
### Eliminar un usuario
```bash
vanguardb delete <id>
```
Ejemplo de uso:
```bash
vanguardb delete ZZdLarGA93iR
```
El programa pedirá confirmación antes de eliminar. Para omitir la confirmación usar `--noconfirm`.
### Exportar un usuario
```bash
vanguardb export user <id> <file>
```
Esto generará un archivo con extensión `.vud` en la ruta especificada.

Ejemplo de uso:
```bash
vanguardb export user ZZdLarGA93iR ~/shigeo.vud
# O sin extensión
vanguardb export user ZZdLarGA93iR ~/shigeo
```
Ambos generarán `shigeo.vud`.
### Exportar base de datos
```bash
vanguardb export database <file>
```
Esto generará un archivo con extensión `.vdb` en la ruta especificada.

Ejemplo de uso:
```bash
vanguardb export database ~/backup-2026-09-24.vdb
# O sin extensión
vanguardb export database ~/backup-2026-09-24
```
Ambos generarán `backup-2026-09-24.vdb`.
### Importar usuario
```bash
vanguardb import user <file> <CombineKeepingNew | CombineKeepingOriginal | Fail | OverWrite | Skip>
```
Esto importará un usuario desde un archivo `.vud`
### Importar base de datos completa
```bash
vanguardb import database <file> <CombineKeepingNew | CombineKeepingOriginal | Fail | OverWrite | Skip>
```
Esto importará los usuarios guardados en un archivo `.vdb`
### Resolución de conflictos
Hay varias estrategias en caso de que un usuario que se va a importar ya exista:
- `CombineKeepingOriginal`: Se mantendran los datos actuales pero se combinarán las listas.
- `CombineKeepingNew`: Se reemplazarán los datos viejos con los nuevos pero se combinarán las listas.
- `Fail`: La importación fallará.
- `Skip`: Se omitirá.
- `OverWrite`: Se sobreescribirá por completo.
### Configuración
La configuración se guarda en un archivo `config.yaml`, puedes consultar en dónde se está guardando la configuración con:
```bash
vanguardb config get configPath
```
Archivo de configuración de ejemplo:
```YAML
# Directorio donde se guardarán los archivos .vud.
# El índice se guarda en el directorio de configuración.
#
# =====================================
# ============ ADVERTENCIA ============
# =====================================
#
# Cambiar esta configuración puede conllevar a pérdida de datos.
# Manejar con cuidado.
data_base_path: /home/gabriel/.local/share/vanguardb

# Directorio de archivos temporales
temp_path: /tmp/
```

---
## Estructura del proyecto
### Tecnologías usadas
- `MessagePack` para serializar usuarios en archivos `<GUID.vud>` (Vanguard User Data), el índice en un archivo `<index.ivdb>` (Index Vanguard Data Base) y archivos de distribución de base de datos `.vdb` (Vanguard Data Base).
- `System.CommandLine` para parseo de comandos.
- `YamlDotNet` para configuración.
### Estructura de archivos
```text
̣̣Vanguard-Core/
|
| - Program.cs           # Punto de entrada y definición del rootCommand
| - GlobalUsings.cs      # Opciones globales del proyecto
| - Vanguard-Core.csproj # Opciones de .NET SDK
| - export.sh            # Script rápido para exportar ejecutables autocontenidos
| - install.sh           # Script de instalación para Linux
| - inspall.ps1          # Script de instalación para Windows
| - LICENSE              # Licencia MIT
| - README.md            # Este documento
| - .gitignore           # Lista de archivos que git ignora
| - Helpers/             # Utilidades varias
|   | - ConsoleHelper.cs         # Utilidad para dibujar en consola
| - Builders/            # Constructores de subcomandos
|   | - AddCommand.cs            # Comando add
|   | - ConfigCommand.cs         # Comando config
|   | - ConsultCommandBuilder.cs # Comando consullt
|   | - DeleteCommandBuilder.cs  # Comando delete
|   | - ExportCommand.cs         # Comando de exportación
|   | - ImportCommand.cs         # Comando de importación
|   | - ModifyCommand.cs         # Comando modify
|   | - SearchCommand.cs         # Comando search
| - Commands/            # Lógica de comandos
|   | - AddCommand.cs            # Comando add
|   | - ConfigCommand.cs         # Comando config
|   | - ConsultCommand.cs        # Comando consullt
|   | - DeleteCommand.cs         # Comando delete
|   | - DataCommand.cs           # Comandos de importación y exportación
|   | - ModifyCommand.cs         # Comando modify
|   | - SearchCommand.cs         # Comando search
| - Dataservices/        # Operaciones directas en la base de datos
|   | - Config.cs                # Lógica para obtener y guardar configuraciones
|   | - Delete.cs                # Operaciónes de eliminación
|   | - Export.cs                # Lógica de exportación
|   | - Import.cs                # Lógica de importación
|   | - LockDataBase.cs          # Sistema de bloqueo de base de datos
|   | - Modify.cs                # Operaciónes de modificación
|   | - Pack.cs                  # Ayuda a desempaquetar bases de datos
|   | - Query.cs                 # Operaciones de consulta
|   | - Write.cs                 # Operaciones de escritura
| - Models/              # Objetos reutilizables
|   | - Config.cs                # Modelo de configuración global
|   | - IndexModel.cs            # Modelo de entradas de índice
|   | - ModifyingUserModel.cs    # Modelo de cambios para aplicar a usuarios
|   | - SearchModel.cs           # Modelo de consulta para búsquedas
|   | - UserAction.cs            # Modelo de acciones de usuario
|   | - UserModel.cs             # Modelo de usuario
| - Validators/          # Utilidades para validación
    | - FileSystemValidators.cs  # Validaciones de sistema de archivos
    | - UserValidators.cs        # Validaciones de campos de usuario
```
---
## Compilación y distribución
### Prerequisitos
1. Tener instalado en SDK de .NET
2. Emulador de terminal (Kitty, Alacrity, etc)
3. Un cerebro
### Exportación rápida
Ejecutar el script para generar binarios autocontenidos para Linux y Windows:
```bash
./export.sh
```
Esto generará los binarios en `publish/linux-x64` y `publish/windows-x64`.

---
## Licencia y créditos
- Autor: CMDPlayer216.
- Licencia: MIT (consultar archivo LICENCE para mśa información).
---