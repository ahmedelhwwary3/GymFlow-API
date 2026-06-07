CREATE TABLE Subscriptions
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    MemberId INT NOT NULL,

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

    CONSTRAINT CK_Subscriptions_SubscriptionPlan
        CHECK (SubscriptionPlan IN (1,2,3,4)),

    CONSTRAINT CK_Subscriptions_Price
        CHECK (Price > 0),

    CONSTRAINT CK_Subscriptions_Dates
        CHECK (EndDate > StartDate)
);
