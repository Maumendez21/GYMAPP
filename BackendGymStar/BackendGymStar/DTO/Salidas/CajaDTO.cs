namespace BackendGymStar.DTO.Salidas
{
    public class CajaDTO
    {
        public int Cajaid { get; set; }
        public DateTime? CajafechaApertura { get; set; }
        public DateTime? CajafechaCierre { get; set; }
        public double? CajamontoApertura { get; set; }
        public double? CajamontoActual { get; set; }
        public double? CajamontoCierre { get; set; }
        public long? EstId { get; set; }
    }
}
