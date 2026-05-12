using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Check if AspNetRoles table exists before creating Identity tables
            // This handles the case where the database already has Identity tables
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetRoles')
                BEGIN
                    CREATE TABLE [AspNetRoles] (
                        [Id] nvarchar(450) NOT NULL,
                        [Description] nvarchar(250) NULL,
                        [Name] nvarchar(256) NULL,
                        [NormalizedName] nvarchar(256) NULL,
                        [ConcurrencyStamp] nvarchar(max) NULL,
                        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
                    );
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetUsers')
                BEGIN
                    CREATE TABLE [AspNetUsers] (
                        [Id] nvarchar(450) NOT NULL,
                        [FullName] nvarchar(250) NULL,
                        [JobTitle] nvarchar(250) NULL,
                        [UserName] nvarchar(256) NULL,
                        [NormalizedUserName] nvarchar(256) NULL,
                        [Email] nvarchar(256) NULL,
                        [NormalizedEmail] nvarchar(256) NULL,
                        [EmailConfirmed] bit NOT NULL,
                        [PasswordHash] nvarchar(max) NULL,
                        [SecurityStamp] nvarchar(max) NULL,
                        [ConcurrencyStamp] nvarchar(max) NULL,
                        [PhoneNumber] nvarchar(max) NULL,
                        [PhoneNumberConfirmed] bit NOT NULL,
                        [TwoFactorEnabled] bit NOT NULL,
                        [LockoutEnd] datetimeoffset NULL,
                        [LockoutEnabled] bit NOT NULL,
                        [AccessFailedCount] int NOT NULL,
                        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
                    );
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AssetItems')
                BEGIN
                    CREATE TABLE [AssetItems] (
                        [Id] int NOT NULL IDENTITY(1, 1),
                        [Title] nvarchar(150) NULL,
                        [AssetId] nvarchar(max) NULL,
                        [Description] nvarchar(250) NULL,
                        [Type] int NOT NULL,
                        [Location] nvarchar(250) NULL,
                        [Priority] int NOT NULL,
                        CONSTRAINT [PK_AssetItems] PRIMARY KEY ([Id])
                    );
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ToDoItems')
                BEGIN
                    CREATE TABLE [ToDoItems] (
                        [Id] int NOT NULL IDENTITY(1, 1),
                        [Title] nvarchar(max) NOT NULL,
                        [Description] nvarchar(max) NULL,
                        [IsDone] bit NOT NULL,
                        CONSTRAINT [PK_ToDoItems] PRIMARY KEY ([Id])
                    );
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetRoleClaims')
                BEGIN
                    CREATE TABLE [AspNetRoleClaims] (
                        [Id] int NOT NULL IDENTITY(1, 1),
                        [RoleId] nvarchar(450) NOT NULL,
                        [ClaimType] nvarchar(max) NULL,
                        [ClaimValue] nvarchar(max) NULL,
                        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
                        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
                    );
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetUserClaims')
                BEGIN
                    CREATE TABLE [AspNetUserClaims] (
                        [Id] int NOT NULL IDENTITY(1, 1),
                        [UserId] nvarchar(450) NOT NULL,
                        [ClaimType] nvarchar(max) NULL,
                        [ClaimValue] nvarchar(max) NULL,
                        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
                        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
                    );
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetUserLogins')
                BEGIN
                    CREATE TABLE [AspNetUserLogins] (
                        [LoginProvider] nvarchar(450) NOT NULL,
                        [ProviderKey] nvarchar(450) NOT NULL,
                        [ProviderDisplayName] nvarchar(max) NULL,
                        [UserId] nvarchar(450) NOT NULL,
                        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
                        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
                    );
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetUserRoles')
                BEGIN
                    CREATE TABLE [AspNetUserRoles] (
                        [UserId] nvarchar(450) NOT NULL,
                        [RoleId] nvarchar(450) NOT NULL,
                        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
                        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
                        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
                    );
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetUserTokens')
                BEGIN
                    CREATE TABLE [AspNetUserTokens] (
                        [UserId] nvarchar(450) NOT NULL,
                        [LoginProvider] nvarchar(450) NOT NULL,
                        [Name] nvarchar(450) NOT NULL,
                        [Value] nvarchar(max) NULL,
                        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
                        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
                    );
                END
            ");

            // Create indexes if they don't exist
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_AspNetRoleClaims_RoleId' AND object_id = OBJECT_ID('AspNetRoleClaims'))
                BEGIN
                    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'RoleNameIndex' AND object_id = OBJECT_ID('AspNetRoles'))
                BEGIN
                    CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_AspNetUserClaims_UserId' AND object_id = OBJECT_ID('AspNetUserClaims'))
                BEGIN
                    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_AspNetUserLogins_UserId' AND object_id = OBJECT_ID('AspNetUserLogins'))
                BEGIN
                    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_AspNetUserRoles_RoleId' AND object_id = OBJECT_ID('AspNetUserRoles'))
                BEGIN
                    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'EmailIndex' AND object_id = OBJECT_ID('AspNetUsers'))
                BEGIN
                    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'UserNameIndex' AND object_id = OBJECT_ID('AspNetUsers'))
                BEGIN
                    CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AssetItems");

            migrationBuilder.DropTable(
                name: "ToDoItems");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
