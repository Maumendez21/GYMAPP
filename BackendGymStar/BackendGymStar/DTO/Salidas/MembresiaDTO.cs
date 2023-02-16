namespace BackendGymStar.DTO.Salidas
{
    public class MembresiaDTO
    {
        public int? Memid { get; set; }
        public string? Memnombre { get; set; }
        public string? Memdescripcion { get; set; }
        public double Memprecio { get; set; }
        public int Memduracion { get; set; }
        public int Mempersonas { get; set; }
        public long? EstId { get; set; }
        public long GymId { get; set; }
        public int MemusrReg { get; set; }
    }
}
