# Race Day Event Management API MVC (ASP.NET Core Web API)

## 📖 Overview
This project is a robust **C# ASP.NET Core Web API** designed for managing sporting events, participants, and organisers. Built to satisfy the rigorous criteria of our course assignment, the system features role-based access control (RBAC), complex event enrolment workflows, database transactional integrity, and automated results tracking.

The architecture adheres strictly to RESTful design principles and clean architectural patterns, utilizing native Data Models and parameter serialization objects to prevent validation breakdowns and decouple the persistence layer from the client interface.

---

## 🛠️ Tech Stack & Architecture
* **Framework:** .NET 8.0 / 9.0 (ASP.NET Core Web API)
* **Database & ORM:** Microsoft SQL Server using native **ADO.NET (`SqlConnection`, `SqlCommand`)** raw parameter query execution paths
* **Security:** Native secure session caching tracking structures with cryptographic **BCrypt.Net** password hashing algorithms
* **Validation:** Explicitly managed imperative validation gates embedded directly within controller actions
* **Documentation:** Integrated REST endpoint signatures mapped directly via explicit routing decorators

---

## ⚙️ Functional Requirements

### 1. 🔑 Authentication & Authorization
Secured via a unified pipeline processing structured data envelopes. Access pathways evaluate the explicit string metadata fields passed back from sequential database checks to determine tracking roles: `Organiser` or `Participant`.

* **`POST /api/Login/login`** -> Verifies organiser parameters against cryptographic password hashes using an implicit work factor configuration.
* **`POST /api/Login/participant/login`** -> Sequentially validates participant account matching rules across separate data structures.

```csharp
[HttpPost("login")]
public IActionResult Logins([FromBody] LoginModel model)

[HttpPost("participant/login")]
public IActionResult ParticipantLogin([FromBody] LoginModel model) 
```

### 2. 👤 User Profile Management
Authenticated users can manage their personal profiles, mapping to data layouts like names, unique contact numbers, and emails.

* **`POST /api/Organiser/register-organiser`** -> Commits a newly registered organiser profile containing tracking columns like company organisation details into the repository framework.

```csharp
[HttpPost("register-organiser")]
public async Task<IActionResult> store_organiserInfo([FromBody] prog_part_two.Models.Organisers model) 
```

### 3. 🎉 Event Management
Allows creation and tracking of multiple physical events using parameterized insertion routines to map data directly onto target schema columns.
* **Organisers:** Complete execution authority to publish, adjust, or completely delete race entries from the catalog.
* **Participants:** Unified search exploration triggers to view available public race indexes.
* **Tracked Metrics:** EventName (*Max 15 chars*), EventType (*Max 15 chars*), Distance (*Max 5 chars*), EventDate (*Max 10 chars*), EventLocation (*Max 50 chars*), and EventDescription (*Max 50 chars*).

```csharp
[HttpPost("store-event")]
public async Task<IActionResult> StoreEvent([FromBody] prog_part_two.Models.Event model) 

[HttpGet("events/categories")]
public IActionResult GetEventCategories() 
```

### 4. 🏷️ Category Management
Organisers configure standalone classification tracks or division groups inside the storage structures to map upcoming race boundaries. Both roles can view active categories.

```csharp
[HttpPost("store-category-only")]
public async Task<IActionResult> StoreCategoryOnly([FromBody] prog_part_two.Models.Category model) 
```

### 5. 📝 Event Enrolments
Participants register for events by mapping individual account records to target race configurations, linking explicit identity bridges across separate table indices.

```csharp
[HttpPost("store_participant")]
public IActionResult StoreParticipantInfo(string name, string email, int organiserId) 
```

### 6. 🏆 Results Processing
Organisers handle operational scoring parameters, allowing execution gates to publish runner completion durations, placement lists, and leaderboard track positions safely. Participants can securely pull their historical scoreboards.

---

## 🛡️ Assignment Business Logic & Validation Rules

To prevent data corruption and enforce domain rules, the API implements strict business validation checking directly inside the core controller action gates:

