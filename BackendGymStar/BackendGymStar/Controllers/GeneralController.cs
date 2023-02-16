using BackendGymStar.DTO.Entradas;
using BackendGymStar.DTO.General;
using BackendGymStar.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BackendGymStar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        private readonly IGeneralRepository _generalRepository;

        public GeneralController(IGeneralRepository generalRepository)
        {
            this._generalRepository = generalRepository;
        }

        [HttpPost("AddGym")]
        public async Task<ActionResult> AddGym([FromBody] AddGym request)
        {

            //string token = Request.Headers["token"];
            //if (Request.Headers["token"] == "" || token == null)
            //{
            //    return Unauthorized();
            //}

            var response = await _generalRepository.addGym(request);
            return Ok(response);
        }


        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] UserLogin userInfo)
        {
            var response = await _generalRepository.Login(userInfo);
            return Ok(response);

        }

        [HttpGet("validateToken/{id}")]
        public async Task<ActionResult> validateToken(long id)
        {
            // return Ok(Request.Headers["x-token"]);
            string token = Request.Headers["x-token"];
            string token2 = Request.Headers["Authorization"];
            if (Request.Headers["x-token"] == "" || token == null)
            {
                return Unauthorized();
            }
            var resp = await _generalRepository.validateToken(token, id);




            return Ok(resp);
        }


        [HttpGet("getUserGym/{id}")]
        public async Task<ActionResult> getUserGym(long id)
        {
            UserGymN response = await _generalRepository.getNameUserGym(id);
            return Ok(response);
        }

    }
}
