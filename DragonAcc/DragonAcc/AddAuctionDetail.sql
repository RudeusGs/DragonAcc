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

CREATE TABLE [AuctionDetails] (
    [Id] int NOT NULL IDENTITY, 
    [AuctionId] int NULL,
    [UserID] int NULL,
    [RaisePrice] int NULL,
    [CreatedDate] datetime2 NOT NULL,   
    CONSTRAINT [PK_AuctionDetails] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241116142355_AddAuctionDetailOnly', N'8.0.10');
GO

COMMIT;
GO
