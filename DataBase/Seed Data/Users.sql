INSERT INTO Users
(
  
    FullName,
    Email,
    Phone,
    Gender,
    DateOfBirth,
    PasswordHash,
    Role,
    RefreshTokenHash,
    RefreshTokenExpiresAt,
    RefreshTokenRevokedAt,
    CreatedAt,
    UpdatedAt,
    IsDeleted,
    DeletedAt,
    IsActive
)
VALUES
(
    
    'Ahmed Admin',
    'admin@gym.com',
    '01000000001',
    1,
    '1990-05-10',
    '$2a$11$qnWnUE9/4q28wyud/jAbh.VAIPAlSxYs2TiOm3pxQrAMiX4GgxM/2',
    1,
    NULL,
    NULL,
    NULL,
    '2026-06-07 16:50:23.6066667',
    NULL,
    0,
    NULL,
    1
),
(
    
    'Mohamed Coach',
    'coach@gym.com',
    '01000000002',
    1,
    '1992-08-15',
    '$2a$11$LJchrJ0K8PgM62hZ1b/YsO8oJZ/EkyGGt2v2tPQpyz7gxA9GSzZmC',
    2,
    NULL,
    NULL,
    NULL,
    '2026-06-07 16:50:23.6066667',
    NULL,
    0,
    NULL,
    1
),
(
    
    'Ali Member',
    'member@gym.com',
    '01000000003',
    1,
    '1998-01-20',
    '$2a$11$HrpwYKBXogxxTJCK4cueXOA/.L.6KQ30AShvmS7JWz6d1I39hPqri',
    3,
    NULL,
    NULL,
    NULL,
    '2026-06-07 16:50:23.6066667',
    NULL,
    0,
    NULL,
    1
),
(
    
    'Inactive User',
    'inactive@gym.com',
    '01000000004',
    1,
    '1995-07-01',
    '$2a$11$yNz6wqKeHQUC1XslwRk.ZOlB/M5ZvU0kzrbe0FB1/RqdUC4isj6em',
    3,
    NULL,
    NULL,
    NULL,
    '2026-06-07 16:50:23.6066667',
    NULL,
    0,
    NULL,
    0
),
(
   
    'Deleted User',
    'deleted@gym.com',
    '01000000005',
    1,
    '1993-03-03',
    '$2a$11$wHbQh7yncTt8k9cjGPq/UejPPzaqQNUqTI78tuOSOL3UmjDs6mySC',
    3,
    NULL,
    NULL,
    NULL,
    '2026-06-07 16:50:23.6066667',
    NULL,
    1,
    '2026-06-07 16:50:23.6066667',
    1
),
(
    
    'Normal User',
    'user@gym.com',
    '01000000006',
    1,
    '1997-11-12',
    '$2a$11$cm5.dNdRll0z/9eoabCWAeR4xNyFPs30c1c5v2WKdTHZpyjRxTn/O',
    3,
    NULL,
    NULL,
    NULL,
    '2026-06-07 16:50:23.6066667',
    NULL,
    0,
    NULL,
    1
);