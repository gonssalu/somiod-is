CREATE TABLE [dbo].[Resources] (
    [Id]           INT          NOT NULL,
    [Name]         VARCHAR (50) NOT NULL UNIQUE,
    [Content]      VARCHAR (50) NULL,
    [CreationDate] DATETIME     NOT NULL,
    [Parent]       INT          NULL,
    [Event]        VARCHAR (50) NULL,
    [Endpoint]     VARCHAR (50) NULL,
    [Type]         VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([Parent]) REFERENCES [dbo].[Resources] ([Id])
);

