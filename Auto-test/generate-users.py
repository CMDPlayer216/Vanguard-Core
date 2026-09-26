import argparse
import random
import subprocess
import sys

# Listas de datos para generación aleatoria
ROLES = [
    "Shigeo Kageyama", "Reigen Arataka", "Ritsu Kageyama", "Teruki Hanazawa",
    "Dimple", "Naruto Uzumaki", "Sasuke Uchiha", "Kakashi Hatake",
    "Goku", "Vegeta", "Piccolo", "Luffy", "Zoro", "Sanji", "Saitama", "Genos"
]

PRONOUNS_LIST = [
    ["Él", "He", "Him"],
    ["Ella", "She", "Her"],
    ["Ello", "They", "Them"],
    ["Él", "Elle"]
]

FANDOMS_LIST = [
    "Mob Psycho 100", "Naruto", "Dragon Ball", "One Piece",
    "One Punch Man", "Jujutsu Kaisen", "Bleach", "Attack on Titan"
]


def generar_usuario_aleatorio():
    """Genera únicamente los campos obligatorios para un usuario."""
    primary_role = random.choice(ROLES)
    age = str(random.randint(13, 60))
    pronouns = "|".join(random.choice(PRONOUNS_LIST))
    
    num_fandoms = random.randint(1, 3)
    fandoms = "|".join(random.sample(FANDOMS_LIST, num_fandoms))

    return {
        "primary_role": primary_role,
        "age": age,
        "pronouns": pronouns,
        "fandoms": fandoms
    }


def agregar_usuario(data):
    """Ejecuta el comando vanguardb add con los campos estrictamente requeridos."""
    cmd = [
        "vanguardb",
        "add",  # Subcomando requerido por la CLI
        data["primary_role"],
        data["age"],
        data["pronouns"],
        data["fandoms"]
    ]

    try:
        resultado = subprocess.run(cmd, check=True, capture_output=True, text=True)
        return True, resultado.stdout
    except subprocess.CalledProcessError as e:
        return False, e.stderr
    except FileNotFoundError:
        return False, "Error: El ejecutable 'vanguardb' no está instalado o no se encuentra en el PATH."


def main():
    parser = argparse.ArgumentParser(description="Agrega usuarios aleatorios (solo campos requeridos) a Vanguard Core.")
    parser.add_argument("cantidad", type=int, help="Cantidad de usuarios aleatorios a agregar")
    
    args = parser.parse_args()

    if args.cantidad <= 0:
        print("La cantidad de usuarios debe ser un número positivo.")
        sys.exit(1)

    print(f"Iniciando la agregación de {args.cantidad} usuario(s) aleatorio(s)...\n")

    exitos = 0
    fallos = 0

    for i in range(1, args.cantidad + 1):
        usuario = generar_usuario_aleatorio()
        exito, salida = agregar_usuario(usuario)

        if exito:
            exitos += 1
            print(f"[{i}/{args.cantidad}] Usuario '{usuario['primary_role']}' agregado correctamente.")
        else:
            fallos += 1
            print(f"[{i}/{args.cantidad}] Error al agregar usuario: {salida.strip()}")
            if "no se encuentra en el PATH" in salida:
                break

    print(f"\nProceso finalizado. Éxitos: {exitos} | Fallos: {fallos}")


if __name__ == "__main__":
    main()
