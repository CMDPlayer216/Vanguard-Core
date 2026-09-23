#!/bin/bash

mkdir -p ./publish/linux-x64
mkdir -p ./publish/windows-x64

dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true -o ./publish/linux-x64
chmod +x ./publish/linux-x64/Vanguard-Core
cp -f ./publish/linux-x64/Vanguard-Core ~/Escritorio/vanguard-core-linux-x64

dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./publish/windows-x64
chmod +x ./publish/windows-x64/Vanguard-Core.exe
cp -f ./publish/windows-x64/Vanguard-Core.exe ~/Escritorio/vanguard-core-windows-x64.exe