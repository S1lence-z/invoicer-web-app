# Invoicer Application - Developer Documentation

## 1. Introduction

Welcome to the Invoicer application documentation. This document provides developers with an overview of the project structure, key components, technologies used, and guidelines for development.

The application aims to provide functionality for creating, managing, and potentially generating PDF versions of invoices, along with managing related entities like customers/business entities, bank accounts, and numbering schemes.

**Note:** This documentation reflects the project structure as of `12. 5. 2025`. An architectural refactoring is planned to better align with Clean Architecture principles. Sections related to `Backend` and `Shared` will likely see significant changes.

## 2. Getting Started

### Prerequisites

*   .NET SDK 8.0
*   Docker (for containerized development and deployment)
*   An IDE (preferably Visual Studio 2022)

### Setup & Running

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/S1lence-z/invoicer-web-app.git
    cd invoicer-web-app/invoicer
    ```
2.  **Restore Dependencies:**
    ```bash
    dotnet restore Invoicer.sln
    ```
3.  **Database Setup:**
    *   Create the database file `Invoicer.db` in the `Backend/Database/` directory.
    *   Apply Entity Framework Core migrations:
        ```bash
        cd src/Backend # Navigate to the Backend project directory
        dotnet ef database update
        ```
    * The `Backend/Database/Patches/InsertSampleData.sql` script might be available for seeding initial data for testing. Execute this against your database if needed.
4.  **Build the Solution:**
    ```bash
    dotnet build Invoicer.sln
    ```
5.  **Run the Application:**
 - **Using Docker Compose:**
        ```
        docker-compose -f docker-compose.dev.yml up
        ```
        This will start the application in development mode with the necessary services.
  - **Manually**
    *   **Backend API:**
        ```bash
        cd src/Backend
        dotnet run
        ```
        The API will typically be available at `https://localhost:xxxx` or `http://localhost:yyyy` (check the console output).
    *   **Frontend UI:**
        ```bash
        cd src/Frontend
        dotnet run
        ```
        The Blazor WebAssembly application will typically be available at a different port (check console output). Access it via your web browser.

## 3. Project Architecture Overview

The solution follows a multi-project structure, currently organized as follows:

### 3.1. `Invoicer.Domain`

*   **Purpose:** Contains the core business logic, models (entities), and domain-specific rules, independent of other layers.
*   **Key Contents:**
    *   `Models/`: Contains Plain Old C# Objects (POCOs) representing the core business entities (e.g., `Invoice.cs`, `Entity.cs`, `Address.cs`, `NumberingScheme.cs`). These should ideally only contain data and core business rules/methods.
    *   `Services/`: Contains domain services encapsulating logic that doesn't naturally belong to a single entity (e.g., `InvoiceNumberGenerator.cs`).
    *   `Interfaces/`: Defines interfaces for domain services or abstractions needed *within* the domain itself (e.g., `IInvoiceNumberGenerator.cs`).

### 3.2. `Invoicer.Application`

*   **Purpose:** Intended to orchestrate application use cases/features. It defines interfaces for infrastructure concerns and contains application-specific logic like mapping.
*   **Key Contents:**
    *   `Interfaces/`: Defines contracts (interfaces) for services that the application layer needs but are implemented elsewhere (typically Infrastructure, but currently some might be in `Backend`). Examples: `IInvoicePdfGenerator.cs`, `IEntityInvoiceNumberingStateService.cs`.
    *   `Mappers/`: Contains logic (e.g., using AutoMapper profiles or manual mapping) to convert between Domain entities and Data Transfer Objects (DTOs) or other models (e.g., `InvoiceMapper.cs`).
    *   *(Future)* This layer should ideally contain Command/Query handlers, DTOs, validation logic, and application-specific exceptions after refactoring.

### 3.3. `Invoicer.Backend`

*   **Purpose:** Currently serves multiple roles:
    1.  **API Layer:** Exposes HTTP endpoints for the `Frontend` or other clients.
    2.  **Infrastructure/Persistence Layer:** Contains database interaction logic (EF Core) and implementations for some services defined in `Application` or `Shared`.
    3.  **Composition Root:** Configures dependency injection, middleware, and application startup (`Program.cs`).
*   **Key Contents:**
    *   `Controllers/`: ASP.NET Core API controllers handling incoming HTTP requests (e.g., `InvoiceController.cs`).
    *   `Database/`: Contains the Entity Framework Core `DbContext` (`ApplicationDbContext.cs`), migrations (`Migrations/`), and potentially seed scripts/patches. **(This belongs in an Infrastructure layer)**.
    *   `Services/`: Contains concrete implementations of services, including those performing data access using EF Core (e.g., `InvoiceService.cs`, `AddressService.cs`) and external interactions (`AresApiService.cs`). **(These implementations belong in an Infrastructure layer)**.
    *   `Utils/`: Utility classes specific to the backend, including implementations like `InvoicePdfGenerator.cs`. **(Implementations belong in an Infrastructure layer)**.
    *   `appsettings.json`: Configuration files.
    *   `Program.cs`: Application entry point and service configuration.

