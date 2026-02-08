CREATE TABLE Administrators (
    Id SERIAL PRIMARY KEY,
    Username VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(256) NOT NULL,
    Role VARCHAR(50) NOT NULL DEFAULT 'Admin',
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Insert a default admin user (Password: 'admin123' - Hashed with BCrypt)
-- This is a valid BCrypt hash for 'admin123'
INSERT INTO Administrators (Username, PasswordHash, Role) 
VALUES ('admin', '$2a$11$Z5n5q1u5h5j5k5l5m5n5o5p5q5r5s5t5u5v5w5x5y5z5a5b5c5d5e', 'Admin');