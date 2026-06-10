# Sistema de Venta de Boletos para Eventos

Sistema web para administrar y vender boletos para conciertos, conferencias y espectáculos.  
**Backend:** .NET 10 + Clean Architecture + SQLite  
**Frontend:** SPA en HTML/React (un solo archivo `index.html`)

---

## ✅ Requisitos previos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Un navegador moderno (Chrome, Edge, Firefox)
- No se requiere instalar nada más (SQLite está incluido)

---

## 🚀 Cómo ejecutar el proyecto

### Paso 1 — Clonar el repositorio

```bash
git clone https://github.com/tu-usuario/tu-repo.git
cd tu-repo
```

### Paso 2 — Levantar el Backend

```bash
cd MiApp.API
dotnet run
```

La API quedará corriendo en: **http://localhost:5223**

> La base de datos SQLite (`miapp.db`) se crea automáticamente en el primer arranque y se llena con datos de prueba.

### Paso 3 — Abrir el Frontend

Abrir el archivo `index.html` que está en la **raíz del proyecto** directamente en el navegador.

**Opción A — Doble clic** sobre `index.html`

**Opción B — Con Live Server (VS Code):**  
Click derecho sobre `index.html` → *Open with Live Server*  
(El puerto debe estar entre los permitidos: 3000, 3001, 8000 ó 5500)

---

## 👤 Usuarios de prueba

| Rol | Email | Contraseña |
|-----|-------|------------|
| Administrador | admin@miapp.com | Admin123! |
| Lector | reader@miapp.com | Reader123! |

---

## 🗺️ Navegación

### Portal público (sin login)
- Ver todos los eventos activos
- Ver detalle de cada evento con zonas y precios
- **Comprar boletos** — solo necesitas ingresar tu correo

### Panel de administrador (login requerido)
- **Dashboard** — ingresos totales, compras realizadas, top eventos
- **Eventos** — crear, editar y cancelar eventos con sus zonas (VIP, Preferente, General)
- **Compras** — historial completo de todas las transacciones

---

## 🏗️ Estructura del proyecto

```
SegundoExamenParcialWeb/
├── index.html                  ← Frontend completo (abrir en el navegador)
├── MiApp.API/                  ← API REST (.NET 10)
│   ├── Controllers/
│   ├── Program.cs
│   └── miapp.db                ← Base de datos SQLite (se genera automáticamente)
├── MiApp.Application/          ← Lógica de negocio (CQRS + MediatR)
├── MiApp.Domain/               ← Entidades y contratos
└── MiApp.Infrastructure/       ← Repositorios y EF Core
```

---

## ⚙️ Detalles técnicos

- **Arquitectura:** Clean Architecture (Domain → Application → Infrastructure → API)
- **Patrón:** CQRS con MediatR
- **Base de datos:** SQLite con Entity Framework Core
- **Autenticación:** JWT Bearer Tokens
- **Validaciones:** FluentValidation
- **Puerto API:** `http://localhost:5223`

---

## ❗ Solución de problemas comunes

**La página no carga datos / aparece error de CORS**  
→ Asegúrate de abrir el `index.html` desde uno de estos orígenes:
`localhost:3000`, `localhost:5500`, `127.0.0.1:5500`  
No abrir como `file:///` directo si hay problemas de CORS.

**Error al iniciar la API**  
→ Verifica que el puerto 5223 no esté ocupado:
```bash
# Windows
netstat -ano | findstr :5223

# Mac/Linux
lsof -i :5223
```

**La base de datos no se crea**  
→ Ejecuta desde la carpeta `MiApp.API/`:
```bash
dotnet ef database update --project ../MiApp.Infrastructure
dotnet run
```


