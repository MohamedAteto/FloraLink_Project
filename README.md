# 🌿 FloraLink — Smart Plant Monitoring System

A full-stack IoT plant monitoring system with ASP.NET Core backend, React dashboard, and ESP32 sensor integration.

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

IoT/
  esp32_sensor.ino          ← Arduino sketch for ESP32
```

---

## Quick Start

### Backend

```bash
cd FloraLink_Project

# Restore packages
dotnet restore

# Create initial migration
dotnet ef migrations add InitialCreate --project . --startup-project .

# Apply migration (creates DB + seeds plant types)
dotnet ef database update

# Run API
dotnet run
```

API runs at: `https://localhost:7001`  
Swagger UI: `https://localhost:7001/swagger`

### Frontend

```bash
cd floralink-dashboard
npm install
npm run dev
```

Dashboard runs at: `http://localhost:5173`

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

## IoT Device Setup

1. Open `IoT/esp32_sensor.ino` in Arduino IDE
2. Install libraries: `ArduinoJson`, `DHT sensor library`, `WiFi`
3. Set your WiFi credentials and API URL
4. Set `SENSOR_ID` to match the SensorId you enter when adding a plant
5. Flash to ESP32

The device sends data every 5 minutes. The backend automatically:
- Calculates health score
- Triggers alerts if thresholds are breached
- Auto-waters if moisture is critically low

---

## Seeded Plant Types

| Type | Moisture Range | Temp Range | Critical Moisture |
|------|---------------|------------|-------------------|
| Cactus | 20–40% | 20–35°C | 10% |
| Tropical | 60–80% | 18–30°C | 40% |
| Herb | 50–70% | 15–28°C | 30% |
| Fern | 70–90% | 15–25°C | 50% |
| Succulent | 25–50% | 15–32°C | 15% |

---

## Health Score Formula

```
HealthScore = 100 - moistureDeviation - temperatureDeviation

deviation = 0 if within ideal range
deviation = (distance_from_range / range_boundary) * 50 if outside
```

Status mapping: 80–100 → Happy | 60–80 → OK | 40–60 → Thirsty | 0–40 → Stressed
