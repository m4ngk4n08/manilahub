create database manilahub;

CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Password] [varchar](3999) NOT NULL,
	[Contact_Number] [varchar](11) ,
	[Agent_Id] [int] ,
	[Balance] [varchar](10) default 0,
	[Referral_Code] [char](5) ,
	[Role] [int] default 5,
	[Status] [int] default 1,
	[Created] [datetime] default CURRENT_TIMESTAMP,
	[CreatedBy] [varchar](50) ,
	[ModifiedBy] [varchar](50) 
) ON [PRIMARY]
GO

create table Session(
SessionId int identity(1,1),
UserId int,
Bearer_Token varchar(3999),
Expiration datetime,
IsActive tinyint default 1,
Created datetime default current_timestamp
)
