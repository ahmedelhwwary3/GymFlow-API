 INSERT INTO Exercises
(Name, Description, TargetMuscleGroup, CreatedAt, UpdatedAt, IsDeleted, DeletedAt)
VALUES
-- Chest
('Barbell Bench Press', 'Classic compound chest exercise using a barbell.', 1, '2026-02-10', NULL, 0, NULL),
('Incline Barbell Bench Press', 'Targets upper chest with an incline bench.', 1, '2026-02-11', '2026-04-05', 0, NULL),
('Decline Bench Press', 'Emphasizes lower chest development.', 1, '2026-02-12', NULL, 0, NULL),
('Dumbbell Bench Press', 'Chest press variation using dumbbells.', 1, '2026-02-13', NULL, 0, NULL),
('Incline Dumbbell Press', 'Upper chest focused dumbbell press.', 1, '2026-02-14', '2026-05-01', 0, NULL),
('Chest Fly Machine', 'Isolation exercise for chest muscles.', 1, '2026-02-15', NULL, 0, NULL),
('Cable Crossover', 'Cable movement for chest contraction.', 1, '2026-02-16', NULL, 0, NULL),
('Push Up', 'Bodyweight chest exercise.', 1, '2026-02-17', NULL, 0, NULL),

-- Back
('Pull Up', 'Bodyweight exercise targeting lats and upper back.', 2, '2026-02-18', NULL, 0, NULL),
('Lat Pulldown', 'Machine exercise for lat development.', 2, '2026-02-19', '2026-04-15', 0, NULL),
('Seated Cable Row', 'Horizontal pulling exercise for back thickness.', 2, '2026-02-20', NULL, 0, NULL),
('Barbell Row', 'Compound back exercise using a barbell.', 2, '2026-02-21', NULL, 0, NULL),
('T-Bar Row', 'Back thickness exercise using T-bar setup.', 2, '2026-02-22', NULL, 0, NULL),
('Single Arm Dumbbell Row', 'Unilateral rowing movement.', 2, '2026-02-23', '2026-04-20', 0, NULL),
('Straight Arm Pulldown', 'Isolation exercise for lats.', 2, '2026-02-24', NULL, 0, NULL),
('Rack Pull', 'Partial deadlift emphasizing upper back.', 2, '2026-02-25', NULL, 0, NULL),

-- Shoulders
('Overhead Press', 'Primary compound shoulder exercise.', 3, '2026-02-26', NULL, 0, NULL),
('Seated Dumbbell Press', 'Shoulder press variation using dumbbells.', 3, '2026-02-27', NULL, 0, NULL),
('Lateral Raise', 'Isolation movement for side delts.', 3, '2026-02-28', '2026-05-10', 0, NULL),
('Front Raise', 'Targets anterior deltoids.', 3, '2026-03-01', NULL, 0, NULL),
('Rear Delt Fly', 'Isolation exercise for rear deltoids.', 3, '2026-03-02', NULL, 0, NULL),
('Face Pull', 'Cable exercise for rear delts and posture.', 3, '2026-03-03', NULL, 0, NULL),
('Arnold Press', 'Rotational dumbbell shoulder press.', 3, '2026-03-04', '2026-04-28', 0, NULL),

-- Biceps
('Barbell Curl', 'Classic biceps exercise using a barbell.', 4, '2026-03-05', NULL, 0, NULL),
('EZ Bar Curl', 'Biceps curl using EZ bar.', 4, '2026-03-06', NULL, 0, NULL),
('Dumbbell Curl', 'Alternating dumbbell curl.', 4, '2026-03-07', NULL, 0, NULL),
('Hammer Curl', 'Targets brachialis and biceps.', 4, '2026-03-08', '2026-05-02', 0, NULL),
('Preacher Curl', 'Strict biceps isolation exercise.', 4, '2026-03-09', NULL, 0, NULL),
('Cable Curl', 'Cable-based biceps movement.', 4, '2026-03-10', NULL, 0, NULL),

-- Rare / Obsolete
('Zottman Curl Machine', 'Rare machine variation of Zottman curl.', 4, '2026-03-11', NULL, 1, '2026-05-15');







