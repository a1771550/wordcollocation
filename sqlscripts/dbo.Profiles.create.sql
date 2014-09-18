USE [WordCollocationTest]
GO

/****** Object:  Table [dbo].[Profiles]    Script Date: 09/18/2014 15:50:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Profiles](
	[UserId] [int] NOT NULL,
	[FirstName] [nvarchar](20) NULL,
	[LastName] [nvarchar](20) NULL,
	[Address] [nvarchar](50) NULL,
	[Address2] [nvarchar](50) NULL,
	[City] [nvarchar](20) NULL,
	[State] [nvarchar](20) NULL,
	[Country] [nvarchar](20) NULL,
	[Phone] [nvarchar](50) NULL,
	[ForumName] [nvarchar](50) NULL,
	[ShowEmail] [bit] NULL,
	[Signature] [nvarchar](200) NULL,
	[Website] [nvarchar](50) NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Profiles] ADD  CONSTRAINT [DF_Profile_ShowEmail]  DEFAULT ((0)) FOR [ShowEmail]
GO

ALTER TABLE [dbo].[Profiles]  WITH CHECK ADD  CONSTRAINT [FK_Profiles_WcUsers] FOREIGN KEY([UserId])
REFERENCES [dbo].[WcUsers] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Profiles] CHECK CONSTRAINT [FK_Profiles_WcUsers]
GO

