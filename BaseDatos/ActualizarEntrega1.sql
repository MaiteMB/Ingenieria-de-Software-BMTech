
USE BMTech;
GO
IF OBJECT_ID('Idioma', 'U') IS NULL
    CREATE TABLE Idioma
    (
        codigo VARCHAR(2) NOT NULL PRIMARY KEY,
        nombre NVARCHAR(50) NOT NULL
    );
GO
IF OBJECT_ID('Etiqueta', 'U') IS NULL
    CREATE TABLE Etiqueta
    (
        etiqueta NVARCHAR(200) NOT NULL PRIMARY KEY
    );
GO
IF OBJECT_ID('Traduccion', 'U') IS NULL
    CREATE TABLE Traduccion
    (
        codigoIdioma VARCHAR(2) NOT NULL,
        etiqueta NVARCHAR(200) NOT NULL,
        textoTraducido NVARCHAR(250) NOT NULL,
        CONSTRAINT PK_Traduccion PRIMARY KEY (codigoIdioma, etiqueta),
        CONSTRAINT FK_Traduccion_Idioma FOREIGN KEY (codigoIdioma) REFERENCES Idioma(codigo),
        CONSTRAINT FK_Traduccion_Etiqueta FOREIGN KEY (etiqueta) REFERENCES Etiqueta(etiqueta)
    );
GO
IF NOT EXISTS (SELECT 1 FROM Idioma WHERE codigo='es')
    INSERT INTO Idioma VALUES ('es', N'Español');
IF NOT EXISTS (SELECT 1 FROM Idioma WHERE codigo='en')
    INSERT INTO Idioma VALUES ('en', N'English');
GO

DECLARE @textos TABLE (etiqueta NVARCHAR(200), ingles NVARCHAR(250));
INSERT INTO @textos VALUES
    (N'Registrar cliente', N'Register customer'),
    (N'Gestión de clientes de BMTech', N'BMTech customer management'),
    (N'Datos del cliente', N'Customer information'),
    (N'DNI', N'ID number'),
    (N'Nombre', N'First name'),
    (N'Apellido', N'Last name'),
    (N'Teléfono', N'Phone'),
    (N'Correo electrónico', N'Email'),
    (N'Buscar cliente', N'Find customer'),
    (N'Limpiar', N'Clear'),
    (N'Historial de modificaciones y restauración de producto', N'Product history and restoration'),
    (N'Control de cambios de producto', N'Product change history'),
    (N'Versiones registradas', N'Saved versions'),
    (N'Detalle del cambio', N'Change details'),
    (N'Actualizar', N'Refresh'),
    (N'Restaurar versión', N'Restore version'),
    (N'Sistema de gestión comercial para BMTech', N'BMTech business management system'),
    (N'Mostrar contraseña', N'Show password'),
    (N'Salir', N'Exit'),
    (N'Ingresar', N'Sign in'),
    (N'Contraseña', N'Password'),
    (N'Inicio de sesión', N'Sign in'),
    (N'Clientes', N'Customers'),
    (N'Productos', N'Products'),
    (N'Ventas', N'Sales'),
    (N'Seguridad', N'Security'),
    (N'Control cambios producto', N'Product change history'),
    (N'Verificar integridad', N'Check integrity'),
    (N'Regenerar integridad', N'Recalculate integrity'),
    (N'Seleccione una opción del menú', N'Select a menu option'),
    (N'Gestión de productos', N'Product management'),
    (N'Alta, modificación y consulta de productos', N'Add, edit and find products'),
    (N'Datos del producto', N'Product information'),
    (N'Código', N'Code'),
    (N'Descripción', N'Description'),
    (N'Marca', N'Brand'),
    (N'Modelo', N'Model'),
    (N'Precio', N'Price'),
    (N'Buscar producto', N'Find product'),
    (N'Buscar', N'Search'),
    (N'Registrar', N'Register'),
    (N'Modificar', N'Edit'),
    (N'Activar/Desactivar', N'Enable/Disable'),
    (N'Registro de ventas, productos y cantidades', N'Sales, products and quantities'),
    (N'Registro de ventas', N'Sales registration'),
    (N'DNI cliente', N'Customer ID'),
    (N'Agregar', N'Add'),
    (N'Cantidad', N'Quantity'),
    (N'Detalle de venta', N'Sale details'),
    (N'Registrar venta', N'Register sale'),
    (N'Usuarios', N'Users'),
    (N'Bitacora', N'Event log'),
    (N'Cambiar contraseña', N'Change password'),
    (N'Clave actual', N'Current password'),
    (N'Nueva clave', N'New password'),
    (N'Confirmar clave', N'Confirm password'),
    (N'Guardar', N'Save'),
    (N'Perfiles, familias y patentes', N'Profiles, groups and permissions'),
    (N'Identificador', N'Identifier'),
    (N'Nuevo', N'New'),
    (N'Guardar datos', N'Save information'),
    (N'Guardar asignaciones', N'Save assignments'),
    (N'Permisos', N'Permissions'),
    (N'Clave inicial', N'Initial password'),
    (N'Perfil', N'Profile'),
    (N'Bloquear', N'Block'),
    (N'Desbloquear', N'Unblock'),
    (N'Subtotal', N'Subtotal'),
    (N'Activo', N'Enabled'),
    (N'Campo', N'Field'),
    (N'Valor', N'Value'),
    (N'Fecha', N'Date'),
    (N'Usuario', N'User'),
    (N'Acción', N'Action'),
    (N'codigo', N'Code'),
    (N'descripcion', N'Description'),
    (N'stock', N'Stock'),
    (N'email', N'Email'),
    (N'intentos', N'Attempts'),
    (N'telefono', N'Phone'),
    (N'correoElectronico', N'Email'),
    (N'digitoVerificador', N'Check digit'),
    (N'idLog', N'Event ID'),
    (N'accion', N'Action'),
    (N'modulo', N'Module'),
    (N'criticidad', N'Severity'),
    (N'id', N'Identifier');

