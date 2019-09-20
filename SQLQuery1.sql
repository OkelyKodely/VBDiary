USE [test]
GO

/****** Object:  Table [dbo].[diary]    Script Date: 9/20/2019 4:51:29 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[diary](
	[name] [text] NULL,
	[content] [text] NULL,
	[id] [int] IDENTITY(1,1) NOT NULL,
	[inputdate] [datetime] NULL,
 CONSTRAINT [PK_diary] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


