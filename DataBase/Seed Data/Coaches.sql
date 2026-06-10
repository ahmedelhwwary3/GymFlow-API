SET IDENTITY_INSERT Users ON;

INSERT INTO Users
( FullName, Email, Phone, Gender, DateOfBirth, PasswordHash, Role,
 RefreshTokenHash, RefreshTokenExpiresAt, RefreshTokenRevokedAt,
 CreatedAt, UpdatedAt, IsDeleted, DeletedAt, IsActive)
VALUES
('Ahmed Hassan','ahmed.hassan@gmail.com','01010000001',1,'1990-01-15','$2a$11$qnWnUE9/4q28wyud/jAbh.VAIPAlSxYs2TiOm3pxQrAMiX4GgxM/2',2,NULL,NULL,NULL,GETUTCDATE(),NULL,0,NULL,1),
('Mohamed Ali','mohamed.ali@gmail.com','01010000002',1,'1991-02-10','$2a$11$qnWnUE9/4q28wyud/jAbh.VAIPAlSxYs2TiOm3pxQrAMiX4GgxM/2',2,NULL,NULL,NULL,GETUTCDATE(),NULL,0,NULL,1),
('Omar Fathy','omar.fathy@gmail.com','01010000003',1,'1992-03-20','$2a$11$qnWnUE9/4q28wyud/jAbh.VAIPAlSxYs2TiOm3pxQrAMiX4GgxM/2',2,NULL,NULL,NULL,GETUTCDATE(),NULL,0,NULL,1),
('Youssef Adel','youssef.adel@gmail.com','01010000004',1,'1993-04-11','$2a$11$qnWnUE9/4q28wyud/jAbh.VAIPAlSxYs2TiOm3pxQrAMiX4GgxM/2',2,NULL,NULL,NULL,GETUTCDATE(),NULL,0,NULL,1),

('Karim Nabil','karim.nabil@gmail.com','01010000005',1,'1994-05-09','$2a$11$qnWnUE9/4q28wyud/jAbh.VAIPAlSxYs2TiOm3pxQrAMiX4GgxM/2',2,NULL,NULL,NULL,GETUTCDATE(),NULL,0,NULL,1),
('Mostafa Emad','mostafa.emad@gmail.com','01010000006',1,'1989-06-17','$2a$11$qnWnUE9/4q28wyud/jAbh.VAIPAlSxYs2TiOm3pxQrAMiX4GgxM/2',2,NULL,NULL,NULL,GETUTCDATE(),NULL,0,NULL,1),
('Mahmoud Tarek','mahmoud.tarek@gmail.com','01010000007',1,'1990-07-14','$2a$11$qnWnUE9/4q28wyud/jAbh.VAIPAlSxYs2TiOm3pxQrAMiX4GgxM/2',2,NULL,NULL,NULL,GETUTCDATE(),NULL,0,NULL,1),
('Amr Khaled','amr.khaled@gmail.com','01010000008',1,'1991-08-22','$2a$11$qnWnUE9/4q28wyud/jAbh.VAIPAlSxYs2TiOm3pxQrAMiX4GgxM/2',2,NULL,NULL,NULL,GETUTCDATE(),NULL,0,NULL,1),

('Hossam Samir','hossam.samir@gmail.com','01010000009',1,'1992-09-13','$2a$11$qnWnUE9/4q28wyud/jAbh.VAIPAlSxYs2TiOm3pxQrAMiX4GgxM/2',2,NULL,NULL,NULL,GETUTCDATE(),NULL,0,NULL,1),
('Ibrahim Atef','ibrahim.atef@gmail.com','01010000010',1,'1988-10-01','$2a$11$qnWnUE9/4q28wyud/jAbh.VAIPAlSxYs2TiOm3pxQrAMiX4GgxM/2',2,NULL,NULL,NULL,GETUTCDATE(),NULL,0,NULL,1),

