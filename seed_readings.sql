-- Seed sensor readings for plants 3,4,5,6
DECLARE @pid INT, @i INT, @m FLOAT, @t FLOAT, @h FLOAT

-- Plant 3 (Tropico - Tomato)
IF NOT EXISTS (SELECT 1 FROM SensorReadings WHERE PlantId=3)
BEGIN
    SET @pid=3; SET @i=180;
    WHILE @i>=0
    BEGIN
        SET @m=62+RAND()*15; SET @t=23+RAND()*7; SET @h=72+RAND()*23;
        INSERT INTO SensorReadings(PlantId,SoilMoisture,Temperature,HealthScore,PlantStatus,RecordedAt)
        VALUES(@pid,@m,@t,@h,'Happy',DATEADD(hour,-@i*4,GETUTCDATE()));
        SET @i=@i-1;
    END
END

-- Plant 4 (Basilio - Basil)
IF NOT EXISTS (SELECT 1 FROM SensorReadings WHERE PlantId=4)
BEGIN
    SET @pid=4; SET @i=180;
    WHILE @i>=0
    BEGIN
        SET @m=58+RAND()*14; SET @t=22+RAND()*6; SET @h=68+RAND()*27;
        INSERT INTO SensorReadings(PlantId,SoilMoisture,Temperature,HealthScore,PlantStatus,RecordedAt)
        VALUES(@pid,@m,@t,@h,'Happy',DATEADD(hour,-@i*4,GETUTCDATE()));
        SET @i=@i-1;
    END
END

-- Plant 5 (Fernanda - Pothos)
IF NOT EXISTS (SELECT 1 FROM SensorReadings WHERE PlantId=5)
BEGIN
    SET @pid=5; SET @i=180;
    WHILE @i>=0
    BEGIN
        SET @m=43+RAND()*14; SET @t=21+RAND()*7; SET @h=71+RAND()*24;
        INSERT INTO SensorReadings(PlantId,SoilMoisture,Temperature,HealthScore,PlantStatus,RecordedAt)
        VALUES(@pid,@m,@t,@h,'Happy',DATEADD(hour,-@i*4,GETUTCDATE()));
        SET @i=@i-1;
    END
END

-- Plant 6 (Minty - Mint)
IF NOT EXISTS (SELECT 1 FROM SensorReadings WHERE PlantId=6)
BEGIN
    SET @pid=6; SET @i=180;
    WHILE @i>=0
    BEGIN
        SET @m=63+RAND()*14; SET @t=21+RAND()*6; SET @h=73+RAND()*22;
        INSERT INTO SensorReadings(PlantId,SoilMoisture,Temperature,HealthScore,PlantStatus,RecordedAt)
        VALUES(@pid,@m,@t,@h,'Happy',DATEADD(hour,-@i*4,GETUTCDATE()));
        SET @i=@i-1;
    END
END

SELECT PlantId, COUNT(*) as Readings FROM SensorReadings GROUP BY PlantId ORDER BY PlantId;
