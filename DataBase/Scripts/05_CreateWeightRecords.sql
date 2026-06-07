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
