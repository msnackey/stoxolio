# Plan: Set Up DotNet Backend + React Frontend for Stoxolio
This plan establishes a full-stack portfolio management application with an ASP.NET Core API backend, React TypeScript frontend using Vite, and SQLite database. The structure uses separate service and client folders with JWT authentication, password hashing, backend-computed fields, environment-based configuration, and environment-specific databases (test with sample data, production clean).

## Steps
1. Create root-level configuration (.gitignore, README.md) defining project structure and setup instructions
2. Initialize ASP.NET Core backend in service/ with Entity Framework Core, SQLite DbContext, User and authentication models, Category and Stock models with computed fields
3. Create authentication endpoints: user registration (with password hashing via BCrypt/PBKDF2) and login (returns JWT token)
4. Create API controllers for Category and Stock with CRUD endpoints; computed fields (value, price_change, value_change) calculated in backend via DTOs
5. Implement JWT authentication middleware for protected routes and Bearer token validation
6. Configure CORS to accept frontend requests and allow credentials
7. Set up React TypeScript frontend in client/ with Vite; create authentication context using localStorage for JWT tokens, API client service reading base URL from .env file
8. Create environment-specific SQLite databases: stoxolio.db (production, clean) and stoxolio.test.db (test with seed data and test user account)
9. Add database initialization and seeding logic; configure appsettings.json to switch databases based on environment variable