$ErrorActionPreference = "Stop"

Write-Host "== Restoring =="
dotnet restore

Write-Host "== Building =="
dotnet build -c Release --no-restore

Write-Host "== Running unit + integration tests =="
dotnet test -c Release --no-build

Write-Host "== Building Docker images =="
docker compose build

Write-Host "== Starting containers =="
docker compose up -d

Write-Host ""
Write-Host "Identity Swagger: http://localhost:5001/swagger"
Write-Host "Audit Swagger:     http://localhost:5002/swagger"
Write-Host "Postgres:          localhost:5432"