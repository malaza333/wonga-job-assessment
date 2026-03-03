# Wonga Job Assessment

## Project Structure

    WongaJobAssement/
    │
    ├── Wonga-backend/
    │   ├── IdentityService.Api/
    │   ├── AuditService.Api/
    │   └── docker-compose.yml (optional backend-only)
    │
    ├── Wonga-frontend/
    │   ├── .env.development
    │   ├── .env.docker
    │   ├── Dockerfile
    │   └── package.json
    │
    └── docker-compose.yml   ← MAIN compose file (runs EVERYTHING)

---

# Prerequisites

- Docker Desktop installed and running
- Node.js (for local frontend development)

---

# 🚀 Run Backend + Frontend Together (Recommended)

The main `docker-compose.yml` is located in:

    WongaJobAssement/

This compose file runs: - PostgreSQL - Identity API - Audit API -
Frontend (Vite)

---

## Step 1 --- Navigate to root folder

    cd WongaJobAssement

---

## Step 2 --- Build and run everything

    docker compose up --build

Docker will start:

- PostgreSQL
- Audit API
- Identity API
- Frontend

---

## Access URLs

Frontend:

    http://localhost:5173

Identity API:

    http://localhost:5001

Audit API:

    http://localhost:5002

---

# 🔁 How Frontend Connects to Backend

Frontend uses:

    import.meta.env.VITE_API_URL

When running in Docker:

`.env.docker`

    VITE_API_URL=http://identityapi:8080/api

Why?

Inside Docker: - `localhost` = container itself - Containers communicate
using service names - `identityapi` is the service name in
docker-compose

---

# 🛑 Stop Everything

From root folder:

    docker compose down

---

# 🔄 Rebuild Everything

    docker compose up --build --force-recreate

---

# 🧠 Development Options

## Option 1 (Best Dev Workflow)

- Run backend in Docker
- Run frontend locally

Backend:

    cd Wonga-backend
    docker compose up --build

Frontend:

    cd ../Wonga-frontend
    Run:
    npm install
    npm run dev

Uses `.env.development`

    VITE_API_URL=http://localhost:5001/api

---

## Option 2 (Full Docker Mode)

From root:

    docker compose up --build

Uses `.env.docker`

    VITE_API_URL=http://identityapi:8080/api

---

# Available Frontend Scripts

    "scripts": {
      "dev": "vite",
      "dev:docker": "vite --host 0.0.0.0 --mode docker",
      "build": "vite build",
      "preview": "vite preview"
    }

Command Loads

---

npm run dev .env.development
npm run dev:docker .env.docker

---

# Useful Docker Commands

View running containers:

    docker ps

Stop all containers:

    docker compose down

Remove volumes:

    docker compose down -v

---

---🚀🚀🚀🚀🚀🚀🚀🚀 Wonga-Backend 🚀🚀🚀🚀🚀🚀🚀🚀🚀🚀🚀🚀

For backend-specific setup and script execution instructions,
please refer to the README located inside the Wonga-backend folder.

---

# Author

Name : Thomas Selepe
Email : malaza333@gmail.com
