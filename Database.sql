CREATE DATABASE PDVnetControleCaixa;
GO

USE PDVnetControleCaixa;
GO

CREATE TABLE MovimentacaoCaixa (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Descricao NVARCHAR(100) NOT NULL,
    Tipo INT NOT NULL,
    Categoria NVARCHAR(MAX) NULL,
    Valor DECIMAL(18,2) NOT NULL,
    DataMovimento DATETIME2 NOT NULL,
    Status INT NOT NULL
);
GO

CREATE TABLE ConfiguracoesCaixa (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    SaldoInicial DECIMAL(18,2) NOT NULL,
    SaldoMinimo DECIMAL(18,2) NOT NULL DEFAULT 100,
    DataAtualizacao DATETIME2 NOT NULL
);
GO
