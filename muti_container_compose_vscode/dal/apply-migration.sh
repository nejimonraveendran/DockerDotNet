#Make sure you apply: chmod +x ./apply-migration.sh

#Add migration: dotnet ef migrations add "migration_name"
#Remove last migration: dotnet ef migrations remove
#Update DB to latest: dotnet ef database update
#Update DB to specific migration: dotnet ef database update "migration_name"
#Remove all migrations from DB: dotnet ef database update 
#Generate script: dotnet ef migrations script "from_migration" "to_migration"

echo "Applying migration..."
dotnet ef database update