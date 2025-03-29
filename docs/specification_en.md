# Invoice Generator – Specification

## Program Name
**Invoicer**

## Description
Invoicer is a C# application with a web graphical interface designed to automate and simplify the process of generating invoices. The primary user is my father, who currently creates invoices manually. This application will significantly ease his work by automating the invoice generation process.

The program will allow the user to choose the invoice numbering based on their preference, enter individual customers and providers as entities, and search for them by their company identification number (IC).

## Technologies and Methods Used
- **GUI:** Blazor
- **Backend:** ASP.NET
- **API Integration (NPRG038 – Networking):** ARES API ([ARES API Swagger UI](https://ares.gov.cz/swagger-ui/))
- **Database (NPRG057 – ADO.net):** SQLite
- **PDF:** QuestPDF library for exporting invoices to PDF

## Main Functionality
### Invoice Generation
- Invoices can be pre-filled by entering the company identification number (IC) of the entity using the ARES API.
- The user will be able to add entities (addresses, bank accounts).
- The user will see the invoices they have generated and manage them.

### Invoice Storage
- Invoices will be stored in a local SQLite database.

### Invoice Export
- Option to export invoices to PDF for printing or forwarding.