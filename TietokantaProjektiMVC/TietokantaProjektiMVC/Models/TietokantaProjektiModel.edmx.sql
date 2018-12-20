
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/14/2018 08:39:43
-- Generated from EDMX file: E:\VisualStudio 2017\TietokantaProjektiMVC\TietokantaProjektiMVC\TietokantaProjektiMVC\Models\TietokantaProjektiModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [TietokantaProjektiData];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Tunnit_Henkilot]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tunnit] DROP CONSTRAINT [FK_Tunnit_Henkilot];
GO
IF OBJECT_ID(N'[dbo].[FK_Tunnit_Projektit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tunnit] DROP CONSTRAINT [FK_Tunnit_Projektit];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Henkilot]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Henkilot];
GO
IF OBJECT_ID(N'[dbo].[Projektit]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Projektit];
GO
IF OBJECT_ID(N'[dbo].[Tunnit]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tunnit];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Henkilot'
CREATE TABLE [dbo].[Henkilot] (
    [HenkiloID] int IDENTITY(1,1) NOT NULL,
    [Etunimi] nvarchar(50)  NULL,
    [Sukunimi] nvarchar(50)  NULL,
    [Osoite] nvarchar(100)  NULL,
    [Esimies] int  NULL,
    [Postinumero] varchar(5)  NULL
);
GO

-- Creating table 'Projektit'
CREATE TABLE [dbo].[Projektit] (
    [ProjektiID] int IDENTITY(1,1) NOT NULL,
    [ProjektiNimi] nvarchar(100)  NULL,
    [Esimies] int  NULL,
    [Avattu] datetime  NULL,
    [Suljettu] datetime  NULL,
    [Status] nvarchar(10)  NULL
);
GO

-- Creating table 'Tunnit'
CREATE TABLE [dbo].[Tunnit] (
    [TuntiID] int IDENTITY(1,1) NOT NULL,
    [ProjektiID] int  NULL,
    [HenkiloID] int  NULL,
    [Pvm] datetime  NULL,
    [ProjektiTunti] int  NULL,
    [ProjektiTunnit] decimal(15,7)  NULL,
    [SuunnitellutTunnit] decimal(15,7)  NULL,
    [ToteutuneetTunnit] decimal(15,7)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [HenkiloID] in table 'Henkilot'
ALTER TABLE [dbo].[Henkilot]
ADD CONSTRAINT [PK_Henkilot]
    PRIMARY KEY CLUSTERED ([HenkiloID] ASC);
GO

-- Creating primary key on [ProjektiID] in table 'Projektit'
ALTER TABLE [dbo].[Projektit]
ADD CONSTRAINT [PK_Projektit]
    PRIMARY KEY CLUSTERED ([ProjektiID] ASC);
GO

-- Creating primary key on [TuntiID] in table 'Tunnit'
ALTER TABLE [dbo].[Tunnit]
ADD CONSTRAINT [PK_Tunnit]
    PRIMARY KEY CLUSTERED ([TuntiID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [HenkiloID] in table 'Tunnit'
ALTER TABLE [dbo].[Tunnit]
ADD CONSTRAINT [FK_Tunnit_Henkilot]
    FOREIGN KEY ([HenkiloID])
    REFERENCES [dbo].[Henkilot]
        ([HenkiloID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Tunnit_Henkilot'
CREATE INDEX [IX_FK_Tunnit_Henkilot]
ON [dbo].[Tunnit]
    ([HenkiloID]);
GO

-- Creating foreign key on [ProjektiID] in table 'Tunnit'
ALTER TABLE [dbo].[Tunnit]
ADD CONSTRAINT [FK_Tunnit_Projektit]
    FOREIGN KEY ([ProjektiID])
    REFERENCES [dbo].[Projektit]
        ([ProjektiID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Tunnit_Projektit'
CREATE INDEX [IX_FK_Tunnit_Projektit]
ON [dbo].[Tunnit]
    ([ProjektiID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------