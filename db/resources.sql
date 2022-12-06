CREATE TABLE Resources
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(50) NULL, 
    [Content] VARCHAR(50) NULL, 
    [CreationDate] DATETIME NOT NULL, 
    [Parent] INT FOREIGN KEY REFERENCES Resources(Id), 
    [Event] VARCHAR(50) NULL,
    [Endpoint] VARCHAR(50) NULL,
	[Type] VARCHAR(50) NOT NULL,
)