INSERT INTO Etiqueta (etiqueta)
SELECT etiqueta FROM @textos t
WHERE NOT EXISTS (SELECT 1 FROM Etiqueta e WHERE e.etiqueta=t.etiqueta);

INSERT INTO Traduccion (codigoIdioma, etiqueta, textoTraducido)
SELECT 'es', etiqueta, etiqueta FROM @textos t
WHERE NOT EXISTS (SELECT 1 FROM Traduccion r WHERE r.codigoIdioma='es' AND r.etiqueta=t.etiqueta);

INSERT INTO Traduccion (codigoIdioma, etiqueta, textoTraducido)
SELECT 'en', etiqueta, ingles FROM @textos t
WHERE NOT EXISTS (SELECT 1 FROM Traduccion r WHERE r.codigoIdioma='en' AND r.etiqueta=t.etiqueta);
GO

IF COL_LENGTH('Cliente', 'digitoVerificador') IS NULL
    ALTER TABLE [Cliente] ADD digitoVerificador INT NOT NULL DEFAULT 0;
GO
IF COL_LENGTH('Producto', 'digitoVerificador') IS NULL
    ALTER TABLE [Producto] ADD digitoVerificador INT NOT NULL DEFAULT 0;
GO
IF COL_LENGTH('Usuario', 'digitoVerificador') IS NULL
    ALTER TABLE [Usuario] ADD digitoVerificador INT NOT NULL DEFAULT 0;
GO
IF COL_LENGTH('Perfil', 'digitoVerificador') IS NULL
    ALTER TABLE [Perfil] ADD digitoVerificador INT NOT NULL DEFAULT 0;
GO
IF COL_LENGTH('Familia', 'digitoVerificador') IS NULL
    ALTER TABLE [Familia] ADD digitoVerificador INT NOT NULL DEFAULT 0;
GO
IF COL_LENGTH('Patente', 'digitoVerificador') IS NULL
    ALTER TABLE [Patente] ADD digitoVerificador INT NOT NULL DEFAULT 0;
GO
IF COL_LENGTH('FamiliaPatente', 'digitoVerificador') IS NULL
    ALTER TABLE [FamiliaPatente] ADD digitoVerificador INT NOT NULL DEFAULT 0;
GO
IF COL_LENGTH('PerfilFamilia', 'digitoVerificador') IS NULL
    ALTER TABLE [PerfilFamilia] ADD digitoVerificador INT NOT NULL DEFAULT 0;
GO
IF COL_LENGTH('LogEventos', 'digitoVerificador') IS NULL
    ALTER TABLE [LogEventos] ADD digitoVerificador INT NOT NULL DEFAULT 0;
GO
IF COL_LENGTH('Venta', 'digitoVerificador') IS NULL
    ALTER TABLE [Venta] ADD digitoVerificador INT NOT NULL DEFAULT 0;
GO
IF COL_LENGTH('DetalleVenta', 'digitoVerificador') IS NULL
    ALTER TABLE [DetalleVenta] ADD digitoVerificador INT NOT NULL DEFAULT 0;
GO
IF COL_LENGTH('HistorialCambiosProducto', 'digitoVerificador') IS NULL
    ALTER TABLE [HistorialCambiosProducto] ADD digitoVerificador INT NOT NULL DEFAULT 0;
GO
IF COL_LENGTH('Idioma', 'digitoVerificador') IS NULL
    ALTER TABLE [Idioma] ADD digitoVerificador INT NOT NULL DEFAULT 0;
GO
IF COL_LENGTH('Etiqueta', 'digitoVerificador') IS NULL
    ALTER TABLE [Etiqueta] ADD digitoVerificador INT NOT NULL DEFAULT 0;
GO
IF COL_LENGTH('Traduccion', 'digitoVerificador') IS NULL
    ALTER TABLE [Traduccion] ADD digitoVerificador INT NOT NULL DEFAULT 0;
