﻿CREATE TABLE [dbo].[Application] (
    [Id]           INT          IDENTITY(1,1) NOT NULL,
    [Name]         VARCHAR (50) NOT NULL UNIQUE,
    [CreationDate] DATETIME     NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Module] (
    [Id]           INT          IDENTITY(1,1) NOT NULL,
    [Name]         VARCHAR (50) NOT NULL UNIQUE,
    [CreationDate] DATETIME     NOT NULL,
    [Parent]       INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([Parent]) REFERENCES [dbo].[Application] ([Id]),
	CONSTRAINT uq_module UNIQUE([Parent], [Name])
);

CREATE TABLE [dbo].[Subscription] (
    [Id]           INT          IDENTITY(1,1) NOT NULL,
    [Name]         VARCHAR (50) NOT NULL,
    [CreationDate] DATETIME     NOT NULL,
    [Parent]       INT          NOT NULL,
    [Event]        VARCHAR (50) NOT NULL,
    [Endpoint]     VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([Parent]) REFERENCES [dbo].[Module] ([Id]),
	CONSTRAINT uq_subscription UNIQUE([Parent], [Name])
);

CREATE TABLE [dbo].[Data] (
    [Id]           INT          IDENTITY(1,1) NOT NULL,
    [Content]      VARCHAR (50) NOT NULL,
    [CreationDate] DATETIME     NOT NULL,
    [Parent]       INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([Parent]) REFERENCES [dbo].[Module] ([Id])
);