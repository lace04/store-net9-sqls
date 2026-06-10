# Store

ASP.NET Core 9 MVC application for managing products, categories, orders, and users.

## Tech Stack

- **Framework**: ASP.NET Core 9 (MVC)
- **CSS**: Tailwind CSS v4
- **Database**: SQL Server + Entity Framework Core
- **Client-side**: jQuery, jQuery Validation

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js](https://nodejs.org/) (for Tailwind CSS)
- SQL Server instance

## Setup

### 1. Configure the database connection

Set your connection string via User Secrets (development):

```bash
cd Store
dotnet user-secrets set "ConnectionStrings:SqlString" "Data Source=(local);Database=store_db;Trusted_Connection=True;TrustServerCertificate=True;"
```

### 2. Apply migrations

```bash
dotnet ef database update
```

### 3. Build Tailwind CSS

```bash
cd Store
npm install
npm run build
```

For development (auto-rebuild on changes):

```bash
npm run dev
```

### 4. Run the application

```bash
dotnet run
```

## Project structure

```
Store/
├── Controllers/       # MVC controllers
├── Context/           # EF Core DbContext
├── Entities/          # Domain models (Category, Product, Order, OrderItem, User)
├── Models/            # ViewModels
├── Views/             # Razor views
│   ├── Category/
│   ├── Home/
│   └── Shared/
├── wwwroot/
│   ├── css/
│   │   ├── site.css   # Tailwind source (with @theme customization)
│   │   └── app.css    # Generated Tailwind output
│   ├── js/
│   └── lib/           # jQuery, jQuery Validation
└── package.json       # npm scripts for Tailwind
```

## Tailwind CSS

Custom theme colors are defined in `wwwroot/css/site.css`:

| Token      | Hex       | Usage                |
|------------|-----------|----------------------|
| `primary`  | `#0f172a` | Headings, key text   |
| `accent`   | `#d97706` | Buttons, links       |
| `surface`  | `#f8fafc` | Page background      |
| `border`   | `#e2e8f0` | Dividers, borders    |

## Scripts

| Command           | Description                           |
|-------------------|---------------------------------------|
| `npm run build`   | Build Tailwind CSS for production     |
| `npm run dev`     | Watch mode (auto-rebuild on changes)  |
| `dotnet run`      | Start the ASP.NET application         |
