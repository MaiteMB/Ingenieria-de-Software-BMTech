IF DB_ID('BMTech') IS NULL
    CREATE DATABASE BMTech;
GO
USE [BMTech];
GO

-- Idiomas
CREATE TABLE Idioma
(
    codigo VARCHAR(2) NOT NULL,
    nombre NVARCHAR(50) NOT NULL,
    digitoVerificador INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_Idioma PRIMARY KEY (codigo)
);
GO
CREATE TABLE Etiqueta
(
    etiqueta NVARCHAR(200) NOT NULL,
    digitoVerificador INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_Etiqueta PRIMARY KEY (etiqueta)
);
GO
CREATE TABLE Traduccion
(
    codigoIdioma VARCHAR(2) NOT NULL,
    etiqueta NVARCHAR(200) NOT NULL,
    textoTraducido NVARCHAR(250) NOT NULL,
    digitoVerificador INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_Traduccion PRIMARY KEY (codigoIdioma, etiqueta),
    CONSTRAINT FK_Traduccion_Idioma
        FOREIGN KEY (codigoIdioma)
        REFERENCES Idioma(codigo),
    CONSTRAINT FK_Traduccion_Etiqueta
        FOREIGN KEY (etiqueta)
        REFERENCES Etiqueta(etiqueta)
);
GO

-- Seguridad y permisos
CREATE TABLE Perfil
(
    idperfil VARCHAR(50) NOT NULL,
    digitoVerificador INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_Perfil PRIMARY KEY (idperfil)
);
GO
CREATE TABLE Patente
(
    idPatente VARCHAR(50) NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    digitoVerificador INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_Patente PRIMARY KEY (idPatente)
);
GO
CREATE TABLE Familia
(
    idFamilia VARCHAR(50) NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    digitoVerificador INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_Familia PRIMARY KEY (idFamilia)
);
GO
CREATE TABLE FamiliaPatente
(
    idFamilia VARCHAR(50) NOT NULL,
    idPatente VARCHAR(50) NOT NULL,
    digitoVerificador INT NOT NULL DEFAULT 0,

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
    digitoVerificador INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_PerfilFamilia PRIMARY KEY (idperfil, idFamilia),
    CONSTRAINT FK_PerfilFamilia_Perfil
        FOREIGN KEY (idperfil)
        REFERENCES Perfil(idperfil),
    CONSTRAINT FK_PerfilFamilia_Familia
        FOREIGN KEY (idFamilia)
        REFERENCES Familia(idFamilia)
);
GO
CREATE TABLE PerfilPerfil
(
    idPerfilPadre VARCHAR(50) NOT NULL,
    idPerfilHijo VARCHAR(50) NOT NULL,
    digitoVerificador INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_PerfilPerfil PRIMARY KEY (idPerfilPadre, idPerfilHijo),
    CONSTRAINT FK_PerfilPerfil_Perfil
        FOREIGN KEY (idPerfilPadre)
        REFERENCES Perfil(idperfil),
    CONSTRAINT FK_PerfilPerfil_Perfil2
        FOREIGN KEY (idPerfilHijo)
        REFERENCES Perfil(idperfil),
    CONSTRAINT CK_PerfilPerfil_Datos CHECK (idPerfilPadre <> idPerfilHijo)
);
GO
CREATE TABLE PerfilPatente
(
    idperfil VARCHAR(50) NOT NULL,
    idPatente VARCHAR(50) NOT NULL,
    digitoVerificador INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_PerfilPatente PRIMARY KEY (idperfil, idPatente),
    CONSTRAINT FK_PerfilPatente_Perfil
        FOREIGN KEY (idperfil)
        REFERENCES Perfil(idperfil),
    CONSTRAINT FK_PerfilPatente_Patente
        FOREIGN KEY (idPatente)
        REFERENCES Patente(idPatente)
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
    digitoVerificador INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_Usuario PRIMARY KEY (email),
    CONSTRAINT FK_Usuario_Perfil
        FOREIGN KEY (idperfil)
        REFERENCES Perfil(idperfil),
    CONSTRAINT CK_Usuario_Intentos CHECK (intentos >= 0 AND intentos <= 3)
);
GO

-- Bitacora, integridad y copias
CREATE TABLE LogEventos
(
    idLog INT IDENTITY(1,1) NOT NULL,
    email VARCHAR(150) NOT NULL,
    fecha DATETIME NOT NULL DEFAULT GETDATE(),
    accion VARCHAR(255) NOT NULL,
    modulo VARCHAR(100) NOT NULL,
    criticidad INT NOT NULL,
    digitoVerificador INT NOT NULL DEFAULT 0,

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
CREATE TABLE [Backup]
(
    idBackup INT IDENTITY(1,1) NOT NULL,
    fecha DATETIME NOT NULL,
    usuario VARCHAR(150) NOT NULL,
    ruta NVARCHAR(500) NOT NULL,
    digitoVerificador INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_Backup PRIMARY KEY (idBackup),
    CONSTRAINT UQ_Backup_Ruta UNIQUE (ruta)
);
GO

-- Ventas
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
    CONSTRAINT UQ_Producto_Codigo UNIQUE (codigo),
    CONSTRAINT CK_Producto_Precio CHECK (precio > 0),
    CONSTRAINT CK_Producto_Stock CHECK (stock >= 0)
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
    usuario VARCHAR(150) NOT NULL DEFAULT SUSER_SNAME(),
    fecha DATETIME NOT NULL DEFAULT GETDATE(),
    accion VARCHAR(20) NOT NULL,

    CONSTRAINT PK_HistorialCambiosProducto PRIMARY KEY (idHistorial)
);
GO
CREATE TABLE Venta
(
    idVenta INT IDENTITY(1,1) NOT NULL,
    dniCliente VARCHAR(20) NOT NULL,
    emailUsuario VARCHAR(150) NULL,
    fecha DATETIME NOT NULL DEFAULT GETDATE(),
    total DECIMAL(18,2) NOT NULL,
    digitoVerificador INT NOT NULL DEFAULT 0,
    estado VARCHAR(20) NOT NULL DEFAULT 'PAGADA',
    fechaEntrega DATETIME NULL,

    CONSTRAINT PK_Venta PRIMARY KEY (idVenta),
    CONSTRAINT FK_Venta_Cliente
        FOREIGN KEY (dniCliente)
        REFERENCES Cliente(dni),
    CONSTRAINT FK_Venta_Usuario
        FOREIGN KEY (emailUsuario)
        REFERENCES Usuario(email),
    CONSTRAINT CK_Venta_Estado CHECK (estado IN ('PENDIENTE', 'PAGADA', 'FINALIZADA', 'CANCELADA')),
    CONSTRAINT CK_Venta_Total CHECK (total > 0)
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
    digitoVerificador INT NOT NULL DEFAULT 0,
    codigoProducto VARCHAR(50) NOT NULL,
    descripcionProducto VARCHAR(150) NOT NULL,

    CONSTRAINT PK_DetalleVenta PRIMARY KEY (idDetalleVenta),
    CONSTRAINT FK_DetalleVenta_Venta
        FOREIGN KEY (idVenta)
        REFERENCES Venta(idVenta),
    CONSTRAINT FK_DetalleVenta_Producto
        FOREIGN KEY (idProducto)
        REFERENCES Producto(idProducto),
    CONSTRAINT CK_DetalleVenta_Cantidad CHECK (cantidad > 0),
    CONSTRAINT CK_DetalleVenta_Importes CHECK (precioUnitario > 0 AND subtotal = precioUnitario * cantidad)
);
GO
CREATE TABLE Pago
(
    idPago INT IDENTITY(1,1) NOT NULL,
    idVenta INT NOT NULL,
    medioPago VARCHAR(20) NOT NULL,
    importe DECIMAL(18,2) NOT NULL,
    numeroOperacion NVARCHAR(500) NOT NULL,
    fecha DATETIME NOT NULL,
    verificado BIT NOT NULL,
    digitoVerificador INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_Pago PRIMARY KEY (idPago),
    CONSTRAINT FK_Pago_Venta
        FOREIGN KEY (idVenta)
        REFERENCES Venta(idVenta),
    CONSTRAINT CK_Pago_Medio CHECK (medioPago IN ('Efectivo', 'Transferencia')),
    CONSTRAINT CK_Pago_Importe CHECK (importe >= 0)
);
GO
CREATE TABLE Comprobante
(
    idComprobante INT IDENTITY(1,1) NOT NULL,
    idVenta INT NOT NULL,
    fecha DATETIME NOT NULL,
    cliente VARCHAR(201) NOT NULL,
    dniCliente VARCHAR(20) NOT NULL,
    vendedor VARCHAR(201) NOT NULL,
    total DECIMAL(18,2) NOT NULL,
    digitoVerificador INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_Comprobante PRIMARY KEY (idComprobante),
    CONSTRAINT UQ_Comprobante_Venta UNIQUE (idVenta),
    CONSTRAINT FK_Comprobante_Venta
        FOREIGN KEY (idVenta)
        REFERENCES Venta(idVenta)
);
GO

