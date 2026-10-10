USE BMTech;
GO

IF COL_LENGTH('Cliente', 'digitoVerificador') IS NULL
    ALTER TABLE Cliente ADD digitoVerificador INT NOT NULL DEFAULT 0;
GO

ALTER TRIGGER TR_Producto_HistorialCambios
ON Producto
AFTER UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO HistorialCambiosProducto
    (
        idProducto, codigo, descripcion, marca, modelo,
        precio, stock, activo, digitoVerificador, usuario, fecha, accion
    )
    SELECT
        d.idProducto, d.codigo, d.descripcion, d.marca, d.modelo,
        d.precio, d.stock, d.activo, d.digitoVerificador,
        SYSTEM_USER, GETDATE(),
        CASE WHEN EXISTS (SELECT 1 FROM inserted) THEN 'MODIFICACION' ELSE 'BAJA' END
    FROM deleted d;
END;
GO
