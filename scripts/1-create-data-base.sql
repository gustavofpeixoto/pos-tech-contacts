IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Contacts] (
    [Id] uniqueidentifier NOT NULL,
    [Name] varchar(200) NULL,
    [Surname] varchar(500) NULL,
    [Email] varchar(500) NULL,
    [Phone] varchar(25) NULL,
    [Ddd] int NOT NULL,
    CONSTRAINT [PK_Contacts] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250113132447_Initial', N'8.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Contacts]') AND [c].[name] = N'Ddd');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Contacts] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Contacts] DROP COLUMN [Ddd];
GO

ALTER TABLE [Contacts] ADD [DddId] uniqueidentifier NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
GO

CREATE TABLE [Regions] (
    [Id] uniqueidentifier NOT NULL,
    [RegionName] varchar(500) NULL,
    CONSTRAINT [PK_Regions] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Ddds] (
    [Id] uniqueidentifier NOT NULL,
    [DddCode] int NOT NULL,
    [RegionId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Ddds] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Ddds_Regions_RegionId] FOREIGN KEY ([RegionId]) REFERENCES [Regions] ([Id]) ON DELETE NO ACTION
);
GO

CREATE INDEX [IX_Contacts_DddId] ON [Contacts] ([DddId]);
GO

CREATE INDEX [IX_Ddds_RegionId] ON [Ddds] ([RegionId]);
GO

ALTER TABLE [Contacts] ADD CONSTRAINT [FK_Contacts_Ddds_DddId] FOREIGN KEY ([DddId]) REFERENCES [Ddds] ([Id]) ON DELETE NO ACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250115232116_ChangeRerionTableNameToRegions', N'8.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Ddds] ADD [State] varchar(10) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250115235912_AddColumnStateToDdds', N'8.0.11');
GO

COMMIT;
GO

