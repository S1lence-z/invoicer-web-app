# Invoicer

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT) <!-- Example badge - Update if needed -->
[![.NET Version](https://img.shields.io/badge/.NET-8.0-blueviolet)](https://dotnet.microsoft.com/download/dotnet/8.0) <!-- Example badge -->

Simple and effective web application for creating and managing invoices.

## Description

Invoicer is a fullstack C# web application designed for straightforward invoice creation and management. It utilizes .NET 8, ASP.NET Core for the backend API, Blazor WebAssembly for the frontend UI, and Entity Framework Core for data persistence with SQLite.

## Specification

Detailed functional and non-functional requirements are available here:

*   [English Specification](./docs/specification_en.md)
*   [Czech Specification](./docs/specification_cz.md)

## Key Features

*   **Invoice Management:** Create, Read, Update, Delete (CRUD) operations for invoices and their line items.
*   **Entity Management:** Manage business entities (customers/suppliers).
*   **Numbering Schemes:** Configure automatic invoice numbering sequences.
*   **PDF Generation:** Generate PDF versions of invoices using QuestPDF.
*   **ARES Integration:** Look up Czech business entity information via the ARES registry.

## Technology Stack

*   **Platform:** .NET 8.0
*   **Backend:** ASP.NET Core Web API
*   **Frontend:** Blazor WebAssembly (WASM)
*   **Database:** SQLite
*   **ORM:** Entity Framework Core 8
*   **PDF Generation:** QuestPDF

## Prerequisites

*   [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
*   [Docker](https://www.docker.com/products/docker-desktop/) (Recommended for easy setup)
*   A compatible IDE (Visual Studio 2022, VS Code with C# Dev Kit, JetBrains Rider)
*   Git

## Getting Started

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/S1lence-z/invoicer-web-app.git
    cd invoicer-web-app/invoicer
    ```

2.  **Choose your running method:**

    **Option A: Using Docker Compose (Recommended for Development)**

    This is the easiest way to get started as it handles dependencies and database setup.

    ```bash
    # Ensure Docker Desktop is running
    docker-compose -f docker-compose.dev.yml up --build
    ```
    This command will build the necessary Docker images and start the backend API and frontend application containers.

    **Option B: Running Manually**

    If you prefer not to use Docker:

    a.  **Restore Dependencies:**
        ```bash
        dotnet restore Invoicer.sln
        ```
    b.  **Setup Database:** Navigate to the Backend project directory and apply EF Core migrations. This will create the `Invoicer.db` SQLite file if it doesn't exist.
        ```bash
        cd src/Backend
        dotnet ef database update
        cd ../.. # Return to the solution root directory
        ```
        *Optional:* Execute the `src/Backend/Database/Patches/InsertSampleData.sql` script against the created `Invoicer.db` file using a SQLite tool if you need sample data.

    c.  **Build the Solution:**
        ```bash
        dotnet build Invoicer.sln --configuration Debug
        ```
    d.  **Run Backend API:**
        ```bash
        cd src/Backend
        dotnet run
        ```

    e.  **Run Frontend UI (in a separate terminal):**
        ```bash
        cd src/Frontend
        dotnet run
        ```

3.  **Access the Application:**
    Open your web browser and navigate to the Frontend URL, [http://localhost:5100](http://localhost:5100), noted in the previous step or the URL exposed by Docker (look for the port mapping in the Docker Compose output).

## Usage

Once the application is running, navigate to the web interface in your browser. You can:

*   Manage Entities (your company details, clients).
*   Define Numbering Schemes for automatic invoice numbers.
*   Create, view, edit, and delete invoices.
*   Download generated PDF invoices.
*   Use the ARES lookup feature when adding Czech entities.

For detailed usage instructions, see the [User Documentation](./docs/user_docs.md).

## Project Architecture

The application follows a layered architecture approach:

*   **Domain:** Core business entities, logic, and interfaces.
*   **Application:** Use case orchestration, application-specific logic, DTOs, and infrastructure interfaces.
*   **Backend:** ASP.NET Core API, Infrastructure implementations (currently, including EF Core, PDF gen), Dependency Injection setup.
*   **Frontend:** Blazor WASM User Interface.
*   **Shared:** Common DTOs, Enums, Extensions (scope to be refined).

For a detailed breakdown, see the [Developer Documentation](./docs/developer_docs.md).

## Documentation

*   **User Documentation:** [Click here](./docs/user_docs.md) (Explains how to use the application features)
*   **Developer Documentation:** [Click here](./docs/developer_docs.md) (Covers architecture, setup, code structure)
*   **Specification:** [English](./docs/specification_en.md) | [Czech](./docs/specification_cz.md)

## License

This project is licensed under the **MIT License**. See the [LICENSE](./LICENSE) file for details.