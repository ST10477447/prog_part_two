# Event Management API (C# ASP.NET Core)

## 📖 Overview
This project is a **C# ASP.NET Core Web API** for managing events, participants, and organisers.  
It supports authentication, user profiles, event enrolments, and results tracking.  
The API follows RESTful principles and is designed for scalability.

---

## ⚙️ Functional Requirements

### 🔑 Authentication
- Users can **register** and **log in** based on their role (`Organiser` or `Participant`).
- JWT tokens secure sessions.

```csharp
[HttpPost("register")]
public async Task<IActionResult> Register(UserDto user) { ... }

[HttpPost("login")]
public async Task<IActionResult> Login(LoginDto login) { ... }
---

### User Profile
Both roles can view and update their profile information.
[Authorize]
[HttpGet("profile")]
public async Task<IActionResult> GetProfile() { ... }
[Authorize]
[HttpPut("profile")]
public async Task<IActionResult> UpdateProfile(ProfileDto profile) { ... }

---
🎉 Events
Organisers can create, update, delete events.
Both roles can view events.
Each event captures:
Name, Description, Date, Location, Distance, Event Type (Run, Walk, Cycle).
[HttpPost("events")]
public async Task<IActionResult> CreateEvent(EventDto eventDto) { ... }
[HttpGet("events")]
public async Task<IActionResult> GetEvents() { ... }
---
🏷 Categories
Organisers define age or distance categories (e.g., Under 20, Senior, 10km, 21km).
Both roles can view categories.
[HttpPost("categories")]
public async Task<IActionResult> AddCategory(CategoryDto category) { ... }
[HttpGet("categories")]
public async Task<IActionResult> GetCategories() { ... }
---
📝 Event Enrolments
Participants enrol in events by selecting a category.
System records the link between Participant, Event, and Category.
Organisers can view enrolments.
[HttpPost("enrolments")]
public async Task<IActionResult> Enrol(EnrolmentDto enrolment) { ... }
[HttpGet("enrolments/{eventId}")]
public async Task<IActionResult> GetEventEnrolments(int eventId) { ... }
---
🏆 Results
Organisers capture finish times and positions.
Participants can view their own results.
[HttpPost("results")]
public async Task<IActionResult> RecordResult(ResultDto result) { ... }
[HttpGet("results/{participantId}")]
public async Task<IActionResult> GetResults(int participantId) { ... }
---
## EventManagementAPI/
│
├── Controllers/        # API endpoints
├── Models/             # DTOs and Entity classes
├── Services/           # Business logic
├── Data/               # EF Core DbContext
├── Program.cs          # Entry point
└── README.md           # Documentation
---
✅ Notes
1. Use Entity Framework Core for database persistence.
2. Secure endpoints with [Authorize].
3. Store secrets (like JWT keys) in appsettings.json.

