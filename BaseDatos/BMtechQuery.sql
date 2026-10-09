use master;

CREATE DATABASE BMTech;
GO

USE BMTech;
GO

CREATE TABLE Perfil
(
    idperfil VARCHAR(50) NOT NULL,
    CONSTRAINT PK_Perfil PRIMARY KEY (idperfil)
);
GO
CREATE TABLE Patente
(
    idPatente VARCHAR(50) NOT NULL,
    nombre VARCHAR(100) NOT NULL,

    CONSTRAINT PK_Patente PRIMARY KEY (idPatente)
);
GO

CREATE TABLE Familia
(
    idFamilia VARCHAR(50) NOT NULL,
    nombre VARCHAR(100) NOT NULL,

    CONSTRAINT PK_Familia PRIMARY KEY (idFamilia)
);
GO

CREATE TABLE FamiliaPatente
(
    idFamilia VARCHAR(50) NOT NULL,
    idPatente VARCHAR(50) NOT NULL,

    CONSTRAINT PK_FamiliaPatente PRIMARY KEY (idFamilia, idPatente),
    CONSTRAINT FK_FamiliaPatente_Familia
        FOREIGN KEY (idFamilia)
        REFERENCES Familia(idFamilia),
    CONSTRAINT FK_FamiliaPatente_Patente
        FOREIGN KEY (idPatente)
        REFERENCES Patente(idPatente)
);
GO

CREATE TABLE PerfilFamilia
(
    idperfil VARCHAR(50) NOT NULL,
    idFamilia VARCHAR(50) NOT NULL,

    CONSTRAINT PK_PerfilFamilia PRIMARY KEY (idperfil, idFamilia),
    CONSTRAINT FK_PerfilFamilia_Perfil
        FOREIGN KEY (idperfil)
        REFERENCES Perfil(idperfil),
    CONSTRAINT FK_PerfilFamilia_Familia
        FOREIGN KEY (idFamilia)
        REFERENCES Familia(idFamilia)
);
GO

INSERT INTO Perfil (idperfil) VALUES ('Administrador');
INSERT INTO Perfil (idperfil) VALUES ('Vendedor');
GO

INSERT INTO Patente (idPatente, nombre) VALUES ('CLIENTES', 'Gestión de clientes');
INSERT INTO Patente (idPatente, nombre) VALUES ('PRODUCTOS', 'Gestión de productos');
INSERT INTO Patente (idPatente, nombre) VALUES ('VENTAS', 'Registro de ventas');
INSERT INTO Patente (idPatente, nombre) VALUES ('SEGURIDAD', 'Seguridad');
GO

INSERT INTO Familia (idFamilia, nombre) VALUES ('ADMINISTRACION', 'Administración del sistema');
INSERT INTO Familia (idFamilia, nombre) VALUES ('VENTA', 'Operaciones de venta');
GO

INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES ('ADMINISTRACION', 'CLIENTES');
INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES ('ADMINISTRACION', 'PRODUCTOS');
INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES ('ADMINISTRACION', 'VENTAS');
INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES ('ADMINISTRACION', 'SEGURIDAD');
INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES ('VENTA', 'CLIENTES');
INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES ('VENTA', 'PRODUCTOS');
INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES ('VENTA', 'VENTAS');
GO

INSERT INTO PerfilFamilia (idperfil, idFamilia) VALUES ('Administrador', 'ADMINISTRACION');
INSERT INTO PerfilFamilia (idperfil, idFamilia) VALUES ('Vendedor', 'VENTA');
GO
CREATE TABLE Usuario
(
    email VARCHAR(150) NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    password VARCHAR(100) NOT NULL,
    activo BIT NOT NULL DEFAULT 1,
    intentos INT NOT NULL DEFAULT 3,
    idperfil VARCHAR(50) NOT NULL,

    CONSTRAINT PK_Usuario PRIMARY KEY (email),
    CONSTRAINT FK_Usuario_Perfil
        FOREIGN KEY (idperfil)
        REFERENCES Perfil(idperfil)
);
GO

CREATE TABLE LogEventos
(
    idLog INT IDENTITY(1,1) NOT NULL,
    email VARCHAR(150) NOT NULL,
    fecha DATETIME NOT NULL DEFAULT GETDATE(),
    accion VARCHAR(255) NOT NULL,
    modulo VARCHAR(100) NOT NULL,
    criticidad INT NOT NULL,

    CONSTRAINT PK_LogEventos PRIMARY KEY (idLog)
);
GO


CREATE TABLE DigitoVerificador
(
    tabla VARCHAR(50) NOT NULL,
    digitoVertical INT NOT NULL,

    CONSTRAINT PK_DigitoVerificador PRIMARY KEY (tabla)
);
GO
CREATE TABLE Cliente
(
    dni VARCHAR(20) NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    telefono VARCHAR(50) NOT NULL,
    correoElectronico VARCHAR(150) NOT NULL,
    digitoVerificador INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_Cliente PRIMARY KEY (dni)
);
GO

