# EmployeesSystemAPI

EmployeesSystemAPI is a RESTful API built with ASP.NET Core for managing employees, departments, and positions. It provides endpoints for CRUD operations and handles relationships between entities using Entity Framework Core. This API is ideal for HR systems, employee management, and organizational structure applications.

---

## Features

- Manage Employees: Create, Read, Update, Delete
- Manage Departments: CRUD operations with employee count and manager assignments
- Manage Positions: CRUD with safeguards for default positions
- DTO-based API design for clean data transfer
- Entity Framework Core integration for data persistence
- Relational data loading with eager and explicit loading
- Proper error handling and HTTP status codes

---

## Technologies Used

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server (or your database)
- Swagger (OpenAPI) for API documentation

---

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- SQL Server or any compatible database
- IDE like Visual Studio or VS Code
  
---
### Installation

1. Clone the repository

   ```
   git clone https://github.com/Mohamed-Abd-El-Moteleb/EmployeesSystemAPI.git
   cd EmployeesSystemAPI
   ```
2. Configure your database connection string in appsettings.json
   
3. Run database migrations
   ```
   dotnet ef database update
   ```
5. Run the application
   ```
   dotnet run
   ```
6. Navigate to https://localhost:["port"]/swagger to explore the API documentation

---
## API Endpoints Overview:

- ``GET /api/employees`` - Get all employees
- ``GET /api/employees/{id}`` - Get employee by ID
- ``POST /api/employees`` - Create new employee
- ``PUT /api/employees/{id}`` - Update employee
- ``DELETE /api/employees/{id}`` - Delete employee
- ``GET /api/departments`` - Get all departments with employee counts
- ``GET /api/departments/{id}`` - Get department by ID
- ``POST /api/departments`` - Create new department
- ``PUT /api/departments/{id}`` - Update department
- ``DELETE /api/departments/{id}`` - Delete department (with restrictions)
- ``GET /api/positions`` - Get all positions
- ``GET /api/positions/{id}`` - Get position by ID
- ``POST /api/positions`` - Create new position
- ``PUT /api/positions/{id}`` - Update position
- ``DELETE /api/positions/{id}`` - Delete position (with restrictions)
---
