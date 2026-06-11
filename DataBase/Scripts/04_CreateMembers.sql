CREATE TABLE Members
(
    Id INT PRIMARY KEY, 

    CoachId INT NULL,

    Address NVARCHAR(300) NOT NULL,

    Height DECIMAL(5,2) NOT NULL,

    FitnessGoal INT NOT NULL,  

    CONSTRAINT FK_Members_Coaches
        FOREIGN KEY(CoachId)
        REFERENCES Coaches(Id),

        CONSTRAINT FK_Members_Users
        FOREIGN KEY(Id)
        REFERENCES Users(Id),

    CONSTRAINT CK_Members_Height
        CHECK (Height > 0),

    CONSTRAINT CK_Members_FitnessGoal
        CHECK (FitnessGoal IN (1,2,3))
);
