![AuctoValue Logo](Frontend/src/assets/logo.svg)

A full-stack auction fee calculator application for vehicles, built as part of a technical assessment.

## Overview

AuctoValue is a web application that calculates fee breakdowns for vehicle auctions. It supports both common and luxury vehicles, calculating base fees, special fees, association fees, and storage fees with a real-time, responsive interface.

## Technologies Used

### Backend
- **C# / .NET 9.0** - Modern web API framework
- **ASP.NET Core** - RESTful API with controller-based architecture
- **MSTest** - Unit testing framework

### Frontend
- **Vue.js 3** - Progressive JavaScript framework with Composition API
- **TypeScript** - Type-safe JavaScript
- **Tailwind CSS** - Utility-first CSS framework
- **Vite** - Next-generation frontend build tool

### Development Tools
- **ESLint** - JavaScript/TypeScript linting
- **Prettier** - Code formatting

## Architecture & Design Decisions

### What I Did and Why

#### Backend Architecture

**Service-Oriented Design**

I implemented a clean separation of concerns with the following structure:

- **Controllers** (`AuctionController.cs`) - Handle HTTP routing and request/response management
- **Services** (`AuctionCalculatorService.cs`) - Contains all business logic for fee calculations
- **Models** - Separated into individual files for maintainability:
  - `CalculateRequest.cs` - Input model for API requests
  - `FeeBreakdown.cs` - Output model containing all calculated fees
  - `VehicleType.cs` - Enum for vehicle classification

**Why this approach?**

While I could have implemented everything in a single file or put the logic directly in the controller, this would not be professional or maintainable. The service layer architecture:
- Makes the code testable (services can be unit tested independently)
- Follows industry best practices for C# applications
- Separates business logic from HTTP concerns
- Makes the codebase scalable and maintainable

**Configuration-Driven Design**

All fee calculations are driven by `appsettings.json`, allowing easy configuration changes without code modifications:
- Base fee percentages and limits
- Special fee percentages
- Storage fees
- CORS origins

**Middleware & Security**

- CORS configuration to restrict frontend origins
- Rate limiting (20 requests/minute) to prevent abuse
- Comprehensive error handling and logging

#### Frontend Architecture

**Component Structure**

The Vue.js application follows a clean architecture:
- **Models** - TypeScript interfaces matching backend DTOs for type safety
- **Services** (`feeApi.ts`) - Centralized API communication layer
- **Assets** - Logo and branding materials
- **App.vue** - Main application component with reactive form handling

**Why Vue.js with Tailwind CSS?**

I chose Vue.js for its simplicity and reactivity, paired with Tailwind CSS because I genuinely enjoy its utility-first approach. This combination allows for rapid development with maintainable, responsive designs.

**Development Quality Tools**

- ESLint and Prettier ensure consistent code style
- TypeScript provides compile-time type checking
- Reactive watchers for real-time fee calculations

#### Testing Strategy

**MSTest Unit Tests** (`AuctionCalculatorServiceTests.cs`)

I focused testing efforts on the service layer where business logic resides. The test suite:
- Tests all fee calculation scenarios (common/luxury vehicles)
- Validates edge cases (minimum/maximum fees)
- Uses configuration from `appsettings.json` for realistic testing
- Follows AAA pattern (Arrange, Act, Assert)

**Why only service tests?**

Controllers are thin wrappers around services with minimal logic, and models are simple data containers. Testing the service layer provides the most value for ensuring calculation accuracy.

### What I Didn't Do (But Would in Production)

#### Security Enhancements

**Token-Based Authentication**

Currently, the application only validates CORS origins. In a production environment, I would implement:
- JWT or session-based token authentication
- Frontend requests a token on application load
- Backend validates tokens via middleware for all protected routes
- Token refresh mechanisms for long-lived sessions

**Why not implemented?**

For this assessment's scope, CORS origin validation provides adequate protection. Full authentication would be essential for public-facing production.

#### Frontend Improvements

**Component Architecture**
- Create reusable UI components (Input, Select, FeeCard, etc.)
- Implement a proper layout system with nested routing
- Use a component library (e.g., PrimeVue, Vuetify) for consistent UI
- Create a centralized API client service to manage:
  - All HTTP requests
  - Token management and injection
  - Error handling and retry logic
  - Request/response interceptors

**Why not implemented?**
With only one view and minimal interactions, creating dozens of small components would be over-engineering. In a larger application, this structure would be essential.

#### Backend Improvements

**Data Layer**

- Database integration (Entity Framework Core with SQL Server/PostgreSQL)
- Repository pattern for data access
- Database migrations for schema management

**DTOs (Data Transfer Objects)**

- Separate internal domain models from API contracts
- Use AutoMapper for model transformations
- Versioned API DTOs for backward compatibility

**Middleware**

I implemented rate limiting middleware. Additional middleware for production would include:
- Authentication/Authorization middleware (already structured for this)
- Request/Response logging middleware
- Global exception handling middleware
- Request validation middleware

