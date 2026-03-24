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

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260206210532_CreacionInicial', N'8.0.23');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260206211455_AjusteModelo', N'8.0.23');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Visitas] (
    [Id] int NOT NULL IDENTITY,
    [Matricula] nvarchar(max) NOT NULL,
    [FechaVisita] datetime2 NOT NULL,
    [Edad] int NOT NULL,
    [Talla] float NULL,
    [Peso] float NULL,
    [TieneAlergias] bit NOT NULL,
    [EspecificarAlergia] nvarchar(max) NULL,
    [EnfermedadesCronicas] nvarchar(max) NULL,
    [FrecuenciaCardiaca] nvarchar(max) NULL,
    [FrecuenciaRespiratoria] nvarchar(max) NULL,
    [Saturacion] nvarchar(max) NULL,
    [Temperatura] float NULL,
    [PresionArterial] nvarchar(max) NULL,
    [Diagnostico] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Visitas] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260206212839_CorreccionFinal', N'8.0.23');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Preinscripciones] (
    [Id] int NOT NULL IDENTITY,
    [Matricula] nvarchar(max) NULL,
    [PreinscripcionId] int NOT NULL,
    [Folio] nvarchar(max) NULL,
    [CarreraSolicitada] nvarchar(max) NOT NULL,
    [Promedio] decimal(18,2) NOT NULL,
    [MedioDifusion] nvarchar(max) NULL,
    [FechaPreinscripcion] datetime2 NOT NULL,
    [EstadoPreinscripcion] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Preinscripciones] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [VisitasPsicologicas] (
    [Id] int NOT NULL IDENTITY,
    [Matricula] nvarchar(max) NOT NULL,
    [FechaVisita] datetime2 NOT NULL,
    [Edad] int NOT NULL,
    [TerapiaPrevia] bit NOT NULL,
    [MotivoConsultaPrevia] nvarchar(max) NULL,
    [MedicacionPsiquiatrica] nvarchar(max) NULL,
    [MotivoConsulta] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_VisitasPsicologicas] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [PreinscripcionDatosPersonales] (
    [Id] int NOT NULL IDENTITY,
    [PreinscripcionId] int NOT NULL,
    [Nombre] nvarchar(max) NOT NULL,
    [ApellidoPaterno] nvarchar(max) NOT NULL,
    [ApellidoMaterno] nvarchar(max) NULL,
    [CURP] nvarchar(max) NOT NULL,
    [FechaNacimiento] datetime2 NOT NULL,
    [Sexo] nvarchar(max) NOT NULL,
    [EstadoCivil] nvarchar(max) NULL,
    [Email] nvarchar(max) NOT NULL,
    [Telefono] nvarchar(max) NULL,
    CONSTRAINT [PK_PreinscripcionDatosPersonales] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PreinscripcionDatosPersonales_Preinscripciones_PreinscripcionId] FOREIGN KEY ([PreinscripcionId]) REFERENCES [Preinscripciones] ([Id]) ON DELETE CASCADE
);
GO

CREATE UNIQUE INDEX [IX_PreinscripcionDatosPersonales_PreinscripcionId] ON [PreinscripcionDatosPersonales] ([PreinscripcionId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260321020218_AgregaVisitasPsicologicas', N'8.0.23');
GO

COMMIT;
GO

