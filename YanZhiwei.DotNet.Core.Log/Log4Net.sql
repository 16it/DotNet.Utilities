
CREATE TABLE [dbo].[Log4net]
    (
      [Id] [INT] IDENTITY(1, 1)
                 NOT NULL ,
      [Level] [NVARCHAR](50) NULL ,
      [Logger] [NVARCHAR](255) NULL ,
      [Host] [NVARCHAR](50) NULL ,
      [Date] [DATETIME] NULL ,
      [Thread] [NVARCHAR](255) NULL ,
      [Message] [NVARCHAR](MAX) NULL ,
      [Exception] [NVARCHAR](MAX) NULL ,
      CONSTRAINT [PK_Log4net] PRIMARY KEY CLUSTERED ( [Id] ASC )
        WITH ( PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,
               IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON,
               ALLOW_PAGE_LOCKS = ON ) ON [PRIMARY]
    )
ON  [PRIMARY] TEXTIMAGE_ON [PRIMARY];

GO


