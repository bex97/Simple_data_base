CREATE TABLE [dbo].[Users]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Imie] NVARCHAR(50) NOT NULL, 
    [Nazwisko] NVARCHAR(50) NOT NULL, 
    [E-mail] NVARCHAR(50) NOT NULL, 
    [Plec] CHAR(10) NULL, 
    [Adres] NVARCHAR(50) NOT NULL, 
    [Telefon] NVARCHAR(50) NOT NULL
)