-- Servicio tecnico
CREATE TABLE Equipo
(
    idEquipo INT IDENTITY(1,1) NOT NULL,
    dniCliente VARCHAR(20) NOT NULL,
    tipo VARCHAR(60) NOT NULL,
    marca VARCHAR(100) NOT NULL,
    modelo VARCHAR(100) NOT NULL,
    numeroSerie VARCHAR(100) NOT NULL,
    digitoVerificador INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_Equipo PRIMARY KEY (idEquipo),
    CONSTRAINT FK_Equipo_Cliente
        FOREIGN KEY (dniCliente)
        REFERENCES Cliente(dni)
);
GO
CREATE TABLE OrdenServicio
(
    idOrden INT IDENTITY(1,1) NOT NULL,
    idEquipo INT NOT NULL,
    fechaIngreso DATETIME NOT NULL,
    problema VARCHAR(1000) NOT NULL,
    accesorios VARCHAR(500) NOT NULL,
    observaciones VARCHAR(1000) NOT NULL,
    estado VARCHAR(30) NOT NULL,
    tecnico VARCHAR(150) NULL,
    fechaAsignacion DATETIME NULL,
    prioridad VARCHAR(10) NOT NULL DEFAULT 'Normal',
    digitoVerificador INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_OrdenServicio PRIMARY KEY (idOrden),
    CONSTRAINT FK_OrdenServicio_Equipo
        FOREIGN KEY (idEquipo)
        REFERENCES Equipo(idEquipo),
    CONSTRAINT FK_OrdenServicio_Usuario
        FOREIGN KEY (tecnico)
        REFERENCES Usuario(email),
    CONSTRAINT CK_OrdenServicio_Estado CHECK (estado IN ('Recibida', 'Asignada', 'Diagnosticada', 'Esperando respuesta', 'Aprobada', 'Rechazada', 'En reparacion', 'Lista para retirar', 'Entregada')),
    CONSTRAINT CK_OrdenServicio_Prioridad CHECK (prioridad IN ('Normal', 'Alta'))
);
GO
CREATE TABLE Diagnostico
(
    idDiagnostico INT IDENTITY(1,1) NOT NULL,
    idOrden INT NOT NULL,
    fecha DATETIME NOT NULL,
    falla VARCHAR(1000) NOT NULL,
    trabajo VARCHAR(1000) NOT NULL,
    repuestosPrevistos VARCHAR(1000) NOT NULL,
    diasEstimados INT NOT NULL,
    digitoVerificador INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_Diagnostico PRIMARY KEY (idDiagnostico),
    CONSTRAINT UQ_Diagnostico_idOrden UNIQUE (idOrden),
    CONSTRAINT FK_Diagnostico_OrdenServicio
        FOREIGN KEY (idOrden)
        REFERENCES OrdenServicio(idOrden),
    CONSTRAINT CK_Diagnostico_DiasEstimados CHECK (diasEstimados >= 1 AND diasEstimados <= 365)
);
GO
CREATE TABLE Presupuesto
(
    idPresupuesto INT IDENTITY(1,1) NOT NULL,
    idOrden INT NOT NULL,
    fecha DATETIME NOT NULL,
    vigencia DATE NOT NULL,
    manoObra DECIMAL(18,2) NOT NULL,
    total DECIMAL(18,2) NOT NULL,
    comunicado BIT NOT NULL DEFAULT 0,
    fechaComunicacion DATETIME NULL,
    respuesta VARCHAR(20) NULL,
    fechaRespuesta DATETIME NULL,
    observaciones VARCHAR(1000) NOT NULL DEFAULT '',
    digitoVerificador INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_Presupuesto PRIMARY KEY (idPresupuesto),
    CONSTRAINT FK_Presupuesto_OrdenServicio
        FOREIGN KEY (idOrden)
        REFERENCES OrdenServicio(idOrden),
    CONSTRAINT CK_Presupuesto_ManoObra CHECK (manoObra >= 0),
    CONSTRAINT CK_Presupuesto_Total CHECK (total > 0),
    CONSTRAINT CK_Presupuesto_Respuesta CHECK (respuesta IN ('Aprobada', 'Rechazada'))
);
GO
CREATE TABLE DetallePresupuesto
(
    idDetallePresupuesto INT IDENTITY(1,1) NOT NULL,
    idPresupuesto INT NOT NULL,
    idProducto INT NOT NULL,
    cantidad INT NOT NULL,
    precioUnitario DECIMAL(18,2) NOT NULL,
    digitoVerificador INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_DetallePresupuesto PRIMARY KEY (idDetallePresupuesto),
    CONSTRAINT UQ_DetallePresupuesto_idPresupuesto_idProducto UNIQUE (idPresupuesto, idProducto),
    CONSTRAINT FK_DetallePresupuesto_Presupuesto
        FOREIGN KEY (idPresupuesto)
        REFERENCES Presupuesto(idPresupuesto),
    CONSTRAINT FK_DetallePresupuesto_Producto
        FOREIGN KEY (idProducto)
        REFERENCES Producto(idProducto),
    CONSTRAINT CK_DetallePresupuesto_Cantidad CHECK (cantidad > 0),
    CONSTRAINT CK_DetallePresupuesto_PrecioUnitario CHECK (precioUnitario > 0)
);
GO
CREATE TABLE Reparacion
(
    idReparacion INT IDENTITY(1,1) NOT NULL,
    idOrden INT NOT NULL,
    fecha DATETIME NOT NULL,
    tareas VARCHAR(2000) NOT NULL,
    digitoVerificador INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_Reparacion PRIMARY KEY (idReparacion),
    CONSTRAINT UQ_Reparacion_idOrden UNIQUE (idOrden),
    CONSTRAINT FK_Reparacion_OrdenServicio
        FOREIGN KEY (idOrden)
        REFERENCES OrdenServicio(idOrden)
);
GO
CREATE TABLE RepuestoServicio
(
    idRepuestoServicio INT IDENTITY(1,1) NOT NULL,
    idReparacion INT NOT NULL,
    idProducto INT NOT NULL,
    cantidad INT NOT NULL,
    digitoVerificador INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_RepuestoServicio PRIMARY KEY (idRepuestoServicio),
    CONSTRAINT FK_RepuestoServicio_Reparacion
        FOREIGN KEY (idReparacion)
        REFERENCES Reparacion(idReparacion),
    CONSTRAINT FK_RepuestoServicio_Producto
        FOREIGN KEY (idProducto)
        REFERENCES Producto(idProducto),
    CONSTRAINT CK_RepuestoServicio_Cantidad CHECK (cantidad > 0)
);
GO
CREATE TABLE PruebaEquipo
(
    idPrueba INT IDENTITY(1,1) NOT NULL,
    idReparacion INT NOT NULL,
    fecha DATETIME NOT NULL,
    descripcion VARCHAR(2000) NOT NULL,
    satisfactoria BIT NOT NULL,
    digitoVerificador INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_PruebaEquipo PRIMARY KEY (idPrueba),
    CONSTRAINT FK_PruebaEquipo_Reparacion
        FOREIGN KEY (idReparacion)
        REFERENCES Reparacion(idReparacion)
);
GO
CREATE TABLE AvisoRetiro
(
    idAviso INT IDENTITY(1,1) NOT NULL,
    idOrden INT NOT NULL,
    fecha DATETIME NOT NULL,
    medio VARCHAR(100) NOT NULL,
    digitoVerificador INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_AvisoRetiro PRIMARY KEY (idAviso),
    CONSTRAINT FK_AvisoRetiro_OrdenServicio
        FOREIGN KEY (idOrden)
        REFERENCES OrdenServicio(idOrden)
);
GO
CREATE TABLE PagoServicio
(
    idPagoServicio INT IDENTITY(1,1) NOT NULL,
    idOrden INT NOT NULL,
    fecha DATETIME NOT NULL,
    medioPago VARCHAR(20) NOT NULL,
    importe DECIMAL(18,2) NOT NULL,
    numeroOperacion NVARCHAR(500) NOT NULL,
    verificado BIT NOT NULL,
    digitoVerificador INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_PagoServicio PRIMARY KEY (idPagoServicio),
    CONSTRAINT UQ_PagoServicio_idOrden UNIQUE (idOrden),
    CONSTRAINT FK_PagoServicio_OrdenServicio
        FOREIGN KEY (idOrden)
        REFERENCES OrdenServicio(idOrden),
    CONSTRAINT CK_PagoServicio_MedioPago CHECK (medioPago IN ('Efectivo', 'Transferencia')),
    CONSTRAINT CK_PagoServicio_Importe CHECK (importe > 0),
    CONSTRAINT CK_PagoServicio_Verificado CHECK (verificado = 1)
);
GO
CREATE TABLE ComprobanteServicio
(
    idComprobante INT IDENTITY(1,1) NOT NULL,
    idOrden INT NOT NULL,
    idPagoServicio INT NOT NULL,
    fecha DATETIME NOT NULL,
    cliente VARCHAR(220) NOT NULL,
    equipo VARCHAR(270) NOT NULL,
    trabajo VARCHAR(2000) NOT NULL,
    total DECIMAL(18,2) NOT NULL,
    medioPago VARCHAR(20) NOT NULL,
    digitoVerificador INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_ComprobanteServicio PRIMARY KEY (idComprobante),
    CONSTRAINT UQ_ComprobanteServicio_idPagoServicio UNIQUE (idPagoServicio),
    CONSTRAINT UQ_ComprobanteServicio_idOrden UNIQUE (idOrden),
    CONSTRAINT FK_ComprobanteServicio_OrdenServicio
        FOREIGN KEY (idOrden)
        REFERENCES OrdenServicio(idOrden),
    CONSTRAINT FK_ComprobanteServicio_PagoServicio
        FOREIGN KEY (idPagoServicio)
        REFERENCES PagoServicio(idPagoServicio)
);
GO
CREATE TABLE EntregaServicio
(
    idEntrega INT IDENTITY(1,1) NOT NULL,
    idOrden INT NOT NULL,
    fecha DATETIME NOT NULL,
    personaRetira VARCHAR(150) NOT NULL,
    garantia VARCHAR(1000) NOT NULL,
    digitoVerificador INT NOT NULL DEFAULT 0,

    CONSTRAINT PK_EntregaServicio PRIMARY KEY (idEntrega),
    CONSTRAINT UQ_EntregaServicio_idOrden UNIQUE (idOrden),
    CONSTRAINT FK_EntregaServicio_OrdenServicio
        FOREIGN KEY (idOrden)
        REFERENCES OrdenServicio(idOrden)
);
GO

-- Historial de cambios de productos
GO
CREATE TRIGGER TR_Producto_HistorialCambios
ON Producto
AFTER UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO HistorialCambiosProducto
    (
        idProducto,codigo,descripcion,marca,modelo,precio,stock,activo,
        digitoVerificador,usuario,fecha,accion
    )
    SELECT d.idProducto,d.codigo,d.descripcion,d.marca,d.modelo,d.precio,d.stock,d.activo,
        0,COALESCE(CONVERT(VARCHAR(150),SESSION_CONTEXT(N'usuario')),SYSTEM_USER),
        GETDATE(),CASE WHEN i.idProducto IS NULL THEN 'BAJA' ELSE 'MODIFICACION' END
    FROM deleted d LEFT JOIN inserted i ON d.idProducto=i.idProducto
    WHERE i.idProducto IS NULL OR EXISTS
    (
        SELECT d.codigo,d.descripcion,d.marca,d.modelo,d.precio,d.stock,d.activo
        EXCEPT
        SELECT i.codigo,i.descripcion,i.marca,i.modelo,i.precio,i.stock,i.activo
    );
END;
GO

-- Datos iniciales
INSERT INTO Idioma (codigo, nombre) VALUES
    ('en', 'English'),
    ('es', N'Español');
GO
INSERT INTO Etiqueta (etiqueta) VALUES
    ('   Venta: '),
    (N'¿Confirma registrar la venta por un total de $'),
    (N'¿Confirma restaurar el producto a la versión seleccionada?'),
    ('Accesorios recibidos'),
    ('accion'),
    (N'Acción'),
    ('Activar/Desactivar'),
    ('Actividad'),
    ('Activo'),
    ('Actualizar'),
    ('Agregar'),
    ('Alta'),
    (N'Alta, modificación y consulta de productos'),
    ('Apellido'),
    ('Aprobada'),
    ('Archivo guardado.'),
    ('Asignacion'),
    ('Asignaciones guardadas.'),
    ('Asignar tecnico'),
    ('Ayuda'),
    ('Base restaurada. Abra nuevamente el sistema para verificar integridad e iniciar sesion.'),
    ('Bitacora'),
    ('Bloquear'),
    ('BMTech - COMPROBANTE INTERNO (NO FISCAL)'),
    ('Buscar'),
    ('Buscar cliente'),
    ('Buscar producto'),
    ('Cada traduccion debe tener entre 1 y 250 caracteres.'),
    (N'Cambiar contraseña'),
    ('Campo'),
    ('CANCELADA'),
    ('Cancelar'),
    ('Cancelar venta'),
    ('Cantidad'),
    ('Cierre la sesion antes de preparar integridad.'),
    ('Clave actual'),
    ('Clave inicial'),
    ('Clave modificada.'),
    ('cliente'),
    ('Cliente encontrado.'),
    ('Cliente registrado correctamente.'),
    ('Cliente seleccionado.'),
    ('Cliente: '),
    ('Clientes'),
    ('Clientes encontrados.'),
    ('Cobrado'),
    ('Cobro y entrega'),
    ('codigo'),
    (N'Código'),
    ('Codigo de idioma');
INSERT INTO Etiqueta (etiqueta) VALUES
    ('Comprobante'),
    ('Comprobante de venta'),
    ('Comprobante: '),
    ('Comunicado'),
    ('Confirma cancelar esta venta pendiente?'),
    ('Confirma eliminar '),
    ('Confirma la entrega de productos y comprobante?'),
    ('Confirma que entrego los productos y el comprobante?'),
    ('Confirma registrar este cliente?'),
    ('Confirmar'),
    ('Confirmar cambio de estado de '),
    ('Confirmar clave'),
    ('Confirmar cobro y entrega'),
    ('Confirmar venta'),
    ('Consumido'),
    (N'Contraseña'),
    (N'Contraseña incorrecta. Intentos restantes: '),
    ('Control cambios producto'),
    ('Control de cambios de producto'),
    ('Copia creada: '),
    ('Copias de seguridad'),
    (N'Correo electrónico'),
    ('correoElectronico'),
    ('Crear copia'),
    ('Credenciales incorrectas.'),
    ('criticidad'),
    ('Datos del cliente'),
    ('Datos del producto'),
    ('Datos guardados.'),
    ('Debe agregar al menos un producto a la venta.'),
    ('Debe indicar el pago.'),
    ('Debe indicar un usuario activo para iniciar sesion.'),
    ('Debe ingresar el apellido del cliente.'),
    ('Debe ingresar el apellido del usuario.'),
    (N'Debe ingresar el código del producto.'),
    (N'Debe ingresar el correo electrónico del cliente.'),
    ('Debe ingresar el DNI del cliente.'),
    ('Debe ingresar el email del usuario.'),
    ('Debe ingresar el modelo del producto.'),
    ('Debe ingresar el nombre del cliente.'),
    ('Debe ingresar el nombre del usuario.'),
    ('Debe ingresar el perfil del usuario.'),
    ('Debe ingresar el precio del producto.'),
    ('Debe ingresar el stock del producto.'),
    (N'Debe ingresar el teléfono del cliente.'),
    (N'Debe ingresar email y contraseña.'),
    (N'Debe ingresar la contraseña del usuario.'),
    (N'Debe ingresar la descripción del producto.'),
    ('Debe ingresar la marca del producto.'),
    ('Debe ingresar los datos de la venta.');
