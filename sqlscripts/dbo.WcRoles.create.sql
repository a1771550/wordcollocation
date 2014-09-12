CREATE TABLE [dbo].[WcRoles] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (20) NOT NULL,
    [RowVersion] ROWVERSION    NOT NULL,
    [CanDel] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_WcRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
);

