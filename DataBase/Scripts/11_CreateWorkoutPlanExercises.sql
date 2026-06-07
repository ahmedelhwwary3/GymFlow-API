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