INSERT INTO Etiqueta (etiqueta) VALUES
    ('Debe ingresar los datos del cliente.'),
    ('Debe ingresar los datos del producto.'),
    ('Debe ingresar los datos del usuario.'),
    ('Debe iniciar sesion para registrar una venta.'),
    ('Debe iniciar sesion.'),
    ('Debe seleccionar un cliente.'),
    ('Debe seleccionar un producto para modificar.'),
    (N'Debe seleccionar un producto válido.'),
    ('Debe seleccionar un producto.'),
    (N'Debe seleccionar una versión.'),
    ('Desbloquear'),
    ('descripcion'),
    (N'Descripción'),
    ('Desde'),
    ('Detalle de venta'),
    ('Detalle del cambio'),
    ('Diagnostico'),
    ('Dias estimados'),
    ('Diferencias en: '),
    ('digitoVerificador'),
    ('DNI'),
    ('DNI cliente'),
    ('DNI del cliente'),
    ('dniCliente'),
    ('Efectivo'),
    ('El cliente ya se encuentra registrado.'),
    ('El codigo del idioma debe tener dos letras minusculas.'),
    ('El correo electronico no es valido.'),
    ('El detalle no puede ser nulo.'),
    ('El DNI debe contener solo numeros.'),
    ('El importe del pago no es valido.'),
    ('El numero de operacion es demasiado largo.'),
    ('El pago no esta verificado o el importe no coincide. Guardar venta pendiente?'),
    ('El precio debe ser mayor a cero.'),
    ('El precio debe tener como maximo dos decimales.'),
    ('El precio del producto debe ser mayor a cero.'),
    ('El producto cambio de precio, no esta activo o no tiene stock suficiente.'),
    ('El rango de fechas es incorrecto.'),
    ('El stock no puede ser negativo.'),
    ('El total supera el importe permitido.'),
    ('El usuario no esta activo.'),
    ('El usuario se encuentra bloqueado.'),
    ('El vendedor no esta activo.'),
    ('Eliminar'),
    ('email'),
    ('estado'),
    ('Estado del producto actualizado correctamente.'),
    ('Esto acepta los datos actuales como correctos. Revise la base antes de continuar.'),
    ('Estructura de permisos'),
    ('Exportar XML');
INSERT INTO Etiqueta (etiqueta) VALUES
    ('Falla detectada'),
    ('Faltan el archivo .bak o su archivo .key.'),
    ('Faltan las traducciones.'),
    ('Familia'),
    ('Fecha'),
    ('Fecha: '),
    ('fechaEntrega'),
    ('FINALIZADA'),
    ('Garantia del trabajo'),
    ('Generar presupuesto'),
    (N'Gestión de clientes de BMTech'),
    (N'Gestión de productos'),
    ('Guardar'),
    ('Guardar asignaciones'),
    ('Guardar datos'),
    ('Guardar diagnostico'),
    ('Guardar PDF'),
    ('Hasta'),
    ('Historial'),
    ('Historial actualizado.'),
    (N'Historial de modificaciones y restauración de producto'),
    ('Historial de ventas'),
    ('id'),
    ('idBackup'),
    ('Identificador'),
    ('Idioma no disponible.'),
    ('Idiomas'),
    ('Idiomas y traducciones'),
    ('idLog'),
    ('idperfil'),
    ('idVenta'),
    ('Importe'),
    ('Imprimir'),
    ('Indique el numero de operacion de la transferencia.'),
    ('Indique un nombre de idioma valido.'),
    ('Informes'),
    ('Informes de operaciones'),
    ('Ingresar'),
    ('Ingrese la nueva clave.'),
    ('Ingrese las credenciales de un administrador activo.'),
    ('Ingreso'),
    (N'Inicio de sesión'),
    ('Integridad'),
    ('Integridad preparada. Ahora puede ingresar.'),
    ('intentos'),
    ('La cantidad debe ser mayor a cero.'),
    ('La carpeta no existe.'),
    ('La clave actual es incorrecta.'),
    ('La clave de la copia no es valida.'),
    ('La clave debe tener entre 1 y 72 bytes.');
INSERT INTO Etiqueta (etiqueta) VALUES
    ('La clave es demasiado larga.'),
    ('La integridad de Seguridad no esta preparada o presenta diferencias.'),
    ('La integridad de Seguridad presenta diferencias.'),
    ('La integridad de Usuario presenta diferencias.'),
    ('La integridad del sistema es correcta.'),
    ('La integridad del sistema fue regenerada.'),
    ('La integridad ya fue preparada. Para regenerarla ingrese como administrador.'),
    ('La operacion puede haberse guardado, pero fallo la bitacora: '),
    ('La ruta de backup no es valida.'),
    ('La venta debe estar pagada y tener comprobante antes de entregar.'),
    ('La venta no esta pendiente o el importe no coincide.'),
    ('La venta no esta pendiente.'),
    ('La venta no existe.'),
    ('La venta quedo pagada. Puede reintentar el comprobante o la entrega desde Historial de ventas. '),
    ('Las claves no coinciden.'),
    ('Limpiar'),
    ('Los datos del cliente superan el largo permitido.'),
    ('Los datos del producto superan el largo permitido.'),
    ('Los datos del usuario superan el largo permitido.'),
    ('Maestros'),
    ('Mano de obra'),
    ('Marca'),
    ('Medio de pago'),
    ('Medio por el que se aviso al cliente'),
    ('medioPago'),
    ('Modelo'),
    ('Modificar'),
    ('modulo'),
    (N'Mostrar contraseña'),
    ('No hay stock suficiente.'),
    ('No puede bloquear su propia cuenta.'),
    ('No puede cambiar su propio perfil durante la sesion.'),
    ('No puede eliminar un idioma base o el idioma en uso.'),
    ('No se encontraron productos.'),
    (N'No se encontró el producto seleccionado.'),
    (N'No se encontró un cliente con ese DNI.'),
    ('No se permite ingresar sin registrar el inicio de sesion.'),
    ('No se pudo actualizar el usuario.'),
    ('No se pudo cambiar la clave.'),
    ('No se pudo cargar el idioma. Revise el script de idiomas. '),
    ('No se pudo modificar.'),
    ('No se pudo registrar el cierre de sesion. '),
    ('No se pudo registrar el cliente.'),
    ('No se pudo registrar.'),
    ('No se pudo restaurar el producto.'),
    ('No tiene permiso para esta operacion.'),
    ('Nombre'),
    ('Nueva clave'),
    ('Nuevas tareas realizadas'),
    ('Nuevo');
INSERT INTO Etiqueta (etiqueta) VALUES
    ('Nuevo consumo'),
    ('Nuevo ingreso'),
    ('Numero'),
    ('Numero de operacion'),
    ('Numero de operacion de transferencia'),
    ('Numero de serie (si dispone)'),
    ('numeroOperacion'),
    ('Observaciones de ingreso'),
    ('Observaciones de la respuesta'),
    ('Operacion guardada, bitacora pendiente.'),
    ('Orden'),
    ('PAGADA'),
    ('Pago de la venta'),
    ('Pago pendiente'),
    ('Pago verificado por el empleado'),
    ('Pagos'),
    ('Patente'),
    ('PENDIENTE'),
    ('Perfil'),
    ('Perfiles, familias y patentes'),
    ('Permisos'),
    ('Persona que retira'),
    ('Precio'),
    ('Precio acordado'),
    ('precioUnitario'),
    ('Preparar integridad'),
    ('Presupuesto'),
    ('Presupuestos guardados'),
    ('Prioridad'),
    ('Problema informado'),
    ('Producto'),
    ('Producto agregado a la venta.'),
    ('Producto modificado correctamente.'),
    ('Producto registrado correctamente.'),
    ('Producto restaurado correctamente.'),
    ('Producto seleccionado.'),
    ('Productos'),
    ('Productos encontrados.'),
    ('Pruebas realizadas y resultados'),
    ('Pruebas satisfactorias'),
    ('Pruebas y aviso'),
    ('Pulse Nuevo para registrar otro usuario.'),
    ('Quitar producto'),
    ('Rechazada'),
    ('Regenerar integridad'),
    ('Registrar'),
    ('Registrar aviso realizado'),
    ('Registrar cliente'),
    ('Registrar comunicacion'),
    ('Registrar entrega');
INSERT INTO Etiqueta (etiqueta) VALUES
    ('Registrar ingreso'),
    ('Registrar pruebas'),
    ('Registrar respuesta'),
    ('Registrar tareas y consumos'),
    ('Registrar venta'),
    ('Registro de ventas'),
    ('Registro de ventas, productos y cantidades'),
    ('Reparacion'),
    ('Repuesto'),
    ('Repuesto utilizado'),
    ('Repuestos previstos'),
    ('Respuesta'),
    ('Restaurar copia'),
    ('Restaurar producto'),
    ('Restaurar reemplaza los datos actuales por los de esta copia y cierra el sistema. Haga antes una copia actual. Continuar?'),
    (N'Restaurar versión'),
    ('Revise el precio y el stock del producto.'),
    ('Revise integridad antes de operar. Diferencias en: '),
    ('Revise la conexion con la base de datos. '),
    ('Revise la integridad antes de hacer una copia.'),
    ('Revise la integridad de los pagos.'),
    ('ruta'),
    ('Salir'),
    ('Se recalcularan los digitos con los datos actuales. Continuar solo si reviso que son correctos.'),
    ('Seguridad'),
    ('Seleccione un elemento.'),
    ('Seleccione un medio de pago.'),
    ('Seleccione un perfil existente.'),
    ('Seleccione un perfil o una familia.'),
    ('Seleccione un usuario.'),
    (N'Seleccione una opción del menú'),
    ('Seleccione una venta.'),
    ('Servicio tecnico'),
    (N'Sistema de gestión comercial para BMTech'),
    ('Solo se cancela una venta pendiente.'),
    ('Solo se emite comprobante de una venta pagada.'),
    ('Solo un administrador puede regenerar integridad.'),
    ('SQL Server debe tener permiso de escritura en la carpeta. Conserve juntos el .bak y el .key en un lugar privado.'),
    ('stock'),
    ('Subtotal'),
    ('Tareas registradas'),
    ('Tecnico asignado'),
    ('telefono'),
    (N'Teléfono'),
    ('Texto original'),
    ('Tipo de equipo'),
    ('Tipo de informe'),
    ('Todavia no hay comprobante.'),
    ('Total'),
    ('Total: ');
INSERT INTO Etiqueta (etiqueta) VALUES
    ('Total: 0,00'),
    ('Trabajo propuesto'),
    ('Traduccion'),
    ('Transferencia'),
    ('Un producto no puede tener dos precios en la misma venta.'),
    ('Una familia no puede contenerse a si misma.'),
    ('Usuario'),
    ('Usuario bloqueado por superar los intentos permitidos.'),
    ('Usuario modificado.'),
    ('Usuario no encontrado.'),
    ('Usuario registrado.'),
    ('Usuarios'),
    ('Valor'),
    ('vendedor'),
    ('Vendedor: '),
    ('Venta pendiente guardada. Verifique el pago desde Historial de ventas.'),
    ('Venta registrada correctamente. Nro: '),
    ('Ventas'),
    ('Ver comprobante'),
    ('verificado'),
    ('Verificar integridad'),
    ('Verificar pago'),
    ('Verifique el pago por el importe exacto antes de continuar.'),
    (N'Versión seleccionada.'),
    ('Versiones registradas'),
    ('Vigencia'),
    (N'Ya existe un producto registrado con ese código.'),
    ('Ya existe un usuario registrado con ese email.'),
    (N'Ya hay una sesión iniciada.');
