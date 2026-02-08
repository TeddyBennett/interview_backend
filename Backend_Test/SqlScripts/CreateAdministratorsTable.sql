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
VALUES ('admin', '$2a$11$SxMErH4V0GjdvgxR8o/I5eAqsQaps8V9nuxJ4VftFuuPmvGu3C516', 'Admin');