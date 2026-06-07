CREATE TABLE Members
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    UserId INT NOT NULL,

    CoachId INT NULL,

    Address NVARCHAR(300) NOT NULL,

    Height DECIMAL(5,2) NOT NULL,

    FitnessGoal INT NOT NULL,

    CreatedAt DATETIME2 NOT NULL,

    UpdatedAt DATETIME2 NULL,

    IsDeleted BIT NOT NULL
        CONSTRAINT DF_Members_IsDeleted DEFAULT(0),

    DeletedAt DATETIME2 NULL,

    CONSTRAINT UQ_Members_UserId
        UNIQUE(UserId),

    CONSTRAINT FK_Members_Users
        FOREIGN KEY(UserId)
        REFERENCES Users(Id),

    CONSTRAINT FK_Members_Coaches
        FOREIGN KEY(CoachId)
        REFERENCES Coaches(Id),

    CONSTRAINT CK_Members_Height
        CHECK (Height > 0),

    CONSTRAINT CK_Members_FitnessGoal
        CHECK (FitnessGoal IN (1,2,3))
);
