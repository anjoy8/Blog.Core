USE [WMBlogDB]
GO
/****** Object:  Table [dbo].[Advertisement]    Script Date: 3/4/2019 3:09:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Advertisement](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Createdate] [datetime] NOT NULL,
	[ImgUrl] [nvarchar](512) NULL,
	[Title] [nvarchar](64) NULL,
	[Url] [nvarchar](256) NULL,
	[Remark] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Advertisement] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlogArticle]    Script Date: 3/4/2019 3:09:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlogArticle](
	[bID] [int] IDENTITY(1,1) NOT NULL,
	[bsubmitter] [nvarchar](60) NULL,
	[btitle] [nvarchar](256) NULL,
	[bcategory] [nvarchar](max) NULL,
	[bcontent] [text] NULL,
	[btraffic] [int] NOT NULL,
	[bcommentNum] [int] NOT NULL,
	[bUpdateTime] [datetime] NOT NULL,
	[bCreateTime] [datetime] NOT NULL,
	[bRemark] [nvarchar](max) NULL,
 CONSTRAINT [PK_BlogArticle] PRIMARY KEY CLUSTERED 
(
	[bID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Guestbook]    Script Date: 3/4/2019 3:09:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Guestbook](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[blogId] [int] NOT NULL,
	[createdate] [datetime] NOT NULL,
	[username] [nvarchar](max) NULL,
	[phone] [nvarchar](max) NULL,
	[QQ] [nvarchar](max) NULL,
	[body] [nvarchar](max) NULL,
	[ip] [nvarchar](max) NULL,
	[isshow] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.Guestbook] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Module]    Script Date: 3/4/2019 3:09:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Module](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NULL,
	[ParentId] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[LinkUrl] [nvarchar](100) NULL,
	[Area] [nvarchar](max) NULL,
	[Controller] [nvarchar](max) NULL,
	[Action] [nvarchar](max) NULL,
	[Icon] [nvarchar](100) NULL,
	[Code] [nvarchar](10) NULL,
	[OrderSort] [int] NOT NULL,
	[Description] [nvarchar](100) NULL,
	[IsMenu] [bit] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[CreateId] [int] NULL,
	[CreateBy] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[ModifyId] [int] NULL,
	[ModifyBy] [nvarchar](50) NULL,
	[ModifyTime] [datetime] NULL,
 CONSTRAINT [PK_dbo.Module] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ModulePermission]    Script Date: 3/4/2019 3:09:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ModulePermission](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NULL,
	[ModuleId] [int] NOT NULL,
	[PermissionId] [int] NOT NULL,
	[CreateId] [int] NULL,
	[CreateBy] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[ModifyId] [int] NULL,
	[ModifyBy] [nvarchar](50) NULL,
	[ModifyTime] [datetime] NULL,
 CONSTRAINT [PK_dbo.ModulePermission] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OperateLog]    Script Date: 3/4/2019 3:09:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OperateLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NULL,
	[Area] [nvarchar](max) NULL,
	[Controller] [nvarchar](max) NULL,
	[Action] [nvarchar](max) NULL,
	[IPAddress] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[LogTime] [datetime] NULL,
	[LoginName] [nvarchar](max) NULL,
	[UserId] [int] NOT NULL,
	[User_uID] [int] NULL,
 CONSTRAINT [PK_dbo.OperateLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PasswordLib]    Script Date: 3/4/2019 3:09:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PasswordLib](
	[PLID] [int] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NULL,
	[plURL] [nvarchar](200) NULL,
	[plPWD] [nvarchar](100) NULL,
	[plAccountName] [nvarchar](200) NULL,
	[plStatus] [int] NULL,
	[plErrorCount] [int] NULL,
	[plHintPwd] [nvarchar](200) NULL,
	[plHintquestion] [nvarchar](200) NULL,
	[plCreateTime] [datetime] NULL,
	[plUpdateTime] [datetime] NULL,
	[plLastErrTime] [datetime] NULL,
	[test] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.PasswordLib] PRIMARY KEY CLUSTERED 
(
	[PLID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permission]    Script Date: 3/4/2019 3:09:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permission](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[OrderSort] [int] NOT NULL,
	[Mid] [int] NULL,
	[Pid] [int] NULL,
	[IsButton] [bit] NULL,
	[Icon] [nvarchar](100) NULL,
	[Description] [nvarchar](100) NULL,
	[Enabled] [bit] NOT NULL,
	[CreateId] [int] NULL,
	[CreateBy] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[ModifyId] [int] NULL,
	[ModifyBy] [nvarchar](50) NULL,
	[ModifyTime] [datetime] NULL,
 CONSTRAINT [PK_dbo.Permission] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 3/4/2019 3:09:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [nvarchar](100) NULL,
	[OrderSort] [int] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[CreateId] [int] NULL,
	[CreateBy] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[ModifyId] [int] NULL,
	[ModifyBy] [nvarchar](50) NULL,
	[ModifyTime] [datetime] NULL,
 CONSTRAINT [PK_dbo.Role] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleModulePermission]    Script Date: 3/4/2019 3:09:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleModulePermission](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NULL,
	[RoleId] [int] NOT NULL,
	[ModuleId] [int] NOT NULL,
	[PermissionId] [int] NULL,
	[CreateId] [int] NULL,
	[CreateBy] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[ModifyId] [int] NULL,
	[ModifyBy] [nvarchar](50) NULL,
	[ModifyTime] [datetime] NULL,
 CONSTRAINT [PK_dbo.RoleModulePermission] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sysUserInfo]    Script Date: 3/4/2019 3:09:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sysUserInfo](
	[uID] [int] IDENTITY(1,1) NOT NULL,
	[uLoginName] [varchar](60) NULL,
	[uLoginPWD] [varchar](60) NULL,
	[uRealName] [varchar](60) NULL,
	[uStatus] [int] NOT NULL,
	[uRemark] [varchar](max) NULL,
	[uCreateTime] [datetime] NOT NULL,
	[uUpdateTime] [datetime] NOT NULL,
	[uLastErrTime] [datetime] NOT NULL,
	[uErrorCount] [int] NOT NULL,
	[name] [varchar](60) NULL,
	[sex] [int] NULL,
	[age] [int] NULL,
	[birth] [datetime] NULL,
	[addr] [varchar](200) NULL,
	[tdIsDelete] [bit] NULL,
 CONSTRAINT [PK_dbo.sysUserInfo] PRIMARY KEY CLUSTERED 
(
	[uID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Topic]    Script Date: 3/4/2019 3:09:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Topic](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[tLogo] [nvarchar](200) NULL,
	[tName] [nvarchar](200) NULL,
	[tDetail] [nvarchar](400) NULL,
	[tSectendDetail] [nvarchar](200) NULL,
	[tIsDelete] [bit] NOT NULL,
	[tRead] [int] NOT NULL,
	[tCommend] [int] NOT NULL,
	[tGood] [int] NOT NULL,
	[tCreatetime] [datetime] NOT NULL,
	[tUpdatetime] [datetime] NOT NULL,
	[tAuthor] [nvarchar](200) NULL,
 CONSTRAINT [PK_dbo.Topic] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TopicDetail]    Script Date: 3/4/2019 3:09:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TopicDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TopicId] [int] NOT NULL,
	[tdLogo] [nvarchar](200) NULL,
	[tdName] [nvarchar](200) NULL,
	[tdContent] [nvarchar](max) NULL,
	[tdDetail] [nvarchar](400) NULL,
	[tdSectendDetail] [nvarchar](200) NULL,
	[tdIsDelete] [bit] NOT NULL,
	[tdRead] [int] NOT NULL,
	[tdCommend] [int] NOT NULL,
	[tdGood] [int] NOT NULL,
	[tdCreatetime] [datetime] NOT NULL,
	[tdUpdatetime] [datetime] NOT NULL,
	[tdTop] [int] NOT NULL,
	[tdAuthor] [nvarchar](200) NULL,
 CONSTRAINT [PK_dbo.TopicDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 3/4/2019 3:09:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IsDeleted] [bit] NULL,
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[CreateId] [int] NULL,
	[CreateBy] [nvarchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[ModifyId] [int] NULL,
	[ModifyBy] [nvarchar](50) NULL,
	[ModifyTime] [datetime] NULL,
 CONSTRAINT [PK_dbo.UserRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[BlogArticle] ON 

INSERT [dbo].[BlogArticle] ([bID], [bsubmitter], [btitle], [bcategory], [bcontent], [btraffic], [bcommentNum], [bUpdateTime], [bCreateTime], [bRemark]) VALUES (1, N'admin', N'IIS new add website ，some wrong:The requested page cannot be accessed because the related configuration data for the page is invalid.', N'技术博文', N'                            <p>Question:</p><h1><a href="https://www.cnblogs.com/yipeng-yu/p/6210380.html">The requested page cannot be accessed because the related configuration data for the page is invalid.</a></h1><p>HTTP Error 500.19 - Internal Server Error The requested page cannot be accessed because the related configuration data for the page is invalid.</p><p>Detailed Error Information:</p><p>Module IIS Web Core</p><p>Notification Unknown</p><p>Handler Not yet determined</p><p>Error Code 0x80070003</p><p>Config Error Cannot read configuration file</p><p>Config File \?\D:\Projects\...\web.config</p><p>Requested URL http:// localhost:8080/</p><p>Physical Path</p><p>Logon Method Not yet determined</p><p>Logon User Not yet determined</p><p>Request Tracing Directory C:\Users\...\TraceLogFiles\</p><p>Config Source:</p><p>Answer:</p><p>1，find the site''s application pools</p><p>2,"Advanced Settings" ==&gt; Indentity ==&gt;&nbsp; Custom account</p><p><br></p><p><br></p>', 123, 1, CAST(N'2018-06-20T15:36:52.663' AS DateTime), CAST(N'2018-06-20T15:36:50.960' AS DateTime), N'')

SET IDENTITY_INSERT [dbo].[BlogArticle] OFF
SET IDENTITY_INSERT [dbo].[Guestbook] ON 

INSERT [dbo].[Guestbook] ([id], [blogId], [createdate], [username], [phone], [QQ], [body], [ip], [isshow]) VALUES (1, 1, CAST(N'2018-06-20T15:42:12.760' AS DateTime), NULL, NULL, NULL, NULL, N'::1', 0)
SET IDENTITY_INSERT [dbo].[Guestbook] OFF
SET IDENTITY_INSERT [dbo].[Module] ON 

INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (7, 0, NULL, N'values接口', N'/api/values', NULL, NULL, NULL, NULL, NULL, 1, NULL, 0, 1, NULL, NULL, CAST(N'2019-02-20T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (8, 0, NULL, N'claims的接口', N'/api/claims', NULL, NULL, NULL, NULL, NULL, 1, NULL, 0, 1, NULL, NULL, CAST(N'2019-02-20T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (9, 0, NULL, N'UserRole接口', N'/api/UserRole', NULL, NULL, NULL, NULL, NULL, 1, NULL, 0, 1, NULL, NULL, CAST(N'2019-02-20T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (10, 0, NULL, N'', N'/api/v2/Apb/apbs', NULL, NULL, NULL, NULL, NULL, 1, NULL, 0, 1, NULL, NULL, CAST(N'2019-02-20T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (11, 0, NULL, N'修改 tibug 文章', N'/api/TopicDetail/update', NULL, NULL, NULL, NULL, NULL, 1, NULL, 0, 1, NULL, NULL, CAST(N'2019-02-20T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (12, 0, NULL, N'删除tibug文章', N'/api/TopicDetail/delete', NULL, NULL, NULL, NULL, NULL, 1, NULL, 0, 1, NULL, NULL, CAST(N'2019-02-20T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (13, 0, NULL, N'获取用户', N'/api/user/get', NULL, NULL, NULL, NULL, NULL, 1, NULL, 0, 1, NULL, NULL, CAST(N'2019-02-20T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (14, 0, NULL, N'获取用户详情', N'/api/user/get/\d+', NULL, NULL, NULL, NULL, NULL, 1, NULL, 0, 1, NULL, NULL, CAST(N'2019-02-20T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (15, 1, NULL, N'角色接口', N'/api/role', NULL, NULL, NULL, NULL, NULL, 0, NULL, 0, 1, 18, N'提bug账号', CAST(N'2019-02-20T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-20T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (16, 0, NULL, N'添加用户', N'/api/user/post', NULL, NULL, NULL, NULL, NULL, 0, NULL, 0, 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (17, 0, NULL, N'删除用户', N'/api/user/delete', NULL, NULL, NULL, NULL, NULL, 0, NULL, 0, 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (18, 0, NULL, N'修改用户', N'/api/user/put', NULL, NULL, NULL, NULL, NULL, 0, NULL, 0, 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (19, 0, NULL, N'获取api接口', N'/api/module/get', NULL, NULL, NULL, NULL, NULL, 0, NULL, 0, 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (20, 0, NULL, N'删除api接口', N'/api/module/delete', NULL, NULL, NULL, NULL, NULL, 0, NULL, 0, 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (21, 0, NULL, N'修改api接口', N'/api/module/put', NULL, NULL, NULL, NULL, NULL, 0, NULL, 0, 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (22, 0, NULL, N'添加api接口', N'/api/module/post', NULL, NULL, NULL, NULL, NULL, 0, NULL, 0, 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (23, 0, NULL, N'获取菜单', N'/api/permission/get', NULL, NULL, NULL, NULL, NULL, 0, NULL, 0, 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (24, 0, NULL, N'删除菜单', N'/api/permission/delete', NULL, NULL, NULL, NULL, NULL, 0, NULL, 0, 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (25, 0, NULL, N'修改菜单', N'/api/permission/put', NULL, NULL, NULL, NULL, NULL, 0, NULL, 0, 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (26, 0, NULL, N'添加菜单', N'/api/permission/post', NULL, NULL, NULL, NULL, NULL, 0, NULL, 0, 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (27, 0, NULL, N'获取菜单树', N'/api/permission/getpermissiontree', NULL, NULL, NULL, NULL, NULL, 0, NULL, 0, 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (28, 0, NULL, N'获取角色', N'/api/role/get', NULL, NULL, NULL, NULL, NULL, 0, NULL, 0, 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (29, 0, NULL, N'删除角色', N'/api/role/delete', NULL, NULL, NULL, NULL, NULL, 0, NULL, 0, 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (30, 0, NULL, N'修改角色', N'/api/role/put', NULL, NULL, NULL, NULL, NULL, 0, NULL, 0, 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (31, 0, NULL, N'添加角色', N'/api/role/post', NULL, NULL, NULL, NULL, NULL, 0, NULL, 0, 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (32, 0, NULL, N'获取bug', N'/api/TopicDetail/Get', NULL, NULL, NULL, NULL, NULL, 0, NULL, 0, 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (33, 0, NULL, N'获取博客', N'/api/Blog', NULL, NULL, NULL, NULL, NULL, 0, NULL, 0, 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (34, 0, NULL, N'保存分配', N'/api/permission/Assign', NULL, NULL, NULL, NULL, NULL, 0, NULL, 0, 1, 23, N'后台总管理员', CAST(N'2019-02-23T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:00:00.000' AS DateTime))
INSERT [dbo].[Module] ([Id], [IsDeleted], [ParentId], [Name], [LinkUrl], [Area], [Controller], [Action], [Icon], [Code], [OrderSort], [Description], [IsMenu], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (35, 0, NULL, N'Get导航条', N'/api/permission/GetNavigationBar', NULL, NULL, NULL, NULL, NULL, 0, NULL, 0, 1, 23, N'后台总管理员', CAST(N'2019-02-25T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:00:00.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[Module] OFF
SET IDENTITY_INSERT [dbo].[Permission] ON 

INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (1, 0, N'/', N'QQ欢迎页', 0, 0, 0, 0, N'fa-qq', N'33', 1, 18, N'提bug账号', CAST(N'2019-02-21T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (2, 0, N'/', N'用户角色管理', 0, 0, 0, 0, N'fa-users', N'11', 1, 18, N'提bug账号', CAST(N'2019-02-21T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (3, 0, N'/Admin/Roles', N'角色管理', 0, 28, 2, 0, NULL, N'', 1, 18, N'提bug账号', CAST(N'2019-02-21T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (4, 0, N'/Admin/Users', N'用户管理', 0, 13, 2, 0, NULL, N'', 1, 18, N'提bug账号', CAST(N'2019-02-21T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (5, 0, N'/', N'菜单权限管理', 0, 0, 0, 0, N'fa-sitemap', N'', 1, 18, N'提bug账号', CAST(N'2019-02-21T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (6, 0, N'/Permission/Modules', N'接口管理', 0, 19, 5, 0, NULL, N'', 1, 18, N'提bug账号', CAST(N'2019-02-21T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (7, 0, N'/Permission/Menu', N'菜单管理', 0, 23, 5, 0, NULL, N'', 1, 18, N'提bug账号', CAST(N'2019-02-21T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (8, 0, N'/Thanks', N'致谢页', 10, 0, 0, 0, N'fa-star ', N'', 1, 18, N'提bug账号', CAST(N'2019-02-21T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (9, 0, N'无', N'查询', 0, 13, 3, 1, NULL, N'这个用户页的查询按钮', 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (10, 0, N'/', N'报表管理', 0, 0, 0, 0, N'fa-line-chart', N'', 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (11, 0, N'/Chart/From', N'表单', 0, 0, 10, 0, NULL, N'', 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (12, 0, N'/Chart/Charts', N'图表', 0, 0, 10, 0, NULL, N'', 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (13, 0, N' ', N'新增', 0, 16, 3, 1, NULL, N'', 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (14, 0, N' ', N'编辑', 0, 18, 3, 1, NULL, N'', 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (15, 0, N' ', N'删除', 0, 17, 3, 1, NULL, N'', 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (16, 0, N' ', N'查询', 0, 28, 4, 1, NULL, N'', 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (17, 0, N' ', N'新增', 0, 31, 4, 1, NULL, N'', 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (18, 0, N' ', N'编辑', 0, 30, 4, 1, NULL, N'', 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (19, 0, N' ', N'删除', 0, 29, 4, 1, NULL, N'', 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (20, 0, N' ', N'查询', 0, 19, 6, 1, NULL, N'', 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (21, 0, N' ', N'新增', 0, 22, 6, 1, NULL, N'', 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (22, 0, N' ', N'编辑', 0, 21, 6, 1, NULL, N'', 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (23, 0, N' ', N'删除', 0, 20, 6, 1, NULL, N'', 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (24, 0, N' ', N'查询', 0, 23, 7, 1, NULL, N'', 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (25, 0, N' ', N'新增', 0, 26, 7, 1, NULL, N'', 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (26, 0, N' ', N'编辑', 0, 25, 7, 1, NULL, N'', 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (27, 0, N' ', N'删除', 0, 24, 7, 1, NULL, N'', 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (28, 0, N'/Tibug', N'问题管理', 0, 32, 0, 0, N'fa-bug', N'', 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (29, 0, N'/Blogs', N'博客管理', 0, 33, 0, 0, N'fa-file-word-o', N'', 1, 18, N'提bug账号', CAST(N'2019-02-22T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (30, 0, N' ', N'编辑', 0, 11, 28, 1, NULL, N'', 1, 18, N'提bug账号', CAST(N'2019-02-23T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-23T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (31, 0, N' ', N'删除', 0, 12, 28, 1, NULL, N'', 1, 18, N'提bug账号', CAST(N'2019-02-23T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-23T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (32, 0, N' ', N'查询', 0, 32, 28, 1, NULL, N'', 1, 23, N'后台总管理员', CAST(N'2019-02-23T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-23T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (33, 0, N' ', N'菜单树', 0, 27, 7, 1, NULL, N'', 1, 23, N'后台总管理员', CAST(N'2019-02-23T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (34, 0, N'/Permission/Assign', N'权限分配', 0, 0, 5, 0, NULL, N'', 1, 23, N'后台总管理员', CAST(N'2019-02-23T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (35, 0, N' ', N'保存权限', 0, 34, 34, 1, NULL, N'', 1, 23, N'后台总管理员', CAST(N'2019-02-25T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:00:00.000' AS DateTime))
INSERT [dbo].[Permission] ([Id], [IsDeleted], [Code], [Name], [OrderSort], [Mid], [Pid], [IsButton], [Icon], [Description], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (36, 0, N' ', N'左侧导航', 0, 35, 7, 1, N'', N'', 1, 23, N'后台总管理员', CAST(N'2019-02-25T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:00:00.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[Permission] OFF
SET IDENTITY_INSERT [dbo].[Role] ON 

INSERT [dbo].[Role] ([Id], [IsDeleted], [Name], [Description], [OrderSort], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (1, 0, N'Admin', N'普通管理', 1, 1, NULL, NULL, CAST(N'2018-11-02T00:34:40.290' AS DateTime), NULL, NULL, CAST(N'2018-11-02T00:34:40.293' AS DateTime))
INSERT [dbo].[Role] ([Id], [IsDeleted], [Name], [Description], [OrderSort], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (2, 0, N'System', N'系统管理', 1, 1, NULL, NULL, CAST(N'2018-11-02T00:34:40.290' AS DateTime), NULL, NULL, CAST(N'2018-11-02T00:34:40.293' AS DateTime))
INSERT [dbo].[Role] ([Id], [IsDeleted], [Name], [Description], [OrderSort], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (3, 0, N'Tibug', N'tibug系统管理', 1, 1, NULL, NULL, CAST(N'2018-11-02T00:34:40.290' AS DateTime), NULL, NULL, CAST(N'2018-11-02T00:34:40.293' AS DateTime))
INSERT [dbo].[Role] ([Id], [IsDeleted], [Name], [Description], [OrderSort], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (4, 0, N'SuperAdmin', N'超级管理', 0, 1, 23, N'blogadmin', CAST(N'2019-02-18T00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-02-18T00:00:00.000' AS DateTime))
INSERT [dbo].[Role] ([Id], [IsDeleted], [Name], [Description], [OrderSort], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (5, 1, N'AdminTest', NULL, 1, 1, 18, N'提bug账号', CAST(N'2019-02-19T15:31:31.227' AS DateTime), NULL, NULL, CAST(N'2019-02-19T15:31:31.227' AS DateTime))
INSERT [dbo].[Role] ([Id], [IsDeleted], [Name], [Description], [OrderSort], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (6, 0, N'AdminTest', N'测试管理', 1, 1, 23, N'后台总管理员', CAST(N'2019-02-19T15:32:42.183' AS DateTime), NULL, NULL, CAST(N'2019-02-19T15:32:42.183' AS DateTime))
INSERT [dbo].[Role] ([Id], [IsDeleted], [Name], [Description], [OrderSort], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (7, 0, N'AdminTest2', N'测试管理2', 1, 1, 23, N'后台总管理员', CAST(N'2019-02-26T11:01:23.223' AS DateTime), NULL, NULL, CAST(N'2019-02-26T11:01:23.223' AS DateTime))
INSERT [dbo].[Role] ([Id], [IsDeleted], [Name], [Description], [OrderSort], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (8, NULL, N'211', N'', 1, 1, NULL, NULL, CAST(N'2019-02-27T12:02:05.467' AS DateTime), NULL, NULL, CAST(N'2019-02-27T12:02:05.467' AS DateTime))
INSERT [dbo].[Role] ([Id], [IsDeleted], [Name], [Description], [OrderSort], [Enabled], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (9, NULL, N'2', N'', 1, 1, NULL, NULL, CAST(N'2019-02-28T09:38:08.527' AS DateTime), NULL, NULL, CAST(N'2019-02-28T09:38:08.527' AS DateTime))
SET IDENTITY_INSERT [dbo].[Role] OFF
SET IDENTITY_INSERT [dbo].[RoleModulePermission] ON 

INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (1, 0, 1, 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (2, 0, 1, 8, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (3, 0, 1, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (4, 0, 1, 10, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (5, 0, 2, 10, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (6, 0, 3, 11, 30, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (7, 0, 3, 12, 31, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (8, 0, 3, 13, 9, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (10, 0, 3, 32, 28, NULL, NULL, CAST(N'2019-02-23T19:22:46.473' AS DateTime), NULL, NULL, CAST(N'2019-02-23T19:22:46.473' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (14, 0, 4, 13, 3, NULL, NULL, CAST(N'2019-02-23T20:04:23.867' AS DateTime), NULL, NULL, CAST(N'2019-02-23T20:04:23.867' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (15, 0, 4, 13, 9, NULL, NULL, CAST(N'2019-02-23T20:04:23.973' AS DateTime), NULL, NULL, CAST(N'2019-02-23T20:04:23.973' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (16, 0, 4, 16, 13, NULL, NULL, CAST(N'2019-02-23T20:04:24.000' AS DateTime), NULL, NULL, CAST(N'2019-02-23T20:04:24.000' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (17, 0, 4, 18, 14, NULL, NULL, CAST(N'2019-02-23T20:04:24.037' AS DateTime), NULL, NULL, CAST(N'2019-02-23T20:04:24.037' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (18, 0, 4, 17, 15, NULL, NULL, CAST(N'2019-02-23T20:04:24.067' AS DateTime), NULL, NULL, CAST(N'2019-02-23T20:04:24.067' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (27, 0, 4, 0, 2, NULL, NULL, CAST(N'2019-02-23T21:00:30.703' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:00:30.703' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (28, 0, 4, 28, 4, NULL, NULL, CAST(N'2019-02-23T21:00:30.987' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:00:30.987' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (29, 0, 4, 28, 16, NULL, NULL, CAST(N'2019-02-23T21:00:31.010' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:00:31.010' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (30, 0, 4, 31, 17, NULL, NULL, CAST(N'2019-02-23T21:00:31.030' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:00:31.030' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (31, 0, 4, 30, 18, NULL, NULL, CAST(N'2019-02-23T21:00:31.053' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:00:31.053' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (32, 0, 4, 29, 19, NULL, NULL, CAST(N'2019-02-23T21:00:31.083' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:00:31.083' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (33, 0, 4, 0, 1, NULL, NULL, CAST(N'2019-02-23T21:00:55.233' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:00:55.233' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (34, 0, 4, 0, 5, NULL, NULL, CAST(N'2019-02-23T21:00:55.253' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:00:55.253' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (35, 0, 4, 19, 6, NULL, NULL, CAST(N'2019-02-23T21:00:55.277' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:00:55.277' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (36, 0, 4, 19, 20, NULL, NULL, CAST(N'2019-02-23T21:00:55.297' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:00:55.297' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (37, 0, 4, 22, 21, NULL, NULL, CAST(N'2019-02-23T21:00:55.317' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:00:55.317' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (38, 0, 4, 21, 22, NULL, NULL, CAST(N'2019-02-23T21:00:55.340' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:00:55.340' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (39, 0, 4, 20, 23, NULL, NULL, CAST(N'2019-02-23T21:00:55.360' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:00:55.360' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (40, 0, 4, 23, 7, NULL, NULL, CAST(N'2019-02-23T21:00:55.383' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:00:55.383' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (41, 0, 4, 23, 24, NULL, NULL, CAST(N'2019-02-23T21:00:55.407' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:00:55.407' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (42, 0, 4, 26, 25, NULL, NULL, CAST(N'2019-02-23T21:00:55.430' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:00:55.430' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (43, 0, 4, 25, 26, NULL, NULL, CAST(N'2019-02-23T21:00:55.450' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:00:55.450' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (44, 0, 4, 24, 27, NULL, NULL, CAST(N'2019-02-23T21:00:55.470' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:00:55.470' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (45, 0, 4, 0, 8, NULL, NULL, CAST(N'2019-02-23T21:01:03.630' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:01:03.630' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (46, 0, 4, 0, 10, NULL, NULL, CAST(N'2019-02-23T21:01:03.653' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:01:03.653' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (47, 0, 4, 0, 11, NULL, NULL, CAST(N'2019-02-23T21:01:03.673' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:01:03.673' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (48, 0, 4, 0, 12, NULL, NULL, CAST(N'2019-02-23T21:01:03.697' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:01:03.697' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (49, 0, 4, 32, 28, NULL, NULL, CAST(N'2019-02-23T21:01:03.720' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:01:03.720' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (50, 0, 4, 11, 30, NULL, NULL, CAST(N'2019-02-23T21:01:03.747' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:01:03.747' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (51, 0, 4, 12, 31, NULL, NULL, CAST(N'2019-02-23T21:01:03.770' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:01:03.770' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (52, 0, 4, 33, 29, NULL, NULL, CAST(N'2019-02-23T21:01:03.790' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:01:03.790' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (53, 0, 4, 32, 32, NULL, NULL, CAST(N'2019-02-23T21:20:14.093' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:20:14.093' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (66, 0, 6, 0, 1, NULL, NULL, CAST(N'2019-02-23T21:34:27.543' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:34:27.543' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (71, 0, 6, 0, 8, NULL, NULL, CAST(N'2019-02-23T21:34:27.670' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:34:27.670' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (72, 0, 6, 0, 10, NULL, NULL, CAST(N'2019-02-23T21:34:27.693' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:34:27.693' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (73, 0, 6, 0, 11, NULL, NULL, CAST(N'2019-02-23T21:34:27.713' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:34:27.713' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (74, 0, 6, 0, 12, NULL, NULL, CAST(N'2019-02-23T21:34:27.750' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:34:27.750' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (76, 0, 6, 33, 29, NULL, NULL, CAST(N'2019-02-23T21:34:27.803' AS DateTime), NULL, NULL, CAST(N'2019-02-23T21:34:27.803' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (79, 0, 6, 0, 2, NULL, NULL, CAST(N'2019-02-25T00:25:33.150' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:25:33.150' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (80, 0, 6, 13, 3, NULL, NULL, CAST(N'2019-02-25T00:25:33.230' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:25:33.230' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (81, 0, 6, 28, 4, NULL, NULL, CAST(N'2019-02-25T00:25:33.247' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:25:33.247' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (82, 0, 6, 0, 5, NULL, NULL, CAST(N'2019-02-25T00:25:33.270' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:25:33.270' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (83, 0, 6, 19, 6, NULL, NULL, CAST(N'2019-02-25T00:25:33.290' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:25:33.290' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (84, 0, 6, 23, 7, NULL, NULL, CAST(N'2019-02-25T00:25:33.313' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:25:33.313' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (86, 0, 6, 32, 28, NULL, NULL, CAST(N'2019-02-25T00:25:33.360' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:25:33.360' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (87, 0, 4, 34, 34, NULL, NULL, CAST(N'2019-02-25T00:27:12.167' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:27:12.167' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (88, 0, 4, 27, 33, NULL, NULL, CAST(N'2019-02-25T00:27:12.187' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:27:12.187' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (89, 0, 6, 13, 9, NULL, NULL, CAST(N'2019-02-25T00:27:51.850' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:27:51.850' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (90, 0, 6, 28, 16, NULL, NULL, CAST(N'2019-02-25T00:27:51.867' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:27:51.867' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (91, 0, 6, 19, 20, NULL, NULL, CAST(N'2019-02-25T00:27:51.887' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:27:51.887' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (92, 0, 6, 23, 24, NULL, NULL, CAST(N'2019-02-25T00:27:51.907' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:27:51.907' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (93, 0, 6, 32, 32, NULL, NULL, CAST(N'2019-02-25T00:27:51.927' AS DateTime), NULL, NULL, CAST(N'2019-02-25T00:27:51.927' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (95, 0, 4, 34, 35, NULL, NULL, CAST(N'2019-02-25T01:26:32.940' AS DateTime), NULL, NULL, CAST(N'2019-02-25T01:26:32.940' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (96, 0, 6, 27, 33, NULL, NULL, CAST(N'2019-02-25T01:27:59.570' AS DateTime), NULL, NULL, CAST(N'2019-02-25T01:27:59.570' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (98, 0, 4, 35, 36, NULL, NULL, CAST(N'2019-02-25T14:28:41.340' AS DateTime), NULL, NULL, CAST(N'2019-02-25T14:28:41.340' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (99, 0, 6, 0, 34, NULL, NULL, CAST(N'2019-02-25T16:55:56.397' AS DateTime), NULL, NULL, CAST(N'2019-02-25T16:55:56.397' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (100, 0, 7, 0, 1, NULL, NULL, CAST(N'2019-02-26T11:31:34.077' AS DateTime), NULL, NULL, CAST(N'2019-02-26T11:31:34.077' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (101, 0, 7, 0, 2, NULL, NULL, CAST(N'2019-02-26T11:31:34.723' AS DateTime), NULL, NULL, CAST(N'2019-02-26T11:31:34.723' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (102, 0, 7, 13, 4, NULL, NULL, CAST(N'2019-02-26T11:31:35.073' AS DateTime), NULL, NULL, CAST(N'2019-02-26T11:31:35.073' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (105, 0, 7, 0, 10, NULL, NULL, CAST(N'2019-02-26T11:31:36.093' AS DateTime), NULL, NULL, CAST(N'2019-02-26T11:31:36.093' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (106, 0, 7, 0, 12, NULL, NULL, CAST(N'2019-02-26T11:31:36.430' AS DateTime), NULL, NULL, CAST(N'2019-02-26T11:31:36.430' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (107, 0, 7, 0, 8, NULL, NULL, CAST(N'2019-02-26T11:31:36.773' AS DateTime), NULL, NULL, CAST(N'2019-02-26T11:31:36.773' AS DateTime))
INSERT [dbo].[RoleModulePermission] ([Id], [IsDeleted], [RoleId], [ModuleId], [PermissionId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (108, 0, 7, 28, 16, NULL, NULL, CAST(N'2019-02-26T11:31:37.110' AS DateTime), NULL, NULL, CAST(N'2019-02-26T11:31:37.110' AS DateTime))
SET IDENTITY_INSERT [dbo].[RoleModulePermission] OFF
SET IDENTITY_INSERT [dbo].[sysUserInfo] ON 

INSERT [dbo].[sysUserInfo] ([uID], [uLoginName], [uLoginPWD], [uRealName], [uStatus], [uRemark], [uCreateTime], [uUpdateTime], [uLastErrTime], [uErrorCount], [name], [sex], [age], [birth], [addr], [tdIsDelete]) VALUES (1, N'laozhang', N'xxxxx', N'老张', 0, NULL, CAST(N'2018-08-08T00:00:00.000' AS DateTime), CAST(N'2018-08-08T00:00:00.000' AS DateTime), CAST(N'2018-08-08T00:00:00.000' AS DateTime), 0, N'老张的哲学', 1, 0, CAST(N'2019-01-01T00:00:00.000' AS DateTime), NULL, 0)
INSERT [dbo].[sysUserInfo] ([uID], [uLoginName], [uLoginPWD], [uRealName], [uStatus], [uRemark], [uCreateTime], [uUpdateTime], [uLastErrTime], [uErrorCount], [name], [sex], [age], [birth], [addr], [tdIsDelete]) VALUES (13, N'laoli', N'xxxx', N'laoli', 0, NULL, CAST(N'2018-06-25T17:43:22.287' AS DateTime), CAST(N'2018-06-25T17:43:22.990' AS DateTime), CAST(N'2018-06-25T17:43:22.613' AS DateTime), 0, NULL, 1, 0, CAST(N'2019-01-01T00:00:00.000' AS DateTime), NULL, 0)
INSERT [dbo].[sysUserInfo] ([uID], [uLoginName], [uLoginPWD], [uRealName], [uStatus], [uRemark], [uCreateTime], [uUpdateTime], [uLastErrTime], [uErrorCount], [name], [sex], [age], [birth], [addr], [tdIsDelete]) VALUES (14, N'user', N'xxxx', N'userli', 0, NULL, CAST(N'2018-06-25T17:53:41.473' AS DateTime), CAST(N'2018-06-25T17:53:41.473' AS DateTime), CAST(N'2018-06-25T17:53:41.473' AS DateTime), 0, N'广告', 1, 0, CAST(N'2019-01-01T00:00:00.000' AS DateTime), NULL, 0)
INSERT [dbo].[sysUserInfo] ([uID], [uLoginName], [uLoginPWD], [uRealName], [uStatus], [uRemark], [uCreateTime], [uUpdateTime], [uLastErrTime], [uErrorCount], [name], [sex], [age], [birth], [addr], [tdIsDelete]) VALUES (15, N'admins', N'xxxx', N'admins', 0, NULL, CAST(N'2018-11-01T23:59:19.877' AS DateTime), CAST(N'2018-11-01T23:59:19.877' AS DateTime), CAST(N'2018-11-01T23:59:19.877' AS DateTime), 0, NULL, NULL, NULL, CAST(N'2019-01-01T00:00:00.000' AS DateTime), NULL, 0)
INSERT [dbo].[sysUserInfo] ([uID], [uLoginName], [uLoginPWD], [uRealName], [uStatus], [uRemark], [uCreateTime], [uUpdateTime], [uLastErrTime], [uErrorCount], [name], [sex], [age], [birth], [addr], [tdIsDelete]) VALUES (16, N'1', N'1', N'1', 0, NULL, CAST(N'2018-11-23T22:37:10.420' AS DateTime), CAST(N'2018-11-23T22:37:10.420' AS DateTime), CAST(N'2018-11-23T22:37:10.420' AS DateTime), 0, NULL, 0, 0, CAST(N'2019-01-01T00:00:00.000' AS DateTime), NULL, 1)
INSERT [dbo].[sysUserInfo] ([uID], [uLoginName], [uLoginPWD], [uRealName], [uStatus], [uRemark], [uCreateTime], [uUpdateTime], [uLastErrTime], [uErrorCount], [name], [sex], [age], [birth], [addr], [tdIsDelete]) VALUES (17, N'23', N'2', N'23', 0, NULL, CAST(N'2018-12-19T23:27:26.807' AS DateTime), CAST(N'2018-12-19T23:27:26.807' AS DateTime), CAST(N'2018-12-19T23:27:26.807' AS DateTime), 0, N'笑笑笑', 1, 18, CAST(N'2019-01-02T00:00:00.000' AS DateTime), N'dd', 1)
INSERT [dbo].[sysUserInfo] ([uID], [uLoginName], [uLoginPWD], [uRealName], [uStatus], [uRemark], [uCreateTime], [uUpdateTime], [uLastErrTime], [uErrorCount], [name], [sex], [age], [birth], [addr], [tdIsDelete]) VALUES (18, N'tibug', N'xxxx', N'提bug账号', 0, NULL, CAST(N'2018-12-20T03:48:01.390' AS DateTime), CAST(N'2018-12-20T03:48:01.390' AS DateTime), CAST(N'2018-12-20T03:48:01.390' AS DateTime), 0, NULL, 1, 0, CAST(N'2019-01-01T00:00:00.000' AS DateTime), NULL, 0)
INSERT [dbo].[sysUserInfo] ([uID], [uLoginName], [uLoginPWD], [uRealName], [uStatus], [uRemark], [uCreateTime], [uUpdateTime], [uLastErrTime], [uErrorCount], [name], [sex], [age], [birth], [addr], [tdIsDelete]) VALUES (19, N'test', N'test', N'后台测试1号', 0, NULL, CAST(N'2019-02-15T19:14:58.013' AS DateTime), CAST(N'2019-02-15T19:14:58.013' AS DateTime), CAST(N'2019-02-15T19:14:58.013' AS DateTime), 0, N'测试是', 1, 3, CAST(N'2019-02-06T00:00:00.000' AS DateTime), N'', 0)
INSERT [dbo].[sysUserInfo] ([uID], [uLoginName], [uLoginPWD], [uRealName], [uStatus], [uRemark], [uCreateTime], [uUpdateTime], [uLastErrTime], [uErrorCount], [name], [sex], [age], [birth], [addr], [tdIsDelete]) VALUES (20, N'5', N'5', N'5', 0, NULL, CAST(N'2019-02-15T19:16:51.063' AS DateTime), CAST(N'2019-02-15T19:16:51.063' AS DateTime), CAST(N'2019-02-15T19:16:51.063' AS DateTime), 0, N'5', 1, 1, CAST(N'2019-01-01T00:00:00.000' AS DateTime), N'', 1)
INSERT [dbo].[sysUserInfo] ([uID], [uLoginName], [uLoginPWD], [uRealName], [uStatus], [uRemark], [uCreateTime], [uUpdateTime], [uLastErrTime], [uErrorCount], [name], [sex], [age], [birth], [addr], [tdIsDelete]) VALUES (21, N'6', N'6', N'6', 0, NULL, CAST(N'2019-02-15T19:18:55.157' AS DateTime), CAST(N'2019-02-15T19:18:55.157' AS DateTime), CAST(N'2019-02-15T19:18:55.157' AS DateTime), 0, N'', 1, 1, CAST(N'2019-01-01T00:00:00.000' AS DateTime), N'', 1)
INSERT [dbo].[sysUserInfo] ([uID], [uLoginName], [uLoginPWD], [uRealName], [uStatus], [uRemark], [uCreateTime], [uUpdateTime], [uLastErrTime], [uErrorCount], [name], [sex], [age], [birth], [addr], [tdIsDelete]) VALUES (22, N'2', N'xxx', N'2', 0, NULL, CAST(N'2019-02-16T14:43:27.230' AS DateTime), CAST(N'2019-02-16T14:43:27.230' AS DateTime), CAST(N'2019-02-16T14:43:27.230' AS DateTime), 0, N'5555', 1, 5, CAST(N'2019-02-21T00:00:00.000' AS DateTime), N'', 1)
INSERT [dbo].[sysUserInfo] ([uID], [uLoginName], [uLoginPWD], [uRealName], [uStatus], [uRemark], [uCreateTime], [uUpdateTime], [uLastErrTime], [uErrorCount], [name], [sex], [age], [birth], [addr], [tdIsDelete]) VALUES (23, N'blogadmin', N'xxx', N'后台总管理员', 0, NULL, CAST(N'2019-02-18T13:36:20.183' AS DateTime), CAST(N'2019-02-18T13:36:20.183' AS DateTime), CAST(N'2019-02-18T13:36:20.183' AS DateTime), 0, N'', 1, 10, CAST(N'2019-02-18T00:00:00.000' AS DateTime), N'', 0)
INSERT [dbo].[sysUserInfo] ([uID], [uLoginName], [uLoginPWD], [uRealName], [uStatus], [uRemark], [uCreateTime], [uUpdateTime], [uLastErrTime], [uErrorCount], [name], [sex], [age], [birth], [addr], [tdIsDelete]) VALUES (24, N'test2', N'test2', N'后台测试2号', 0, NULL, CAST(N'2019-02-26T11:15:04.963' AS DateTime), CAST(N'2019-02-26T11:15:04.963' AS DateTime), CAST(N'2019-02-26T11:15:04.963' AS DateTime), 0, N'', 0, 12, CAST(N'2019-02-26T00:00:00.000' AS DateTime), N'北京市', 0)
INSERT [dbo].[sysUserInfo] ([uID], [uLoginName], [uLoginPWD], [uRealName], [uStatus], [uRemark], [uCreateTime], [uUpdateTime], [uLastErrTime], [uErrorCount], [name], [sex], [age], [birth], [addr], [tdIsDelete]) VALUES (25, N'1', N'xx', N'1', 0, NULL, CAST(N'2019-02-26T11:18:08.227' AS DateTime), CAST(N'2019-02-26T11:18:08.227' AS DateTime), CAST(N'2019-02-26T11:18:08.227' AS DateTime), 0, N'', -1, 0, CAST(N'2019-02-26T00:00:00.000' AS DateTime), N'', 0)
SET IDENTITY_INSERT [dbo].[sysUserInfo] OFF
SET IDENTITY_INSERT [dbo].[Topic] ON 

INSERT [dbo].[Topic] ([Id], [tLogo], [tName], [tDetail], [tSectendDetail], [tIsDelete], [tRead], [tCommend], [tGood], [tCreatetime], [tUpdatetime], [tAuthor]) VALUES (1, N'/Upload/20180626/95445c8e288e47e3af7a180b8a4cc0c7.jpg', N'《罗马人的故事》', N'这是一个荡气回肠的故事', NULL, 0, 0, 0, 0, CAST(N'2018-06-26T15:56:03.190' AS DateTime), CAST(N'2018-06-26T15:56:03.177' AS DateTime), N'Laozhang')

SET IDENTITY_INSERT [dbo].[Topic] OFF
SET IDENTITY_INSERT [dbo].[TopicDetail] ON 

INSERT [dbo].[TopicDetail] ([Id], [TopicId], [tdLogo], [tdName], [tdContent], [tdDetail], [tdSectendDetail], [tdIsDelete], [tdRead], [tdCommend], [tdGood], [tdCreatetime], [tdUpdatetime], [tdTop], [tdAuthor]) VALUES (1, 1, NULL, N'第一章　罗马的诞生 第一节　传说的年代', N'<p>第一节　传说的年代</p><p>每个民族都有自己的神话传说。大概希望知道本民族的来源是个很自然的愿望吧。但这是一个难题，因为这几乎不可能用科学的方法来解释清楚。不过所有的民族都没有这样的奢求。他们只要有一个具有一定的条理性，而又能振奋其民族精神的浪漫故事就行，别抬杠，象柏杨那样将中国的三皇五帝都来个科学分析，来评论他们的执政之优劣是大可不必的。</p><p>对於罗马人，他们有一个和特洛伊城的陷落相关的传说。</p><p>位於小亚细亚西岸的繁荣的城市特洛伊，在遭受了阿加美农统帅的希腊联军的十年围攻之後，仍未陷落。希腊联军於是留下一个巨大的木马後假装撤兵。特洛伊人以为那是希腊联军留给自己的礼物，就将它拉入城内。</p><p>当庆祝胜利的狂欢结束，特洛伊人满怀对明日的和平生活的希望熟睡後，藏在木马内的希腊士兵一个又一个地爬了出来。就在这天夜里，特洛伊城便在火光和叫喊中陷落了。全城遭到大屠杀 ，幸免於死的人全都沦为奴隶。混乱之中只有特洛伊国王的驸马阿伊尼阿斯带着老父，儿子等数人在女神维娜斯的帮助下成功地逃了出来。这驸马爷乃是女神维娜斯与凡人男子之间的儿子，女神维娜斯不忍心看着自己的儿子被希腊士兵屠杀 。</p><p>这阿驸马一行人分乘几条船，离开了火光冲天的特洛伊城。在女神维娜斯的指引下，浪迹地中海，最後在意大利西岸登陆。当地的国王看上了阿伊尼阿斯并把自己的女儿嫁给了他。他又是驸马了，与他的新妻过起了幸福的生活。难民们也安定了下来。</p><p>阿伊尼阿斯死後，跟随他逃难来的儿子继承了王位。新王在位三十年後，离开了这块地方，到台伯河(Tiber)下游建了一个新城亚尔巴龙迦城。这便是罗马城的前身了。</p><p>罗马人自古相信罗马城是公元前731年4月21日由罗莫路和勒莫(Romulus and Remus)建设的。而这两个孪生兄弟是从特洛伊逃出的阿伊尼阿斯的子孙。後来，罗马人接触了希腊文化後才知道特洛伊的陷落是在公元前十三世纪，老早的事了。罗马人好象并没有对这段空白有任何烦恼，随手编出一串传说，把那空白给填补了。反正传说这事荒唐一点的更受欢迎。经过了一堆搞不清谁是谁的王的统治，出现了一个什麽王的公主。</p><p>公主的叔父在篡夺了王位後，为了防止公主结婚生子威胁自己的王位，便任命未婚的公主为巫女。这是主管祭神的职位，象修女一样不得结婚。</p><p>不巧一日这美丽的公主在祭事的空余，来到小河边午睡。也是合当有事，被过往的战神玛尔斯(Mars)一见钟情。这玛尔斯本是靠挑起战争混饭吃的，但也常勾引 良家妇女。这天战神也没错过机会，立刻由天而降，与公主一试云雨。据说战神的技术特神，公主还没来得及醒便完事升天去了。後来公主生了一双胞胎，起名罗莫路和勒莫。</p><p>叔父闻知此事大怒，将公主投入大牢，又把那双胞胎放在篮子里抛入台伯河，指望那篮子漂入大海将那双胞胎淹死。类似的故事在旧约圣经里也有，那是关於摩西的事，好象这类传说在当地十分流行。</p><p>再说那兄弟俩的篮子被河口附近茂密的灌木丛钩住而停了下来，俩人哭声引来的一只过路的母狼。意大利的狼都带点慈悲心，不但没吃了俩人当点心，还用自己的奶去喂他们，这才救了俩小命。</p><p>不过，总是由狼养活也没法交&nbsp;待，於是又一日一放羊的在这地盘上溜哒，发现了兄弟俩，将他们抱了回去扶养成人 。据说现在这一带仍有许多放羊的。</p><p>兄弟俩长大後成了这一带放羊人的头，在与别的放羊人的圈子的打斗中不断地扩展自己的势力范围。圈子大了，情报也就多了，终于有一天，罗莫路和勒莫知道了自己身事。</p><p>兄弟俩就带着手下的放羊人呼啸着去打破了亚尔巴龙迦城，杀了那国王，将王位又交&nbsp;还给了自己祖父。他们的母亲似乎已经死在了大牢里。但兄弟俩也没在亚尔巴龙迦城多住，他们认为亚尔巴龙迦城位於山地，虽然易守难攻，却不利发展。加上兄弟俩是在台伯河的下游长大的，所以便回到原地，建了个新城。除了手下的放羊人又加上了附近的放羊人和农民。</p><p>消灭了共同的敌人後，兄弟俩的关系开始恶化。有人说是为了新城的命名，有人说是为了新城的城址，也有人说是为了争夺王位。兄弟俩於是分割统治，各占一小山包。但纷争又开始了，勒莫跳过了罗莫路为表示势力范围而挖的沟。对於这种侵犯他人权力的行为，罗莫路大义灭亲地在自己兄弟的後脑上重重地来了一锄头，勒莫便被灭了。</p><p></p><p>於是这城便以罗莫路的名字命名为罗马，这就是公元前731年4月21日的事了，到现在这天仍是意大利的节日，罗马人会欢天喜地的庆祝罗莫路杀了自己的…不，是庆祝罗马建城。王位当然也得由罗莫路来坐，一切问题都没了。这时四年一度的奥林匹克运动会在希腊已经开了六回，罗马也从传说的时代走出，近入了历史时代。</p><p><br></p>', N'标题', NULL, 0, 7, 0, 0, CAST(N'2018-06-26T17:28:32.563' AS DateTime), CAST(N'2018-06-26T17:28:32.513' AS DateTime), 0, NULL)

SET IDENTITY_INSERT [dbo].[TopicDetail] OFF
SET IDENTITY_INSERT [dbo].[UserRole] ON 

INSERT [dbo].[UserRole] ([Id], [IsDeleted], [UserId], [RoleId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (1, 0, 15, 1, NULL, NULL, CAST(N'2018-11-02T00:51:25.060' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[UserRole] ([Id], [IsDeleted], [UserId], [RoleId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (2, 0, 14, 2, NULL, NULL, CAST(N'2018-11-02T00:51:25.060' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[UserRole] ([Id], [IsDeleted], [UserId], [RoleId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (3, 0, 18, 3, NULL, NULL, CAST(N'2018-11-02T00:51:25.060' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[UserRole] ([Id], [IsDeleted], [UserId], [RoleId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (4, 0, 23, 4, 23, NULL, CAST(N'2019-02-18T23:53:26.423' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[UserRole] ([Id], [IsDeleted], [UserId], [RoleId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (5, 0, 1, 2, 1, NULL, CAST(N'2019-02-19T00:20:44.190' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[UserRole] ([Id], [IsDeleted], [UserId], [RoleId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (6, 0, 1, 1, 1, NULL, CAST(N'2019-02-19T00:20:54.087' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[UserRole] ([Id], [IsDeleted], [UserId], [RoleId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (8, 0, 13, 1, 13, NULL, CAST(N'2019-02-19T00:21:53.173' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[UserRole] ([Id], [IsDeleted], [UserId], [RoleId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (9, 0, 19, 6, 19, NULL, CAST(N'2019-02-25T00:38:05.700' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[UserRole] ([Id], [IsDeleted], [UserId], [RoleId], [CreateId], [CreateBy], [CreateTime], [ModifyId], [ModifyBy], [ModifyTime]) VALUES (10, 0, 24, 7, 24, NULL, CAST(N'2019-02-26T11:15:19.433' AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[UserRole] OFF
ALTER TABLE [dbo].[TopicDetail] ADD  DEFAULT ((0)) FOR [tdTop]
GO
ALTER TABLE [dbo].[Module]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Module_dbo.Module_ParentId] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Module] ([Id])
GO
ALTER TABLE [dbo].[Module] CHECK CONSTRAINT [FK_dbo.Module_dbo.Module_ParentId]
GO
ALTER TABLE [dbo].[ModulePermission]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ModulePermission_dbo.Module_ModuleId] FOREIGN KEY([ModuleId])
REFERENCES [dbo].[Module] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ModulePermission] CHECK CONSTRAINT [FK_dbo.ModulePermission_dbo.Module_ModuleId]
GO
ALTER TABLE [dbo].[ModulePermission]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ModulePermission_dbo.Permission_PermissionId] FOREIGN KEY([PermissionId])
REFERENCES [dbo].[Permission] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ModulePermission] CHECK CONSTRAINT [FK_dbo.ModulePermission_dbo.Permission_PermissionId]
GO
ALTER TABLE [dbo].[OperateLog]  WITH CHECK ADD  CONSTRAINT [FK_dbo.OperateLog_dbo.sysUserInfo_User_uID] FOREIGN KEY([User_uID])
REFERENCES [dbo].[sysUserInfo] ([uID])
GO
ALTER TABLE [dbo].[OperateLog] CHECK CONSTRAINT [FK_dbo.OperateLog_dbo.sysUserInfo_User_uID]
GO
ALTER TABLE [dbo].[TopicDetail]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TopicDetail_dbo.Topic_TopicId] FOREIGN KEY([TopicId])
REFERENCES [dbo].[Topic] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TopicDetail] CHECK CONSTRAINT [FK_dbo.TopicDetail_dbo.Topic_TopicId]
GO
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserRole_dbo.Role_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_dbo.UserRole_dbo.Role_RoleId]
GO
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserRole_dbo.sysUserInfo_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[sysUserInfo] ([uID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_dbo.UserRole_dbo.sysUserInfo_UserId]
GO
