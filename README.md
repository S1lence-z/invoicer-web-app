# Invoicer

## Description
Invoicer is a simple application for creating invoices. It is a fullstack C# web application. The application is designed to be simple and easy to use.

## Specification
Here you can see the specification of the Invoicer application in both [czech](./docs/specification_cz.md) and [english](./docs/specification_en.md).

## Prerequisites
*TBD*

## Installation
### Docker Containers
#### Backend
 1. Build the backend image
```bash
docker build -f ./Backend/Dockerfile.custom -t invoicer-backend .
```
 2. Run the backend container
```bash
docker run -d -p 8080:8080 -p 8081:8081 --name invoicer-backend invoicer-backend
```

#### Frontend
*TBD*

## Usage
*TBD*

## Documentation
- User Documentation: [Click here](./docs/user-docs.md)
- Developer Documentation: [Click here](./docs/developer-docs.md)

## License
*TBD*
