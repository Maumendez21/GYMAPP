use GYMSTAR
select *  from Usuario
select *  from Gimnasio

select * from Usuario

select * from Membresia

select *  from ROL

select *  from Gimnasio
select * from Caja
select * from Producto

select * from Pago

select *  from Usuario

select *  from Estatus

DELETE Gimnasio WHERE GYMID = 3

DELETE Usuario WHERE USRID = 2

select * from DetallePago
select * from Pago
select * from DetalleCaja 


select p.PAGOID, p.Descripcion, p.Total, p.FechaPago, p.Referencia, dc.DETCAJAID from DetalleCaja as dc 
inner join Pago AS p on dc.PagoId = p.PAGOID

select * from ConceptoPago
select * from MembresiaSocio
select * from Socio
select * from Membresia
select * from Usuario