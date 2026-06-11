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
