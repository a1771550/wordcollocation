/****** Object:  Table [dbo].[WcUsers]    Script Date: 09/11/2014 13:23:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WcUsers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](12) NOT NULL,
	[Email] [nvarchar](50) NULL,
	[RoleId] [int] NOT NULL CONSTRAINT [DF_WcUsers_RoleId]  DEFAULT ((4)),
	[RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[WcUsers]  WITH CHECK ADD  CONSTRAINT [FK_WcUsers_WcRoles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[WcRoles] ([Id])
GO

ALTER TABLE [dbo].[WcUsers] CHECK CONSTRAINT [FK_WcUsers_WcRoles]
GO