GO
IF OBJECT_ID('DigitoVerificador', 'U') IS NULL
    CREATE TABLE DigitoVerificador
    (
        tabla VARCHAR(50) NOT NULL PRIMARY KEY,
        digitoVertical INT NOT NULL
    );
GO
IF NOT EXISTS (SELECT 1 FROM DigitoVerificador WHERE tabla='Cliente')
    INSERT INTO DigitoVerificador(tabla,digitoVertical) VALUES('Cliente',-1);
IF NOT EXISTS (SELECT 1 FROM DigitoVerificador WHERE tabla='Producto')
    INSERT INTO DigitoVerificador(tabla,digitoVertical) VALUES('Producto',-1);
IF NOT EXISTS (SELECT 1 FROM DigitoVerificador WHERE tabla='Usuario')
    INSERT INTO DigitoVerificador(tabla,digitoVertical) VALUES('Usuario',-1);
IF NOT EXISTS (SELECT 1 FROM DigitoVerificador WHERE tabla='Perfil')
    INSERT INTO DigitoVerificador(tabla,digitoVertical) VALUES('Perfil',-1);
IF NOT EXISTS (SELECT 1 FROM DigitoVerificador WHERE tabla='Familia')
    INSERT INTO DigitoVerificador(tabla,digitoVertical) VALUES('Familia',-1);
IF NOT EXISTS (SELECT 1 FROM DigitoVerificador WHERE tabla='Patente')
    INSERT INTO DigitoVerificador(tabla,digitoVertical) VALUES('Patente',-1);
IF NOT EXISTS (SELECT 1 FROM DigitoVerificador WHERE tabla='FamiliaPatente')
    INSERT INTO DigitoVerificador(tabla,digitoVertical) VALUES('FamiliaPatente',-1);
IF NOT EXISTS (SELECT 1 FROM DigitoVerificador WHERE tabla='PerfilFamilia')
    INSERT INTO DigitoVerificador(tabla,digitoVertical) VALUES('PerfilFamilia',-1);
IF NOT EXISTS (SELECT 1 FROM DigitoVerificador WHERE tabla='LogEventos')
    INSERT INTO DigitoVerificador(tabla,digitoVertical) VALUES('LogEventos',-1);
IF NOT EXISTS (SELECT 1 FROM DigitoVerificador WHERE tabla='Venta')
    INSERT INTO DigitoVerificador(tabla,digitoVertical) VALUES('Venta',-1);
IF NOT EXISTS (SELECT 1 FROM DigitoVerificador WHERE tabla='DetalleVenta')
    INSERT INTO DigitoVerificador(tabla,digitoVertical) VALUES('DetalleVenta',-1);
IF NOT EXISTS (SELECT 1 FROM DigitoVerificador WHERE tabla='HistorialCambiosProducto')
    INSERT INTO DigitoVerificador(tabla,digitoVertical) VALUES('HistorialCambiosProducto',-1);
IF NOT EXISTS (SELECT 1 FROM DigitoVerificador WHERE tabla='Idioma')
    INSERT INTO DigitoVerificador(tabla,digitoVertical) VALUES('Idioma',-1);
IF NOT EXISTS (SELECT 1 FROM DigitoVerificador WHERE tabla='Etiqueta')
    INSERT INTO DigitoVerificador(tabla,digitoVertical) VALUES('Etiqueta',-1);
IF NOT EXISTS (SELECT 1 FROM DigitoVerificador WHERE tabla='Traduccion')
    INSERT INTO DigitoVerificador(tabla,digitoVertical) VALUES('Traduccion',-1);
GO
IF OBJECT_ID('TR_Producto_HistorialCambios', 'TR') IS NOT NULL
    DROP TRIGGER TR_Producto_HistorialCambios;
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

-- Completa el circuito de CU02 sin borrar ventas existentes.
IF COL_LENGTH('Venta','estado') IS NULL
    ALTER TABLE Venta ADD estado VARCHAR(20) NOT NULL CONSTRAINT DF_Venta_Estado DEFAULT 'PAGADA';
IF COL_LENGTH('Venta','fechaEntrega') IS NULL
    ALTER TABLE Venta ADD fechaEntrega DATETIME NULL;
GO
IF COL_LENGTH('DetalleVenta','codigoProducto') IS NULL
    ALTER TABLE DetalleVenta ADD codigoProducto VARCHAR(50) NULL;
IF COL_LENGTH('DetalleVenta','descripcionProducto') IS NULL
    ALTER TABLE DetalleVenta ADD descripcionProducto VARCHAR(150) NULL;
