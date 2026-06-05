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
CREATE TABLE [Venues] (
    [VenueId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Location] nvarchar(max) NOT NULL,
    [Capacity] int NOT NULL,
    [ImageUrl] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Venues] PRIMARY KEY ([VenueId])
);

CREATE TABLE [Events] (
    [EventId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Date] datetime2 NOT NULL,
    [VenueId] int NOT NULL,
    [ImageUrl] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Events] PRIMARY KEY ([EventId]),
    CONSTRAINT [FK_Events_Venues_VenueId] FOREIGN KEY ([VenueId]) REFERENCES [Venues] ([VenueId])
);

CREATE TABLE [Bookings] (
    [BookingId] int NOT NULL IDENTITY,
    [EventId] int NOT NULL,
    [VenueId] int NOT NULL,
    [CustomerName] nvarchar(max) NOT NULL,
    [BookingDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Bookings] PRIMARY KEY ([BookingId]),
    CONSTRAINT [FK_Bookings_Events_EventId] FOREIGN KEY ([EventId]) REFERENCES [Events] ([EventId]),
    CONSTRAINT [FK_Bookings_Venues_VenueId] FOREIGN KEY ([VenueId]) REFERENCES [Venues] ([VenueId])
);

CREATE INDEX [IX_Bookings_EventId] ON [Bookings] ([EventId]);

CREATE INDEX [IX_Bookings_VenueId] ON [Bookings] ([VenueId]);

CREATE INDEX [IX_Events_VenueId] ON [Events] ([VenueId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260315200131_Eventtime8', N'9.0.0');

ALTER TABLE [Bookings] ADD [ImageUrl] nvarchar(max) NULL;

ALTER TABLE [Bookings] ADD [SeatsBooked] int NOT NULL DEFAULT 0;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260317204753_AddBookingFields', N'9.0.0');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260317220219_removeImaggeUrlFromBooking', N'9.0.0');

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Bookings]') AND [c].[name] = N'ImageUrl');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Bookings] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Bookings] DROP COLUMN [ImageUrl];

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260411123152_CreateEventMigration', N'9.0.0');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260503180451_NewDataBase', N'9.0.0');

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Venues]') AND [c].[name] = N'ImageUrl');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Venues] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Venues] ALTER COLUMN [ImageUrl] nvarchar(max) NULL;

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Events]') AND [c].[name] = N'ImageUrl');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Events] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Events] ALTER COLUMN [ImageUrl] nvarchar(max) NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260507194313_NewDataBase3', N'9.0.0');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260507203810_AddVenueCapacity', N'9.0.0');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260512013327_SearchFeature', N'9.0.0');

ALTER TABLE [Events] ADD [EventTypeId] int NOT NULL DEFAULT 0;

CREATE TABLE [EventTypes] (
    [EventTypeId] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Description] nvarchar(max) NULL,
    CONSTRAINT [PK_EventTypes] PRIMARY KEY ([EventTypeId])
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'EventTypeId', N'Description', N'Name') AND [object_id] = OBJECT_ID(N'[EventTypes]'))
    SET IDENTITY_INSERT [EventTypes] ON;
INSERT INTO [EventTypes] ([EventTypeId], [Description], [Name])
VALUES (1, NULL, N'Concert'),
(2, NULL, N'Workshop'),
(3, NULL, N'Conference'),
(4, NULL, N'Seminar');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'EventTypeId', N'Description', N'Name') AND [object_id] = OBJECT_ID(N'[EventTypes]'))
    SET IDENTITY_INSERT [EventTypes] OFF;

CREATE INDEX [IX_Events_EventTypeId] ON [Events] ([EventTypeId]);

ALTER TABLE [Events] ADD CONSTRAINT [FK_Events_EventTypes_EventTypeId] FOREIGN KEY ([EventTypeId]) REFERENCES [EventTypes] ([EventTypeId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260512015318_AddEventTypeTable', N'9.0.0');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260520230820_SeedEventTypes', N'9.0.0');

COMMIT;
GO