* **Email Structure Constraints:** User accounts require a specific corporate syntax envelope. The backend validation wire throws an immediate error if email parameters do not explicitly contain or end with `@gmail.com`.
* **Password Length Rules:** Access codes are evaluated stringently against size rules to maintain algorithmic security integrity. Organiser registration passwords must be at least 8 characters long, while participant passwords must exceed 6 characters.
* **Input Sanitization:** Character fields enforce exact truncation guardrails mirroring database allocation parameters. The system automatically restricts `EventName` and `EventType` to 15 characters, `Distance` to 5 characters, and venue metadata coordinates to 50 characters maximum.

### 🛑 Global Error Handling
The application uses structured try-catch execution blocks wrapping code pathways to intercept server faults gracefully. Instead of leaking raw connection metadata or internal file paths to the client, it captures database exceptions and maps them to clean, parameterized tracking structures:

```json
{
  "message": "Invalid Organiser email, or password.",
  "status": 401,
  "error": "Database Error or Constraint Violation details if logged in developer tracking mode"
}
```

---

## 🗄️ Database & Seed Data

The project leverages **Microsoft SQL Server** as the persistent storage engine, processing transactions through local data instances (`(localdb)\api_part_1`) targeting the database file catalog schema `prog_part_ll`.

### 🌱 Data Seeding
Upon initialization within your environment configuration, the local database table architecture requires the direct manual initialization or registration of base structural roles to fulfill evaluation tests:
* **Default Organiser Account:** Registered using valid `@gmail.com` parameters paired with an enterprise-grade BCrypt hashed tracking key string.
* **Table Constraint Safety:** The `Events` relational structure enforces strict identity mappings, deploying fallback null parameters across foreign key indices (`OrganiserId`, `ParticipantId`, `CategoryId`) to preserve table structural normalization.

---

## 📂 Project Structure
The project follows standard ASP.NET Core architectural layouts to enforce clean separation of concerns:

```text
prog_part_two/
│
├── Controllers/       # API endpoints processing input streams (Login, Organiser)
├── Models/            # Model classes mirroring database schemas (LoginModel, Organisers, Event, Category)
├── Properties/        # Local execution tracking configurations (launchSettings.json)
├── Program.cs         # App bootstrapping pipeline, service registration, and path endpoints map
└── appsettings.json   # Environment variables containing local database connection string credentials
```

---

## 🚀 Getting Started & Setup