GO
INSERT INTO Traduccion (codigoIdioma, etiqueta, textoTraducido) VALUES
    ('en', '   Venta: ', '   Sale: '),
    ('en', N'¿Confirma registrar la venta por un total de $', 'Confirm a sale for a total of $'),
    ('en', N'¿Confirma restaurar el producto a la versión seleccionada?', 'Restore the product to the selected version?'),
    ('en', 'Accesorios recibidos', 'Received accessories'),
    ('en', 'accion', 'Action'),
    ('en', N'Acción', 'Action'),
    ('en', 'Activar/Desactivar', 'Enable/Disable'),
    ('en', 'Actividad', 'Activity'),
    ('en', 'Activo', 'Enabled'),
    ('en', 'Actualizar', 'Refresh'),
    ('en', 'Agregar', 'Add'),
    ('en', 'Alta', 'High'),
    ('en', N'Alta, modificación y consulta de productos', 'Add, edit and find products'),
    ('en', 'Apellido', 'Last name'),
    ('en', 'Aprobada', 'Approved'),
    ('en', 'Archivo guardado.', 'File saved.'),
    ('en', 'Asignacion', 'Assignment'),
    ('en', 'Asignaciones guardadas.', 'Assignments saved.'),
    ('en', 'Asignar tecnico', 'Assign technician'),
    ('en', 'Ayuda', 'Help'),
    ('en', 'Base restaurada. Abra nuevamente el sistema para verificar integridad e iniciar sesion.', 'Database restored. Reopen the app to check integrity and sign in.'),
    ('en', 'Bitacora', 'Event log'),
    ('en', 'Bloquear', 'Block'),
    ('en', 'BMTech - COMPROBANTE INTERNO (NO FISCAL)', 'BMTech - INTERNAL RECEIPT (NOT A TAX INVOICE)'),
    ('en', 'Buscar', 'Search'),
    ('en', 'Buscar cliente', 'Find customer'),
    ('en', 'Buscar producto', 'Find product'),
    ('en', 'Cada traduccion debe tener entre 1 y 250 caracteres.', 'Each translation must contain between 1 and 250 characters.'),
    ('en', N'Cambiar contraseña', 'Change password'),
    ('en', 'Campo', 'Field'),
    ('en', 'CANCELADA', 'CANCELLED'),
    ('en', 'Cancelar', 'Cancel'),
    ('en', 'Cancelar venta', 'Cancel sale'),
    ('en', 'Cantidad', 'Quantity'),
    ('en', 'Cierre la sesion antes de preparar integridad.', 'Sign out before initializing integrity.'),
    ('en', 'Clave actual', 'Current password'),
    ('en', 'Clave inicial', 'Initial password'),
    ('en', 'Clave modificada.', 'Password changed.'),
    ('en', 'cliente', 'Customer'),
    ('en', 'Cliente encontrado.', 'Customer found.'),
    ('en', 'Cliente registrado correctamente.', 'Customer registered successfully.'),
    ('en', 'Cliente seleccionado.', 'Customer selected.'),
    ('en', 'Cliente: ', 'Customer: '),
    ('en', 'Clientes', 'Customers'),
    ('en', 'Clientes encontrados.', 'Customers found.'),
    ('en', 'Cobrado', 'Collected'),
    ('en', 'Cobro y entrega', 'Payment and handover'),
    ('en', 'codigo', 'Code'),
    ('en', N'Código', 'Code'),
    ('en', 'Codigo de idioma', 'Language code');
INSERT INTO Traduccion (codigoIdioma, etiqueta, textoTraducido) VALUES
    ('en', 'Comprobante', 'Receipt'),
    ('en', 'Comprobante de venta', 'Sale receipt'),
    ('en', 'Comprobante: ', 'Receipt: '),
    ('en', 'Comunicado', 'Communicated'),
    ('en', 'Confirma cancelar esta venta pendiente?', 'Confirm cancellation of this pending sale?'),
    ('en', 'Confirma eliminar ', 'Confirm deletion of '),
    ('en', 'Confirma la entrega de productos y comprobante?', 'Confirm delivery of products and receipt?'),
    ('en', 'Confirma que entrego los productos y el comprobante?', 'Confirm that the products and receipt were delivered?'),
    ('en', 'Confirma registrar este cliente?', 'Confirm registration of this customer?'),
    ('en', 'Confirmar', 'Confirm'),
    ('en', 'Confirmar cambio de estado de ', 'Confirm status change for '),
    ('en', 'Confirmar clave', 'Confirm password'),
    ('en', 'Confirmar cobro y entrega', 'Confirm payment and handover'),
    ('en', 'Confirmar venta', 'Confirm sale'),
    ('en', 'Consumido', 'Used'),
    ('en', N'Contraseña', 'Password'),
    ('en', N'Contraseña incorrecta. Intentos restantes: ', 'Incorrect password. Remaining attempts: '),
    ('en', 'Control cambios producto', 'Product change history'),
    ('en', 'Control de cambios de producto', 'Product change history'),
    ('en', 'Copia creada: ', 'Backup created: '),
    ('en', 'Copias de seguridad', 'Backups'),
    ('en', N'Correo electrónico', 'Email'),
    ('en', 'correoElectronico', 'Email'),
    ('en', 'Crear copia', 'Create backup'),
    ('en', 'Credenciales incorrectas.', 'Incorrect credentials.'),
    ('en', 'criticidad', 'Severity'),
    ('en', 'Datos del cliente', 'Customer information'),
    ('en', 'Datos del producto', 'Product information'),
    ('en', 'Datos guardados.', 'Information saved.'),
    ('en', 'Debe agregar al menos un producto a la venta.', 'Add at least one product to the sale.'),
    ('en', 'Debe indicar el pago.', 'Enter payment information.'),
    ('en', 'Debe indicar un usuario activo para iniciar sesion.', 'Use an active user to sign in.'),
    ('en', 'Debe ingresar el apellido del cliente.', 'Enter the customer''s last name.'),
    ('en', 'Debe ingresar el apellido del usuario.', 'Enter the user''s last name.'),
    ('en', N'Debe ingresar el código del producto.', 'Enter the product code.'),
    ('en', N'Debe ingresar el correo electrónico del cliente.', 'Enter the customer''s email.'),
    ('en', 'Debe ingresar el DNI del cliente.', 'Enter the customer ID number.'),
    ('en', 'Debe ingresar el email del usuario.', 'Enter the user''s email.'),
    ('en', 'Debe ingresar el modelo del producto.', 'Enter the product model.'),
    ('en', 'Debe ingresar el nombre del cliente.', 'Enter the customer''s first name.'),
    ('en', 'Debe ingresar el nombre del usuario.', 'Enter the user''s first name.'),
    ('en', 'Debe ingresar el perfil del usuario.', 'Enter the user''s profile.'),
    ('en', 'Debe ingresar el precio del producto.', 'Enter the product price.'),
    ('en', 'Debe ingresar el stock del producto.', 'Enter the product stock.'),
    ('en', N'Debe ingresar el teléfono del cliente.', 'Enter the customer''s phone.'),
    ('en', N'Debe ingresar email y contraseña.', 'Enter email and password.'),
    ('en', N'Debe ingresar la contraseña del usuario.', 'Enter the user''s password.'),
    ('en', N'Debe ingresar la descripción del producto.', 'Enter the product description.'),
    ('en', 'Debe ingresar la marca del producto.', 'Enter the product brand.'),
    ('en', 'Debe ingresar los datos de la venta.', 'Enter sale information.');
INSERT INTO Traduccion (codigoIdioma, etiqueta, textoTraducido) VALUES
    ('en', 'Debe ingresar los datos del cliente.', 'Enter customer information.'),
    ('en', 'Debe ingresar los datos del producto.', 'Enter product information.'),
    ('en', 'Debe ingresar los datos del usuario.', 'Enter user information.'),
    ('en', 'Debe iniciar sesion para registrar una venta.', 'Sign in to register a sale.'),
    ('en', 'Debe iniciar sesion.', 'Sign in first.'),
    ('en', 'Debe seleccionar un cliente.', 'Select a customer.'),
    ('en', 'Debe seleccionar un producto para modificar.', 'Select a product to edit.'),
    ('en', N'Debe seleccionar un producto válido.', 'Select a valid product.'),
    ('en', 'Debe seleccionar un producto.', 'Select a product.'),
    ('en', N'Debe seleccionar una versión.', 'Select a version.'),
    ('en', 'Desbloquear', 'Unblock'),
    ('en', 'descripcion', 'Description'),
    ('en', N'Descripción', 'Description'),
    ('en', 'Desde', 'From'),
    ('en', 'Detalle de venta', 'Sale details'),
    ('en', 'Detalle del cambio', 'Change details'),
    ('en', 'Diagnostico', 'Diagnosis'),
    ('en', 'Dias estimados', 'Estimated days'),
    ('en', 'Diferencias en: ', 'Differences in: '),
    ('en', 'digitoVerificador', 'Check digit'),
    ('en', 'DNI', 'ID number'),
    ('en', 'DNI cliente', 'Customer ID'),
    ('en', 'DNI del cliente', 'Customer ID'),
    ('en', 'dniCliente', 'Customer ID'),
    ('en', 'Efectivo', 'Cash'),
    ('en', 'El cliente ya se encuentra registrado.', 'The customer is already registered.'),
    ('en', 'El codigo del idioma debe tener dos letras minusculas.', 'The language code must contain two lowercase letters.'),
    ('en', 'El correo electronico no es valido.', 'The email is not valid.'),
    ('en', 'El detalle no puede ser nulo.', 'The detail cannot be null.'),
    ('en', 'El DNI debe contener solo numeros.', 'The ID number must contain only digits.'),
    ('en', 'El importe del pago no es valido.', 'The payment amount is not valid.'),
    ('en', 'El numero de operacion es demasiado largo.', 'The transaction number is too long.'),
    ('en', 'El pago no esta verificado o el importe no coincide. Guardar venta pendiente?', 'Payment is unverified or the amount does not match. Save a pending sale?'),
    ('en', 'El precio debe ser mayor a cero.', 'The price must be greater than zero.'),
    ('en', 'El precio debe tener como maximo dos decimales.', 'The price must have at most two decimal places.'),
    ('en', 'El precio del producto debe ser mayor a cero.', 'The product price must be greater than zero.'),
    ('en', 'El producto cambio de precio, no esta activo o no tiene stock suficiente.', 'The product price changed, is inactive or has insufficient stock.'),
    ('en', 'El rango de fechas es incorrecto.', 'The date range is incorrect.'),
    ('en', 'El stock no puede ser negativo.', 'Stock cannot be negative.'),
    ('en', 'El total supera el importe permitido.', 'The total exceeds the allowed amount.'),
    ('en', 'El usuario no esta activo.', 'The user is not active.'),
    ('en', 'El usuario se encuentra bloqueado.', 'The user is blocked.'),
    ('en', 'El vendedor no esta activo.', 'The seller is not active.'),
    ('en', 'Eliminar', 'Delete'),
    ('en', 'email', 'Email'),
    ('en', 'estado', 'Status'),
    ('en', 'Estado del producto actualizado correctamente.', 'Product status updated successfully.'),
    ('en', 'Esto acepta los datos actuales como correctos. Revise la base antes de continuar.', 'This accepts current data as correct. Review the database before continuing.'),
    ('en', 'Estructura de permisos', 'Permission structure'),
    ('en', 'Exportar XML', 'Export XML');
INSERT INTO Traduccion (codigoIdioma, etiqueta, textoTraducido) VALUES
    ('en', 'Falla detectada', 'Detected fault'),
    ('en', 'Faltan el archivo .bak o su archivo .key.', 'The .bak file or its .key file is missing.'),
    ('en', 'Faltan las traducciones.', 'Translations are missing.'),
    ('en', 'Familia', 'Group'),
    ('en', 'Fecha', 'Date'),
    ('en', 'Fecha: ', 'Date: '),
    ('en', 'fechaEntrega', 'Delivery date'),
    ('en', 'FINALIZADA', 'COMPLETED'),
    ('en', 'Garantia del trabajo', 'Work warranty'),
    ('en', 'Generar presupuesto', 'Create estimate'),
    ('en', N'Gestión de clientes de BMTech', 'BMTech customer management'),
    ('en', N'Gestión de productos', 'Product management'),
    ('en', 'Guardar', 'Save'),
    ('en', 'Guardar asignaciones', 'Save assignments'),
    ('en', 'Guardar datos', 'Save information'),
    ('en', 'Guardar diagnostico', 'Save diagnosis'),
    ('en', 'Guardar PDF', 'Save PDF'),
    ('en', 'Hasta', 'To'),
    ('en', 'Historial', 'History'),
    ('en', 'Historial actualizado.', 'History refreshed.'),
    ('en', N'Historial de modificaciones y restauración de producto', 'Product history and restoration'),
    ('en', 'Historial de ventas', 'Sales history'),
    ('en', 'id', 'Identifier'),
    ('en', 'idBackup', 'Backup ID'),
    ('en', 'Identificador', 'Identifier'),
    ('en', 'Idioma no disponible.', 'Language unavailable.'),
    ('en', 'Idiomas', 'Languages'),
    ('en', 'Idiomas y traducciones', 'Languages and translations'),
    ('en', 'idLog', 'Event ID'),
    ('en', 'idperfil', 'Profile'),
    ('en', 'idVenta', 'Sale ID'),
    ('en', 'Importe', 'Amount'),
    ('en', 'Imprimir', 'Print'),
    ('en', 'Indique el numero de operacion de la transferencia.', 'Enter the bank transfer transaction number.'),
    ('en', 'Indique un nombre de idioma valido.', 'Enter a valid language name.'),
    ('en', 'Informes', 'Reports'),
    ('en', 'Informes de operaciones', 'Operation reports'),
    ('en', 'Ingresar', 'Sign in'),
    ('en', 'Ingrese la nueva clave.', 'Enter the new password.'),
    ('en', 'Ingrese las credenciales de un administrador activo.', 'Enter the credentials of an active administrator.'),
    ('en', 'Ingreso', 'Intake'),
    ('en', N'Inicio de sesión', 'Sign in'),
    ('en', 'Integridad', 'Integrity'),
    ('en', 'Integridad preparada. Ahora puede ingresar.', 'Integrity initialized. You can now sign in.'),
    ('en', 'intentos', 'Attempts'),
    ('en', 'La cantidad debe ser mayor a cero.', 'Quantity must be greater than zero.'),
    ('en', 'La carpeta no existe.', 'The folder does not exist.'),
    ('en', 'La clave actual es incorrecta.', 'The current password is incorrect.'),
    ('en', 'La clave de la copia no es valida.', 'The backup key is invalid.'),
    ('en', 'La clave debe tener entre 1 y 72 bytes.', 'The password must contain between 1 and 72 bytes.');
