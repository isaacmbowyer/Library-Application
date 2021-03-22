SET IDENTITY_INSERT [dbo].[Book] ON
INSERT INTO [dbo].[Book] ([Id], [Title], [AuthorFirstName], [AuthorLastName], [ISBN], [Image], [Genre], [Quantity]) VALUES (1, N'The Hunger Games', N'Suzanne', N'Collins', 978140, N'default.jpg', 0, 5)
INSERT INTO [dbo].[Book] ([Id], [Title], [AuthorFirstName], [AuthorLastName], [ISBN], [Image], [Genre], [Quantity]) VALUES (2, N'The 100', N'Kass', N'Morgan', 978144, N'default.jpg', 0, NULL)
INSERT INTO [dbo].[Book] ([Id], [Title], [AuthorFirstName], [AuthorLastName], [ISBN], [Image], [Genre], [Quantity]) VALUES (3, N'The Maze Runner', N'James', N'Dasher', 978190, N'default.jpg', 1, 1)
INSERT INTO [dbo].[Book] ([Id], [Title], [AuthorFirstName], [AuthorLastName], [ISBN], [Image], [Genre], [Quantity]) VALUES (4, N'Divergent', N'Veronica', N'Roth', 978006, N'default.jpg', 0, NULL)
INSERT INTO [dbo].[Book] ([Id], [Title], [AuthorFirstName], [AuthorLastName], [ISBN], [Image], [Genre], [Quantity]) VALUES (5, N'Hello World', N'Carter ', N'Sande', 978123, N'default.jpg', 2, NULL)
SET IDENTITY_INSERT [dbo].[Book] OFF
