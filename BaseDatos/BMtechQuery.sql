use master;

CREATE DATABASE BMTech;

USE BMTech;
GO

CREATE TABLE Perfil
(
    idperfil VARCHAR(50) NOT NULL,
    CONSTRAINT PK_Perfil PRIMARY KEY (idperfil)
);
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