INSERT INTO Traduccion (codigoIdioma, etiqueta, textoTraducido) VALUES
    ('en', 'La clave es demasiado larga.', 'The password is too long.'),
    ('en', 'La integridad de Seguridad no esta preparada o presenta diferencias.', 'Security integrity is not initialized or has differences.'),
    ('en', 'La integridad de Seguridad presenta diferencias.', 'Security integrity has differences.'),
    ('en', 'La integridad de Usuario presenta diferencias.', 'User integrity has differences.'),
    ('en', 'La integridad del sistema es correcta.', 'System integrity is correct.'),
    ('en', 'La integridad del sistema fue regenerada.', 'System integrity was recalculated.'),
    ('en', 'La integridad ya fue preparada. Para regenerarla ingrese como administrador.', 'Integrity was already initialized. Sign in as administrator to recalculate it.'),
    ('en', 'La operacion puede haberse guardado, pero fallo la bitacora: ', 'The operation may have been saved, but the event log failed: '),
    ('en', 'La ruta de backup no es valida.', 'The backup path is not valid.'),
    ('en', 'La venta debe estar pagada y tener comprobante antes de entregar.', 'The sale must be paid and have a receipt before delivery.'),
    ('en', 'La venta no esta pendiente o el importe no coincide.', 'The sale is not pending or the amount does not match.'),
    ('en', 'La venta no esta pendiente.', 'The sale is not pending.'),
    ('en', 'La venta no existe.', 'The sale does not exist.'),
    ('en', 'La venta quedo pagada. Puede reintentar el comprobante o la entrega desde Historial de ventas. ', 'The sale remains paid. Retry the receipt or delivery from Sales history. '),
    ('en', 'Las claves no coinciden.', 'The passwords do not match.'),
    ('en', 'Limpiar', 'Clear'),
    ('en', 'Los datos del cliente superan el largo permitido.', 'Customer information exceeds the allowed length.'),
    ('en', 'Los datos del producto superan el largo permitido.', 'Product information exceeds the allowed length.'),
    ('en', 'Los datos del usuario superan el largo permitido.', 'User information exceeds the allowed length.'),
    ('en', 'Maestros', 'Master data'),
    ('en', 'Mano de obra', 'Labor'),
    ('en', 'Marca', 'Brand'),
    ('en', 'Medio de pago', 'Payment method'),
    ('en', 'Medio por el que se aviso al cliente', 'Customer notification method'),
    ('en', 'medioPago', 'Payment method'),
    ('en', 'Modelo', 'Model'),
    ('en', 'Modificar', 'Edit'),
    ('en', 'modulo', 'Module'),
    ('en', N'Mostrar contraseña', 'Show password'),
    ('en', 'No hay stock suficiente.', 'There is not enough stock.'),
    ('en', 'No puede bloquear su propia cuenta.', 'You cannot block your own account.'),
    ('en', 'No puede cambiar su propio perfil durante la sesion.', 'You cannot change your own profile during the session.'),
    ('en', 'No puede eliminar un idioma base o el idioma en uso.', 'You cannot delete a base language or the language in use.'),
    ('en', 'No se encontraron productos.', 'No products found.'),
    ('en', N'No se encontró el producto seleccionado.', 'The selected product was not found.'),
    ('en', N'No se encontró un cliente con ese DNI.', 'No customer was found with that ID number.'),
    ('en', 'No se permite ingresar sin registrar el inicio de sesion.', 'Sign-in cannot proceed without recording its event.'),
    ('en', 'No se pudo actualizar el usuario.', 'The user could not be updated.'),
    ('en', 'No se pudo cambiar la clave.', 'The password could not be changed.'),
    ('en', 'No se pudo cargar el idioma. Revise el script de idiomas. ', 'The language could not be loaded. Check the language script. '),
    ('en', 'No se pudo modificar.', 'The information could not be updated.'),
    ('en', 'No se pudo registrar el cierre de sesion. ', 'Sign-out could not be recorded. '),
    ('en', 'No se pudo registrar el cliente.', 'The customer could not be registered.'),
    ('en', 'No se pudo registrar.', 'The information could not be registered.'),
    ('en', 'No se pudo restaurar el producto.', 'The product could not be restored.'),
    ('en', 'No tiene permiso para esta operacion.', 'You do not have permission for this operation.'),
    ('en', 'Nombre', 'First name'),
    ('en', 'Nueva clave', 'New password'),
    ('en', 'Nuevas tareas realizadas', 'New work performed'),
    ('en', 'Nuevo', 'New');
INSERT INTO Traduccion (codigoIdioma, etiqueta, textoTraducido) VALUES
    ('en', 'Nuevo consumo', 'New usage'),
    ('en', 'Nuevo ingreso', 'New intake'),
    ('en', 'Numero', 'Number'),
    ('en', 'Numero de operacion', 'Transaction number'),
    ('en', 'Numero de operacion de transferencia', 'Transfer reference number'),
    ('en', 'Numero de serie (si dispone)', 'Serial number (if available)'),
    ('en', 'numeroOperacion', 'Transaction number'),
    ('en', 'Observaciones de ingreso', 'Intake notes'),
    ('en', 'Observaciones de la respuesta', 'Response notes'),
    ('en', 'Operacion guardada, bitacora pendiente.', 'Operation saved, event log pending.'),
    ('en', 'Orden', 'Order'),
    ('en', 'PAGADA', 'PAID'),
    ('en', 'Pago de la venta', 'Sale payment'),
    ('en', 'Pago pendiente', 'Pending payment'),
    ('en', 'Pago verificado por el empleado', 'Payment verified by the employee'),
    ('en', 'Pagos', 'Payments'),
    ('en', 'Patente', 'Permission'),
    ('en', 'PENDIENTE', 'PENDING'),
    ('en', 'Perfil', 'Profile'),
    ('en', 'Perfiles, familias y patentes', 'Profiles, groups and permissions'),
    ('en', 'Permisos', 'Permissions'),
    ('en', 'Persona que retira', 'Collected by'),
    ('en', 'Precio', 'Price'),
    ('en', 'Precio acordado', 'Agreed price'),
    ('en', 'precioUnitario', 'Unit price'),
    ('en', 'Preparar integridad', 'Initialize integrity'),
    ('en', 'Presupuesto', 'Estimate'),
    ('en', 'Presupuestos guardados', 'Saved estimates'),
    ('en', 'Prioridad', 'Priority'),
    ('en', 'Problema informado', 'Reported problem'),
    ('en', 'Producto', 'Product'),
    ('en', 'Producto agregado a la venta.', 'Product added to the sale.'),
    ('en', 'Producto modificado correctamente.', 'Product updated successfully.'),
    ('en', 'Producto registrado correctamente.', 'Product registered successfully.'),
    ('en', 'Producto restaurado correctamente.', 'Product restored successfully.'),
    ('en', 'Producto seleccionado.', 'Product selected.'),
    ('en', 'Productos', 'Products'),
    ('en', 'Productos encontrados.', 'Products found.'),
    ('en', 'Pruebas realizadas y resultados', 'Tests and results'),
    ('en', 'Pruebas satisfactorias', 'Successful tests'),
    ('en', 'Pruebas y aviso', 'Tests and notification'),
    ('en', 'Pulse Nuevo para registrar otro usuario.', 'Click New to register another user.'),
    ('en', 'Quitar producto', 'Remove product'),
    ('en', 'Rechazada', 'Rejected'),
    ('en', 'Regenerar integridad', 'Recalculate integrity'),
    ('en', 'Registrar', 'Register'),
    ('en', 'Registrar aviso realizado', 'Record completed notification'),
    ('en', 'Registrar cliente', 'Register customer'),
    ('en', 'Registrar comunicacion', 'Record communication'),
    ('en', 'Registrar entrega', 'Record delivery');
INSERT INTO Traduccion (codigoIdioma, etiqueta, textoTraducido) VALUES
    ('en', 'Registrar ingreso', 'Register intake'),
    ('en', 'Registrar pruebas', 'Record tests'),
    ('en', 'Registrar respuesta', 'Record response'),
    ('en', 'Registrar tareas y consumos', 'Record work and usage'),
    ('en', 'Registrar venta', 'Register sale'),
    ('en', 'Registro de ventas', 'Sales registration'),
    ('en', 'Registro de ventas, productos y cantidades', 'Sales, products and quantities'),
    ('en', 'Reparacion', 'Repair'),
    ('en', 'Repuesto', 'Part'),
    ('en', 'Repuesto utilizado', 'Used part'),
    ('en', 'Repuestos previstos', 'Expected parts'),
    ('en', 'Respuesta', 'Response'),
    ('en', 'Restaurar copia', 'Restore backup'),
    ('en', 'Restaurar producto', 'Restore product'),
    ('en', 'Restaurar reemplaza los datos actuales por los de esta copia y cierra el sistema. Haga antes una copia actual. Continuar?', 'Restoring replaces current data with this backup and closes the app. Create a current backup first. Continue?'),
    ('en', N'Restaurar versión', 'Restore version'),
    ('en', 'Revise el precio y el stock del producto.', 'Check the product price and stock.'),
    ('en', 'Revise integridad antes de operar. Diferencias en: ', 'Check integrity before working. Differences in: '),
    ('en', 'Revise la conexion con la base de datos. ', 'Check the database connection. '),
    ('en', 'Revise la integridad antes de hacer una copia.', 'Check integrity before creating a backup.'),
    ('en', 'Revise la integridad de los pagos.', 'Check payment integrity.'),
    ('en', 'ruta', 'Path'),
    ('en', 'Salir', 'Exit'),
    ('en', 'Se recalcularan los digitos con los datos actuales. Continuar solo si reviso que son correctos.', 'Check digits will be recalculated from current data. Continue only after reviewing their correctness.'),
    ('en', 'Seguridad', 'Security'),
    ('en', 'Seleccione un elemento.', 'Select an item.'),
    ('en', 'Seleccione un medio de pago.', 'Select a payment method.'),
    ('en', 'Seleccione un perfil existente.', 'Select an existing profile.'),
    ('en', 'Seleccione un perfil o una familia.', 'Select a profile or group.'),
    ('en', 'Seleccione un usuario.', 'Select a user.'),
    ('en', N'Seleccione una opción del menú', 'Select a menu option'),
    ('en', 'Seleccione una venta.', 'Select a sale.'),
    ('en', 'Servicio tecnico', 'Technical service'),
    ('en', N'Sistema de gestión comercial para BMTech', 'BMTech business management system'),
    ('en', 'Solo se cancela una venta pendiente.', 'Only a pending sale can be cancelled.'),
    ('en', 'Solo se emite comprobante de una venta pagada.', 'A receipt can only be issued for a paid sale.'),
    ('en', 'Solo un administrador puede regenerar integridad.', 'Only an administrator can recalculate integrity.'),
    ('en', 'SQL Server debe tener permiso de escritura en la carpeta. Conserve juntos el .bak y el .key en un lugar privado.', 'SQL Server needs write access to the folder. Keep the .bak and .key together in a private location.'),
    ('en', 'stock', 'Stock'),
    ('en', 'Subtotal', 'Subtotal'),
    ('en', 'Tareas registradas', 'Recorded work'),
    ('en', 'Tecnico asignado', 'Assigned technician'),
    ('en', 'telefono', 'Phone'),
    ('en', N'Teléfono', 'Phone'),
    ('en', 'Texto original', 'Original text'),
    ('en', 'Tipo de equipo', 'Device type'),
    ('en', 'Tipo de informe', 'Report type'),
    ('en', 'Todavia no hay comprobante.', 'There is no receipt yet.'),
    ('en', 'Total', 'Total'),
    ('en', 'Total: ', 'Total: ');
