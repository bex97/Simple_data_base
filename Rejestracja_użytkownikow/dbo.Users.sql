CREATE TABLE [dbo].[Users] (
    [Id]       INT           NULL,
    [UserName] NVARCHAR (50) NOT NULL,
    [Password] NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([UserName])
);

