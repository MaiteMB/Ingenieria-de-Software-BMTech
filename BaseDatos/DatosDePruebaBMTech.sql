USE [BMTech];
GO

IF EXISTS(SELECT 1 FROM Cliente) OR EXISTS(SELECT 1 FROM Producto) OR EXISTS(SELECT 1 FROM Venta)
    THROW 50000, 'Los datos de prueba solo se cargan en una base nueva.', 1;
GO

-- Clientes
INSERT INTO Cliente (dni, nombre, apellido, telefono, correoElectronico) VALUES
    ('32874516', 'Lucia', 'Fernandez', '1145827396', 'lucia.fernandez@example.com'),
    ('35416982', 'Martin', 'Acosta', '1162738451', 'martin.acosta@example.com'),
    ('29651470', 'Sofia', 'Gimenez', '1153920174', 'sofia.gimenez@example.com'),
    ('40217893', 'Diego', 'Romero', '1138465012', 'diego.romero@example.com');
GO

-- Productos
INSERT INTO Producto (codigo, descripcion, marca, modelo, precio, stock, activo) VALUES
    ('NOT-001', 'Notebook 15 pulgadas', 'Lenovo', 'IdeaPad 3', 850000, 4, 1),
    ('MON-001', 'Monitor 24 pulgadas', 'Samsung', 'S24C310', 210000, 6, 1),
    ('TEC-001', 'Teclado USB', 'Logitech', 'K120', 15000, 9, 1),
    ('MOU-001', 'Mouse inalambrico', 'Logitech', 'M170', 12000, 14, 1),
    ('MEM-001', 'Memoria RAM 8 GB', 'Kingston', 'DDR4 3200 MHz', 35000, 8, 1),
    ('DIS-001', 'Disco solido 480 GB', 'Kingston', 'A400', 48000, 10, 1),
    ('FUE-001', 'Fuente de alimentacion', 'Sentey', '500W', 42000, 5, 1),
    ('IMP-001', 'Impresora multifuncion', 'HP', 'DeskJet 2775', 160000, 3, 1);
GO

-- Venta finalizada: Lucia compro un teclado y un mouse en efectivo
INSERT INTO Venta (dniCliente, emailUsuario, fecha, total, estado, fechaEntrega)
VALUES ('32874516', 'vendedor@bmtech.com', DATEADD(DAY, -3, GETDATE()), 27000, 'FINALIZADA', DATEADD(DAY, -3, GETDATE()));

DECLARE @venta INT = SCOPE_IDENTITY();

INSERT INTO DetalleVenta (idVenta, idProducto, cantidad, precioUnitario, subtotal, codigoProducto, descripcionProducto)
SELECT @venta, idProducto, 1, precio, precio, codigo, descripcion
FROM Producto WHERE codigo IN ('TEC-001', 'MOU-001');

INSERT INTO Pago (idVenta, medioPago, importe, numeroOperacion, fecha, verificado)
VALUES (@venta, 'Efectivo', 27000, N'', DATEADD(DAY, -3, GETDATE()), 1);

INSERT INTO Comprobante (idVenta, fecha, cliente, dniCliente, vendedor, total)
VALUES (@venta, DATEADD(DAY, -3, GETDATE()), 'Lucia Fernandez', '32874516', 'Usuario Vendedor', 27000);
GO

-- Servicio tecnico: una orden recibida y otra asignada a un tecnico
INSERT INTO Equipo (dniCliente, tipo, marca, modelo, numeroSerie) VALUES
    ('35416982', 'Notebook', 'HP', '240 G8', 'HP240G8-001'),
    ('29651470', 'PC de escritorio', 'Generica', 'Ryzen 5', 'PC-RYZ-002');

INSERT INTO OrdenServicio (idEquipo, fechaIngreso, problema, accesorios, observaciones, estado, tecnico, fechaAsignacion, prioridad)
SELECT idEquipo, DATEADD(DAY, -1, GETDATE()), 'No enciende', 'Cargador', 'Golpe en la tapa', 'Recibida', NULL, NULL, 'Normal'
FROM Equipo WHERE numeroSerie = 'HP240G8-001';

INSERT INTO OrdenServicio (idEquipo, fechaIngreso, problema, accesorios, observaciones, estado, tecnico, fechaAsignacion, prioridad)
SELECT idEquipo, DATEADD(DAY, -2, GETDATE()), 'Se reinicia sola', 'Ninguno', 'Sin observaciones', 'Asignada', 'vendedor@bmtech.com', DATEADD(DAY, -1, GETDATE()), 'Alta'
FROM Equipo WHERE numeroSerie = 'PC-RYZ-002';
GO
