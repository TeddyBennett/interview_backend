-- Create Documents Table
CREATE TABLE Documents (
    Id VARCHAR(50) PRIMARY KEY, -- Unique ID from DocumentNumberGenerator
    DocumentTypeId INT NOT NULL REFERENCES DocumentTypes(Id),
    DocumentNumber VARCHAR(100) NOT NULL,
    IssuedCountryId INT NOT NULL REFERENCES Countries(Id),
    IssueDate DATE,
    ExpiryDate DATE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Alter Passengers table to support string DocId and remove old document fields
-- We'll drop and recreate or just alter. Recreating is cleaner for this stage.
DROP TABLE IF EXISTS Passengers;

CREATE TABLE Passengers (
    PassengerId SERIAL PRIMARY KEY,
    DocId VARCHAR(50) NOT NULL REFERENCES Documents(Id),
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    DateOfBirth DATE,
    Gender VARCHAR(10),
    FaceImageUrl VARCHAR(500),
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
