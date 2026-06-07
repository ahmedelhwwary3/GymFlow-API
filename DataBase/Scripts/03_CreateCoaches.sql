CREATE TABLE Coaches
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    UserId INT NOT NULL,

    Specialization INT NOT NULL,

    HireDate DATE NOT NULL,

    Salary DECIMAL(18,2) NOT NULL,

    IsActive BIT NOT NULL
        CONSTRAINT DF_Coaches_IsActive DEFAULT(1),

    CreatedAt DATETIME2 NOT NULL,

    UpdatedAt DATETIME2 NULL,

    IsDeleted BIT NOT NULL
        CONSTRAINT DF_Coaches_IsDeleted DEFAULT(0),

    DeletedAt DATETIME2 NULL,

    CONSTRAINT UQ_Coaches_UserId
        UNIQUE(UserId),

    CONSTRAINT FK_Coaches_Users
        FOREIGN KEY(UserId)
        REFERENCES Users(Id),

    CONSTRAINT CK_Coaches_Specialization
        CHECK (Specialization IN (1,2,3,4)),

    CONSTRAINT CK_Coaches_Salary
        CHECK (Salary >= 0)
);
