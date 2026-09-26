import json
import random
import subprocess
import sys

# Archivo de entrada
JSON_FILE = 'index.json'

# Porcentaje o cantidad fija de IDs a eliminar de forma aleatoria.
# Cambia 'PERCENTAGE' por la proporción que desees (ej. 0.3 para eliminar el 30%).
PERCENTAGE_TO_DELETE = 0.2 

def main():
    try:
        with open(JSON_FILE, 'r', encoding='utf-8') as f:
            data = json.load(f)
    except FileNotFoundError:
        print(f"Error: No se encontró el archivo {JSON_FILE}")
        sys.exit(1)
    except json.JSONDecodeError:
        print("Error al decodificar el archivo JSON.")
        sys.exit(1)

    # Extraer todas las IDs
    all_ids = [item['id'] for item in data if 'id' in item]
    print(f"Total de IDs encontradas: {len(all_ids)}")

    # Calcular cuántas IDs se van a eliminar
    num_to_delete = int(len(all_ids) * PERCENTAGE_TO_DELETE)
    
    # Si prefieres fijar un número exacto (ej. 100), descomenta la siguiente línea:
    # num_to_delete = 100

    # Seleccionar IDs de forma aleatoria sin repetición
    ids_to_delete = random.sample(all_ids, num_to_delete)
    print(f"Se eliminarán aleatoriamente {len(ids_to_delete)} IDs...\n")

    # Ejecutar el comando para cada ID seleccionada
    for idx, item_id in enumerate(ids_to_delete, 1):
        command = ["vanguardb", "delete", item_id, "--noconfirm"]
        print(f"[{idx}/{len(ids_to_delete)}] Ejecutando: {' '.join(command)}")
        
        try:
            # Ejecutar comando en la terminal
            result = subprocess.run(command, check=True, text=True, capture_output=True)
            print(f"  Resultado: OK")
        except subprocess.CalledProcessError as e:
            print(f"  Error al eliminar {item_id}: {e.stderr.strip()}")

if __name__ == '__main__':
    main()
