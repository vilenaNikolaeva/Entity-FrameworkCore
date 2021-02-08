
CREATE TABLE Users(
Id BIGINT PRIMARY KEY IDENTITY,
Username NVARCHAR (30) NOT NULL,
[Password] VARCHAR(26),
ProfilePicture VARBINARY (MAX),
CHECK (DATALENGTH (ProfilePicture) <= 921600),
LastLongTime DATETIME2,
IsDeleted BIT
)

INSERT INTO Users(Username, [Password], ProfilePicture, LastLongTime,IsDeleted)
VALUES
('Pesho','123', NULL, NULL,0),
('Gosho','567', NULL, NULL,0),
('Mariq','227', NULL, NULL,0),
('Katq','554', NULL, NULL,0),
('Test','123', NULL, NULL,1)

SELECT *FROM Users

ALTER TABLE Users
DROP CONSTRAINT PK__Users__3214EC0747E4DFCE

ALTER TABLE Users
ADD CONSTRAINT PK_CompositeIdUserName
PRIMARY KEY (Id,Username)

ALTER TABLE Users
ADD CONSTRAINT DF_LastLogingTime
DEFAULT GETDATE() FOR  LastLongTime

INSERT INTO Users(Username, [Password], ProfilePicture,IsDeleted)
VALUES
('Testt','123', NULL,1)

SELECT * FROM Users

-- 12.NOT DONE!