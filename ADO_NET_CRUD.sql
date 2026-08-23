IF DB_ID('StudentDB') IS NULL
BEGIN
    CREATE DATABASE StudentDB;
END
GO -- this is a comment

USE StudentDB;
GO

IF OBJECT_ID('Students', 'U') IS NULL
BEGIN
    CREATE TABLE Students
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Age INT NOT NULL,
        Course NVARCHAR(100) NOT NULL
    );
END
GO


SELECT *
FROM Students;

EXEC sp_help 'Students';


DROP TABLE Students;
GO