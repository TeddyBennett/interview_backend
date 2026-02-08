-- Create Countries Table
CREATE TABLE Countries (
    Id SERIAL PRIMARY KEY,
    Code VARCHAR(10) NOT NULL UNIQUE,
    Name VARCHAR(100) NOT NULL
);

-- Create DocumentTypes Table
CREATE TABLE DocumentTypes (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL
);

-- Create Passengers Table
CREATE TABLE Passengers (
    PassengerId SERIAL PRIMARY KEY,
    DocId INT NOT NULL,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    DateOfBirth DATE,
    Gender VARCHAR(10),
    CountryId INT REFERENCES Countries(Id),
    IdfDocTypeId INT REFERENCES DocumentTypes(Id),
    IdfDocNumber VARCHAR(50) NOT NULL,
    FaceImageUrl VARCHAR(500),
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Seed Data for Countries
INSERT INTO Countries (Code, Name) VALUES 
('LAO', 'Lao PDR'),
('THA', 'Thailand'),
('VNM', 'Vietnam'),
('CHN', 'China'),
('USA', 'United States');

-- Seed Data for Document Types
INSERT INTO DocumentTypes (Name) VALUES 
('Passport'),
('ID Card'),
('Driving License'),
('Border Pass');