GO
UPDATE d SET codigoProducto=p.codigo,descripcionProducto=p.descripcion
FROM DetalleVenta d JOIN Producto p ON p.idProducto=d.idProducto
WHERE d.codigoProducto IS NULL OR d.descripcionProducto IS NULL;
GO
ALTER TABLE DetalleVenta ALTER COLUMN codigoProducto VARCHAR(50) NOT NULL;
ALTER TABLE DetalleVenta ALTER COLUMN descripcionProducto VARCHAR(150) NOT NULL;
GO
IF OBJECT_ID('Pago','U') IS NULL
    CREATE TABLE Pago
    (
        idPago INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Pago PRIMARY KEY,
        idVenta INT NOT NULL CONSTRAINT FK_Pago_Venta REFERENCES Venta(idVenta),
        medioPago VARCHAR(20) NOT NULL,
        importe DECIMAL(18,2) NOT NULL,
        numeroOperacion NVARCHAR(500) NOT NULL,
        fecha DATETIME NOT NULL,
        verificado BIT NOT NULL,
        digitoVerificador INT NOT NULL DEFAULT 0,
        CONSTRAINT CK_Pago_Medio CHECK(medioPago IN ('Efectivo','Transferencia')),
        CONSTRAINT CK_Pago_Importe CHECK(importe>=0)
    );
GO

INSERT INTO Pago(idVenta,medioPago,importe,numeroOperacion,fecha,verificado)
SELECT v.idVenta,'Efectivo',v.total,N'',v.fecha,1 FROM Venta v
WHERE v.estado IN ('PAGADA','FINALIZADA') AND NOT EXISTS(SELECT 1 FROM Pago p WHERE p.idVenta=v.idVenta);
GO
IF OBJECT_ID('Comprobante','U') IS NULL
    CREATE TABLE Comprobante
    (
        idComprobante INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Comprobante PRIMARY KEY,
        idVenta INT NOT NULL CONSTRAINT UQ_Comprobante_Venta UNIQUE
            CONSTRAINT FK_Comprobante_Venta REFERENCES Venta(idVenta),
        fecha DATETIME NOT NULL,
        cliente VARCHAR(201) NOT NULL,
        dniCliente VARCHAR(20) NOT NULL,
        vendedor VARCHAR(201) NOT NULL,
        total DECIMAL(18,2) NOT NULL,
        digitoVerificador INT NOT NULL DEFAULT 0
    );
GO
IF OBJECT_ID('Backup','U') IS NULL
    CREATE TABLE [Backup]
    (
        idBackup INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Backup PRIMARY KEY,
        fecha DATETIME NOT NULL,
        usuario VARCHAR(150) NOT NULL,
        ruta NVARCHAR(500) NOT NULL CONSTRAINT UQ_Backup_Ruta UNIQUE,
        digitoVerificador INT NOT NULL DEFAULT 0
    );
GO
IF NOT EXISTS(SELECT 1 FROM sys.check_constraints WHERE name='CK_Venta_Estado')
    ALTER TABLE Venta ADD CONSTRAINT CK_Venta_Estado CHECK(estado IN ('PENDIENTE','PAGADA','FINALIZADA','CANCELADA'));
IF NOT EXISTS(SELECT 1 FROM sys.check_constraints WHERE name='CK_Venta_Total')
    ALTER TABLE Venta ADD CONSTRAINT CK_Venta_Total CHECK(total>0);
IF NOT EXISTS(SELECT 1 FROM sys.check_constraints WHERE name='CK_Producto_Precio')
    ALTER TABLE Producto ADD CONSTRAINT CK_Producto_Precio CHECK(precio>0);
IF NOT EXISTS(SELECT 1 FROM sys.check_constraints WHERE name='CK_Producto_Stock')
    ALTER TABLE Producto ADD CONSTRAINT CK_Producto_Stock CHECK(stock>=0);
IF NOT EXISTS(SELECT 1 FROM sys.check_constraints WHERE name='CK_DetalleVenta_Cantidad')
    ALTER TABLE DetalleVenta ADD CONSTRAINT CK_DetalleVenta_Cantidad CHECK(cantidad>0);
IF NOT EXISTS(SELECT 1 FROM sys.check_constraints WHERE name='CK_DetalleVenta_Importes')
    ALTER TABLE DetalleVenta ADD CONSTRAINT CK_DetalleVenta_Importes CHECK(precioUnitario>0 AND subtotal=precioUnitario*cantidad);
IF NOT EXISTS(SELECT 1 FROM sys.check_constraints WHERE name='CK_Usuario_Intentos')
    ALTER TABLE Usuario ADD CONSTRAINT CK_Usuario_Intentos CHECK(intentos BETWEEN 0 AND 3);
GO
IF NOT EXISTS (SELECT 1 FROM DigitoVerificador WHERE tabla='Pago')
    INSERT INTO DigitoVerificador(tabla,digitoVertical) VALUES('Pago',-1);
IF NOT EXISTS (SELECT 1 FROM DigitoVerificador WHERE tabla='Comprobante')
    INSERT INTO DigitoVerificador(tabla,digitoVertical) VALUES('Comprobante',-1);
IF NOT EXISTS (SELECT 1 FROM DigitoVerificador WHERE tabla='Backup')
    INSERT INTO DigitoVerificador(tabla,digitoVertical) VALUES('Backup',-1);
GO

