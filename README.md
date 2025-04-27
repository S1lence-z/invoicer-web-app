# Invoicer

## Description
Invoicer is a simple application for creating invoices. It is a fullstack C# web application. The application is designed to be simple and easy to use.

## Specification
Here you can see the specification of the Invoicer application in both [czech](./docs/specification_cz.md) and [english](./docs/specification_en.md).

## Prerequisites
*TBD*

## Installation
### Individual Docker Containers
#### Backend
 1. Build the backend image
```bash
docker build -f ./Backend/Dockerfile.build -t invoicer-backend .
```
 2. Run the backend container
```bash
docker run -d -p 8080:8080 -p 8081:8081 --name invoicer-backend invoicer-backend
```
#### Frontend
 1. Build the frontend image
```bash
docker build -f ./Frontend/Dockerfile.build -t invoicer-frontend .
```
 2. Run the frontend container
```bash
docker run -d -p 5100:80 --name invoicer-frontend invoicer-frontend
```

### Docker Compose
#### Development
1. Run the following command to run the application in development mode:
```bash
docker-compose -f docker-compose.dev.yml up -d --build
```
#### Production (_TBD_)
1. Run the following command to run the application in production mode:
```bash
docker-compose -f docker-compose.prod.yml up -d --build
```


#### Frontend
*TBD*

## Usage
*TBD*

## Documentation
- User Documentation: [Click here](./docs/user_docs.md)
- Developer Documentation: [Click here](./docs/developer_docs.md)

## License
*TBD*
