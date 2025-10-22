
# 🧩 Challenge Devsu — Backend (.NET 8 + PostgreSQL)

Backend REST API desarrollada con **.NET 8**, **Entity Framework Core** y **PostgreSQL**.  
Implementa un flujo completo de **Clientes**, **Cuentas**, **Movimientos** y **Reportes**, con manejo global de excepciones, validaciones y despliegue por Docker.

---

## 🚀 Tecnologías principales

| Componente | Versión | Descripción |
|-------------|----------|-------------|
| .NET SDK | 8.0 | Plataforma base para la API |
| Entity Framework Core | 8.0 | ORM para PostgreSQL |
| PostgreSQL | 14+ | Base de datos relacional |
| Docker | latest | Contenedores de despliegue |
| Swagger | Incluido | Documentación interactiva de API |

---

## ⚙️ Requisitos

- .NET SDK 8 (para ejecución local)
- Docker
- PostgreSQL 14+
- Postman (opcional, para probar endpoints)

---

## 🔧 Variables de entorno requeridas

### 🔹 API (.NET)

| Variable | Descripción | Ejemplo |
|-----------|-------------|---------|
| HOST | Hosto conexión bd | localhost |
| DATABASE | Nombre bd | challenge_bank_devsu |
| USER_ID | Nombre usuario bd | user_bd |
| PASSWORD | Contraseña usuario bd | xxxxxxxxxxx |
| PORT | Puerto conexión bd | 5432 |
| LIMITE_DIARIO_RETIRO | Cantidad de retiro permitido diario | 10000 |

---

Swagger: [https://localhost:44335/swagger](https://localhost:44335/swagger)

---

## 🗄️ Estructura de la base de datos

| Tabla | Descripción |
|--------|-------------|
| tbl_client | Datos del cliente |
| tbl_account | Cuentas bancarias asociadas |
| tbl_move | Movimientos de crédito o débito |
| tbl_log | Registro de eventos internos |

---

## 📜 Script de creación

```sql
El script se podrá encontrar en la carpeta script-bd
```

## 💻 Ejecución local

```bash
docker build -f Challenge.Devsu.Api/Dockerfile -t challenge-backend-devsu .
docker run -d --name challenge-backend-devsu ^
  -e ASPNETCORE_ENVIRONMENT=Development ^
  -e USER_ID=xxxxxxx ^
  -e PORT=5432 ^
  -e PASSWORD=xxxxxx ^
  -e LIMITE_DIARIO_RETIRO=10000 ^
  -e HOST=host.docker.internal ^
  -e DATABASE=challenge_bank_devsu ^
  -e TZ=America/Bogota ^
  -p 44335:8080 ^
  challenge-backend-devsu
```

Swagger disponible en: `http://localhost:44335/swagger`
