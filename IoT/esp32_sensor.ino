/*
 * FloraLink ESP32 Sensor Node
 * Reads soil moisture + temperature and POSTs to FloraLink API
 * 
 * Hardware:
 *   - ESP32 Dev Board
 *   - Capacitive Soil Moisture Sensor (analog pin 34)
 *   - DHT22 Temperature/Humidity Sensor (digital pin 4)
 *   - Relay Module for water pump (digital pin 26)
 */

#include <WiFi.h>
#include <HTTPClient.h>
#include <ArduinoJson.h>
#include <DHT.h>

// ── Configuration ─────────────────────────────────────────────────────────────
const char* WIFI_SSID     = "YOUR_WIFI_SSID";
const char* WIFI_PASSWORD = "YOUR_WIFI_PASSWORD";
const char* API_URL       = "https://YOUR_SERVER/api/sensor-data";
const char* SENSOR_ID     = "ESP32-001";  // Must match plant's SensorId in DB

// ── Pin Definitions ───────────────────────────────────────────────────────────
#define MOISTURE_PIN    34
#define DHT_PIN          4
#define DHT_TYPE       DHT22
#define PUMP_RELAY_PIN  26

// ── Calibration (adjust for your sensor) ─────────────────────────────────────
#define MOISTURE_DRY   3200   // ADC value when completely dry
#define MOISTURE_WET    800   // ADC value when fully saturated

// ── Timing ────────────────────────────────────────────────────────────────────
#define SEND_INTERVAL_MS  (5 * 60 * 1000)  // Send every 5 minutes

DHT dht(DHT_PIN, DHT_TYPE);

void setup() {
  Serial.begin(115200);
  pinMode(PUMP_RELAY_PIN, OUTPUT);
  digitalWrite(PUMP_RELAY_PIN, LOW);  // Pump off by default

  dht.begin();
  connectWiFi();
}

void loop() {
  if (WiFi.status() != WL_CONNECTED) connectWiFi();

  float moisture    = readMoisture();
  float temperature = dht.readTemperature();

  if (isnan(temperature)) {
    Serial.println("DHT read failed, skipping.");
    delay(SEND_INTERVAL_MS);
    return;
  }

  Serial.printf("Moisture: %.1f%%  Temp: %.1f°C\n", moisture, temperature);
  sendSensorData(moisture, temperature);

  delay(SEND_INTERVAL_MS);
}

float readMoisture() {
  int raw = analogRead(MOISTURE_PIN);
  float pct = map(raw, MOISTURE_DRY, MOISTURE_WET, 0, 100);
  return constrain(pct, 0.0f, 100.0f);
}

void connectWiFi() {
  Serial.print("Connecting to WiFi");
  WiFi.begin(WIFI_SSID, WIFI_PASSWORD);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("\nConnected: " + WiFi.localIP().toString());
}

void sendSensorData(float moisture, float temperature) {
  HTTPClient http;
  http.begin(API_URL);
  http.addHeader("Content-Type", "application/json");

  StaticJsonDocument<128> doc;
  doc["sensorId"]    = SENSOR_ID;
  doc["soilMoisture"] = moisture;
  doc["temperature"]  = temperature;

  String body;
  serializeJson(doc, body);

  int code = http.POST(body);
  if (code == 200) {
    Serial.println("Data sent successfully.");
  } else {
    Serial.printf("POST failed: %d\n", code);
  }
  http.end();
}
