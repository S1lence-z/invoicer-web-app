## Developer Documentation

**Invoicer - Developer Documentation**

**1. Introduction**

*   **1.1. Project Goal:** To create a web application (ASP.NET Core backend, Blazor frontend) for generating, managing, and exporting invoices, primarily for a single user. It utilizes the Czech ARES API for entity lookup and SQLite for local data storage.
*   **1.2. Technology Stack:**
    *   Backend: C#, ASP.NET Core Web API
    *   Frontend: Blazor
    *   Database: SQLite with Entity Framework Core
    *   API Integration: `HttpClient` for ARES API
    *   PDF Generation: QuestPDF library
*   **1.3. High-Level Architecture:** The solution follows a standard multi-project structure promoting Separation of Concerns:
    *   `Invoicer.Backend`: Hosts the ASP.NET Core Web API, business logic, database access, and external service integrations.
    *   `Invoicer.Domain`: Contains core domain models, interfaces, enums, and DTOs shared across projects. It has minimal dependencies.
    *   `Invoicer.Frontend`: The Blazor user interface project, consuming the Backend API.

**2. Getting Started**

*   **2.1. Prerequisites:**
    *   .NET SDK (.NET 8.0+)
    *   Git
    *   An IDE (Visual Studio 2022+)
    *   (Optional) SQLite browser tool
*   **2.2. Cloning the Repository:**
    ```bash
    git clone https://github.com/S1lence-z/invoicer-web-app/tree/master
    cd invoicer
    ```
*   **2.3. Building the Solution:**
    *   Open `invoicer.sln` in your IDE.
    *   Restore NuGet packages (usually automatic).
    *   Build the solution (Ctrl+Shift+B in Visual Studio).
*   **2.4. Database Setup:**
    *   The application uses Entity Framework Core code-first migrations with SQLite.
    *   The connection string is located in `Invoicer.Backend/appsettings.Development.json`. Default: `Data Source=invoicer.db` (relative path).
    *   **Initial Migration:**
        *   Ensure `Invoicer.Backend` is the startup project.
        *   Open Package Manager Console (VS) or use .NET CLI:
          ```bash
          cd Backend
          dotnet ef database update
          ```
        *   This will create the `invoicer.db` file in the output directory (`Invoicer.Backend/bin/Debug/...`) on the first run or update.
*   **2.5. Running the Application:**
  _TBD_

**3. Solution Architecture Deep Dive**
_TBD_
*   **3.1. `Invoicer.Domain` Project:**
    *   **Purpose:** Core building blocks, independent of infrastructure concerns.
    *   `Models/`: Contains POCOs representing core concepts (e.g., `Invoice.cs`, `Entity.cs`, `Address.cs`, `InvoiceItem.cs`, `BankAccount.cs`). These are mapped by EF Core.
    *   `Interfaces/`: Defines contracts for data access (Repositories) if using Repository Pattern (e.g., `IInvoiceRepository.cs`).
    *   `ServiceInterfaces/`: Defines contracts for business logic services (e.g., `IInvoiceService.cs`, `IEntityService.cs`). These are implemented in the Backend project.
    *   `Enums/`: Application-wide enumerations (e.g., `EntityType.cs`, `InvoiceStatus.cs`).
    *   `AresApiModels/`: DTOs specifically for deserializing responses from the ARES API. Avoid using these directly as domain models if possible; map them.
    *   `Domain.csproj`: Project file, should have minimal dependencies.
*   **3.2. `Invoicer.Backend` Project:**
    *   **Purpose:** Handles API requests, implements business logic, interacts with the database and external services.
    *   `Controllers/`: ASP.NET Core API controllers (e.g., `InvoicesController.cs`, `EntitiesController.cs`). They depend on services defined in `Domain/ServiceInterfaces`. Should be lightweight, primarily routing requests to services.
    *   `Services/`: Concrete implementation of interfaces defined in `Domain.ServiceInterfaces` (e.g., `InvoiceService.cs`, `EntityService.cs`, `AresLookupService.cs`). Contains the core application logic.
    *   `Database/`: Contains the Entity Framework Core `DbContext` (e.g., `ApplicationDbContext.cs`), migrations, and potentially repository implementations if that pattern is used.
    *   `Patches/`: Explain the purpose if used (e.g., one-time data migration scripts, specific configuration patches).
    *   `Utils/`: Backend-specific utility classes.
    *   `Program.cs`: Application entry point. Configures dependency injection (registering services, DbContext, HttpClientFactory), middleware pipeline (CORS, authentication, routing, Swagger).
    *   `appsettings.json` / `appsettings.Development.json`: Configuration files (Connection strings, ARES API base URL, logging levels).
    *   `Dockerfile`: Defines how to build the Docker image for the backend service.
    *   `Backend.http`: File for testing API endpoints directly (e.g., with VS Code REST Client or Visual Studio).
*   **3.3. `Invoicer.Frontend` Project:**
    *   **Purpose:** Provides the Blazor web user interface.
    *   `Pages/`: Routable components representing application pages (e.g., `InvoiceList.razor`, `CreateInvoice.razor`, `EntityManagement.razor`).
    *   `Components/`: Reusable UI components shared across pages (e.g., `EntitySelector.razor`, `InvoiceItemEditor.razor`, `PdfExportButton.razor`).
    *   `Layout/`: Defines the main application structure (`MainLayout.razor`), navigation (`NavMenu.razor`).
    *   `Api/`: Contains client-side services responsible for calling the Backend API. Typically uses `HttpClient` (configured in `Program.cs`) to interact with backend controllers (e.g., `InvoiceApiClient.cs`, `EntityApiClient.cs`).
    *   `Models/`: Frontend-specific view models or DTOs. Might mirror Domain models or be adapted for UI needs.
    *   `Validators/`: Client-side input validation logic (e.g., using DataAnnotations or FluentValidation).
    *   `Utils/`: Frontend-specific utility classes/functions.
    *   `wwwroot/`: Static assets (CSS, JavaScript, images).
    *   `Program.cs`: Frontend entry point. Configures services like `HttpClient` for API communication, potentially state management services, etc.
    *   `_Imports.razor`: Common Blazor directives (`@using`, `@inject`).
    *   `App.razor`: Configures the Blazor router.

**4. Key Feature Implementation Details**

*   **4.1. ARES API Integration:**
    _TBD_
*   **4.2. Database Interaction (EF Core / SQLite):**
    _TBD_
*   **4.3. PDF Generation (QuestPDF):**
    _TBD_
*   **4.4. Invoice Numbering:**
    _TBD_

**5. API (Backend)**

*   The backend exposes a RESTful API for the frontend.
*   Consider using Swagger/OpenAPI (Swashbuckle) for API documentation and testing. Ensure it's configured in `Backend/Program.cs`. Access it via `/swagger` on the backend URL.
*   Endpoints should follow REST principles

**6. Deployment**

*   **Backend:** Can be deployed as a self-contained application, hosted on IIS, Azure App Service, or containerized using the `Dockerfile`.
*   **Frontend (Blazor Server):** Deployed similarly to the Backend (needs an ASP.NET Core host).
*   **Frontend (Blazor WebAssembly):** Can be deployed as static files on any web server (IIS, Nginx, Azure Static Web Apps, GitHub Pages) or hosted within the ASP.NET Core Backend application.
*   Ensure `appsettings.Production.json` is configured correctly for deployment (e.g., production database connection string, logging).
