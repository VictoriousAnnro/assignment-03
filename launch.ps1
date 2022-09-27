Write-Host -ForegroundColor Green "Starting SQL server in docker container..."
$password = New-Guid
docker rm --force Assignment3
docker run --name Assignment3 -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=$password" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
$database = "Assignment3"
$connectionString = "Server=localhost;Database=$database;User Id=sa;Password=$password;Trusted_Connection=False;"
Write-Host ""


Write-Host -ForegroundColor Green "Configuring User Secrets..."
Write-Host "Configuring Connection String"
dotnet user-secrets set "ConnectionStrings:Assignment3" "$connectionString" --project ./Assignment3/
Write-Host ""

Write-Host -ForegroundColor Green "Trusting HTTPS development certificate..."
dotnet dev-certs https --trust
Write-Host ""

Write-Host -ForegroundColor Green "Updating database..."
dotnet ef database update -p ./Assignment3.Entities/ -s ./Assignment3/
Write-Host ""

Write-Host -ForegroundColor Green "Starting application..."
dotnet run --project ./Assignment3/
Write-Host ""