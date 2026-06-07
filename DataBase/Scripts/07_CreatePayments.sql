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
