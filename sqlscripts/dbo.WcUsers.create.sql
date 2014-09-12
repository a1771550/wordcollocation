CREATE TABLE [dbo].[WcUsers] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (50) NOT NULL,
    [Password]   NVARCHAR (12) NOT NULL,
    [Email]      NVARCHAR (50) NULL,
    [RoleId]     INT           CONSTRAINT [DF_WcUsers_RoleId] DEFAULT ((4)) NOT NULL,
    [RowVersion] ROWVERSION    NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_WcUsers_WcRoles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[WcRoles] ([Id])
);

