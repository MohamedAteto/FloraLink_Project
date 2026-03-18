# 🌿 Smart Plant Monitoring System

IoT plant monitoring system with ASP.NET Core backend, React dashboard, and ESP32 sensor integration.

---

## Project Structure

```
FloraLink_Project/          ← ASP.NET Core Web API (.NET 8)
  Domain/                   ← Entities + Repository Interfaces
  Application/              ← Services, DTOs, Service Interfaces
  Infrastructure/           ← EF Core DbContext + Repositories
  API/                      ← Controllers
  Program.cs                ← DI wiring + middleware

floralink-dashboard/        ← React + Vite frontend
  src/
    components/             ← Reusable UI components
    pages/                  ← Route-level pages
    services/api.js         ← Axios API client
    context/AuthContext.jsx ← JWT auth state


API runs at: `https://localhost:7001`  
Swagger UI: `https://localhost:7001/swagger`

### Frontend

```bash
cd floralink-dashboard
npm install
npm run dev
```

Dashboard runs at: `http://localhost:5173`

Email assigned => demo@floralink.io
Password => demo1234
---

## API Endpoints

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | /api/auth/register | No | Register user |
| POST | /api/auth/login | No | Login |
| GET | /api/plants | JWT | Get user's plants |
| POST | /api/plants | JWT | Add plant |
| DELETE | /api/plants/{id} | JWT | Delete plant |
| GET | /api/plant-types | No | List plant types |
| POST | /api/sensor-data | No | IoT device posts data |
| GET | /api/sensor-data/{plantId}/readings | JWT | Get readings |
| POST | /api/watering | JWT | Manual watering |
| GET | /api/watering/{plantId} | JWT | Watering history |
| GET | /api/alerts | JWT | Get unread alerts |
| PATCH | /api/alerts/{id}/read | JWT | Dismiss alert |
| GET | /api/diary/{plantId} | JWT | Get diary entries |
| POST | /api/diary | JWT | Add diary entry |
| DELETE | /api/diary/{id} | JWT | Delete diary entry |

---

## Health Score Formula

HealthScore = 100 - moistureDeviation - temperatureDeviation


Status mapping: 80–100 → Happy | 60–80 → OK | 40–60 → Thirsty | 0–40 → Stressed