CREATE TABLE Producto
(
    idProducto INT IDENTITY(1,1) NOT NULL,
    codigo VARCHAR(50) NOT NULL,
    descripcion VARCHAR(150) NOT NULL,
    marca VARCHAR(100) NOT NULL,
    modelo VARCHAR(100) NOT NULL,
    precio DECIMAL(18,2) NOT NULL,
    stock INT NOT NULL,
    activo BIT NOT NULL DEFAULT 1,
    digitoVerificador INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_Producto PRIMARY KEY (idProducto),
    CONSTRAINT UQ_Producto_Codigo UNIQUE (codigo)
);
GO

CREATE TABLE Venta
(
    idVenta INT IDENTITY(1,1) NOT NULL,
    dniCliente VARCHAR(20) NOT NULL,
    emailUsuario VARCHAR(150) NULL,
    fecha DATETIME NOT NULL DEFAULT GETDATE(),
    total DECIMAL(18,2) NOT NULL,

    CONSTRAINT PK_Venta PRIMARY KEY (idVenta),
    CONSTRAINT FK_Venta_Cliente
        FOREIGN KEY (dniCliente)
        REFERENCES Cliente(dni),
    CONSTRAINT FK_Venta_Usuario
        FOREIGN KEY (emailUsuario)
        REFERENCES Usuario(email)
);
GO

CREATE TABLE DetalleVenta
(
    idDetalleVenta INT IDENTITY(1,1) NOT NULL,
    idVenta INT NOT NULL,
    idProducto INT NOT NULL,
    cantidad INT NOT NULL,
    precioUnitario DECIMAL(18,2) NOT NULL,
    subtotal DECIMAL(18,2) NOT NULL,

    CONSTRAINT PK_DetalleVenta PRIMARY KEY (idDetalleVenta),
    CONSTRAINT FK_DetalleVenta_Venta
        FOREIGN KEY (idVenta)
        REFERENCES Venta(idVenta),
    CONSTRAINT FK_DetalleVenta_Producto
        FOREIGN KEY (idProducto)
        REFERENCES Producto(idProducto)
);
GO

CREATE TABLE HistorialCambiosProducto
(
    idHistorial INT IDENTITY(1,1) NOT NULL,
    idProducto INT NOT NULL,
    codigo VARCHAR(50) NOT NULL,
    descripcion VARCHAR(150) NOT NULL,
    marca VARCHAR(100) NOT NULL,
    modelo VARCHAR(100) NOT NULL,
    precio DECIMAL(18,2) NOT NULL,
    stock INT NOT NULL,
    activo BIT NOT NULL,
    digitoVerificador INT NOT NULL,
    usuario VARCHAR(150) NOT NULL DEFAULT SYSTEM_USER,
    fecha DATETIME NOT NULL DEFAULT GETDATE(),
    accion VARCHAR(20) NOT NULL,

    CONSTRAINT PK_HistorialCambiosProducto PRIMARY KEY (idHistorial)
);
GO

CREATE TRIGGER TR_Producto_HistorialCambios
ON Producto
AFTER UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO HistorialCambiosProducto
    (
        idProducto,
        codigo,
        descripcion,
        marca,
        modelo,
        precio,
        stock,
        activo,
        digitoVerificador,
        usuario,
        fecha,
        accion
    )
    SELECT
        d.idProducto,
        d.codigo,
        d.descripcion,
        d.marca,
        d.modelo,
        d.precio,
        d.stock,
        d.activo,
        d.digitoVerificador,
        SYSTEM_USER,
        GETDATE(),
        CASE
            WHEN EXISTS (SELECT 1 FROM inserted) THEN 'MODIFICACION'
            ELSE 'BAJA'
        END
    FROM deleted d;
END
GO

INSERT INTO Usuario
(
    email,
    nombre,
    apellido,
    password,
    activo,
    intentos,
    idperfil
)
VALUES
(
    'admin@bmtech.com',
    'Usuario',
    'Administrador',
    '$2a$12$J2MsQcFQ66m5S2Gq4tE65OrkTpWQ5MUb2yzvYrBqR42npqmoWmZUW',
    1,
    3,
    'Administrador'
);
SELECT * FROM Perfil;

INSERT INTO Usuario
(
    email,
    nombre,
    apellido,
    password,
    activo,
    intentos,
    idperfil
)
VALUES
(
    'vendedor@bmtech.com',
    'Usuario',
    'Vendedor',
    '$2a$12$J2MsQcFQ66m5S2Gq4tE65OrkTpWQ5MUb2yzvYrBqR42npqmoWmZUW',
    1,
    3,
    'Vendedor'
);

SELECT email, nombre, apellido, activo, intentos, idperfil
FROM Usuario;

