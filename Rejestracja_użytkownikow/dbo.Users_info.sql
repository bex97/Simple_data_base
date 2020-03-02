CREATE TABLE [dbo].[Users_info]
(
	[Id] INT NULL , 
    [UserName] NVARCHAR(50) NOT NULL, 
    [Password] NVARCHAR(50) NOT NULL, 
    [RealName] NVARCHAR(50) NULL, 
    [Age] INT NOT NULL, 
    PRIMARY KEY ([UserName])
)
