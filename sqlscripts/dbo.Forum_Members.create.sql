USE [WordCollocationTest]
GO

/****** Object:  Table [dbo].[Forum_Members]    Script Date: 09/18/2014 15:50:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Forum_Members](
	[UserId] [int] NOT NULL,
	[MemberName] [nvarchar](50) NULL,
	[ShowEmail] [bit] NULL,
	[Signature] [nvarchar](200) NULL,
	[AvatarUrl] [nvarchar](100) NULL,
	[Website] [nvarchar](100) NULL,
	[AddedDate] [datetime] NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Forum_Members] ADD  CONSTRAINT [DF_Forum_Members_ShowEmail]  DEFAULT ((0)) FOR [ShowEmail]
GO

ALTER TABLE [dbo].[Forum_Members]  WITH CHECK ADD  CONSTRAINT [FK_Forum_Members_WcUsers] FOREIGN KEY([UserId])
REFERENCES [dbo].[WcUsers] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Forum_Members] CHECK CONSTRAINT [FK_Forum_Members_WcUsers]
GO

