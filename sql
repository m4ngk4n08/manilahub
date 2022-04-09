create database manilahub;

CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[Contact_Number] [varchar](11) NULL,
	[Agent_Id] [int] NULL,
	[Balance] [varchar](10) NULL,
	[Referral_Code] [char](5) NULL,
	[Role] [int] NULL,
	[Status] [int] NULL,
	[Created] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedBy] [varchar](50) NULL
) ON [PRIMARY]
GO
