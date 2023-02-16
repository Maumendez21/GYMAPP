namespace BackendGymStar.DTO.Entradas
{
    public class AbrirCajaAdd
    {
        public double CajamontoApertura { get; set; }
        public long idUserReg { get; set; }
        public long idGym { get; set; }

    }

    public class PagoAdd
    {
        public string? DescripcionPago { get; set; }
        public int usrPagoId { get; set; } = 0;
        public int usrReg { get; set; } = 0;
        public int conceptoId { get; set; }
        public int cajaId { get; set; }
        public int gymId { get; set; }
        public List<PagoDetalleAdd> detalle { get; set; }
    }

    public class PagoDetalleAdd
    {
        public string? descripcionDetalle { get; set; }
        public double subtotal { get; set; }
        public int cantidad { get; set; }
        public int productId { get; set; } = 0;
    }



}
