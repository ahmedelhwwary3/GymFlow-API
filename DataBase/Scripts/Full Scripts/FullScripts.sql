CREATE DATABASE GymManagementDb;
GO

USE GymManagementDb;
GO



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




CREATE TABLE Coaches
(
    Id INT PRIMARY KEY 

    Specialization INT NOT NULL,

    HireDate DATE NOT NULL,

    Salary DECIMAL(18,2) NOT NULL,

    CONSTRAINT FK_Coaches_Users
        FOREIGN KEY(Id)
        REFERENCES Users(Id),

    CONSTRAINT CK_Coaches_Specialization
        CHECK (Specialization IN (1,2,3,4)),

    CONSTRAINT CK_Coaches_Salary
        CHECK (Salary >= 0)
);


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


CREATE TABLE WeightRecords
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    MemberId INT NOT NULL,

    Weight DECIMAL(5,2) NOT NULL,

    RecordedDate DATE NOT NULL,

    Notes NVARCHAR(500) NULL,

    CONSTRAINT FK_WeightRecords_Members
        FOREIGN KEY(MemberId)
        REFERENCES Members(Id),

    CONSTRAINT UQ_WeightRecords_MemberId_RecordedDate
        UNIQUE(MemberId, RecordedDate),

    CONSTRAINT CK_WeightRecords_Weight
        CHECK (Weight > 0)
);


CREATE TABLE Subscriptions
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    MemberId INT NOT NULL,

    CoachId INT NOT NULL,

    SubscriptionPlan INT NOT NULL,

    Price DECIMAL(10,2) NOT NULL,

    StartDate DATE NOT NULL,

    EndDate DATE NOT NULL,

    FreezeStartDate DATE NULL,

    FreezeEndDate DATE NULL,

    CreatedAt DATETIME2 NOT NULL,

    IsDeleted BIT NOT NULL
        CONSTRAINT DF_Subscriptions_IsDeleted DEFAULT(0),

    DeletedAt DATETIME2 NULL,

    CONSTRAINT FK_Subscriptions_Members
        FOREIGN KEY(MemberId)
        REFERENCES Members(Id),

    CONSTRAINT FK_Subscriptions_Coaches
    FOREIGN KEY(CoachId)
    REFERENCES Coaches(Id),

    CONSTRAINT CK_Subscriptions_SubscriptionPlan
        CHECK (SubscriptionPlan IN (1,2,3,4)),

    CONSTRAINT CK_Subscriptions_Price
        CHECK (Price > 0),

    CONSTRAINT CK_Subscriptions_Dates
        CHECK (EndDate > StartDate)
);





CREATE TABLE Payments
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    SubscriptionId INT NOT NULL,

    Amount DECIMAL(10,2) NOT NULL,

    PaymentDate DATE NOT NULL,

    Notes NVARCHAR(500) NULL,

    CONSTRAINT FK_Payments_Subscriptions
        FOREIGN KEY(SubscriptionId)
        REFERENCES Subscriptions(Id),

    CONSTRAINT CK_Payments_Amount
        CHECK (Amount > 0)
);







CREATE TABLE Attendances
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    MemberId INT NOT NULL,

    AttendanceDate DATE NOT NULL,

    Notes NVARCHAR(500) NULL,

    CONSTRAINT FK_Attendances_Members
        FOREIGN KEY(MemberId)
        REFERENCES Members(Id),

    CONSTRAINT UQ_Attendances_MemberId_AttendanceDate
        UNIQUE(MemberId, AttendanceDate)
);



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

CREATE TABLE WorkoutPlans
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    MemberId INT NOT NULL,

    CoachId INT NOT NULL,

    Name NVARCHAR(150) NOT NULL,

    IsActive BIT NOT NULL
        CONSTRAINT DF_WorkoutPlans_IsActive DEFAULT(1),

    CreatedAt DATETIME2 NOT NULL, 

    CONSTRAINT FK_WorkoutPlans_Members
        FOREIGN KEY(MemberId)
        REFERENCES Members(Id),

    CONSTRAINT FK_WorkoutPlans_Coaches
        FOREIGN KEY(CoachId)
        REFERENCES Coaches(Id)
);

CREATE TABLE WorkoutPlanExercises
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    WorkoutPlanId INT NOT NULL,

    ExerciseId INT NOT NULL,

    Sets INT NOT NULL,

    Reps INT NOT NULL,

    Notes NVARCHAR(500) NULL,

    CONSTRAINT FK_WorkoutPlanExercises_WorkoutPlans
        FOREIGN KEY(WorkoutPlanId)
        REFERENCES WorkoutPlans(Id),

    CONSTRAINT FK_WorkoutPlanExercises_Exercises
        FOREIGN KEY(ExerciseId)
        REFERENCES Exercises(Id),

    CONSTRAINT UQ_WorkoutPlanExercises_WorkoutPlanId_ExerciseId
        UNIQUE (WorkoutPlanId, ExerciseId),

    CONSTRAINT CK_WorkoutPlanExercises_Sets
        CHECK (Sets > 0),

    CONSTRAINT CK_WorkoutPlanExercises_Reps
        CHECK (Reps > 0)
);


CREATE UNIQUE INDEX IX_Users_Email
ON Users(Email);

CREATE UNIQUE INDEX IX_Users_Phone
ON Users(Phone);

CREATE INDEX IX_Members_CoachId
ON Members(CoachId);

CREATE INDEX IX_Subscriptions_CreatedByUserId 
ON Subscriptions(CreatedByUserId); 

CREATE INDEX IX_WeightRecords_MemberId
ON WeightRecords(MemberId);

CREATE INDEX IX_Attendances_MemberId
ON Attendances(MemberId);

CREATE INDEX IX_Subscriptions_MemberId
ON Subscriptions(MemberId);

CREATE INDEX IX_Payments_SubscriptionId
ON Payments(SubscriptionId);

CREATE INDEX IX_WorkoutPlans_MemberId
ON WorkoutPlans(MemberId);

CREATE INDEX IX_WorkoutPlans_CoachId
ON WorkoutPlans(CoachId);

CREATE INDEX IX_WorkoutPlanExercises_WorkoutPlanId
ON WorkoutPlanExercises(WorkoutPlanId);
