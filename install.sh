#!/usr/bin/env bash

# Script de instalación automática para UserDatabase (Linux)
set -e

REPO="CMDPlayer216/Vanguard-Core"
INSTALL_DIR="$HOME/.local/bin"
BINARY_NAME="VanguardCore"

# Colores
GREEN='\033[0;32m'
CYAN='\033[0;36m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m'

echo -e "${CYAN}=== Instalador de Vanguard Core (Linux) ===${NC}\n"

# 1. Verificar que sea Linux
OS="$(uname -s | tr '[:upper:]' '[:lower:]')"
if [ "$OS" != "linux" ]; then
    echo -e "${RED}Error: De momento este script solo soporta Linux.${NC}"
    exit 1
fi

ASSET_NAME="vanguard-core-linux-x64"

# 2. Obtener el tag de la ÚLTIMA versión publicada (incluso pre-releases o tags recientes)
echo -e "${CYAN}Buscando la última versión en GitHub...${NC}"
LATEST_TAG=$(curl -s "https://api.github.com/repos/${REPO}/releases" | grep '"tag_name":' | head -n 1 | sed -E 's/.*"([^"]+)".*/\1/')

if [ -z "$LATEST_TAG" ]; then
    echo -e "${YELLOW}No se pudo detectar el tag automático, intentando vía '/releases/latest'...${NC}"
    DOWNLOAD_URL="https://github.com/${REPO}/releases/latest/download/${ASSET_NAME}"
else
    echo -e "${GREEN}Última versión detectada: ${LATEST_TAG}${NC}"
    DOWNLOAD_URL="https://github.com/${REPO}/releases/download/${LATEST_TAG}/${ASSET_NAME}"
fi

# 3. Crear directorio de instalación
if [ ! -d "$INSTALL_DIR" ]; then
    mkdir -p "$INSTALL_DIR"
fi

TARGET_PATH="$INSTALL_DIR/$BINARY_NAME"

# 4. Descargar
echo -e "${CYAN}Descargando ejecutable (${ASSET_NAME})...${NC}"
curl -sSL -o "$TARGET_PATH" "$DOWNLOAD_URL"

# 5. Dar permisos
chmod +x "$TARGET_PATH"
echo -e "${GREEN}✓ Instalado en: $TARGET_PATH${NC}"

# 6. Configurar PATH
if [[ ":$PATH:" != *":$INSTALL_DIR:"* ]]; then
    SHELL_CONFIG=""
    [ -f "$HOME/.bashrc" ] && SHELL_CONFIG="$HOME/.bashrc"
    [ -f "$HOME/.zshrc" ] && SHELL_CONFIG="$HOME/.zshrc"

    if [ -n "$SHELL_CONFIG" ]; then
        if ! grep -q "$INSTALL_DIR" "$SHELL_CONFIG"; then
            echo "export PATH=\"\$HOME/.local/bin:\$PATH\"" >> "$SHELL_CONFIG"
            echo -e "${GREEN}✓ Se agregó a tu PATH en $SHELL_CONFIG${NC}"
            echo -e "Ejecuta ${CYAN}source $SHELL_CONFIG${NC} para aplicar."
        fi
    fi
fi

echo -e "\n${GREEN}¡Listo! Escribe 'vangardb' en la terminal.${NC}\n"