INSERT INTO Traduccion (codigoIdioma, etiqueta, textoTraducido) VALUES
    ('en', 'Total: 0,00', 'Total: 0.00'),
    ('en', 'Trabajo propuesto', 'Proposed work'),
    ('en', 'Traduccion', 'Translation'),
    ('en', 'Transferencia', 'Bank transfer'),
    ('en', 'Un producto no puede tener dos precios en la misma venta.', 'A product cannot have two prices in the same sale.'),
    ('en', 'Una familia no puede contenerse a si misma.', 'A group cannot contain itself.'),
    ('en', 'Usuario', 'User'),
    ('en', 'Usuario bloqueado por superar los intentos permitidos.', 'User blocked after exceeding the allowed attempts.'),
    ('en', 'Usuario modificado.', 'User updated.'),
    ('en', 'Usuario no encontrado.', 'User not found.'),
    ('en', 'Usuario registrado.', 'User registered.'),
    ('en', 'Usuarios', 'Users'),
    ('en', 'Valor', 'Value'),
    ('en', 'vendedor', 'Seller'),
    ('en', 'Vendedor: ', 'Seller: '),
    ('en', 'Venta pendiente guardada. Verifique el pago desde Historial de ventas.', 'Pending sale saved. Verify payment from Sales history.'),
    ('en', 'Venta registrada correctamente. Nro: ', 'Sale registered successfully. No: '),
    ('en', 'Ventas', 'Sales'),
    ('en', 'Ver comprobante', 'View receipt'),
    ('en', 'verificado', 'Verified'),
    ('en', 'Verificar integridad', 'Check integrity'),
    ('en', 'Verificar pago', 'Verify payment'),
    ('en', 'Verifique el pago por el importe exacto antes de continuar.', 'Verify payment for the exact amount before continuing.'),
    ('en', N'Versión seleccionada.', 'Version selected.'),
    ('en', 'Versiones registradas', 'Saved versions'),
    ('en', 'Vigencia', 'Valid until'),
    ('en', N'Ya existe un producto registrado con ese código.', 'A product with that code is already registered.'),
    ('en', 'Ya existe un usuario registrado con ese email.', 'A user with that email is already registered.'),
    ('en', N'Ya hay una sesión iniciada.', 'There is already an active session.'),
    ('es', '   Venta: ', '   Venta: '),
    ('es', N'¿Confirma registrar la venta por un total de $', N'¿Confirma registrar la venta por un total de $'),
    ('es', N'¿Confirma restaurar el producto a la versión seleccionada?', N'¿Confirma restaurar el producto a la versión seleccionada?'),
    ('es', 'Accesorios recibidos', 'Accesorios recibidos'),
    ('es', 'accion', 'accion'),
    ('es', N'Acción', N'Acción'),
    ('es', 'Activar/Desactivar', 'Activar/Desactivar'),
    ('es', 'Actividad', 'Actividad'),
    ('es', 'Activo', 'Activo'),
    ('es', 'Actualizar', 'Actualizar'),
    ('es', 'Agregar', 'Agregar'),
    ('es', 'Alta', 'Alta'),
    ('es', N'Alta, modificación y consulta de productos', N'Alta, modificación y consulta de productos'),
    ('es', 'Apellido', 'Apellido'),
    ('es', 'Aprobada', 'Aprobada'),
    ('es', 'Archivo guardado.', 'Archivo guardado.'),
    ('es', 'Asignacion', 'Asignacion'),
    ('es', 'Asignaciones guardadas.', 'Asignaciones guardadas.'),
    ('es', 'Asignar tecnico', 'Asignar tecnico'),
    ('es', 'Ayuda', 'Ayuda'),
    ('es', 'Base restaurada. Abra nuevamente el sistema para verificar integridad e iniciar sesion.', 'Base restaurada. Abra nuevamente el sistema para verificar integridad e iniciar sesion.');
INSERT INTO Traduccion (codigoIdioma, etiqueta, textoTraducido) VALUES
    ('es', 'Bitacora', 'Bitacora'),
    ('es', 'Bloquear', 'Bloquear'),
    ('es', 'BMTech - COMPROBANTE INTERNO (NO FISCAL)', 'BMTech - COMPROBANTE INTERNO (NO FISCAL)'),
    ('es', 'Buscar', 'Buscar'),
    ('es', 'Buscar cliente', 'Buscar cliente'),
    ('es', 'Buscar producto', 'Buscar producto'),
    ('es', 'Cada traduccion debe tener entre 1 y 250 caracteres.', 'Cada traduccion debe tener entre 1 y 250 caracteres.'),
    ('es', N'Cambiar contraseña', N'Cambiar contraseña'),
    ('es', 'Campo', 'Campo'),
    ('es', 'CANCELADA', 'CANCELADA'),
    ('es', 'Cancelar', 'Cancelar'),
    ('es', 'Cancelar venta', 'Cancelar venta'),
    ('es', 'Cantidad', 'Cantidad'),
    ('es', 'Cierre la sesion antes de preparar integridad.', 'Cierre la sesion antes de preparar integridad.'),
    ('es', 'Clave actual', 'Clave actual'),
    ('es', 'Clave inicial', 'Clave inicial'),
    ('es', 'Clave modificada.', 'Clave modificada.'),
    ('es', 'cliente', 'cliente'),
    ('es', 'Cliente encontrado.', 'Cliente encontrado.'),
    ('es', 'Cliente registrado correctamente.', 'Cliente registrado correctamente.'),
    ('es', 'Cliente seleccionado.', 'Cliente seleccionado.'),
    ('es', 'Cliente: ', 'Cliente: '),
    ('es', 'Clientes', 'Clientes'),
    ('es', 'Clientes encontrados.', 'Clientes encontrados.'),
    ('es', 'Cobrado', 'Cobrado'),
    ('es', 'Cobro y entrega', 'Cobro y entrega'),
    ('es', 'codigo', 'codigo'),
    ('es', N'Código', N'Código'),
    ('es', 'Codigo de idioma', 'Codigo de idioma'),
    ('es', 'Comprobante', 'Comprobante'),
    ('es', 'Comprobante de venta', 'Comprobante de venta'),
    ('es', 'Comprobante: ', 'Comprobante: '),
    ('es', 'Comunicado', 'Comunicado'),
    ('es', 'Confirma cancelar esta venta pendiente?', 'Confirma cancelar esta venta pendiente?'),
    ('es', 'Confirma eliminar ', 'Confirma eliminar '),
    ('es', 'Confirma la entrega de productos y comprobante?', 'Confirma la entrega de productos y comprobante?'),
    ('es', 'Confirma que entrego los productos y el comprobante?', 'Confirma que entrego los productos y el comprobante?'),
    ('es', 'Confirma registrar este cliente?', 'Confirma registrar este cliente?'),
    ('es', 'Confirmar', 'Confirmar'),
    ('es', 'Confirmar cambio de estado de ', 'Confirmar cambio de estado de '),
    ('es', 'Confirmar clave', 'Confirmar clave'),
    ('es', 'Confirmar cobro y entrega', 'Confirmar cobro y entrega'),
    ('es', 'Confirmar venta', 'Confirmar venta'),
    ('es', 'Consumido', 'Consumido'),
    ('es', N'Contraseña', N'Contraseña'),
    ('es', N'Contraseña incorrecta. Intentos restantes: ', N'Contraseña incorrecta. Intentos restantes: '),
    ('es', 'Control cambios producto', 'Control cambios producto'),
    ('es', 'Control de cambios de producto', 'Control de cambios de producto'),
    ('es', 'Copia creada: ', 'Copia creada: '),
    ('es', 'Copias de seguridad', 'Copias de seguridad');
INSERT INTO Traduccion (codigoIdioma, etiqueta, textoTraducido) VALUES
    ('es', N'Correo electrónico', N'Correo electrónico'),
    ('es', 'correoElectronico', 'correoElectronico'),
    ('es', 'Crear copia', 'Crear copia'),
    ('es', 'Credenciales incorrectas.', 'Credenciales incorrectas.'),
    ('es', 'criticidad', 'criticidad'),
    ('es', 'Datos del cliente', 'Datos del cliente'),
    ('es', 'Datos del producto', 'Datos del producto'),
    ('es', 'Datos guardados.', 'Datos guardados.'),
    ('es', 'Debe agregar al menos un producto a la venta.', 'Debe agregar al menos un producto a la venta.'),
    ('es', 'Debe indicar el pago.', 'Debe indicar el pago.'),
    ('es', 'Debe indicar un usuario activo para iniciar sesion.', 'Debe indicar un usuario activo para iniciar sesion.'),
    ('es', 'Debe ingresar el apellido del cliente.', 'Debe ingresar el apellido del cliente.'),
    ('es', 'Debe ingresar el apellido del usuario.', 'Debe ingresar el apellido del usuario.'),
    ('es', N'Debe ingresar el código del producto.', N'Debe ingresar el código del producto.'),
    ('es', N'Debe ingresar el correo electrónico del cliente.', N'Debe ingresar el correo electrónico del cliente.'),
    ('es', 'Debe ingresar el DNI del cliente.', 'Debe ingresar el DNI del cliente.'),
    ('es', 'Debe ingresar el email del usuario.', 'Debe ingresar el email del usuario.'),
    ('es', 'Debe ingresar el modelo del producto.', 'Debe ingresar el modelo del producto.'),
    ('es', 'Debe ingresar el nombre del cliente.', 'Debe ingresar el nombre del cliente.'),
    ('es', 'Debe ingresar el nombre del usuario.', 'Debe ingresar el nombre del usuario.'),
    ('es', 'Debe ingresar el perfil del usuario.', 'Debe ingresar el perfil del usuario.'),
    ('es', 'Debe ingresar el precio del producto.', 'Debe ingresar el precio del producto.'),
    ('es', 'Debe ingresar el stock del producto.', 'Debe ingresar el stock del producto.'),
    ('es', N'Debe ingresar el teléfono del cliente.', N'Debe ingresar el teléfono del cliente.'),
    ('es', N'Debe ingresar email y contraseña.', N'Debe ingresar email y contraseña.'),
    ('es', N'Debe ingresar la contraseña del usuario.', N'Debe ingresar la contraseña del usuario.'),
    ('es', N'Debe ingresar la descripción del producto.', N'Debe ingresar la descripción del producto.'),
    ('es', 'Debe ingresar la marca del producto.', 'Debe ingresar la marca del producto.'),
    ('es', 'Debe ingresar los datos de la venta.', 'Debe ingresar los datos de la venta.'),
    ('es', 'Debe ingresar los datos del cliente.', 'Debe ingresar los datos del cliente.'),
    ('es', 'Debe ingresar los datos del producto.', 'Debe ingresar los datos del producto.'),
    ('es', 'Debe ingresar los datos del usuario.', 'Debe ingresar los datos del usuario.'),
    ('es', 'Debe iniciar sesion para registrar una venta.', 'Debe iniciar sesion para registrar una venta.'),
    ('es', 'Debe iniciar sesion.', 'Debe iniciar sesion.'),
    ('es', 'Debe seleccionar un cliente.', 'Debe seleccionar un cliente.'),
    ('es', 'Debe seleccionar un producto para modificar.', 'Debe seleccionar un producto para modificar.'),
    ('es', N'Debe seleccionar un producto válido.', N'Debe seleccionar un producto válido.'),
    ('es', 'Debe seleccionar un producto.', 'Debe seleccionar un producto.'),
    ('es', N'Debe seleccionar una versión.', N'Debe seleccionar una versión.'),
    ('es', 'Desbloquear', 'Desbloquear'),
    ('es', 'descripcion', 'descripcion'),
    ('es', N'Descripción', N'Descripción'),
    ('es', 'Desde', 'Desde'),
    ('es', 'Detalle de venta', 'Detalle de venta'),
    ('es', 'Detalle del cambio', 'Detalle del cambio'),
    ('es', 'Diagnostico', 'Diagnostico'),
    ('es', 'Dias estimados', 'Dias estimados'),
    ('es', 'Diferencias en: ', 'Diferencias en: '),
    ('es', 'digitoVerificador', 'digitoVerificador'),
    ('es', 'DNI', 'DNI');