### 3.4. `Invoicer.Frontend`

*   **Purpose:** The user interface layer, built using Blazor WebAssembly. It interacts with the `Backend` via HTTP API calls.
*   **Key Contents:**
    *   `wwwroot/`: Static assets (CSS, JavaScript, images).
    *   `Api/`: Contains client-side services or typed HttpClients responsible for calling the `Backend` API endpoints (e.g., `InvoiceService.cs` here calls the `Backend`'s `InvoiceController`).
    *   `Components/`: Reusable Blazor UI components (.razor files).
    *   `Layout/`: Defines the main application layout structure (`MainLayout.razor`, `NavMenu.razor`).
    *   `Models/`: Contains View Models specific to the Frontend (e.g., `InvoiceFormModel.cs`).
    *   `Pages/`: Routable Blazor components representing application pages (.razor files).
    *   `Services/`: Client-side services for UI logic (e.g., `LoadingService.cs`, `ErrorService.cs`).
    *   `Validators/`: Custom validation attributes used in forms.
    *   `Program.cs`: Frontend application entry point and service configuration.

### 3.5. `Invoicer.Shared`

*   **Purpose:** A library for code shared across multiple projects, primarily `Frontend` and `Backend`.
*   **Key Contents:**
    *   `Api/`: Contains models related to specific external APIs (e.g., `AresApiModels`, `AresApiResponse.cs`). **(Consider moving API-specific contracts/models closer to their usage or into Application/Infrastructure)**.
    *   `DTOs/`: Data Transfer Objects used for communication between layers, especially between `Backend` and `Frontend` (e.g., `InvoiceDto.cs`). **(Many of these might belong in Application)**.
    *   `Enums/`: Enumerations used across different layers (e.g., `InvoiceStatus.cs`, `Currency.cs`).
    *   `Extensions/`: Extension methods for common types (`StringExtensions.cs`, `DateTimeExtensions.cs`).
    *   `Interfaces/`: Currently contains some shared interfaces (`IAresApiResponse.cs`, `IPdfGenerationResult.cs`) and *service contracts* (`ServiceInterfaces/` like `IInvoiceService.cs`). **(Service contracts primarily belong in Application)**.
    *   `PdfGenerator/`: Potentially shared models or base components related to PDF generation.

## 4. Technology Stack

*   **.NET Platform:** :NET 8.0
*   **Backend Framework:** ASP.NET Core Web API
*   **Frontend Framework:** Blazor WebAssembly
*   **Database:** SQLite
*   **ORM:** Entity Framework Core
*   **PDF Generation:** QuestPDF

## 5. Dependencies Overview (as of `12. 5. 2025`)

*   `Frontend` -> `Shared`
*   `Backend` -> `Application`, `Domain`, `Shared`
*   `Application` -> `Domain`, `Shared`
*   `Domain` -> `Shared`
*   `Shared` -> (None of the above)

**Interaction Note:** `Frontend` interacts with `Backend` primarily via HTTP calls defined in `Frontend/Api/` services, targeting the controllers in the `Backend` project.

## 6. Key Concepts & Features

*   **Invoicing:** Core functionality revolves around creating, reading, updating, and deleting Invoices and InvoiceItems. Domain models and Backend services/controllers are central.
*   **Entity Management:** Managing business entities (customers/suppliers) that issue or receive invoices.
*   **Numbering Schemes:** Configurable schemes for generating sequential invoice numbers, potentially per entity and year/period. See `Domain/Services/InvoiceNumberGenerator.cs` and related state management services (`IEntityInvoiceNumberingStateService`).
*   **PDF Generation:** Generating PDF documents for invoices. See `Application/Interfaces/IInvoicePdfGenerator.cs` and its implementation in `Backend/Utils/InvoicePdfGenerator/`.
*   **ARES API Integration:** Interacting with the ARES system (Czech business registry). See `Backend/Services/AresApiService.cs`, `Backend/Controllers/AresController.cs`, and related models in `Shared/Api/`.

## 7. Future Refactoring Goals (High-Level)

*   Introduce a dedicated `Infrastructure` project.
*   Move EF Core DbContext, migrations, and repository/service implementations from `Backend` to `Infrastructure`.
*   Move external service implementations (PDF, ARES client) to `Infrastructure`.
*   Move DTOs and Application-level service interfaces from `Shared` to `Application`.
*   Ensure `Backend` primarily functions as the API/Presentation layer and Composition Root.
*   Minimize the scope of the `Shared` project to only truly universal items (e.g., core enums, minimal base types).