DECLARE @nuevosTextos TABLE(etiqueta NVARCHAR(200), ingles NVARCHAR(250));
INSERT INTO @nuevosTextos VALUES
(N'Desde',N'From'),
(N'Hasta',N'To'),
(N'Actividad',N'Activity'),
(N'Historial de ventas',N'Sales history'),
(N'Pago de la venta',N'Sale payment'),
(N'Total',N'Total'),
(N'Medio de pago',N'Payment method'),
(N'Importe',N'Amount'),
(N'Numero de operacion',N'Transaction number'),
(N'Pago verificado por el empleado',N'Payment verified by the employee'),
(N'Confirmar',N'Confirm'),
(N'Cancelar',N'Cancel'),
(N'Pago pendiente',N'Pending payment'),
(N'Verificar pago',N'Verify payment'),
(N'Comprobante',N'Receipt'),
(N'Pagos',N'Payments'),
(N'Registrar entrega',N'Record delivery'),
(N'Cancelar venta',N'Cancel sale'),
(N'Comprobante de venta',N'Sale receipt'),
(N'Quitar producto',N'Remove product'),
(N'Imprimir',N'Print'),
(N'Crear copia',N'Create backup'),
(N'Restaurar copia',N'Restore backup'),
(N'Copias de seguridad',N'Backups'),
(N'Idiomas',N'Languages'),
(N'Idiomas y traducciones',N'Languages and translations'),
(N'Codigo de idioma',N'Language code'),
(N'Texto original',N'Original text'),
(N'Traduccion',N'Translation'),
(N'Eliminar',N'Delete'),
(N'Preparar integridad',N'Initialize integrity'),
(N'Estructura de permisos',N'Permission structure'),
(N'Confirmar venta',N'Confirm sale'),
(N'Integridad',N'Integrity'),
(N'Familia',N'Group'),
(N'Patente',N'Permission'),
(N'Efectivo',N'Cash'),
(N'Transferencia',N'Bank transfer'),
(N'idVenta',N'Sale ID'),
(N'dniCliente',N'Customer ID'),
(N'cliente',N'Customer'),
(N'vendedor',N'Seller'),
(N'estado',N'Status'),
(N'fechaEntrega',N'Delivery date'),
(N'fecha',N'Date'),
(N'medioPago',N'Payment method'),
(N'numeroOperacion',N'Transaction number'),
(N'verificado',N'Verified'),
(N'ruta',N'Path'),
(N'idBackup',N'Backup ID'),
(N'nombre',N'First name'),
(N'apellido',N'Last name'),
(N'precioUnitario',N'Unit price'),
(N'cantidad',N'Quantity'),
(N'idperfil',N'Profile'),
(N'Datos guardados.',N'Information saved.'),
(N'Asignaciones guardadas.',N'Assignments saved.'),
(N'Clave modificada.',N'Password changed.'),
(N'Cliente encontrado.',N'Customer found.'),
(N'Cliente registrado correctamente.',N'Customer registered successfully.'),
(N'Cliente seleccionado.',N'Customer selected.'),
(N'Clientes encontrados.',N'Customers found.'),
(N'Confirma eliminar ',N'Confirm deletion of '),
(N'Confirma registrar este cliente?',N'Confirm registration of this customer?'),
(N'Confirma que entrego los productos y el comprobante?',N'Confirm that the products and receipt were delivered?'),
(N'Confirma la entrega de productos y comprobante?',N'Confirm delivery of products and receipt?'),
(N'Confirma cancelar esta venta pendiente?',N'Confirm cancellation of this pending sale?'),
(N'Confirmar cambio de estado de ',N'Confirm status change for '),
(N'Contraseña incorrecta. Intentos restantes: ',N'Incorrect password. Remaining attempts: '),
(N'Copia creada: ',N'Backup created: '),
(N'Diferencias en: ',N'Differences in: '),
(N'Total: ',N'Total: '),
(N'Total: 0,00',N'Total: 0.00'),
(N'Venta registrada correctamente. Nro: ',N'Sale registered successfully. No: '),
(N'¿Confirma registrar la venta por un total de $',N'Confirm a sale for a total of $'),
(N'¿Confirma restaurar el producto a la versión seleccionada?',N'Restore the product to the selected version?'),
(N'Debe agregar al menos un producto a la venta.',N'Add at least one product to the sale.'),
(N'Debe indicar el pago.',N'Enter payment information.'),
(N'Debe ingresar el DNI del cliente.',N'Enter the customer ID number.'),
(N'Debe ingresar el apellido del cliente.',N'Enter the customer''s last name.'),
(N'Debe ingresar el apellido del usuario.',N'Enter the user''s last name.'),
(N'Debe ingresar el correo electrónico del cliente.',N'Enter the customer''s email.'),
(N'Debe ingresar el código del producto.',N'Enter the product code.'),
(N'Debe ingresar el email del usuario.',N'Enter the user''s email.'),
(N'Debe ingresar el modelo del producto.',N'Enter the product model.'),
(N'Debe ingresar el nombre del cliente.',N'Enter the customer''s first name.'),
(N'Debe ingresar el nombre del usuario.',N'Enter the user''s first name.'),
(N'Debe ingresar el perfil del usuario.',N'Enter the user''s profile.'),
(N'Debe ingresar el precio del producto.',N'Enter the product price.'),
(N'Debe ingresar el stock del producto.',N'Enter the product stock.'),
(N'Debe ingresar el teléfono del cliente.',N'Enter the customer''s phone.'),
(N'Debe ingresar email y contraseña.',N'Enter email and password.'),
(N'Debe ingresar la contraseña del usuario.',N'Enter the user''s password.'),
(N'Debe ingresar la descripción del producto.',N'Enter the product description.'),
(N'Debe ingresar la marca del producto.',N'Enter the product brand.'),
(N'Debe ingresar los datos de la venta.',N'Enter sale information.'),
(N'Debe ingresar los datos del cliente.',N'Enter customer information.'),
(N'Debe ingresar los datos del producto.',N'Enter product information.'),
(N'Debe ingresar los datos del usuario.',N'Enter user information.'),
(N'Debe iniciar sesion para registrar una venta.',N'Sign in to register a sale.'),
(N'Debe iniciar sesion.',N'Sign in first.'),
(N'Debe indicar un usuario activo para iniciar sesion.',N'Use an active user to sign in.'),
(N'Debe seleccionar un cliente.',N'Select a customer.'),
(N'Debe seleccionar un producto para modificar.',N'Select a product to edit.'),
(N'Debe seleccionar un producto válido.',N'Select a valid product.'),
(N'Debe seleccionar un producto.',N'Select a product.'),
(N'Debe seleccionar una versión.',N'Select a version.'),
(N'El DNI debe contener solo numeros.',N'The ID number must contain only digits.'),
(N'El cliente ya se encuentra registrado.',N'The customer is already registered.'),
(N'El codigo del idioma debe tener dos letras minusculas.',N'The language code must contain two lowercase letters.'),
(N'El correo electronico no es valido.',N'The email is not valid.'),
(N'El detalle no puede ser nulo.',N'The detail cannot be null.'),
(N'El importe del pago no es valido.',N'The payment amount is not valid.'),
(N'El numero de operacion es demasiado largo.',N'The transaction number is too long.'),
(N'El pago no esta verificado o el importe no coincide. Guardar venta pendiente?',N'Payment is unverified or the amount does not match. Save a pending sale?'),
(N'El precio debe ser mayor a cero.',N'The price must be greater than zero.'),
(N'El precio debe tener como maximo dos decimales.',N'The price must have at most two decimal places.'),
(N'El precio del producto debe ser mayor a cero.',N'The product price must be greater than zero.'),
(N'El rango de fechas es incorrecto.',N'The date range is incorrect.'),
(N'El stock no puede ser negativo.',N'Stock cannot be negative.'),
(N'El total supera el importe permitido.',N'The total exceeds the allowed amount.'),
(N'El usuario no esta activo.',N'The user is not active.'),
(N'El usuario se encuentra bloqueado.',N'The user is blocked.'),
(N'Estado del producto actualizado correctamente.',N'Product status updated successfully.'),
(N'Esto acepta los datos actuales como correctos. Revise la base antes de continuar.',N'This accepts current data as correct. Review the database before continuing.'),
(N'Faltan el archivo .bak o su archivo .key.',N'The .bak file or its .key file is missing.'),
(N'Faltan las traducciones.',N'Translations are missing.'),
(N'Historial actualizado.',N'History refreshed.'),
(N'Idioma no disponible.',N'Language unavailable.'),
(N'Indique el numero de operacion de la transferencia.',N'Enter the bank transfer transaction number.'),
(N'Indique un nombre de idioma valido.',N'Enter a valid language name.'),
(N'Ingrese la nueva clave.',N'Enter the new password.'),
(N'Ingrese las credenciales de un administrador activo.',N'Enter the credentials of an active administrator.'),
(N'Integridad preparada. Ahora puede ingresar.',N'Integrity initialized. You can now sign in.'),
(N'La cantidad debe ser mayor a cero.',N'Quantity must be greater than zero.'),
(N'La carpeta no existe.',N'The folder does not exist.'),
(N'La clave actual es incorrecta.',N'The current password is incorrect.'),
(N'La clave de la copia no es valida.',N'The backup key is invalid.'),
(N'La clave es demasiado larga.',N'The password is too long.'),
(N'La clave debe tener entre 1 y 72 bytes.',N'The password must contain between 1 and 72 bytes.'),
(N'La integridad de Seguridad no esta preparada o presenta diferencias.',N'Security integrity is not initialized or has differences.'),
(N'La integridad de Seguridad presenta diferencias.',N'Security integrity has differences.'),
(N'La integridad de Usuario presenta diferencias.',N'User integrity has differences.'),
(N'La integridad del sistema es correcta.',N'System integrity is correct.'),
(N'La integridad del sistema fue regenerada.',N'System integrity was recalculated.'),
(N'La ruta de backup no es valida.',N'The backup path is not valid.'),
(N'La venta no esta pendiente.',N'The sale is not pending.'),
(N'La venta quedo pagada. Puede reintentar el comprobante o la entrega desde Historial de ventas. ',N'The sale remains paid. Retry the receipt or delivery from Sales history. '),
(N'Las claves no coinciden.',N'The passwords do not match.'),
(N'Los datos del cliente superan el largo permitido.',N'Customer information exceeds the allowed length.'),
(N'Los datos del producto superan el largo permitido.',N'Product information exceeds the allowed length.'),
(N'Los datos del usuario superan el largo permitido.',N'User information exceeds the allowed length.'),
(N'No hay stock suficiente.',N'There is not enough stock.'),
(N'No puede bloquear su propia cuenta.',N'You cannot block your own account.'),
(N'No puede cambiar su propio perfil durante la sesion.',N'You cannot change your own profile during the session.'),
(N'No puede eliminar un idioma base o el idioma en uso.',N'You cannot delete a base language or the language in use.'),
(N'No se encontraron productos.',N'No products found.'),
(N'No se encontró el producto seleccionado.',N'The selected product was not found.'),
(N'No se encontró un cliente con ese DNI.',N'No customer was found with that ID number.'),
(N'No se permite ingresar sin registrar el inicio de sesion.',N'Sign-in cannot proceed without recording its event.'),
(N'No se pudo actualizar el usuario.',N'The user could not be updated.'),
(N'No se pudo cambiar la clave.',N'The password could not be changed.'),
(N'No se pudo cargar el idioma. Revise el script de idiomas. ',N'The language could not be loaded. Check the language script. '),
(N'No se pudo modificar.',N'The information could not be updated.'),
(N'No se pudo registrar el cierre de sesion. ',N'Sign-out could not be recorded. '),
(N'No se pudo registrar el cliente.',N'The customer could not be registered.'),
(N'No se pudo registrar.',N'The information could not be registered.'),
(N'No se pudo restaurar el producto.',N'The product could not be restored.'),
(N'No tiene permiso para esta operacion.',N'You do not have permission for this operation.'),
(N'Producto agregado a la venta.',N'Product added to the sale.'),
(N'Producto modificado correctamente.',N'Product updated successfully.'),
(N'Producto registrado correctamente.',N'Product registered successfully.'),
(N'Producto restaurado correctamente.',N'Product restored successfully.'),
(N'Producto seleccionado.',N'Product selected.'),
(N'Productos encontrados.',N'Products found.'),
(N'Pulse Nuevo para registrar otro usuario.',N'Click New to register another user.'),
(N'Revise integridad antes de operar. Diferencias en: ',N'Check integrity before working. Differences in: '),
(N'Revise la conexion y ejecute ActualizarEntrega1.sql. ',N'Check the connection and run ActualizarEntrega1.sql. '),
(N'Revise la integridad antes de hacer una copia.',N'Check integrity before creating a backup.'),
(N'Revise la integridad de los pagos.',N'Check payment integrity.'),
(N'Seleccione un elemento.',N'Select an item.'),
(N'Seleccione un medio de pago.',N'Select a payment method.'),
(N'Seleccione un perfil existente.',N'Select an existing profile.'),
(N'Seleccione un perfil o una familia.',N'Select a profile or group.'),
(N'Seleccione un usuario.',N'Select a user.'),
(N'Seleccione una venta.',N'Select a sale.'),
(N'Solo un administrador puede regenerar integridad.',N'Only an administrator can recalculate integrity.'),
(N'Usuario bloqueado por superar los intentos permitidos.',N'User blocked after exceeding the allowed attempts.'),
(N'Usuario modificado.',N'User updated.'),
(N'Usuario no encontrado.',N'User not found.'),
(N'Usuario registrado.',N'User registered.'),
(N'Venta pendiente guardada. Verifique el pago desde Historial de ventas.',N'Pending sale saved. Verify payment from Sales history.'),
(N'Verifique el pago por el importe exacto antes de continuar.',N'Verify payment for the exact amount before continuing.'),
(N'Versión seleccionada.',N'Version selected.'),
(N'Ya existe un producto registrado con ese código.',N'A product with that code is already registered.'),
(N'Ya existe un usuario registrado con ese email.',N'A user with that email is already registered.'),
(N'Cada traduccion debe tener entre 1 y 250 caracteres.',N'Each translation must contain between 1 and 250 characters.'),
(N'Cierre la sesion antes de preparar integridad.',N'Sign out before initializing integrity.'),
(N'Credenciales incorrectas.',N'Incorrect credentials.'),
(N'Se recalcularan los digitos con los datos actuales. Continuar solo si reviso que son correctos.',N'Check digits will be recalculated from current data. Continue only after reviewing their correctness.'),
(N'SQL Server debe tener permiso de escritura en la carpeta. Conserve juntos el .bak y el .key en un lugar privado.',N'SQL Server needs write access to the folder. Keep the .bak and .key together in a private location.'),
(N'Restaurar reemplaza los datos actuales por los de esta copia y cierra el sistema. Haga antes una copia actual. Continuar?',N'Restoring replaces current data with this backup and closes the app. Create a current backup first. Continue?'),
(N'Base restaurada. Abra nuevamente el sistema para verificar integridad e iniciar sesion.',N'Database restored. Reopen the app to check integrity and sign in.'),
(N'La venta no esta pendiente o el importe no coincide.',N'The sale is not pending or the amount does not match.'),
(N'La venta debe estar pagada y tener comprobante antes de entregar.',N'The sale must be paid and have a receipt before delivery.'),
(N'Solo se cancela una venta pendiente.',N'Only a pending sale can be cancelled.'),
(N'Solo se emite comprobante de una venta pagada.',N'A receipt can only be issued for a paid sale.'),
(N'Todavia no hay comprobante.',N'There is no receipt yet.'),
(N'El vendedor no esta activo.',N'The seller is not active.'),
(N'El producto cambio de precio, no esta activo o no tiene stock suficiente.',N'The product price changed, is inactive or has insufficient stock.'),
(N'Revise el precio y el stock del producto.',N'Check the product price and stock.'),
(N'La venta no existe.',N'The sale does not exist.'),
(N'Un producto no puede tener dos precios en la misma venta.',N'A product cannot have two prices in the same sale.'),
(N'Una familia no puede contenerse a si misma.',N'A group cannot contain itself.'),
(N'Ya hay una sesión iniciada.',N'There is already an active session.'),
(N'Operacion guardada, bitacora pendiente.',N'Operation saved, event log pending.'),
(N'La operacion puede haberse guardado, pero fallo la bitacora: ',N'The operation may have been saved, but the event log failed: ');
INSERT INTO Etiqueta(etiqueta)
SELECT n.etiqueta FROM @nuevosTextos n WHERE NOT EXISTS(SELECT 1 FROM Etiqueta e WHERE e.etiqueta=n.etiqueta);
INSERT INTO Traduccion(codigoIdioma,etiqueta,textoTraducido)
SELECT 'es',n.etiqueta,n.etiqueta FROM @nuevosTextos n WHERE NOT EXISTS(SELECT 1 FROM Traduccion t WHERE t.codigoIdioma='es' AND t.etiqueta=n.etiqueta);
INSERT INTO Traduccion(codigoIdioma,etiqueta,textoTraducido)
SELECT 'en',n.etiqueta,n.ingles FROM @nuevosTextos n WHERE NOT EXISTS(SELECT 1 FROM Traduccion t WHERE t.codigoIdioma='en' AND t.etiqueta=n.etiqueta);
GO