INSERT INTO Traduccion (codigoIdioma, etiqueta, textoTraducido) VALUES
    ('es', 'DNI cliente', 'DNI cliente'),
    ('es', 'DNI del cliente', 'DNI del cliente'),
    ('es', 'dniCliente', 'dniCliente'),
    ('es', 'Efectivo', 'Efectivo'),
    ('es', 'El cliente ya se encuentra registrado.', 'El cliente ya se encuentra registrado.'),
    ('es', 'El codigo del idioma debe tener dos letras minusculas.', 'El codigo del idioma debe tener dos letras minusculas.'),
    ('es', 'El correo electronico no es valido.', 'El correo electronico no es valido.'),
    ('es', 'El detalle no puede ser nulo.', 'El detalle no puede ser nulo.'),
    ('es', 'El DNI debe contener solo numeros.', 'El DNI debe contener solo numeros.'),
    ('es', 'El importe del pago no es valido.', 'El importe del pago no es valido.'),
    ('es', 'El numero de operacion es demasiado largo.', 'El numero de operacion es demasiado largo.'),
    ('es', 'El pago no esta verificado o el importe no coincide. Guardar venta pendiente?', 'El pago no esta verificado o el importe no coincide. Guardar venta pendiente?'),
    ('es', 'El precio debe ser mayor a cero.', 'El precio debe ser mayor a cero.'),
    ('es', 'El precio debe tener como maximo dos decimales.', 'El precio debe tener como maximo dos decimales.'),
    ('es', 'El precio del producto debe ser mayor a cero.', 'El precio del producto debe ser mayor a cero.'),
    ('es', 'El producto cambio de precio, no esta activo o no tiene stock suficiente.', 'El producto cambio de precio, no esta activo o no tiene stock suficiente.'),
    ('es', 'El rango de fechas es incorrecto.', 'El rango de fechas es incorrecto.'),
    ('es', 'El stock no puede ser negativo.', 'El stock no puede ser negativo.'),
    ('es', 'El total supera el importe permitido.', 'El total supera el importe permitido.'),
    ('es', 'El usuario no esta activo.', 'El usuario no esta activo.'),
    ('es', 'El usuario se encuentra bloqueado.', 'El usuario se encuentra bloqueado.'),
    ('es', 'El vendedor no esta activo.', 'El vendedor no esta activo.'),
    ('es', 'Eliminar', 'Eliminar'),
    ('es', 'email', 'email'),
    ('es', 'estado', 'estado'),
    ('es', 'Estado del producto actualizado correctamente.', 'Estado del producto actualizado correctamente.'),
    ('es', 'Esto acepta los datos actuales como correctos. Revise la base antes de continuar.', 'Esto acepta los datos actuales como correctos. Revise la base antes de continuar.'),
    ('es', 'Estructura de permisos', 'Estructura de permisos'),
    ('es', 'Exportar XML', 'Exportar XML'),
    ('es', 'Falla detectada', 'Falla detectada'),
    ('es', 'Faltan el archivo .bak o su archivo .key.', 'Faltan el archivo .bak o su archivo .key.'),
    ('es', 'Faltan las traducciones.', 'Faltan las traducciones.'),
    ('es', 'Familia', 'Familia'),
    ('es', 'Fecha', 'Fecha'),
    ('es', 'Fecha: ', 'Fecha: '),
    ('es', 'fechaEntrega', 'fechaEntrega'),
    ('es', 'FINALIZADA', 'FINALIZADA'),
    ('es', 'Garantia del trabajo', 'Garantia del trabajo'),
    ('es', 'Generar presupuesto', 'Generar presupuesto'),
    ('es', N'Gestión de clientes de BMTech', N'Gestión de clientes de BMTech'),
    ('es', N'Gestión de productos', N'Gestión de productos'),
    ('es', 'Guardar', 'Guardar'),
    ('es', 'Guardar asignaciones', 'Guardar asignaciones'),
    ('es', 'Guardar datos', 'Guardar datos'),
    ('es', 'Guardar diagnostico', 'Guardar diagnostico'),
    ('es', 'Guardar PDF', 'Guardar PDF'),
    ('es', 'Hasta', 'Hasta'),
    ('es', 'Historial', 'Historial'),
    ('es', 'Historial actualizado.', 'Historial actualizado.'),
    ('es', N'Historial de modificaciones y restauración de producto', N'Historial de modificaciones y restauración de producto');
INSERT INTO Traduccion (codigoIdioma, etiqueta, textoTraducido) VALUES
    ('es', 'Historial de ventas', 'Historial de ventas'),
    ('es', 'id', 'id'),
    ('es', 'idBackup', 'idBackup'),
    ('es', 'Identificador', 'Identificador'),
    ('es', 'Idioma no disponible.', 'Idioma no disponible.'),
    ('es', 'Idiomas', 'Idiomas'),
    ('es', 'Idiomas y traducciones', 'Idiomas y traducciones'),
    ('es', 'idLog', 'idLog'),
    ('es', 'idperfil', 'idperfil'),
    ('es', 'idVenta', 'idVenta'),
    ('es', 'Importe', 'Importe'),
    ('es', 'Imprimir', 'Imprimir'),
    ('es', 'Indique el numero de operacion de la transferencia.', 'Indique el numero de operacion de la transferencia.'),
    ('es', 'Indique un nombre de idioma valido.', 'Indique un nombre de idioma valido.'),
    ('es', 'Informes', 'Informes'),
    ('es', 'Informes de operaciones', 'Informes de operaciones'),
    ('es', 'Ingresar', 'Ingresar'),
    ('es', 'Ingrese la nueva clave.', 'Ingrese la nueva clave.'),
    ('es', 'Ingrese las credenciales de un administrador activo.', 'Ingrese las credenciales de un administrador activo.'),
    ('es', 'Ingreso', 'Ingreso'),
    ('es', N'Inicio de sesión', N'Inicio de sesión'),
    ('es', 'Integridad', 'Integridad'),
    ('es', 'Integridad preparada. Ahora puede ingresar.', 'Integridad preparada. Ahora puede ingresar.'),
    ('es', 'intentos', 'intentos'),
    ('es', 'La cantidad debe ser mayor a cero.', 'La cantidad debe ser mayor a cero.'),
    ('es', 'La carpeta no existe.', 'La carpeta no existe.'),
    ('es', 'La clave actual es incorrecta.', 'La clave actual es incorrecta.'),
    ('es', 'La clave de la copia no es valida.', 'La clave de la copia no es valida.'),
    ('es', 'La clave debe tener entre 1 y 72 bytes.', 'La clave debe tener entre 1 y 72 bytes.'),
    ('es', 'La clave es demasiado larga.', 'La clave es demasiado larga.'),
    ('es', 'La integridad de Seguridad no esta preparada o presenta diferencias.', 'La integridad de Seguridad no esta preparada o presenta diferencias.'),
    ('es', 'La integridad de Seguridad presenta diferencias.', 'La integridad de Seguridad presenta diferencias.'),
    ('es', 'La integridad de Usuario presenta diferencias.', 'La integridad de Usuario presenta diferencias.'),
    ('es', 'La integridad del sistema es correcta.', 'La integridad del sistema es correcta.'),
    ('es', 'La integridad del sistema fue regenerada.', 'La integridad del sistema fue regenerada.'),
    ('es', 'La integridad ya fue preparada. Para regenerarla ingrese como administrador.', 'La integridad ya fue preparada. Para regenerarla ingrese como administrador.'),
    ('es', 'La operacion puede haberse guardado, pero fallo la bitacora: ', 'La operacion puede haberse guardado, pero fallo la bitacora: '),
    ('es', 'La ruta de backup no es valida.', 'La ruta de backup no es valida.'),
    ('es', 'La venta debe estar pagada y tener comprobante antes de entregar.', 'La venta debe estar pagada y tener comprobante antes de entregar.'),
    ('es', 'La venta no esta pendiente o el importe no coincide.', 'La venta no esta pendiente o el importe no coincide.'),
    ('es', 'La venta no esta pendiente.', 'La venta no esta pendiente.'),
    ('es', 'La venta no existe.', 'La venta no existe.'),
    ('es', 'La venta quedo pagada. Puede reintentar el comprobante o la entrega desde Historial de ventas. ', 'La venta quedo pagada. Puede reintentar el comprobante o la entrega desde Historial de ventas. '),
    ('es', 'Las claves no coinciden.', 'Las claves no coinciden.'),
    ('es', 'Limpiar', 'Limpiar'),
    ('es', 'Los datos del cliente superan el largo permitido.', 'Los datos del cliente superan el largo permitido.'),
    ('es', 'Los datos del producto superan el largo permitido.', 'Los datos del producto superan el largo permitido.'),
    ('es', 'Los datos del usuario superan el largo permitido.', 'Los datos del usuario superan el largo permitido.'),
    ('es', 'Maestros', 'Maestros'),
    ('es', 'Mano de obra', 'Mano de obra');
INSERT INTO Traduccion (codigoIdioma, etiqueta, textoTraducido) VALUES
    ('es', 'Marca', 'Marca'),
    ('es', 'Medio de pago', 'Medio de pago'),
    ('es', 'Medio por el que se aviso al cliente', 'Medio por el que se aviso al cliente'),
    ('es', 'medioPago', 'medioPago'),
    ('es', 'Modelo', 'Modelo'),
    ('es', 'Modificar', 'Modificar'),
    ('es', 'modulo', 'modulo'),
    ('es', N'Mostrar contraseña', N'Mostrar contraseña'),
    ('es', 'No hay stock suficiente.', 'No hay stock suficiente.'),
    ('es', 'No puede bloquear su propia cuenta.', 'No puede bloquear su propia cuenta.'),
    ('es', 'No puede cambiar su propio perfil durante la sesion.', 'No puede cambiar su propio perfil durante la sesion.'),
    ('es', 'No puede eliminar un idioma base o el idioma en uso.', 'No puede eliminar un idioma base o el idioma en uso.'),
    ('es', 'No se encontraron productos.', 'No se encontraron productos.'),
    ('es', N'No se encontró el producto seleccionado.', N'No se encontró el producto seleccionado.'),
    ('es', N'No se encontró un cliente con ese DNI.', N'No se encontró un cliente con ese DNI.'),
    ('es', 'No se permite ingresar sin registrar el inicio de sesion.', 'No se permite ingresar sin registrar el inicio de sesion.'),
    ('es', 'No se pudo actualizar el usuario.', 'No se pudo actualizar el usuario.'),
    ('es', 'No se pudo cambiar la clave.', 'No se pudo cambiar la clave.'),
    ('es', 'No se pudo cargar el idioma. Revise el script de idiomas. ', 'No se pudo cargar el idioma. Revise el script de idiomas. '),
    ('es', 'No se pudo modificar.', 'No se pudo modificar.'),
    ('es', 'No se pudo registrar el cierre de sesion. ', 'No se pudo registrar el cierre de sesion. '),
    ('es', 'No se pudo registrar el cliente.', 'No se pudo registrar el cliente.'),
    ('es', 'No se pudo registrar.', 'No se pudo registrar.'),
    ('es', 'No se pudo restaurar el producto.', 'No se pudo restaurar el producto.'),
    ('es', 'No tiene permiso para esta operacion.', 'No tiene permiso para esta operacion.'),
    ('es', 'Nombre', 'Nombre'),
    ('es', 'Nueva clave', 'Nueva clave'),
    ('es', 'Nuevas tareas realizadas', 'Nuevas tareas realizadas'),
    ('es', 'Nuevo', 'Nuevo'),
    ('es', 'Nuevo consumo', 'Nuevo consumo'),
    ('es', 'Nuevo ingreso', 'Nuevo ingreso'),
    ('es', 'Numero', 'Numero'),
    ('es', 'Numero de operacion', 'Numero de operacion'),
    ('es', 'Numero de operacion de transferencia', 'Numero de operacion de transferencia'),
    ('es', 'Numero de serie (si dispone)', 'Numero de serie (si dispone)'),
    ('es', 'numeroOperacion', 'numeroOperacion'),
    ('es', 'Observaciones de ingreso', 'Observaciones de ingreso'),
    ('es', 'Observaciones de la respuesta', 'Observaciones de la respuesta'),
    ('es', 'Operacion guardada, bitacora pendiente.', 'Operacion guardada, bitacora pendiente.'),
    ('es', 'Orden', 'Orden'),
    ('es', 'PAGADA', 'PAGADA'),
    ('es', 'Pago de la venta', 'Pago de la venta'),
    ('es', 'Pago pendiente', 'Pago pendiente'),
    ('es', 'Pago verificado por el empleado', 'Pago verificado por el empleado'),
    ('es', 'Pagos', 'Pagos'),
    ('es', 'Patente', 'Patente'),
    ('es', 'PENDIENTE', 'PENDIENTE'),
    ('es', 'Perfil', 'Perfil'),
    ('es', 'Perfiles, familias y patentes', 'Perfiles, familias y patentes'),
    ('es', 'Permisos', 'Permisos');
