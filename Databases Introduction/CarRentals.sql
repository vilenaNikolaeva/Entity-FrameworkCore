CREATE TABLE Categories(
Id INT IDENTITY PRIMARY KEY NOT NULL,
CategoryName NVARCHAR (30)NOT NULL,
DailyRate INT NOT NULL,
WeeklyRate INT,
MonthlyRate INT,
WeekendRate INT)

INSERT INTO Categories(CategoryName,DailyRate,WeeklyRate
,MonthlyRate,WeekendRate)VALUES
('Sports',25,Null,Null,NULL),
('Comby',35,Null,Null,NULL),
('Van',50,Null,Null,NULL)

Select * From Categories

CREATE TABLE Cars(
Id CHAR(1) PRIMARY KEY NOT NULL,
PlateNumber NVARCHAR (6) NOT NULL,
Manufacturer NVARCHAR (30) NOT NULL,
Model NVARCHAR (30) NOT NULL,
CarYear INT NOT NULL,
CategoryId CHAR (2) ,
Doors INT ,
Picture  VARBINARY (900),
Condition CHAR(4)  NOT NULL CHECK(Condition='NEW' OR Condition='USED'),
Available CHAR(4) NOT NULL CHECK(Available='YES' OR Available='NO'))


INSERT INTO Cars(Id,PlateNumber,Manufacturer,Model,CarYear
,CategoryId,Doors,Picture,Condition,Available)VALUES
('A','PB1234','BMW','X5',2003,'B',5,NULL,'NEW','YES'),
('B','PB5678','AUDI','A6',2008,'B',3,NULL,'NEW','YES'),
('C','PB1234','LangeRover','zX',2013,'B',5,NULL,'NEW','YES')

SELECT * FROM Cars

CREATE TABLE Employees(
Id CHAR PRIMARY KEY NOT NULL,
FirstName NVARCHAR (30) NOT NULL,
LastName NVARCHAR (30) NOT NULL,
Title NVARCHAR (100),
Notes NVARCHAR (300))

INSERT INTO Employees(Id,FirstName,LastName,Title,Notes)
VALUES
('A','Vilena','Nikolaeva','Owner',NULL),
('B','Boris','Borisov','Emplyee',NULL),
('C','Nikolai','Nikolaeva','Co-Owner',Null)

CREATE VIEW People AS
SELECT FirstName,LastName,Title 
FROM Employees

Select *FROM People

ALTER TABLE Employees
ADD Salary INT

Select *FROM Employees

UPDATE Employees
SET Salary=20000
WHERE LastName='Nikolaeva'

UPDATE Employees
SET LastName='Nikolaev'
WHERE FirstName='Nikolai'

SELECT*FROM Employees

UPDATE Employees
SET LastName='Nikolaev'
WHERE FirstName='Nikolai'

UPDATE Employees
SET Salary=1000+(1000*0.10)
WHERE Salary IS NULL

SELECT*FROM Employees
