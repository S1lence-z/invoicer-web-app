# Invoicer
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT) [![.NET Version](https://img.shields.io/badge/.NET-8.0-blueviolet)](https://dotnet.microsoft.com/download/dotnet/8.0) [![Blazor](https://img.shields.io/badge/Blazor-WebAssembly-blue)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor) [![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-blue)](https://dotnet.microsoft.com/apps/aspnet) [![QuestPDF](https://img.shields.io/badge/QuestPDF-PDF%20Generation-green)](https://github.com/QuestPDF/QuestPDF)

Simple and effective application for creating and managing invoices, available as a web app or a Windows/Linux desktop app.

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
    docker compose -f docker-compose.dev.yml up --build
    ```
    This command will build the necessary Docker images and start the backend API and frontend application containers.
    
    If you want to run it in production mode, you can use the production Docker Compose file:
    
    ```bash
    docker compose up --build
    ```

    **Option B: Running Manually**

    If you prefer not to use Docker:

    a.  **Restore Dependencies:**
        ```bash
        dotnet restore Invoicer.sln
        ```
    b.  **Setup Database:** Navigate to the Backend project directory and apply EF Core migrations. This will create the `Invoicer.db` SQLite file if it doesn't exist.
        ```bash
        dotnet ef database update -s .\Backend\ -p .\Infrastructure\
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

## Desktop App

The same application can run as a standalone desktop program. The `Desktop` project hosts the ASP.NET Core backend in-process and shows the Blazor UI in a native window using [Photino](https://www.tryphotino.io/), so there is no separate server or browser to manage.

### Prerequisites

*   **Windows 10/11 (x64):** the [WebView2 Runtime](https://developer.microsoft.com/microsoft-edge/webview2/). It is preinstalled on Windows 11 and on up-to-date Windows 10.
*   **Linux (x64, Debian/Ubuntu):**
    ```bash
    sudo apt install libwebkit2gtk-4.1-0 libgtk-3-0 libnotify4 libfontconfig1
    ```

### Download

Prebuilt archives for each tagged release are on the [Releases](https://github.com/S1lence-z/invoicer-web-app/releases) page. Unpack and run `Invoicer.exe` (Windows) or `./Invoicer` (Linux).

### Build from source

```bash
cd invoicer
./build-desktop.sh linux-x64   # or win-x64
```

The output lands in `invoicer/publish-desktop/`. For development, `dotnet run --project Desktop` opens the window directly from the build output.

### Where data is stored

The desktop app always uses SQLite. The database and a diagnostic log live in the per-user application data folder:

| OS | Location |
|----|----------|
| Windows | `%LOCALAPPDATA%\Invoicer\` |
| Linux | `~/.local/share/Invoicer/` |

Invoice PDFs are saved through the native "Save file" dialog.

## Project Architecture

The application follows a layered architecture approach:

*   **Domain:** Core business entities, logic, and interfaces.
*   **Application:** Use case orchestration, application-specific logic, DTOs, and infrastructure interfaces.
*   **Backend:** ASP.NET Core API, Infrastructure implementations (currently, including EF Core, PDF gen), Dependency Injection setup.
*   **Frontend:** Blazor WASM User Interface.
*   **Desktop:** Photino window that hosts Backend and Frontend in one process for the desktop build.
*   **Shared:** Common DTOs, Enums, Extensions (scope to be refined).

For a detailed breakdown, see the [Developer Documentation](./docs/developer_docs.md).

## Documentation

*   **User Documentation:** [English](./docs/user_docs.md) (Explains how to use the application features)
*   **Developer Documentation:** [English](./docs/developer_docs.md) (Covers architecture, setup, code structure)
*   **Specification:** [English](./docs/specification_en.md) | [Czech](./docs/specification_cz.md)

## License

This project is licensed under the **MIT License**. See the [LICENSE](./LICENSE) file for details.