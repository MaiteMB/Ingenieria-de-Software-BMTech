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