INSERT INTO Traduccion (codigoIdioma, etiqueta, textoTraducido) VALUES
    ('es', 'Persona que retira', 'Persona que retira'),
    ('es', 'Precio', 'Precio'),
    ('es', 'Precio acordado', 'Precio acordado'),
    ('es', 'precioUnitario', 'precioUnitario'),
    ('es', 'Preparar integridad', 'Preparar integridad'),
    ('es', 'Presupuesto', 'Presupuesto'),
    ('es', 'Presupuestos guardados', 'Presupuestos guardados'),
    ('es', 'Prioridad', 'Prioridad'),
    ('es', 'Problema informado', 'Problema informado'),
    ('es', 'Producto', 'Producto'),
    ('es', 'Producto agregado a la venta.', 'Producto agregado a la venta.'),
    ('es', 'Producto modificado correctamente.', 'Producto modificado correctamente.'),
    ('es', 'Producto registrado correctamente.', 'Producto registrado correctamente.'),
    ('es', 'Producto restaurado correctamente.', 'Producto restaurado correctamente.'),
    ('es', 'Producto seleccionado.', 'Producto seleccionado.'),
    ('es', 'Productos', 'Productos'),
    ('es', 'Productos encontrados.', 'Productos encontrados.'),
    ('es', 'Pruebas realizadas y resultados', 'Pruebas realizadas y resultados'),
    ('es', 'Pruebas satisfactorias', 'Pruebas satisfactorias'),
    ('es', 'Pruebas y aviso', 'Pruebas y aviso'),
    ('es', 'Pulse Nuevo para registrar otro usuario.', 'Pulse Nuevo para registrar otro usuario.'),
    ('es', 'Quitar producto', 'Quitar producto'),
    ('es', 'Rechazada', 'Rechazada'),
    ('es', 'Regenerar integridad', 'Regenerar integridad'),
    ('es', 'Registrar', 'Registrar'),
    ('es', 'Registrar aviso realizado', 'Registrar aviso realizado'),
    ('es', 'Registrar cliente', 'Registrar cliente'),
    ('es', 'Registrar comunicacion', 'Registrar comunicacion'),
    ('es', 'Registrar entrega', 'Registrar entrega'),
    ('es', 'Registrar ingreso', 'Registrar ingreso'),
    ('es', 'Registrar pruebas', 'Registrar pruebas'),
    ('es', 'Registrar respuesta', 'Registrar respuesta'),
    ('es', 'Registrar tareas y consumos', 'Registrar tareas y consumos'),
    ('es', 'Registrar venta', 'Registrar venta'),
    ('es', 'Registro de ventas', 'Registro de ventas'),
    ('es', 'Registro de ventas, productos y cantidades', 'Registro de ventas, productos y cantidades'),
    ('es', 'Reparacion', 'Reparacion'),
    ('es', 'Repuesto', 'Repuesto'),
    ('es', 'Repuesto utilizado', 'Repuesto utilizado'),
    ('es', 'Repuestos previstos', 'Repuestos previstos'),
    ('es', 'Respuesta', 'Respuesta'),
    ('es', 'Restaurar copia', 'Restaurar copia'),
    ('es', 'Restaurar producto', 'Restaurar producto'),
    ('es', 'Restaurar reemplaza los datos actuales por los de esta copia y cierra el sistema. Haga antes una copia actual. Continuar?', 'Restaurar reemplaza los datos actuales por los de esta copia y cierra el sistema. Haga antes una copia actual. Continuar?'),
    ('es', N'Restaurar versión', N'Restaurar versión'),
    ('es', 'Revise el precio y el stock del producto.', 'Revise el precio y el stock del producto.'),
    ('es', 'Revise integridad antes de operar. Diferencias en: ', 'Revise integridad antes de operar. Diferencias en: '),
    ('es', 'Revise la conexion con la base de datos. ', 'Revise la conexion con la base de datos. '),
    ('es', 'Revise la integridad antes de hacer una copia.', 'Revise la integridad antes de hacer una copia.'),
    ('es', 'Revise la integridad de los pagos.', 'Revise la integridad de los pagos.');
INSERT INTO Traduccion (codigoIdioma, etiqueta, textoTraducido) VALUES
    ('es', 'ruta', 'ruta'),
    ('es', 'Salir', 'Salir'),
    ('es', 'Se recalcularan los digitos con los datos actuales. Continuar solo si reviso que son correctos.', 'Se recalcularan los digitos con los datos actuales. Continuar solo si reviso que son correctos.'),
    ('es', 'Seguridad', 'Seguridad'),
    ('es', 'Seleccione un elemento.', 'Seleccione un elemento.'),
    ('es', 'Seleccione un medio de pago.', 'Seleccione un medio de pago.'),
    ('es', 'Seleccione un perfil existente.', 'Seleccione un perfil existente.'),
    ('es', 'Seleccione un perfil o una familia.', 'Seleccione un perfil o una familia.'),
    ('es', 'Seleccione un usuario.', 'Seleccione un usuario.'),
    ('es', N'Seleccione una opción del menú', N'Seleccione una opción del menú'),
    ('es', 'Seleccione una venta.', 'Seleccione una venta.'),
    ('es', 'Servicio tecnico', 'Servicio tecnico'),
    ('es', N'Sistema de gestión comercial para BMTech', N'Sistema de gestión comercial para BMTech'),
    ('es', 'Solo se cancela una venta pendiente.', 'Solo se cancela una venta pendiente.'),
    ('es', 'Solo se emite comprobante de una venta pagada.', 'Solo se emite comprobante de una venta pagada.'),
    ('es', 'Solo un administrador puede regenerar integridad.', 'Solo un administrador puede regenerar integridad.'),
    ('es', 'SQL Server debe tener permiso de escritura en la carpeta. Conserve juntos el .bak y el .key en un lugar privado.', 'SQL Server debe tener permiso de escritura en la carpeta. Conserve juntos el .bak y el .key en un lugar privado.'),
    ('es', 'stock', 'stock'),
    ('es', 'Subtotal', 'Subtotal'),
    ('es', 'Tareas registradas', 'Tareas registradas'),
    ('es', 'Tecnico asignado', 'Tecnico asignado'),
    ('es', 'telefono', 'telefono'),
    ('es', N'Teléfono', N'Teléfono'),
    ('es', 'Texto original', 'Texto original'),
    ('es', 'Tipo de equipo', 'Tipo de equipo'),
    ('es', 'Tipo de informe', 'Tipo de informe'),
    ('es', 'Todavia no hay comprobante.', 'Todavia no hay comprobante.'),
    ('es', 'Total', 'Total'),
    ('es', 'Total: ', 'Total: '),
    ('es', 'Total: 0,00', 'Total: 0,00'),
    ('es', 'Trabajo propuesto', 'Trabajo propuesto'),
    ('es', 'Traduccion', 'Traduccion'),
    ('es', 'Transferencia', 'Transferencia'),
    ('es', 'Un producto no puede tener dos precios en la misma venta.', 'Un producto no puede tener dos precios en la misma venta.'),
    ('es', 'Una familia no puede contenerse a si misma.', 'Una familia no puede contenerse a si misma.'),
    ('es', 'Usuario', 'Usuario'),
    ('es', 'Usuario bloqueado por superar los intentos permitidos.', 'Usuario bloqueado por superar los intentos permitidos.'),
    ('es', 'Usuario modificado.', 'Usuario modificado.'),
    ('es', 'Usuario no encontrado.', 'Usuario no encontrado.'),
    ('es', 'Usuario registrado.', 'Usuario registrado.'),
    ('es', 'Usuarios', 'Usuarios'),
    ('es', 'Valor', 'Valor'),
    ('es', 'vendedor', 'vendedor'),
    ('es', 'Vendedor: ', 'Vendedor: '),
    ('es', 'Venta pendiente guardada. Verifique el pago desde Historial de ventas.', 'Venta pendiente guardada. Verifique el pago desde Historial de ventas.'),
    ('es', 'Venta registrada correctamente. Nro: ', 'Venta registrada correctamente. Nro: '),
    ('es', 'Ventas', 'Ventas'),
    ('es', 'Ver comprobante', 'Ver comprobante'),
    ('es', 'verificado', 'verificado'),
    ('es', 'Verificar integridad', 'Verificar integridad');
INSERT INTO Traduccion (codigoIdioma, etiqueta, textoTraducido) VALUES
    ('es', 'Verificar pago', 'Verificar pago'),
    ('es', 'Verifique el pago por el importe exacto antes de continuar.', 'Verifique el pago por el importe exacto antes de continuar.'),
    ('es', N'Versión seleccionada.', N'Versión seleccionada.'),
    ('es', 'Versiones registradas', 'Versiones registradas'),
    ('es', 'Vigencia', 'Vigencia'),
    ('es', N'Ya existe un producto registrado con ese código.', N'Ya existe un producto registrado con ese código.'),
    ('es', 'Ya existe un usuario registrado con ese email.', 'Ya existe un usuario registrado con ese email.'),
    ('es', N'Ya hay una sesión iniciada.', N'Ya hay una sesión iniciada.');
GO
INSERT INTO Perfil (idperfil) VALUES
    ('Administrador'),
    ('Vendedor');
GO
INSERT INTO Patente (idPatente, nombre) VALUES
    ('CLIENTES', N'Gestión de clientes'),
    ('PRODUCTOS', N'Gestión de productos'),
    ('SEGURIDAD', 'Seguridad'),
    ('SERVICIOS', 'Servicio tecnico'),
    ('VENTAS', 'Registro de ventas');
GO
INSERT INTO Familia (idFamilia, nombre) VALUES
    ('ADMINISTRACION', N'Administración del sistema'),
    ('VENTA', 'Operaciones de venta');
GO
INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES
    ('ADMINISTRACION', 'CLIENTES'),
    ('ADMINISTRACION', 'PRODUCTOS'),
    ('ADMINISTRACION', 'SEGURIDAD'),
    ('ADMINISTRACION', 'SERVICIOS'),
    ('ADMINISTRACION', 'VENTAS'),
    ('VENTA', 'CLIENTES'),
    ('VENTA', 'PRODUCTOS'),
    ('VENTA', 'SERVICIOS'),
    ('VENTA', 'VENTAS');
GO
INSERT INTO PerfilFamilia (idperfil, idFamilia) VALUES
    ('Administrador', 'ADMINISTRACION'),
    ('Vendedor', 'VENTA');
GO
INSERT INTO Usuario (email, nombre, apellido, password, activo, intentos, idperfil) VALUES
    ('admin@bmtech.com', 'Usuario', 'Administrador', '$2a$12$J2MsQcFQ66m5S2Gq4tE65OrkTpWQ5MUb2yzvYrBqR42npqmoWmZUW', 1, 3, 'Administrador'),
    ('vendedor@bmtech.com', 'Usuario', 'Vendedor', '$2a$12$J2MsQcFQ66m5S2Gq4tE65OrkTpWQ5MUb2yzvYrBqR42npqmoWmZUW', 1, 3, 'Vendedor');
GO
INSERT INTO DigitoVerificador (tabla, digitoVertical) VALUES
    ('AvisoRetiro', -1),
    ('Backup', -1),
    ('Cliente', -1),
    ('Comprobante', -1),
    ('ComprobanteServicio', -1),
    ('DetallePresupuesto', -1),
    ('DetalleVenta', -1),
    ('Diagnostico', -1),
    ('EntregaServicio', -1),
    ('Equipo', -1),
    ('Etiqueta', -1),
    ('Familia', -1),
    ('FamiliaPatente', -1),
    ('HistorialCambiosProducto', -1),
    ('Idioma', -1),
    ('LogEventos', -1),
    ('OrdenServicio', -1),
    ('Pago', -1),
    ('PagoServicio', -1),
    ('Patente', -1),
    ('Perfil', -1),
    ('PerfilFamilia', -1),
    ('PerfilPatente', -1),
    ('PerfilPerfil', -1),
    ('Presupuesto', -1),
    ('Producto', -1),
    ('PruebaEquipo', -1),
    ('Reparacion', -1),
    ('RepuestoServicio', -1),
    ('Traduccion', -1),
    ('Usuario', -1),
    ('Venta', -1);
GO
