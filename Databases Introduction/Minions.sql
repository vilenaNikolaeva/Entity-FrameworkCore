--Create table MInions
CREATE TABLE Minion(
Id INT NOT NULL,
[Name] NVARCHAR(30) NOT NULL,
Age INT)

--Create table Towns
CREATE TABLE Towns(
Id INT  NOT NULL,
[Name] NVARCHAR(30) NOT NULL)

--Alter Minions Table(Add constraint to (Id))
ALTER TABLE Minion
ADD CONSTRAINT Pk_Id
PRIMARY KEY(Id)

--Alter table Add Towns constraint to TownId
ALTER TABLE Towns
ADD CONSTRAINT Pk_TownId
PRIMARY KEY(Id)

--Create new col. in Minions
ALTER TABLE Minion
ADD TownId INT
--
ALTER TABLE Minion
ADD CONSTRAINT FK_MinionTownId
FOREIGN KEY(TownId) REFERENCES Towns(Id)

INSERT INTO Towns (Id,[Name])VALUES
(1,'Sofiq'),
(2,'Plovdiv'),
(3,'Varna')

SELECT * FROM Towns



INSERT INTO Minion(Id,[Name],Age,TownId) VALUES
(1,'Kevin', 22, 1),
(2,'Bob', 15, 3),
(3,'Steward', 16, 2)

TRUNCATE TABLE Minion

DROP TABLE Minion

CREATE TABLE People(
Id INT PRIMARY KEY IDENTITY,
[Name] NVARCHAR (200) NOT NULL,
Picture VARBINARY(2000),
[Height] FLOAT(2),
[Weight] FLOAT(2),
Gender CHAR NOT NULL CHECK (Gender='m' OR Gender='f'),
Birthdate DATETIME NOT NULL,
Biography NVARCHAR(MAX),
)
-- Delete info in table People
TRUNCATE Table People
--Delete Table
DROP TABLE People

INSERT INTO People ([Name], Picture, [Height],[Weight],
Gender,Birthdate,Biography) VALUES
('Vilena',Null,155,49,'f',1994-01-14,Null),
('Liliq',Null,160,52,'f',1992-08-23,Null),
('Hristina',Null,155,54,'f',1993-12-29,Null),
('Emiliq',Null,155,49,'f',1994-06-05,Null),
('Kristina',Null,155,49,'f',1994-03-05,Null)

