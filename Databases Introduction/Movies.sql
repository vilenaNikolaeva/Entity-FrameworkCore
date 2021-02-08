CREATE TABLE Directors (
 Id INT PRIMARY KEY NOT NULL,
 DirectorName NVARCHAR (30)NOT NULL,
 Notes NVARCHAR(200))
 
 
 INSERT INTO Directors(Id,DirectorName,Notes) VALUES
 (1,'J.K.Smith','Tuk-Tam'),
 (2,'I.K M.K',NULL),
 (3,'Ilian',NULL)

 SELECT * FROM Directors

 CREATE TABLE Genres (
 Id INT PRIMARY KEY NOT NULL,
 GenreName NVARCHAR (30)NOT NULL,
 Notes NVARCHAR(200))

 INSERT INTO Genres (Id,GenreName,Notes) VALUES
 (1,'Comedy','Tuk-Tam'),
 (2,'Horror',NULL),
 (3,'Romantic',NULL)

  SELECT * FROM Genres

 CREATE TABLE Categories (
 Id INT PRIMARY KEY NOT NULL,
 CategoryName NVARCHAR (30)NOT NULL,
 Notes NVARCHAR(200))

  INSERT INTO Categories(Id,CategoryName,Notes) VALUES
 (1,'Comedy','Bla-'),
 (2,'Horror','Bla-'),
 (3,'Romantic','Bla!')

  SELECT * FROM Categories