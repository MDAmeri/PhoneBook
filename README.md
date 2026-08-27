# PhoneBook API

A clean and simple multi-user Phone Book RESTful API built with ASP.NET Core and PostgreSQL.

Each user has their own private contacts list. Users can register, login, and manage their contacts (Create, Read, Update, Delete) with optional search/filtering.

## Features

- User Registration & Login with JWT Authentication
- Each user can only access their own contacts
- Create, Read, Update, Delete contacts
- Search contacts by name or phone number
- Clean architecture and ready for further expansion

## Tech Stack

- ASP.NET Core (Web API)
- Entity Framework Core
- PostgreSQL
- ASP.NET Core Identity
- JWT Bearer Authentication
- Swagger / OpenAPI

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- PostgreSQL
- Visual Studio 2022/2026 or VS Code

### Setup

1. Clone the repository:
```bash
   git clone https://github.com/YOUR_USERNAME/PhoneBook.git
   cd PhoneBook
```

2. Configure your database connection and JWT settings using User Secrets (recommended) or environment variables. Example of required settings:
```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Port=5432;Database=PhoneBookDB;Username=your_user;Password=your_password"
    },
    "Jwt": {
        "Key": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
        "Issuer": "PhoneBookAPI",
        "Audience": "PhoneBookClient",
        "ExpireMinutes": 60
    }
}
```

3. Apply migrations:
```bash
dotnet ef database update
```

4. Run the project:
```bash
dotnet run
```

5. Open Swagger UI:
```text
https://localhost:xxxx/swagger
```

## API Endpoints

### Authentication

- `POST /api/Auth/register` — Register a new user
- `POST /api/Auth/login` — Login and receive JWT token

### Contacts (Requires Authorization)

- `GET /api/Contacts` — Get all contacts (supports `?search=` query)
- `GET /api/Contacts/{id}` — Get a specific contact
- `POST /api/Contacts` — Create a new contact
- `PUT /api/Contacts/{id}` — Update a contact
- `DELETE /api/Contacts/{id}` — Delete a contact

## Security Notes

- Sensitive configuration (database password, JWT key) is stored in User Secrets and is not committed to the repository.
- Always use HTTPS in production.
- JWT tokens should be kept secure on the client side.

## Future Improvements

- Frontend (React / Blazor / Angular)
- Refresh Token
- Contact groups / tags
- Pagination
- Unit & Integration tests

---
Made with ❤️ for learning and portfolio purposes.