### Prerequisites
* [.NET SDK 8.0](https://microsoft.com) (or newer)
* Microsoft SQL Server / LocalDB instance (`Data source=(localdb)\api_part_1`)
* IDE: Visual Studio 2022, VS Code, or JetBrains Rider

### Relational Database Configuration
1. Open your connection string parameters and verify alignment with your local database instance: `Data source=(localdb)\api_part_1;database=prog_part_ll;`.
2. Ensure your local database schema tables (`Organiser`, `Participant`, `Events`, `Category`) match the exact relational script rules before starting the solution execution track.

---

## 🧪 Testing the API

### 1. Interactive Runtime Mappings
The application maps runtime endpoints cleanly, exposing access pathways transparently. If configured via development profiles, documentation schemas map to local browser endpoints at execution.

### 2. Postman Collection Integration
For seamless evaluation grading, REST request testing can be conducted using explicit JSON payload configurations:
1. **Target Endpoints:** Configure requests to point to explicit controller paths such as `api/Login/login` or `api/Organiser/store-event`.
2. **Body Definition:** Select `raw` and set the data type filter to `JSON` inside Postman.
3. **Payload Structure:** Format data models to match your C# properties exactly, ensuring objects map matching keys across the HTTP network stream seamlessly:

```json
{
  "Email": "thabethe@gmail.com",
  "Password": "SecurePassword123"
}
```

---

## 📚 References & Background Documentation
* **ASP.NET Core Framework:** Microsoft documentation for building high-performance cross-platform web APIs. [Microsoft ASP.NET Core Docs](https://microsoft.com).
* **ADO.NET Data Client:** Framework documentation for executing direct parameterized commands securely on Microsoft SQL Server. [Microsoft Data SqlClient Docs](https://microsoft.com).

# Race Day Event Management API (ASP.NET Core Web API)

## 📖 Overview
This project is a robust **C# ASP.NET Core Web API** designed for managing sporting events, participants, and organisers. Built to satisfy the rigorous criteria of our course assignment, the system features role-based access control (RBAC), complex event enrolment workflows, database transactional integrity, and automated results tracking. 

The architecture adheres strictly to RESTful design principles and clean architectural patterns, utilizing Data Transfer Objects (DTOs) to prevent over-posting vulnerabilities and decouple the persistence layer from the client interface.

---

## 🛠️ Tech Stack & Architecture
* **Framework:** .NET 8.0 / 9.0 (ASP.NET Core Web API)
* **Database & ORM:** Microsoft SQL Server with Entity Framework (EF) Core
* **Security:** JSON Web Tokens (JWT) with Role-Based Authorization Policies
* **Validation:** FluentValidation for decoupled, explicit request validation
* **Documentation:** Swagger / OpenAPI integration for interactive API exploring

---

## ⚙️ Functional Requirements

### 1. 🔑 Authentication & Authorization
Secured via JWT bearer tokens. Users are granted specific access levels depending on their registration role: `Organiser` or `Participant`.

* **`POST /api/auth/register`** -> Registers a new user with a designated role.
* **`POST /api/auth/login`** -> Validates credentials and returns a JWT token.

```csharp
[HttpPost("register")]
public async Task<IActionResult> Register([FromBody] UserDto user)

[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginDto login) 
```

### 2. 👤 User Profile Management
Authenticated users can manage their personal profiles.

* **`GET /api/profile`** -> Fetches profile details for the currently logged-in user.
* **`PUT /api/profile`** -> Updates profile data.

```csharp
[Authorize]
[HttpGet("profile")]
public async Task<IActionResult> GetProfile() 
```

### 3. 🎉 Event Management
Allows creation and tracking of multiple physical events.
* **Organisers:** Full CRUD permissions (Create, Read, Update, Delete).
* **Participants:** Read-only access (Read).
* **Tracked Metrics:** Name, Description, Date, Location, Distance, and Event Type (*Run, Walk, Cycle*).

```csharp
[Authorize(Roles = "Organiser")]
[HttpPost("events")]
public async Task<IActionResult> CreateEvent([FromBody] EventDto eventDto) 

[AllowAnonymous]
[HttpGet("events")]
public async Task<IActionResult> GetEvents() 
```

### 4. 🏷️ Category Management
Organisers configure age brackets or target distances (e.g., *Under 20, Senior, 10km, 21km*) to structure the competitions. Both roles can view active categories.

```csharp
[Authorize(Roles = "Organiser")]
[HttpPost("categories")]
public async Task<IActionResult> AddCategory([FromBody] CategoryDto category) 
```

### 5. 📝 Event Enrolments
Participants register for events by selecting a specific category. The system maps the explicit relational bond between the `Participant`, `Event`, and `Category`. Organisers retain full oversight of enrollment lists.

```csharp
[Authorize(Roles = "Participant")]
[HttpPost("enrolments")]
public async Task<IActionResult> Enrol([FromBody] EnrolmentDto enrolment) 
```

### 6. 🏆 Results Processing
Organisers capture metrics post-event to determine leaderboard status. Participants can securely pull their historical scoreboards.

```csharp
[Authorize(Roles = "Organiser")]
[HttpPost("results")]
public async Task<IActionResult> RecordResult([FromBody] ResultDto result) 
```

---

## 🛡️ Assignment Business Logic & Validation Rules

To prevent data corruption and enforce domain rules, the API implements strict business validation using **FluentValidation** pipeline behaviors:

* **Event Date Constraints:** Events cannot be created in the past. The application throws a validation error if `EventDate <= DateTime.UtcNow`.
* **Unique Enrolments:** A unique constraint exists across `ParticipantId` and `EventId`. A participant cannot enrol in the same event more than once.
* **Input Sanitization:** Event titles and names are restricted to a maximum length of 100 characters to prevent buffer overflow constraints or storage degradation.

### 🛑 Global Error Handling
The application uses a custom Global Exception Handling Middleware to intercept server faults gracefully. Instead of leaking internal stack traces to the client, it maps domain errors to RFC 7807 **Problem Details** compliant JSON structures:

```json
{
  "type": "https://ietf.org",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "EventDate": ["The Event Date cannot be in the past."]
  }
}
```

---

## 🗄️ Database & Seed Data

The project leverages **Microsoft SQL Server** as the persistent storage engine, configured via Entity Framework Core (`DbContext`).

### 🌱 Data Seeding
Upon application startup within a development environment, the database context runs an automated data-seeder (`DbInitializer.cs`) ensuring the following structures are available for grading:
* **Default Organiser Account:** `organiser@eventapi.com` (Password: `SecureP@ss123!`)
* **Default Participant Account:** `participant@eventapi.com` (Password: `SecureP@ss123!`)
* **Base Categories:** Prefilled default categories including *Under 20*, *Senior*, *10km*, and *21km*.

---

## 📂 Project Structure
The project follows standard ASP.NET Core architectural layouts to enforce clean separation of concerns:

```text
EventManagementAPI/
│
├── Controllers/       # HTTP REST endpoints handling requests and routing
├── Models/            # Database entities and Data Transfer Objects (DTOs)
├── Services/          # Business logic Layer (Validation, Processing)
├── Data/              # EF Core DbContext, Migrations, and DbInitializer (Seed Data)
├── Properties/        # Launch profiles (launchSettings.json)
├── Program.cs         # Application bootstrapping, DI container, and middleware pipeline
└── appsettings.json   # Configuration (SQL Server Connection String, JWT Keys)
```

---

## 🚀 Getting Started & Setup

### Prerequisites
* [.NET SDK 8.0](https://microsoft.com) (or newer)
* Microsoft SQL Server / LocalDB instance
* IDE: Visual Studio 2022, VS Code, or JetBrains Rider

### Database Migrations
1. Update your connection string inside `appsettings.json` under `ConnectionStrings:DefaultConnection`.
2. Open your terminal in the project root directory and run the following Entity Framework tools commands:

```bash
# Add a new migration tracking database state
dotnet ef migrations add InitialEventMgmtMigration

# Apply structural changes directly to SQL Server
dotnet ef database update
```

### Running the Application
Execute the run profile from your CLI:

```bash
dotnet run
```
The application will bootstrap and expose endpoints locally at `https://localhost:7001` and `http://localhost:5000`.

---

## 🧪 Testing the API

### 1. Interactive Swagger UI
Navigate to `https://localhost:7001/swagger` in your browser to view the interactive documentation panel. 

### 2. Postman Collection Integration
A dedicated Postman collection file is exported inside the project root at `/tests/EventManagementAPI.postman_collection.json`. 
1. **Import** the collection into Postman.
2. Run the **Auth -> Login** request first to obtain a bearer token.
3. The collection is preconfigured with a **Test Script** that automatically saves the JWT token into a global environment variable (`{{jwt_token}}`), authorising all consecutive downstream endpoint requests automatically.

---

## 📚 References & Background Documentation
* **ASP.NET Core Framework:** Microsoft documentation for building high-performance cross-platform web APIs. [Microsoft ASP.NET Core Docs](https://microsoft.com).
* **Entity Framework Core:** Object-Relational Mapping framework documentation used for relational database modeling on SQL Server. [EF Core Documentation](https://microsoft.com).
* **JWT Authentication:** Industry standards for implementing role-based authorization inside distributed client-server systems. [RFC 7519 Standards](https://ietf.org).
* **API Error Structures:** Architectural guidance for uniform HTTP error message modeling. [RFC 7807 Problem Details for HTTP APIs](https://ietf.org).

1. Use Entity Framework Core for database persistence.
2. Secure endpoints with [Authorize].
3. Store secrets (like JWT keys) in appsettings.json.

