USE [DataTables.Sample]
GO
/****** Object:  Table [dbo].[Persons]    Script Date: 05-Apr-19 13:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Persons](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](max) NULL,
 CONSTRAINT [PK_Persons] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ToDos]    Script Date: 05-Apr-19 13:43:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ToDos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[CreatedById] [int] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[Deadline] [datetime2](7) NULL,
	[EstimatedDuration] [int] NULL,
	[ExpectedCosts] [decimal](18, 2) NULL,
	[Finished] [bit] NOT NULL,
 CONSTRAINT [PK_ToDos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[ToDos] ADD  DEFAULT ('2019-03-22T13:41:14.3045088+01:00') FOR [CreatedAt]
GO
ALTER TABLE [dbo].[ToDos] ADD  DEFAULT ((0)) FOR [Finished]
GO
ALTER TABLE [dbo].[ToDos]  WITH CHECK ADD  CONSTRAINT [FK_ToDos_Persons_CreatedById] FOREIGN KEY([CreatedById])
REFERENCES [dbo].[Persons] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ToDos] CHECK CONSTRAINT [FK_ToDos_Persons_CreatedById]
GO
