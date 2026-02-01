# Stoxolio

A full-stack portfolio management web application built with ASP.NET Core, React, TypeScript, and SQLite.

## Project Structure

```
stoxolio/
├── service/                # ASP.NET Core backend API (CQRS with Minimal APIs)
│   ├── Auth/               # Authentication service (first-class service)
│   ├── Features/           # Domain features organized by concern
│   ├── Endpoints/          # Centralized endpoint mapping
│   ├── BuildingBlocks/
│   │   └── CQRS/           # CQRS abstractions
│   ├── Extensions/         # Extension methods
│   ├── Models/             # Entity models (User, Category, Stock)
│   ├── Data/               # DbContext
│   ├── DTOs/               # Data transfer objects
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   └── Program.cs
├── client/                 # React + TypeScript frontend with Vite
│   ├── src/
│   │   ├── components/     # React components
│   │   ├── pages/          # Page components
│   │   ├── context/        # Auth context
│   │   ├── services/       # API client
│   │   ├── types/          # TypeScript interfaces
│   │   └── App.tsx
│   ├── .env
│   └── vite.config.ts
├── .gitignore
├── README.md
└── LICENSE
```

## Requirements

- **.NET 10 SDK** (10.0.0 or later with rollForward latestMinor)
- **Node.js 18** or later
- **npm** or **yarn**

## Setup Instructions

### Backend (ASP.NET Core)

1. Navigate to the backend directory:
   ```bash
   cd service
   ```

2. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

3. Build the project:
   ```bash
   dotnet build
   ```

4. Run the backend server:
   ```bash
   # Development mode (with test data and sample data)
   dotnet run --configuration Development
   
   # Production mode (clean database)
   dotnet run --configuration Release
   ```
   The API will be available at `http://localhost:5001/api`.

### Frontend (React + Vite)

1. Navigate to the frontend directory:
   ```bash
   cd client
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. The `.env` file is already configured with the development API URL (`http://localhost:5001/api`). For production, update `.env.production`:
   ```
   VITE_API_BASE_URL=https://api.stoxolio.com/api
   ```

4. Run the development server:
   ```bash
   npm run dev
   ```
   The frontend will be available at `http://localhost:5173`.

## Running the Application

To run both backend and frontend together:

**Terminal 1 - Backend:**
```bash
cd service
dotnet run --configuration Development
```

**Terminal 2 - Frontend:**
```bash
cd client
npm run dev
```

Then navigate to `http://localhost:5173` in your browser.

## Database

The application uses SQLite with two environment-specific databases:

- **Development**: `stoxolio.test.db` — includes sample data (3 test categories with stocks)
- **Production**: `stoxolio.db` — clean database

The database is automatically created and seeded on first run in development mode.

## Default Test Account

In development mode, the following test user and data are automatically created:

**Test User Credentials:**
- **Username**: `testuser`
- **Password**: `testpassword123`
- **Email**: `test@example.com`

**Sample Categories:**
- Tech (Target: $10,000)
- Finance (Target: $5,000)
- Healthcare (Target: $3,000)

**Sample Stocks:**
- Apple (AAPL) - 100 shares @ $150.50 - Tech category
- Microsoft (MSFT) - 50 shares @ $320.00 - Tech category
- JPMorgan Chase (JPM) - 75 shares @ $180.25 - Finance category

## Architecture

### Backend: CQRS with Minimal APIs

The backend uses the **Command Query Responsibility Segregation (CQRS)** pattern with ASP.NET Core's minimal APIs (no traditional MVC controllers):

- **Auth Service**: First-class service handling authentication and JWT token generation
- **Features**: Organized by domain concern (Categories, Stocks) with feature-specific folders
- **Endpoints**: Individual endpoint classes per operation, mapped centrally in `Endpoints/` directory
- **CQRS Pattern**: 
  - **Queries** (IQuery<TResponse>) for read operations with IQueryHandler implementations
  - **Commands** (ICommand<TResponse>) for write operations with ICommandHandler implementations
- **Minimal APIs**: All endpoints registered via extension methods in Program.cs for streamlined configuration

This architecture improves:
- **Maintainability**: Each endpoint is in its own file with clear responsibility
- **Testability**: Query/command handlers can be tested independently
- **Scalability**: Easy to add new endpoints or features following established patterns
- **Separation of Concerns**: Auth is elevated to infrastructure level, features handle domain logic



### Authentication
- `POST /api/auth/register` — Register a new user
- `POST /api/auth/login` — Login and receive JWT token

### Categories (Protected)
- `GET /api/categories` — Get all categories
- `GET /api/categories/{id}` — Get a specific category
- `POST /api/categories` — Create a new category
- `PUT /api/categories/{id}` — Update a category
- `DELETE /api/categories/{id}` — Delete a category

### Stocks (Protected)
- `GET /api/stocks` — Get all stocks (with computed fields)
- `GET /api/stocks/{id}` — Get a specific stock (with computed fields)
- `POST /api/stocks` — Create a new stock
- `PUT /api/stocks/{id}` — Update a stock
- `DELETE /api/stocks/{id}` — Delete a stock

**Note:** All endpoints except `/auth/*` require a valid JWT token in the `Authorization: Bearer <token>` header.

**Computed Fields:** Stock endpoints automatically calculate `value` (shares × price), `priceChange` (current price - previous price), and `valueChange` (shares × priceChange).

## Development Workflow

1. Start the backend server (`cd service && dotnet run`)
2. In another terminal, start the frontend dev server (`cd client && npm run dev`)
3. Open `http://localhost:5173` in your browser
4. Login with the test account to access the portfolio

## Authentication

The application uses JWT (JSON Web Tokens) for authentication:

1. User logs in via the frontend → Backend validates credentials and returns JWT token
2. JWT token is stored in browser localStorage
3. All protected API requests include the token in the `Authorization: Bearer <token>` header
4. Backend validates token and processes the request

## Building for Production

### Backend
```bash
cd service
dotnet publish -c Release -o ./bin/Release/publish
```

### Frontend
```bash
cd client
npm run build
```

The built frontend files will be in `client/dist/` and can be served by the backend or a static file server.