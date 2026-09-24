# Si DrawError salta, el 'if' detecta el código 1 y entra al bloque 'else'
if dotnet run -- delete "ID_INEXISTENTE" --noconfirm; then
    echo "✅ Operación exitosa"
else
    echo "❌ La CLI falló (DrawError detuvo el proceso)"
fi
