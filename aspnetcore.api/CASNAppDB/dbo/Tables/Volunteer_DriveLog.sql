﻿CREATE TABLE [dbo].[Volunteer_DriveLog]
(
    [Id] INT NOT NULL IDENTITY(1,1),
    [IsActive] BIT NOT NULL CONSTRAINT [DF_Volunteer_DriveLog_IsActive] DEFAULT (0x1),
    [Created] DATETIME NOT NULL CONSTRAINT [DF_Volunteer_DriveLog_Created] DEFAULT (GETUTCDATE()),
    [Updated] DATETIME NULL,
    CONSTRAINT [PK_Volunteer_DriveLog] PRIMARY KEY CLUSTERED
    ([Id] ASC) WITH (ALLOW_PAGE_LOCKS=ON,ALLOW_ROW_LOCKS=ON,PAD_INDEX=OFF,IGNORE_DUP_KEY=OFF)
) ON [PRIMARY]