# MIGRATIONS
Create Migration: `dotnet ef migrations add InitialCreate`
Migrate: `dotnet ef database update`
Remove Last Migration: `dotnet ef database update 0` ou `dotnet ef database update Nome da migracao anterior a última` && `dotnet ef migrations remove`

# REACT NATIVE NETWORK ERROR
Adicionar endpoint em applicationUrl propriedade => Properties/lauchSettings.json
"applicationUrl": "https://localhost:5001;http://localhost:5000;http://192.168.0.103:5000"

# DISABLE NOT MAPPED ATTRIBUTES IN MIGRATION
1. available_seats

# PARA GERAR O QRCODE
`apt-get update && apt-get install -y apt-utils libgdiplus libc6-dev`