**Why not implemented?**

This application has no data persistence requirements. Adding a database and repositories would be unnecessary complexity for a calculation service.

#### Testing Improvements

**Frontend Testing**
- Unit tests for components (Vitest)
- End-to-end tests (Playwright/Cypress)
- Integration tests for API service layer

**Backend Testing**

- Integration tests for controllers
- API endpoint testing with WebApplicationFactory
- Load testing for rate limiter validation

**Why not implemented?**

For this scope, comprehensive backend unit tests provide sufficient coverage. E2E tests would be valuable in production but are time-intensive to set up.

## Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js](https://nodejs.org/) (v18 or higher)
- [npm](https://www.npmjs.com/) (comes with Node.js)

### Quick Start (Automated Setup)

For first-time setup, simply run the setup script:

```bash
setup.bat
```

This will automatically:
1. Copy example configuration files
2. Build the backend
3. Install frontend dependencies
4. Run linting and formatting
5. Start both frontend and backend servers

### Manual Setup

If you prefer to set up manually, follow these steps:

#### 1. Clone the Repository

```bash
git clone <repository-url>
cd AuctoValue
```

#### 2. Configure Environment Files

**Backend Configuration**

Copy the example configuration file:
```bash
copy Backend\appsettings.json.example Backend\appsettings.json
```

Edit `Backend/appsettings.json` if needed to customize:
- API port
- CORS origins
- Fee calculation parameters
- Rate limiting settings

**Frontend Configuration**

Copy the example environment file:
```bash
copy Frontend\.env.example Frontend\.env
```

Edit `Frontend/.env` to match your backend URL (default is `http://localhost:5118`).

#### 3. Build and Run Backend

```bash
cd Backend
dotnet restore
dotnet build
dotnet run
```

The backend API will start on `http://localhost:5118` (or the port specified in `appsettings.json`).

#### 4. Install and Run Frontend

```bash
cd Frontend
npm install
npm run dev
```

The frontend will start on `http://localhost:5173`.

#### 5. Run Tests

```bash
cd Tests
dotnet test
```

#### 6. Frontend Code Quality

Run linting:
```bash
cd Frontend
npm run lint
```

Run formatting:
```bash
cd Frontend
npm run format
```

## API Endpoints

### Calculate Fees
**POST** `/api/auction/calculate`

Calculate auction fees for a vehicle.

**Request Body:**
```json
{
  "vehiclePrice": 1000.00,
  "vehicleType": 0
}
```

- `vehicleType`: `0` for Common, `1` for Luxury

**Response:**
```json
{
  "baseFee": 50.00,
  "specialFee": 20.00,
  "associationFee": 10.00,
  "storageFee": 100.00,
  "totalFees": 180.00,
  "grandTotal": 1180.00
}
```

### Health Check
**GET** `/api/auction/health`

Check API health status.

**Response:**
```json
{
  "status": "healthy",
  "timestamp": "2025-01-18T12:00:00Z"
}
```

## Configuration

### Backend Configuration (appsettings.json)

```json
{
  "CorsOrigins": ["http://localhost:5173"],
  "Fees": {
    "StorageFee": 100,
    "BaseFeePercentage": 0.10,
    "CommonBaseFeeMin": 10,
    "CommonBaseFeeMax": 50,
    "LuxuryBaseFeeMin": 25,
    "LuxuryBaseFeeMax": 200,
    "CommonSpecialFeePercentage": 0.02,
    "LuxurySpecialFeePercentage": 0.04
  }
}
```

### Frontend Configuration (.env)

```env
VITE_API_BASE_URL=http://localhost:5118
```

## Rate Limiting

The API implements rate limiting to prevent abuse:
- **Limit:** 20 requests per minute per IP address
- **Response:** `429 Too Many Requests` when exceeded

## Development

### Running the Application

To run the application in development mode, you need to start both the backend and frontend servers in separate terminals:

**Backend Server (Terminal 1)**

```bash
cd Backend
dotnet run
```

The backend API will start on `http://localhost:5118` by default.

Hot reload is available with:
```bash
cd Backend
dotnet watch run
```

**Frontend Server (Terminal 2)**

```bash
cd Frontend
npm run dev
```

The frontend will start on `http://localhost:5173` by default.

Vite provides hot module replacement (HMR) for instant updates during development.

## Building for Production

### Backend

Build the backend for production:

```bash
cd Backend
dotnet publish -c Release -o ./publish
```

Run the production build:
```bash
cd Backend\publish
dotnet AuctoValue.Backend.dll
```

The backend API will start on `http://localhost:5000` by default.

### Frontend

Build the frontend for production:

```bash
cd Frontend
npm run build
```

The production build will be in `Frontend/dist/`.

Preview the production build locally:
```bash
cd Frontend
npm run preview
```

The preview server will start on `http://localhost:4173` by default.