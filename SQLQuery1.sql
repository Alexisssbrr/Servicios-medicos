-- 1. Creamos la tabla de Psicología
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

-- 2. "Engañamos" a Entity Framework para que sepa que ya terminamos
-- Reemplaza el ID de la migración por el último que aparece en tu script
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260321020218_AgregaVisitasPsicologicas', N'8.0.23');
GO