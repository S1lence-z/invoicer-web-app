# Invoicer

## Description
Invoicer is a simple application for creating invoices. It is a fullstack C# web application. The application is designed to be simple and easy to use.

## Specification
Here you can find the specification of the Invoicer application in [czech](./docs/specification.md).

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
docker run -p 8080:8080 -p 8081:8081 --name invoicer-backend invoicer-backend
```

#### Frontend
*TBD*

## Usage
*TBD*

## Documentation
- User Documentation: *TBD*
- Developer Documentation: *TBD*

## License
*TBD*
