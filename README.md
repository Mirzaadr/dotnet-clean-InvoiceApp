# 🧾 Invoice Management System

A modular, extensible **Invoice Management Web Application** built using **ASP.NET Core MVC**, implementing **Clean Architecture** and **Domain-Driven Design (DDD)**.

> Supports invoice creation, client/product management, PDF generation, and email sending. Ideal as a boilerplate or internal tool.

---

## 🚀 Features

- ✅ Create, edit, and manage invoices with invoice items
- ✅ Auto-generate invoice numbers with date-based formatting
- ✅ CRUD operations for Clients and Products
- ✅ Inline table editing with jQuery and Select2
- ✅ Automatically calculates totals, validates duplicates
- ✅ Generate PDF invoices using [QuestPDF](https://github.com/QuestPDF/QuestPDF)
- ✅ Dashboard with invoice, client, and product insights
- ✅ Separate invoice views: All, Drafts, Sent
- ✅ Clean Architecture (Domain, Application, Infrastructure, UI)
- ✅ Swappable InMemory or Postgres persistence
- ❌ Email invoice link with mock/local development setup
- ❌ Authentication-ready (extensible to JWT or Identity)

---

## 🏗️ Project Structure

```
InvoiceApp/
|
├── InvoiceApp.Domain/                # Contains core business logic and domain models
│   ├── Common/                       # Shared domain logic and abstractions
│   |   ├── Interfaces/               # Domain interfaces (e.g., repositories, services)
│   |   └── Models/                   # Common domain models used across modules
│   ├── Clients/                      # Client-related domain entities and logic
│   ├── Invoices/                     # Invoice-related domain entities and logic
│   ├── Products/                     # Product-related domain entities and logic
│   └── SharedKernel/                 # Cross-cutting domain concepts shared between modules
│
├── InvoiceApp.Application/           # Application layer: orchestrates use cases and business rules
│   ├── Commons/                      # Shared application logic/utilities
│   ├── DTOs/                         # Data Transfer Objects for communication between layers
│   ├── Clients/                      # Application services for client operations
│   ├── Invoices/                     # Application services for invoice operations
│   ├── Products/                     # Application services for product operations
│   └── DependencyInjection.cs        # Registers application services for DI
│
├── InvoiceApp.Infrastructure/        # Implements technical details (data access, external services)
│   ├── DomainEvents/                 # Infrastructure for handling domain events
│   ├── Services/                     # Implementations of infrastructure services (e.g., email, logging)
│   ├── Persistence/                  # Data persistence logic
│   |   ├── Configurations/           # Entity Framework or ORM configurations
│   |   ├── Repositories/             # Data access implementations
│   |   ├── AppDbContext.cs           # Main database context
│   |   └── InMemoryDbContext.cs      # In-memory database context for testing
│   └── DependencyInjection.cs        # Registers infrastructure services for DI
│
├── InvoiceApp.Web/                   # Web layer: UI, controllers, and web-specific logic
│   ├── Controllers/                  # MVC/Web API controllers
│   ├── Models/                       # View models for the web layer
│   ├── Properties/                   # Project properties (e.g., launchSettings.json)
│   ├── Services/                     # Web-specific services (e.g., authentication)
│   ├── ViewComponents/               # Reusable UI components
│   ├── Views/                        # Razor views (UI templates)
│   ├── wwwroot/                      # Static web assets (CSS, JS, images)
│   ├── appsettings.json              # Application configuration file
│   └── Program.cs                    # Entry point of the Web App
├── InvoiceApp.sln                    # Solution file (groups all projects)
└── README.md                         # Project documentation
```

---

## ⚙️ Getting Started

### 🧰 Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/)
- (Optional) PostgreSQL if you switch from InMemory
- Visual Studio / VS Code

### 📦 Setup

1. Clone the repo:

   ```bash
   git clone https://github.com/Mirzaadr/dotnet-clean-InvoiceApp.git invoice-app
   cd invoice-app
   ```

2. Install dependencies:

   ```bash
   dotnet restore
   ```

3. Update app settings:

   In `appsettings.Development.json`:

   ```json
   {
     "App": {
       "BaseUrl": "https://localhost:5001"
     },
    "ConnectionStrings": {
       "Default": "Host=<DB_HOST>;Port=<DB_PORT>;Database=<DB_NAME>;Username=<USERNAME>;Password=<PASSWORD>"
     }
   }
   ```

4. Run the app:

   ```bash
   dotnet run --project src/Web
   ```

5. Navigate to: `https://localhost:5001`

---

## 🧪 Seeded Development Setup

- App starts with **fake data** using the `Bogus` library
- PDF files are saved to local disk (mocked storage)
- Email sending is simulated using a mock service

---

## 📤 Invoice Workflow

1. **Create Draft** – Invoice is created with default status `Draft`
2. **Edit & Review** – Add items, client, and details
3. **Send Invoice** – Generates PDF, uploads to storage, and "emails" a download link
4. **Status Updates**:
   - `Draft` → `Sent` when email is sent
   - (Future: `Paid`, `Overdue`)

---

## 🗂️ Pages

- **Dashboard** – Summary of invoices, clients, products
- **Invoices**
  - All Invoices
  - Drafts (pending)
  - Sent Invoices
- **Create/Edit Invoice** – With inline editable item rows
- **Clients** – Create/Edit/View
- **Products** – Create/Edit/View

---

## 📄 PDF Generation

- Uses [QuestPDF](https://github.com/QuestPDF/QuestPDF) (no native deps)
- Generated on send or manual request
- Cleanly designed using C# fluent API

---

## 📧 Email Sending

- Mock implementation for development
- Stores link to generated PDF
- Future-ready for SMTP or SendGrid

---

## 🔒 Authentication

- Authentication system pluggable (custom user, Identity, or JWT)
- Current version does not require login (configurable)

---

## 🧪 Tests

- Unit tests for domain logic and PDF generation
- Event dispatching tested using MediatR
- Uses xUnit + Moq (optional)

---

## 📦 Deployment Notes

- Replace in-memory storage with PostgreSQL or SQL Server
- Configure real file storage (Azure Blob, S3)
- Configure real email provider
- Set production `BaseUrl` in `appsettings.Production.json`

---

## 🙌 Contributing

Pull requests are welcome! Feel free to fork and build on top of this foundation.

---

## 📄 License

MIT License
