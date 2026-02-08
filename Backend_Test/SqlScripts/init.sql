-- Users Table
CREATE TABLE Users (
    Id VARCHAR(26) PRIMARY KEY,
    Username VARCHAR(100) NOT NULL UNIQUE,
    Password VARCHAR(256) NOT NULL,
    FullName VARCHAR(200) NOT NULL,
    ProfileImageUrl VARCHAR(500),
    Role VARCHAR(50) NOT NULL DEFAULT 'User',
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Administrators Table
CREATE TABLE Administrators (
    Id SERIAL PRIMARY KEY,
    Username VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(256) NOT NULL,
    Role VARCHAR(50) NOT NULL DEFAULT 'Admin',
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Insert a default admin user (Password: 'admin123' - Hashed with BCrypt)
INSERT INTO Administrators (Username, PasswordHash, Role) 
VALUES ('admin', '$2a$11$SxMErH4V0GjdvgxR8o/I5eAqsQaps8V9nuxJ4VftFuuPmvGu3C516', 'Admin');

-- API Keys Table
CREATE TABLE ApiKeys (
    Id SERIAL PRIMARY KEY,
    ClientName VARCHAR(100) NOT NULL,
    HashedKey VARCHAR(256) NOT NULL UNIQUE,
    IsEnabled BOOLEAN NOT NULL DEFAULT TRUE,
    CreatedById INT NOT NULL,
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ExpiresAt TIMESTAMP NULL,
    CONSTRAINT FK_ApiKeys_Administrators FOREIGN KEY (CreatedById) REFERENCES Administrators(Id)
);

-- Countries Table
CREATE TABLE Countries (
    Id SERIAL PRIMARY KEY,
    Code VARCHAR(10) NOT NULL UNIQUE,
    Name VARCHAR(100) NOT NULL
);

-- Document Types Table
CREATE TABLE DocumentTypes (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL
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

-- Documents Table
CREATE TABLE Documents (
    Id VARCHAR(50) PRIMARY KEY, -- Unique ID from DocumentNumberGenerator
    DocumentTypeId INT NOT NULL REFERENCES DocumentTypes(Id),
    DocumentNumber VARCHAR(100) NOT NULL,
    IssuedCountryId INT NOT NULL REFERENCES Countries(Id),
    IssueDate DATE,
    ExpiryDate DATE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Passengers Table
CREATE TABLE Passengers (
    PassengerId SERIAL PRIMARY KEY,
    DocId VARCHAR(50) NOT NULL REFERENCES Documents(Id),
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    DateOfBirth DATE,
    Gender VARCHAR(10),
    FaceImageUrl VARCHAR(500),
    CreatedByUserId VARCHAR(26) NOT NULL REFERENCES Users(Id), -- Linked to system user
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
