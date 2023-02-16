using BackendGymStar.Data;

namespace BackendGymStar.DTO.Salidas
{
    public class SalidasResponse
    {
        public string? Token { get; set; }
        public Usuario? user { get; set; }
    }


    public class LoginResp
    {
        public string? Token { get; set; }
        public long? UsrId { get; set; }
        public long? GymId { get; set; }
    }


    public class UserResp
    {
        public string? NombreUser { get; set; }
        public string? NombreCompletoUser { get; set; }
        public string? EmailUser { get; set; }
        public long? UsrId { get; set; }
        public long? GYMId { get; set; }
        public string? Rol { get; set; }
    }

    public class GymResp
    {

        public long Gymid { get; set; }
        public string? Gymnombre { get; set; }
        public string? Gymdireccion { get; set; }
        public string? Gymemail { get; set; }
        public string? Gymtelefono { get; set; }

    }


    public class ValidateTokenClass {
        public UserResp? user { get; set; }
        public GymResp? gym { get; set; }

        public CajaDTO? caja { get; set; }
    }

}
