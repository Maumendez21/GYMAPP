namespace BackendGymStar.DTO.Entradas
{
    public class AsocioAdd
    {
        public List<userAdd> users { get; set; }
        public int Memid { get; set; }
        public long Gymid { get; set; }
    }

    public class userAdd
    {
        public string? Usrnombre { get; set; }
        public string? Usrapp { get; set; }
        public string? Usrapm { get; set; }
        public string? Usremail { get; set; }
        public string? Usrtelefono { get; set; }
    }
}
