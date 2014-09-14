SET IDENTITY_INSERT [dbo].[WcRoles] ON
INSERT INTO [dbo].[WcRoles] ([Id], [Name], [CanDel]) VALUES (1, N'Admin', 0)
INSERT INTO [dbo].[WcRoles] ([Id], [Name], [CanDel]) VALUES (2, N'Editor', 0)
INSERT INTO [dbo].[WcRoles] ([Id], [Name], [CanDel]) VALUES (3, N'Member', 0)
INSERT INTO [dbo].[WcRoles] ([Id], [Name], [CanDel]) VALUES (4, N'Guest', 1)
INSERT INTO [dbo].[WcRoles] ([Id], [Name], [CanDel]) VALUES (5, N'test', 0)
SET IDENTITY_INSERT [dbo].[WcRoles] OFF
