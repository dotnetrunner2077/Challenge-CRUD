# Challenge-CRUD

Solución full-stack que combina una **API REST** en **.NET 6** y una aplicación web **Blazor Server Side**, ambas conectadas a una base de datos **SQL Server**.

---

## 📋 Tabla de contenidos

- [Descripción general](#descripción-general)
- [Tecnologías utilizadas](#tecnologías-utilizadas)
- [Estructura del repositorio](#estructura-del-repositorio)
- [Requisitos previos](#requisitos-previos)
- [Configuración y ejecución](#configuración-y-ejecución)
- [API – Endpoints](#api--endpoints)
- [Web – Vistas](#web--vistas)

---

## Descripción general

Este repositorio implementa un CRUD completo de usuarios con dos proyectos:

| Proyecto | Tecnología | Descripción |
|---|---|---|
| `ChallengeAPI` | .NET 6 Web API | Expone los endpoints REST para gestionar usuarios |
| `ChallengeWeb` | Blazor Server Side | Interfaz web que consume la API para realizar las operaciones CRUD |

Ambos proyectos se conectan a una base de datos **SQL Server** para la persistencia de datos.

---

## Tecnologías utilizadas

- [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [ASP.NET Core Web API](https://learn.microsoft.com/en-us/aspnet/core/web-api/)
- [Blazor Server Side](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-6.0#blazor-server)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)

---

## Estructura del repositorio

```
Challenge-CRUD/
├── ChallengeAPI/               # Proyecto de la API REST (.NET 6)
│   ├── Controllers/
│   │   └── UsersController.cs  # Controlador de usuarios
│   ├── Services/
│   │   └── UserService.cs      # Lógica de negocio
│   ├── DTOs/
│   │   └── UsuariosDTO.cs      # Objeto de transferencia de datos
│   ├── Models/
│   │   └── Usuario.cs          # Modelo de entidad
│   └── appsettings.json        # Configuración (cadena de conexión)
└── ChallengeWeb/               # Proyecto web Blazor Server Side
    ├── Pages/
    │   ├── Users/
    │   │   ├── ListUsers.razor   # Vista – listado de usuarios
    │   │   ├── CreateUser.razor  # Vista – crear usuario
    │   │   ├── EditUser.razor    # Vista – editar usuario
    │   │   ├── DeleteUser.razor  # Vista – eliminar usuario
    │   │   └── DetailUser.razor  # Vista – detalle de usuario
    │   └── Login.razor           # Vista – inicio de sesión
    └── appsettings.json          # Configuración (URL de la API)
```

---

## Requisitos previos

- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (o SQL Server Express)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) o [VS Code](https://code.visualstudio.com/)

---

## Configuración y ejecución

### 1. Base de datos

Crea la base de datos en SQL Server y ejecuta las migraciones (o el script SQL incluido en el proyecto):

```bash
# Desde la carpeta ChallengeAPI
dotnet ef database update
```

### 2. Cadena de conexión

Edita `ChallengeAPI/appsettings.json` con los datos de tu servidor:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR;Database=ChallengeDB;User Id=TU_USUARIO;Password=TU_PASSWORD;"
  }
}
```

### 3. Ejecutar la API

```bash
cd ChallengeAPI
dotnet run
```

La API quedará disponible en `https://localhost:7000` (o el puerto configurado).  
Swagger UI: `https://localhost:7000/swagger`

### 4. Configurar la URL de la API en la Web

Edita `ChallengeWeb/appsettings.json`:

```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:7000"
  }
}
```

### 5. Ejecutar la Web

```bash
cd ChallengeWeb
dotnet run
```

La aplicación web quedará disponible en `https://localhost:7001`.

---

## API – Endpoints

Base URL: `/api/Users`

| Método | Ruta | Descripción |
|--------|------|-------------|
| `GET` | `/api/Users/List` | Obtiene todos los usuarios |
| `GET` | `/api/Users/{id}` | Obtiene un usuario por su ID |
| `POST` | `/api/Users` | Crea un nuevo usuario |
| `POST` | `/api/Users/Login` | Autentica un usuario por email y contraseña |
| `PUT` | `/api/Users` | Actualiza los datos de un usuario existente |
| `DELETE` | `/api/Users` | Elimina un usuario por su ID |

### Detalle de endpoints

#### `GET /api/Users/List`
Retorna la lista completa de usuarios.

**Respuesta exitosa (`200 OK`):**
```json
[
  {
    "id": 1,
    "nombre": "Juan",
    "apellido": "Pérez",
    "email": "juan.perez@example.com"
  }
]
```

---

#### `GET /api/Users/{id}`
Retorna un usuario específico según su ID.

**Parámetros de ruta:**
| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| `id` | `int` | ID del usuario |

**Respuesta exitosa (`200 OK`):**
```json
{
  "id": 1,
  "nombre": "Juan",
  "apellido": "Pérez",
  "email": "juan.perez@example.com"
}
```

---

#### `POST /api/Users`
Crea un nuevo usuario.

**Body (`application/json`):**
```json
{
  "nombre": "Juan",
  "apellido": "Pérez",
  "email": "juan.perez@example.com",
  "password": "contraseña123"
}
```

**Respuesta exitosa (`200 OK`):** objeto del usuario creado.

---

#### `POST /api/Users/Login`
Autentica un usuario con su email y contraseña.

**Query parameters:**
| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| `email` | `string` | Email del usuario |
| `password` | `string` | Contraseña del usuario |

**Respuesta exitosa (`200 OK`):** datos del usuario autenticado o token de sesión.

---

#### `PUT /api/Users`
Actualiza los datos de un usuario existente.

**Body (`application/json`):**
```json
{
  "id": 1,
  "nombre": "Juan Actualizado",
  "apellido": "Pérez",
  "email": "juan.actualizado@example.com",
  "password": "nuevaContraseña"
}
```

**Respuesta exitosa (`200 OK`):** objeto del usuario actualizado.

---

#### `DELETE /api/Users`
Elimina un usuario por su ID.

**Query parameters:**
| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| `id` | `int` | ID del usuario a eliminar |

**Respuesta exitosa (`200 OK`):** confirmación de la operación.

---

## Web – Vistas

La aplicación Blazor Server Side provee las siguientes vistas para interactuar con la API:

| Vista | Ruta | Descripción |
|-------|------|-------------|
| **Login** | `/login` | Formulario de inicio de sesión (email + contraseña) |
| **Listado de usuarios** | `/users` | Tabla con todos los usuarios y acciones (ver, editar, eliminar) |
| **Detalle de usuario** | `/users/{id}` | Muestra la información completa de un usuario |
| **Crear usuario** | `/users/create` | Formulario para registrar un nuevo usuario |
| **Editar usuario** | `/users/edit/{id}` | Formulario para modificar los datos de un usuario existente |
| **Eliminar usuario** | `/users/delete/{id}` | Confirmación antes de eliminar un usuario |

### Flujo de la aplicación

```
Login
  └─► Listado de usuarios
          ├─► Detalle de usuario
          ├─► Crear usuario
          ├─► Editar usuario
          └─► Eliminar usuario (confirmación)
```