-- Exportacion y estados visibles.
DECLARE @exportacion TABLE(etiqueta NVARCHAR(200), ingles NVARCHAR(250));
INSERT INTO @exportacion VALUES
(N'Guardar PDF',N'Save PDF'),
(N'Exportar XML',N'Export XML'),
(N'Archivo guardado.',N'File saved.'),
(N'Restaurar producto',N'Restore product'),
(N'Producto',N'Product'),
(N'Historial',N'History'),
(N'La integridad ya fue preparada. Para regenerarla ingrese como administrador.',N'Integrity was already initialized. Sign in as administrator to recalculate it.'),
(N'BMTech - COMPROBANTE INTERNO (NO FISCAL)',N'BMTech - INTERNAL RECEIPT (NOT A TAX INVOICE)'),
(N'Comprobante: ',N'Receipt: '),
(N'   Venta: ',N'   Sale: '),
(N'Fecha: ',N'Date: '),
(N'Cliente: ',N'Customer: '),
(N'Vendedor: ',N'Seller: '),
(N'TOTAL: ',N'TOTAL: '),
(N'PENDIENTE',N'PENDING'),
(N'PAGADA',N'PAID'),
(N'FINALIZADA',N'COMPLETED'),
(N'CANCELADA',N'CANCELLED');
INSERT INTO Etiqueta(etiqueta)
SELECT e.etiqueta FROM @exportacion e WHERE NOT EXISTS(SELECT 1 FROM Etiqueta t WHERE t.etiqueta=e.etiqueta);
INSERT INTO Traduccion(codigoIdioma,etiqueta,textoTraducido)
SELECT 'es',e.etiqueta,e.etiqueta FROM @exportacion e WHERE NOT EXISTS(SELECT 1 FROM Traduccion t WHERE t.codigoIdioma='es' AND t.etiqueta=e.etiqueta);
INSERT INTO Traduccion(codigoIdioma,etiqueta,textoTraducido)
SELECT 'en',e.etiqueta,e.ingles FROM @exportacion e WHERE NOT EXISTS(SELECT 1 FROM Traduccion t WHERE t.codigoIdioma='en' AND t.etiqueta=e.etiqueta);
GO

UPDATE DigitoVerificador SET digitoVertical=-1;
GO
