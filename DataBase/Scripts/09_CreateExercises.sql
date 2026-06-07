CREATE TABLE Exercises
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    Name NVARCHAR(150) NOT NULL,

    Description NVARCHAR(1000) NULL,

    TargetMuscleGroup INT NOT NULL,

    CreatedAt DATETIME2 NOT NULL,

    UpdatedAt DATETIME2 NULL,

    IsDeleted BIT NOT NULL
        CONSTRAINT DF_Exercises_IsDeleted DEFAULT(0),

    DeletedAt DATETIME2 NULL,

    CONSTRAINT UQ_Exercises_Name
        UNIQUE(Name),

    CONSTRAINT CK_Exercises_TargetMuscleGroup
        CHECK (TargetMuscleGroup IN (1,2,3,4,5,6,7,8))
);
