CREATE TABLE WorkoutPlans
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    MemberId INT NOT NULL,

    CoachId INT NOT NULL, 

    IsActive BIT NOT NULL
        CONSTRAINT DF_WorkoutPlans_IsActive DEFAULT(1),

    CreatedAt DATETIME2 NOT NULL, 

    Name NVARCHAR(100) NOT NULL, 

    CONSTRAINT FK_WorkoutPlans_Members
        FOREIGN KEY(MemberId)
        REFERENCES Members(Id),

    CONSTRAINT FK_WorkoutPlans_Coaches
        FOREIGN KEY(CoachId)
        REFERENCES Coaches(Id)
);
