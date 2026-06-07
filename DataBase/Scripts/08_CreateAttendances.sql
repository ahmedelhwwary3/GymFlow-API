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
