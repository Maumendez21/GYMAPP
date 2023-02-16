using BackendGymStar.DTO;
using BackendGymStar.DTO.Entradas;
using BackendGymStar.DTO.General;
using BackendGymStar.DTO.Salidas;
using Microsoft.OpenApi.Any;

namespace BackendGymStar.Services
{
    public interface IGeneralRepository
    {
        Task<Response<SalidasResponse>> addGym(AddGym request);
        Task<UserGymN> getNameUserGym(long id);
        Task<Response<LoginResp>> Login(UserLogin login);
        bool ValidaEmail(string email);
        Task<Response<ValidateTokenClass>> validateToken(string token, long id);



    }
}
