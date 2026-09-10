CREATE DATABASE StudentDB;
GO

USE StudentDB;
GO

CREATE TABLE Students
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Age INT NOT NULL,
    Phone NVARCHAR(20),
    Course NVARCHAR(100),
    Batch NVARCHAR(20)
);
GO

INSERT INTO Students (Name, Age, Phone, Course, Batch)
VALUES
('Samikshya', 21, '9800000000', 'BSc CSIT', '2080'),
('Rusha', 22, '9811111111', 'BSc CSIT', '2080'),
('Babita', 21, '9822222222', 'BSc CSIT', '2080');
GO