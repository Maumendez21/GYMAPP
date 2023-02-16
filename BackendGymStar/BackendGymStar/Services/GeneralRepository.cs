using Azure.Core;
using BackendGymStar.Data;
using BackendGymStar.DTO;
using BackendGymStar.DTO.Entradas;
using BackendGymStar.DTO.General;
using BackendGymStar.DTO.Salidas;
using BackendGymStar.Services.Caja;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using SHA3.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace BackendGymStar.Services
{
    public class GeneralRepository: IGeneralRepository
    {
        private readonly DataBaseContext _dataBaseContext;
        private readonly IConfiguration _configuration;
        private readonly ICajaRepository _cajaRepository;

        public GeneralRepository(DataBaseContext dataBaseContext, IConfiguration configuration, ICajaRepository cajaRepository)
        {
            this._dataBaseContext = dataBaseContext;
            this._configuration = configuration;
            this._cajaRepository = cajaRepository;
        }

        public async Task<Response<SalidasResponse>> addGym(AddGym request)
        {
            Response<SalidasResponse> salida = new Response<SalidasResponse>();

            if (request.Gymnombre == "" || String.IsNullOrEmpty(request.Gymnombre))
            {
                salida.ok = false;
                salida.msg = "El nombre del Gimnasio es obligatorio";
                return salida;
            }

            if (request.Gymdireccion == "" || String.IsNullOrEmpty(request.Gymdireccion))
            {
                salida.ok = false;
                salida.msg = "La dirección del gimnasio es obligatoria.";
                return salida;
            }

            if (request.Gymtelefono == "" || String.IsNullOrEmpty(request.Gymtelefono))
            {
                salida.ok = false;
                salida.msg = "El telefono del gimnasio es obligatorio.";
                return salida;
            }

            if (request.Usrnombre == "" || String.IsNullOrEmpty(request.Usrnombre))
            {
                salida.ok = false;
                salida.msg = "El nombre del usuario es obligatorio.";
                return salida;
            }
            
            
            if (request.Usrapp == "" || String.IsNullOrEmpty(request.Usrapp))
            {
                salida.ok = false;
                salida.msg = "El apellido paterno es obligatorio.";
                return salida;
            }

            if (request.Usremail == "" || String.IsNullOrEmpty(request.Usremail))
            {
                salida.ok = false;
                salida.msg = "El email del usuario es obligatorio.";
                return salida;
            }
            
            if (request.Usrpassword == "" || String.IsNullOrEmpty(request.Usrpassword))
            {
                salida.ok = false;
                salida.msg = "El password del usuario es obligatorio.";
                return salida;
            }
            
            if (request.Gymemail == "" || String.IsNullOrEmpty(request.Gymemail))
            {
                salida.ok = false;
                salida.msg = "El email del gimnasio es obligatorio.";
                return salida;
            }

            //Regex regexemail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            //Match matchEmail = regexemail.Match(request.Usremail);
            //Match matchEmailGym = regexemail.Match(request.Gymemail);

            if (!ValidaEmail(request.Usremail) || !ValidaEmail(request.Gymemail))
            {
                salida.ok = false;
                salida.msg = "El formato del email es incorrecto.";
                return salida;
            }

            var existUser = await _dataBaseContext.Usuario.Where(x => x.Usremail == request.Usremail).AnyAsync();
            var existGym = await _dataBaseContext.Gimnasio.Where(x => x.Gymemail == request.Gymemail).AnyAsync();

            if (existUser)
            {
                salida.ok = false;
                salida.msg = "Este email ya esta en uso por otro usuario.";
                return salida;
            }
            
            if (existGym)
            {
                salida.ok = false;
                salida.msg = "Este email ya esta en uso por otro gimnasio.";
                return salida;
            }

            Gimnasio gimnasio = new Gimnasio();
            try
            {
                gimnasio.Gymnombre = request.Gymnombre;
                gimnasio.Gymdireccion = request.Gymdireccion;
                gimnasio.Gymlogo = request.Gymlogo;
                gimnasio.Gymemail = request.Gymemail;
                gimnasio.Gymtelefono = request.Gymtelefono;
                gimnasio.GymsitioWeb = request.GymsitioWeb;
                gimnasio.GymfecReg = DateTime.Now;
                gimnasio.Gymactivo = true;
                gimnasio.EstId = 1;
                _dataBaseContext.Gimnasio.Add(gimnasio);
                _dataBaseContext.SaveChanges();
            }
            catch (Exception e)
            {
                salida.ok = false;
                salida.msg = "Error al intentar registrar el gimnasio." + e.Message;
                return salida;
            }

            gimnasio = await _dataBaseContext.Gimnasio.Where(x => x.Gymemail == request.Gymemail).FirstOrDefaultAsync();

            if (gimnasio is null)
            {
                salida.ok = false;
                salida.msg = "Hubo un error comunicate con el administrador";
                return salida;
            }

            Usuario usuario = new Usuario();
            try
            {
                usuario.Usrnombre = request.Usrnombre;
                usuario.Usrapp = request.Usrapp;
                usuario.Usrapm = request.Usrapm;
                usuario.Usrimagen = request.Usrimagen;
                usuario.Usremail = request.Usremail;
                usuario.Usrtelefono = request.Usrtelefono;
                string passEcript = sha3512(request.Usrpassword);
                usuario.Usrpassword = passEcript;
                usuario.GymId = gimnasio.Gymid;
                usuario.RolId = 1;
                usuario.EstId = 3;
                usuario.UsrfecReg = DateTime.Now;
                usuario.Usractivo = true;

                _dataBaseContext.Usuario.Add(usuario);
                _dataBaseContext.SaveChanges();
            }
            catch (Exception e)
            {

                salida.ok = false;
                salida.msg = "Error al intentar registrar el usuario." + e.Message;
                return salida;
            }

            usuario = await _dataBaseContext.Usuario.Where(x => x.Usremail == request.Usremail).FirstOrDefaultAsync();

            SalidasResponse salidaData = new SalidasResponse();
            salidaData.Token = BuildToken(usuario);
            usuario.Usrpassword = "";
            salidaData.user = usuario;

            salida.ok = true;
            salida.msg = "Gimnasio creado exitosamente.";
            salida.data = salidaData;
            return salida;
        }
        public async Task<Response<ValidateTokenClass>> validateToken(string token, long id){

            Response<ValidateTokenClass> salida = new Response<ValidateTokenClass>();
            ValidateTokenClass salidaData = new ValidateTokenClass();

            var mySecret = "GYMSTARKEYJWTWTWTWTWTWTWTWTWTWTWTWTWTWTWTWTWTWTWTWTWWTWTWTWTWTWTWTWTWT";
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));


            var tokenHandler = new JwtSecurityTokenHandler();
            bool isauth = false;

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = System.TimeSpan.Zero,
                    IssuerSigningKey = mySecurityKey,
                }, out SecurityToken validatedToken);

                isauth = true;
            }
            catch
            {
                isauth = false;
            }


            if (!isauth)
            {
                salida.ok = false;
                salida.msg = "No estas autenticado";
                return salida;

            }



            var user = await _dataBaseContext.Usuario.FirstOrDefaultAsync(u => u.Usrid == id);
            if (user is null)
            {
                salida.ok = false;
                salida.msg = "El usuario no existe.";
                return salida;
            }
            var gym = await _dataBaseContext.Gimnasio.FirstOrDefaultAsync(u => u.Gymid == user.GymId);


            // usuario salida
            UserResp userData = new UserResp();
            userData.NombreUser = $"{user.Usrnombre}";
            userData.NombreCompletoUser = $"{user.Usrnombre} {user.Usrapp} {user.Usrapm}";
            userData.GYMId = user.GymId;
            userData.UsrId = user.Usrid;
            string rol = await _dataBaseContext.Rol.Where(r => r.Rolid == user.RolId).Select(r => r.Rolclave).FirstOrDefaultAsync();
            userData.Rol = rol;
            userData.EmailUser = user.Usremail;
            salidaData.user = userData;

            // gym salida
            GymResp gymData = new GymResp();
            gymData.Gymid = gym.Gymid;
            gymData.Gymnombre = gym.Gymnombre;
            gymData.Gymdireccion = gym.Gymdireccion;
            gymData.Gymemail = gym.Gymemail;
            gymData.Gymtelefono = gym.Gymtelefono;
            salidaData.gym = gymData;

            // Caja salida
            CajaDTO caja = await _cajaRepository.cajaByIdGym(gymData.Gymid, (long)userData.UsrId);
            salidaData.caja = caja;


            salida.ok = true;
            salida.msg = "Si estas autenticado";
            salida.data = salidaData;
            return salida;
        }
        public async Task<Response<LoginResp>> Login(UserLogin login)
        {

            Response<LoginResp> salida = new Response<LoginResp>();

            var user = await _dataBaseContext.Usuario.FirstOrDefaultAsync(u => u.Usremail == login.Email.Trim());


            if (!ValidaEmail(login.Email))
            {
                salida.ok = false;
                salida.msg = "El formato del email no es correcto.";
                return salida;
            }

            if (user is null)
            {
                salida.ok = false;
                salida.msg = "Este email no esta registrado intentalo de nuevo.";
                return salida;
            }

            //var gym = await _dataBaseContext.Gimnasio.FirstOrDefaultAsync(u => u.Gymid == user.GymId);



            string passLoginCifrado = sha3512(login.Password.Trim());

            if (passLoginCifrado == user.Usrpassword.Trim())
            {

                LoginResp salidaLogin = new LoginResp();
                salidaLogin.Token = BuildToken(user);
                salidaLogin.UsrId = user.Usrid;
                salidaLogin.GymId = user.GymId;

                salida.ok = true;
                salida.msg = $"{user.Usrnombre} a iniciado sesión";
                salida.data = salidaLogin;
                return salida;
            }
            else
            {
                salida.ok = false;
                salida.msg = "Las credenciales de acceso no son correctas.";
                return salida;
            }
        }
        public async Task<UserGymN> getNameUserGym(long id)
        {
            UserGymN salida = new UserGymN();

            Usuario usuario = await _dataBaseContext.Usuario.FirstOrDefaultAsync(u => u.Usrid == id);

            if (usuario == null)
            {
                salida.nombreUsuario = "No hay usuario";
                salida.nombreGym = "";
                return salida;
            }

            string gimnasio = await (from g in _dataBaseContext.Gimnasio where g.Gymid == usuario.GymId select g.Gymnombre).FirstOrDefaultAsync();


            if (gimnasio == "" || String.IsNullOrEmpty(gimnasio))
            {
                salida.nombreUsuario = "";
                salida.nombreGym = "no hay gym";
                return salida;
            }


            salida.nombreUsuario = usuario.Usrnombre;
            salida.nombreGym = gimnasio;
            return salida;
        }
        // Cifrado
        public string sha3512(string value)
        {
            StringBuilder stringBuilder = new StringBuilder();
            using (var shaAlg = Sha3.Sha3512())
            {
                byte[] hash = shaAlg.ComputeHash(Encoding.UTF8.GetBytes(value));

                for (int i = 0; i < hash.Length; i++)
                {
                    stringBuilder.Append(hash[i].ToString("x2"));
                }
            }
            return stringBuilder.ToString();
        }
        private string BuildToken(Usuario user)
        {
            //var user = _dataBaseContext.Usuario.First(x => x.Usrid == id);
            //string username = user.Usrnombre;
            //string name = user.Usrnombre;
            //long userId = user.Usrid;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Usrnombre),
                new Claim("userNM", user.Usrnombre),
                new Claim(ClaimTypes.Name, user.Usrnombre),
                new Claim("userId", user.Usrid.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddHours(24);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public bool ValidaEmail(string email)
        {
            Regex regexemail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            Match matchEmail = regexemail.Match(email);

            return matchEmail.Success;
        }

    }
}
