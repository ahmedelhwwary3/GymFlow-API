CREATE TABLE Users
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    FullName NVARCHAR(150) NOT NULL,

    Email NVARCHAR(255) NOT NULL,

    Phone NVARCHAR(20) NOT NULL,

    Gender INT NOT NULL,

    DateOfBirth DATE NOT NULL,

    PasswordHash NVARCHAR(500) NOT NULL,

    Role INT NOT NULL,

    RefreshTokenHash NVARCHAR(500) NULL,

    RefreshTokenExpiresAt DATETIME2 NULL,

    RefreshTokenRevokedAt DATETIME2 NULL,

    CreatedAt DATETIME2 NOT NULL,

    UpdatedAt DATETIME2 NULL,

    IsDeleted BIT NOT NULL
        CONSTRAINT DF_Users_IsDeleted DEFAULT(0),

    DeletedAt DATETIME2 NULL,

    IsActive BIT NOT NULL DEFAULT 1,

    CONSTRAINT UQ_Users_Email
        UNIQUE (Email),

    CONSTRAINT UQ_Users_Phone
        UNIQUE (Phone),

    CONSTRAINT CK_Users_Gender
        CHECK (Gender IN (1,2)),

    CONSTRAINT CK_Users_Role
        CHECK (Role IN (1,2,3))
);