('Sherif Magdy','sherif.magdy@gmail.com','01010000011',1,'1993-11-02','$2a$11$qnWnUE9/4q28wyud/jAbh.VAIPAlSxYs2TiOm3pxQrAMiX4GgxM/2',2,NULL,NULL,NULL,GETUTCDATE(),NULL,0,NULL,1),
('Wael Essam','wael.essam@gmail.com','01010000012',1,'1990-12-18','$2a$11$qnWnUE9/4q28wyud/jAbh.VAIPAlSxYs2TiOm3pxQrAMiX4GgxM/2',2,NULL,NULL,NULL,GETUTCDATE(),NULL,0,NULL,1),

('Tamer Salah','tamer.salah@gmail.com','01010000013',1,'1989-01-28','$2a$11$qnWnUE9/4q28wyud/jAbh.VAIPAlSxYs2TiOm3pxQrAMiX4GgxM/2',2,NULL,NULL,NULL,GETUTCDATE(),NULL,0,NULL,1),
('Ayman Hany','ayman.hany@gmail.com','01010000014',1,'1991-04-19','$2a$11$qnWnUE9/4q28wyud/jAbh.VAIPAlSxYs2TiOm3pxQrAMiX4GgxM/2',2,NULL,NULL,NULL,GETUTCDATE(),NULL,0,NULL,1),

('Mina Nader','mina.nader@gmail.com','01010000015',1,'1994-06-30','$2a$11$qnWnUE9/4q28wyud/jAbh.VAIPAlSxYs2TiOm3pxQrAMiX4GgxM/2',2,NULL,NULL,NULL,GETUTCDATE(),NULL,0,NULL,0),
('George Adel','george.adel@gmail.com','01010000016',1,'1990-08-12','$2a$11$qnWnUE9/4q28wyud/jAbh.VAIPAlSxYs2TiOm3pxQrAMiX4GgxM/2',2,NULL,NULL,NULL,GETUTCDATE(),NULL,0,NULL,0),

('Ramy Fawzy','ramy.fawzy@gmail.com','01010000017',1,'1987-03-21','$2a$11$qnWnUE9/4q28wyud/jAbh.VAIPAlSxYs2TiOm3pxQrAMiX4GgxM/2',2,NULL,NULL,NULL,GETUTCDATE(),NULL,1,GETUTCDATE(),1),
('Nader Sameh','nader.sameh@gmail.com','01010000018',1,'1992-07-09','$2a$11$qnWnUE9/4q28wyud/jAbh.VAIPAlSxYs2TiOm3pxQrAMiX4GgxM/2',2,NULL,NULL,NULL,GETUTCDATE(),NULL,1,GETUTCDATE(),1),

('Bishoy Ashraf','bishoy.ashraf@gmail.com','01010000019',1,'1991-10-16','$2a$11$qnWnUE9/4q28wyud/jAbh.VAIPAlSxYs2TiOm3pxQrAMiX4GgxM/2',2,NULL,NULL,NULL,GETUTCDATE(),NULL,0,NULL,1),
('Fady Naguib','fady.naguib@gmail.com','01010000020',1,'1993-12-24','$2a$11$qnWnUE9/4q28wyud/jAbh.VAIPAlSxYs2TiOm3pxQrAMiX4GgxM/2',2,NULL,NULL,NULL,GETUTCDATE(),NULL,0,NULL,1);

SET IDENTITY_INSERT Users OFF;

INSERT INTO Coaches (Id, Specialization, HireDate, Salary)
VALUES
(7 ,1,'2026-05-01',12000),
( 8,2,'2026-05-02',12500),
( 9,3,'2026-05-03',13000),
( 10,4,'2026-05-04',13500),
(11 ,1,'2026-05-05',14000),
(12 ,2,'2026-05-06',14500),
( 13,3,'2026-05-07',15000),
( 14,4,'2026-05-08',15500),
(15 ,1,'2026-05-09',16000),
( 16,2,'2026-05-10',16500),
(17,3,'2026-05-11',17000),
(18,4,'2026-05-12',17500),
(19,1,'2026-05-13',18000),
(20,2,'2026-05-14',18500),
(21,3,'2026-05-15',19000),
(22,4,'2026-05-16',19500),
(23,1,'2026-05-17',20000),
(24,2,'2026-05-18',20500),
(25,3,'2026-05-19',21000),
(26,4,'2026-05-20',21500);