INSERT INTO Exercises
(Name, Description, TargetMuscleGroup, CreatedAt, UpdatedAt, IsDeleted, DeletedAt)
VALUES
-- Triceps
('Close Grip Bench Press', 'Compound triceps movement using a narrow grip.', 5, '2026-03-12', NULL, 0, NULL),
('Tricep Pushdown', 'Cable exercise for triceps development.', 5, '2026-03-13', '2026-05-03', 0, NULL),
('Rope Pushdown', 'Triceps pushdown variation using rope attachment.', 5, '2026-03-14', NULL, 0, NULL),
('Overhead Dumbbell Extension', 'Overhead triceps isolation exercise.', 5, '2026-03-15', NULL, 0, NULL),
('Skull Crusher', 'Lying triceps extension using EZ bar.', 5, '2026-03-16', '2026-04-25', 0, NULL),
('Bench Dip', 'Bodyweight triceps exercise.', 5, '2026-03-17', NULL, 0, NULL),
('Single Arm Cable Extension', 'Unilateral triceps isolation movement.', 5, '2026-03-18', NULL, 0, NULL),

-- Legs
('Barbell Squat', 'Fundamental compound leg exercise.', 6, '2026-03-19', NULL, 0, NULL),
('Front Squat', 'Squat variation emphasizing quads.', 6, '2026-03-20', '2026-05-05', 0, NULL),
('Leg Press', 'Machine-based lower body exercise.', 6, '2026-03-21', NULL, 0, NULL),
('Walking Lunge', 'Unilateral lower body exercise.', 6, '2026-03-22', NULL, 0, NULL),
('Bulgarian Split Squat', 'Single leg squat variation.', 6, '2026-03-23', NULL, 0, NULL),
('Romanian Deadlift', 'Hamstring and glute focused exercise.', 6, '2026-03-24', '2026-04-18', 0, NULL),
('Leg Extension', 'Quadriceps isolation exercise.', 6, '2026-03-25', NULL, 0, NULL),
('Lying Leg Curl', 'Hamstring isolation exercise.', 6, '2026-03-26', NULL, 0, NULL),
('Seated Leg Curl', 'Machine hamstring exercise.', 6, '2026-03-27', NULL, 0, NULL),
('Standing Calf Raise', 'Calf development exercise.', 6, '2026-03-28', NULL, 0, NULL),
('Seated Calf Raise', 'Isolation movement for calves.', 6, '2026-03-29', '2026-05-11', 0, NULL),
('Hack Squat', 'Machine squat variation.', 6, '2026-03-30', NULL, 0, NULL),
('Goblet Squat', 'Dumbbell squat variation.', 6, '2026-03-31', NULL, 0, NULL),

-- Abs
('Crunch', 'Basic abdominal exercise.', 7, '2026-04-01', NULL, 0, NULL),
('Cable Crunch', 'Weighted abdominal crunch using cable machine.', 7, '2026-04-02', NULL, 0, NULL),
('Leg Raise', 'Lower abdominal focused exercise.', 7, '2026-04-03', '2026-05-08', 0, NULL),
('Hanging Leg Raise', 'Advanced abdominal exercise.', 7, '2026-04-04', NULL, 0, NULL),
('Russian Twist', 'Rotational core exercise.', 7, '2026-04-05', NULL, 0, NULL),
('Plank', 'Core stability exercise.', 7, '2026-04-06', NULL, 0, NULL),
('Side Plank', 'Oblique and core stability movement.', 7, '2026-04-07', NULL, 0, NULL),
('Ab Wheel Rollout', 'Advanced core strengthening exercise.', 7, '2026-04-08', '2026-05-06', 0, NULL),
('Mountain Climber', 'Dynamic core and conditioning exercise.', 7, '2026-04-09', NULL, 0, NULL),

-- Full Body
('Deadlift', 'Full body compound lift.', 8, '2026-04-10', NULL, 0, NULL),
('Power Clean', 'Explosive full body Olympic movement.', 8, '2026-04-11', NULL, 0, NULL),
('Clean And Press', 'Olympic lift variation combining pull and press.', 8, '2026-04-12', '2026-05-01', 0, NULL),
('Burpee', 'Bodyweight full body conditioning exercise.', 8, '2026-04-13', NULL, 0, NULL),
('Kettlebell Swing', 'Explosive hip hinge movement.', 8, '2026-04-14', NULL, 0, NULL),
('Thruster', 'Front squat combined with overhead press.', 8, '2026-04-15', NULL, 0, NULL),
('Farmer Walk', 'Grip and full body carrying exercise.', 8, '2026-04-16', '2026-05-07', 0, NULL),
('Battle Rope Slams', 'Conditioning exercise using battle ropes.', 8, '2026-04-17', NULL, 0, NULL),

-- Rare / Obsolete
('Sissy Squat Machine', 'Rare machine-based squat variation.', 6, '2026-04-18', NULL, 1, '2026-05-20');