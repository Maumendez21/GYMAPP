using BackendGymStar.DTO.Entradas;

namespace BackendGymStar.DTO.Salidas
{
    public class MembresiaAsociadas
    {
        public string? Memnombre { get; set; }
        public string? Memdescripcion { get; set; }
        public DateTime FechaPago { get; set; }
        public int diasFaltantes { get; set; }
        public long EstId { get; set; }
        public bool expand { get; set; } = false;
        public List<userAdd>? users { get; set; }
    }


    public class SociosList
    {
        public int idSocio { get; set; }
        public int idMembresia { get; set; }
        public int idMemsocid { get; set; }
        public int idUser{ get; set; }
        public userAdd? user { get; set; }
        public DateTime FechaPago { get; set; }
        public string? Memnombre { get; set; }
        public string? Memdescripcion { get; set; }
        public long EstId { get; set; }
    }


    public class MembresiaPagarGet
    {
        public string? Memnombre { get; set; }
        public string? Memdescripcion { get; set; }
        public List<userAdd>? users { get; set; }
        public int idMemsocid { get; set; }
        public DateTime? proxFechaPago { get; set; }
        public decimal precioMembresia { get; set; }

    }

    public class memsocidPago
    {
        public DateTime FechaPagoNew { get; set; }
        public int DiasRestantes { get; set; }
        public List<userAdd>? users { get; set; }

    }
}
