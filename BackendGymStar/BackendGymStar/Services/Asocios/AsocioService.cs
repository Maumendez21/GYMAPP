using AutoMapper;
using BackendGymStar.Data;
using BackendGymStar.DTO;
using BackendGymStar.DTO.Entradas;
using BackendGymStar.DTO.Salidas;
using Microsoft.EntityFrameworkCore;

namespace BackendGymStar.Services.Asocios
{
    public class AsocioService: IAsocioService
    {
        private readonly DataBaseContext _dataBaseContext;
        private readonly IMapper _mapper;
        private readonly IGeneralRepository _generalRepository;

        public AsocioService(DataBaseContext dataBaseContext, IMapper mapper, IGeneralRepository generalRepository)
        {
            this._dataBaseContext = dataBaseContext;
            this._mapper = mapper;
            this._generalRepository = generalRepository;
        }

        //Socios con membresia
        public async Task<Response<List<SociosList>>> sociosList(long idGym)
        {
            Response<List<SociosList>> response = new Response<List<SociosList>>();
            List<SociosList> sociosLists = new List<SociosList>();

            var socios = await (from socio in _dataBaseContext.Socio
                                orderby socio.Socid
                                select new Socio
                                {
                                    Socid = socio.Socid,
                                    Usrid = socio.Usrid,
                                    MemSocid = socio.MemSocid,
                                    MemSoc = (from memsoc in _dataBaseContext.MembresiaSocio
                                             where memsoc.Memsocid == socio.MemSocid
                                             select new MembresiaSocio
                                             {
                                                 FechaPago = memsoc.FechaPago,
                                                 MembresiaId = memsoc.MembresiaId,
                                                 Membresia = (from mem in _dataBaseContext.Membresia
                                                              where mem.Memid == memsoc.MembresiaId && mem.GymId == idGym
                                                              select mem).FirstOrDefault()
                                             }).FirstOrDefault(),
                                    Usr = (from user in _dataBaseContext.Usuario
                                           where user.Usrid == socio.Usrid select user).FirstOrDefault()
                                }).ToListAsync();

            if (socios is null)
            {
                response.ok = false;
                response.msg = "No existen socios";
                return response;
            }

            foreach (var item in socios)
            {
                if (item.MemSoc.Membresia is null)
                {
                    continue;
                }

                SociosList tempSocio = new SociosList();
                userAdd usrTemp = new userAdd();

                tempSocio.idSocio = item.Socid;
                tempSocio.idMemsocid = (int)item.MemSocid;
                tempSocio.idUser = item.Usrid;
                tempSocio.idMembresia = item.MemSoc.MembresiaId;

                usrTemp.Usrnombre = item.Usr.Usrnombre;
                usrTemp.Usrapp = item.Usr.Usrapp;
                usrTemp.Usrapm = item.Usr.Usrapm;
                usrTemp.Usremail = item.Usr.Usremail;
                usrTemp.Usrtelefono = item.Usr.Usrtelefono;

                tempSocio.user = usrTemp;
                tempSocio.FechaPago = item.MemSoc.FechaPago;
                tempSocio.Memnombre = item.MemSoc.Membresia.Memnombre;
                tempSocio.Memdescripcion = item.MemSoc.Membresia.Memdescripcion;
                tempSocio.EstId = item.Usr.EstId;

                sociosLists.Add(tempSocio);
            }

            response.ok = true;
            response.msg = "Listado de socios";
            response.data = sociosLists;
            return response;
        }
        // Pago de membresia
        public async Task<Response<memsocidPago>> MembresiaUpdate(int memsocid)
        {
            Response<memsocidPago> response = new Response<memsocidPago>();
            memsocidPago data = new memsocidPago();
            List<userAdd> listUsers = new List<userAdd>();


            MembresiaSocio obj = await (from memSo in _dataBaseContext.MembresiaSocio
                                        where memSo.Memsocid == memsocid
                                        orderby memSo.Memsocid descending
                                        select new MembresiaSocio
                                        {
                                            Memsocid = memSo.Memsocid,
                                            FechaPago = memSo.FechaPago,
                                            DiasRestantes = memSo.DiasRestantes,
                                            EstId = memSo.EstId,
                                            Membresia = (from mem in _dataBaseContext.Membresia
                                                         where mem.Memid == memSo.MembresiaId
                                                         select mem).FirstOrDefault(),
                                            Socio = (from soc in _dataBaseContext.Socio
                                                     where soc.MemSocid == memSo.Memsocid
                                                     select new Socio
                                                     {
                                                         Socid = soc.Socid,
                                                         Usr = (from usr in _dataBaseContext.Usuario
                                                                where usr.Usrid == soc.Usrid 
                                                                select usr).FirstOrDefault()
                                                     }).ToList()
                                        }).FirstOrDefaultAsync(); 

            if (obj is null)
            {
                response.ok = true;
                response.msg = "No existe el registro, contactate con el administrador";
                return response;
            }

            int fechapagodays = (obj.FechaPago - DateTime.Now.Date).Days;

            var obj1 = await _dataBaseContext.MembresiaSocio.Where(x => x.Memsocid == memsocid).FirstOrDefaultAsync();

            obj1.FechaInicio = DateTime.Now.Date;
            obj1.FechaPago = obj.FechaPago.AddDays(obj.Membresia.Memduracion);
            obj1.DiasRestantes = fechapagodays + obj.Membresia.Memduracion;
            obj1.EstId = 8;

            try
            {
                var update = await _dataBaseContext.SaveChangesAsync();

            }
            catch (Exception e )
            {
                response.ok = false;
                response.msg = "ERROR: " + e.Message;
                return response;
            }


            data.DiasRestantes = obj1.DiasRestantes;
            data.FechaPagoNew = obj1.FechaPago;

            foreach (var item in obj.Socio)
            {
                if (item is null)
                {
                    continue;
                }


                var socioadd = new userAdd
                {
                    Usrnombre = item.Usr.Usrnombre,
                    Usrapp = item.Usr.Usrapp,
                    Usrapm = item.Usr.Usrapm
                };

                

                listUsers.Add(socioadd);
            }

            data.users = listUsers;
            response.ok = true;
            response.msg = "Subscripción actualizada.";
            response.data = data;
            return response;
        }
        // membresia a pagar
        public async Task<Response<MembresiaPagarGet>> membresiaAPagar(int idSocio, int idMemsocid)
        {
            Response<MembresiaPagarGet> response = new Response<MembresiaPagarGet>();

            if (idSocio == 0 && idMemsocid == 0)
            {
                response.ok = false;
                response.msg = "Es necesario mandar dos id socio o id membresia socio";
                return response;
            }

            MembresiaPagarGet membresiaPagar = new MembresiaPagarGet();
            List<userAdd> userAdd = new List<userAdd>();


            var dataSocio = await (from soc in _dataBaseContext.Socio
                             where soc.Socid == idSocio
                             select new Socio
                             {
                                 Socid = soc.Socid,
                                 MemSocid = soc.MemSocid,
                                 MemSoc = (from memsoc in _dataBaseContext.MembresiaSocio
                                          where memsoc.Memsocid == soc.MemSocid
                                          select new MembresiaSocio
                                          {
                                              FechaPago = memsoc.FechaPago,
                                              Membresia = (from mem in _dataBaseContext.Membresia
                                                           where mem.Memid == memsoc.MembresiaId
                                                           select mem).FirstOrDefault(),
                                              Socio = (from soc in _dataBaseContext.Socio
                                                       where soc.MemSocid == memsoc.Memsocid
                                                       select new Socio
                                                       {
                                                           Socid = soc.Socid,
                                                           Usr = (from usr in _dataBaseContext.Usuario
                                                                  where usr.Usrid == soc.Usrid
                                                                  select usr).FirstOrDefault()
                                                       }).ToList()
                                          }).FirstOrDefault(),

                             }).FirstOrDefaultAsync();

            if (dataSocio is null)
            {
                response.ok = false;
                response.msg = "No existe ninguna membresia asociada.";
                return response;
            }


            membresiaPagar.Memnombre = dataSocio.MemSoc.Membresia.Memnombre;
            membresiaPagar.Memdescripcion = dataSocio.MemSoc.Membresia.Memdescripcion;
            membresiaPagar.idMemsocid = (int)dataSocio.MemSocid;

            membresiaPagar.proxFechaPago = dataSocio.MemSoc.FechaPago.AddDays(dataSocio.MemSoc.Membresia.Memduracion);
            membresiaPagar.precioMembresia = (decimal)dataSocio.MemSoc.Membresia.Memprecio;

            foreach (var item in dataSocio.MemSoc.Socio)
            {
                if (item is null)
                {
                    continue;
                }

                userAdd userTemp = new userAdd();
                userTemp.Usrnombre = item.Usr.Usrnombre;
                userTemp.Usrapp = item.Usr.Usrapp;
                userTemp.Usrapm = item.Usr.Usrapm;
                userTemp.Usremail = item.Usr.Usremail;
                userTemp.Usrtelefono = item.Usr.Usrtelefono;

                userAdd.Add(userTemp);
            }
            membresiaPagar.users = userAdd;

            response.ok = true;
            response.msg = "Información de pago de membresia";
            response.data = membresiaPagar;
            return response;

        }
        // membresias asociadas
        public async Task<Response<List<MembresiaAsociadas>>> membresiasAsociadas(long idGym)
        {
            Response<List<MembresiaAsociadas>> response = new Response<List<MembresiaAsociadas>>();
            List<MembresiaAsociadas> membresiaAsociadas = new List<MembresiaAsociadas>();



            var listMem = await (from memSo in _dataBaseContext.MembresiaSocio
                                 orderby memSo.Memsocid descending
                           select new MembresiaSocio
                           {
                               Memsocid = memSo.Memsocid,
                               FechaPago = memSo.FechaPago,
                               DiasRestantes = memSo.DiasRestantes,
                               EstId = memSo.EstId,
                               Membresia = (from mem in _dataBaseContext.Membresia
                                            where mem.Memid == memSo.MembresiaId && mem.GymId == idGym select mem).FirstOrDefault(),
                               Socio = (from soc in _dataBaseContext.Socio
                                        where soc.MemSocid == memSo.Memsocid
                                        select new Socio
                                        {
                                            Socid = soc.Socid,
                                            Usr = (from usr in _dataBaseContext.Usuario
                                                   where usr.Usrid == soc.Usrid && usr.GymId == idGym select usr).FirstOrDefault()
                                        }).ToList()
                           }).ToListAsync();

            if (listMem is null)
            {
                response.ok = false;
                response.msg = "No existen membresias asociadas";
                return response;
            }

            foreach (var item in listMem)
            {
                if (item.Membresia is null)
                {
                    continue;
                }

                MembresiaAsociadas temp = new MembresiaAsociadas();
                List<userAdd> usersTemp = new List<userAdd>();

                temp.FechaPago = item.FechaPago;
                temp.diasFaltantes = item.DiasRestantes;
                temp.Memnombre = item.Membresia.Memnombre;
                temp.Memdescripcion = item.Membresia.Memdescripcion;
                temp.EstId = item.EstId;
                //temp.users = item.Socio

                foreach (var itemU in item.Socio)
                {
                    if (itemU is null)
                    {
                        continue;
                    }

                    userAdd userTemp = new userAdd();
                    userTemp.Usrnombre = itemU.Usr.Usrnombre;
                    userTemp.Usrapp = itemU.Usr.Usrapp;
                    userTemp.Usrapm = itemU.Usr.Usrapm;
                    userTemp.Usremail = itemU.Usr.Usremail;
                    userTemp.Usrtelefono = itemU.Usr.Usrtelefono;

                    usersTemp.Add(userTemp);
                }

                temp.users = usersTemp;

                membresiaAsociadas.Add(temp);
            }


            response.ok = true;
            response.msg = "Lista de membresias Asociadas";
            response.data = membresiaAsociadas;
            return response;
        }
        public async Task<Response<GeneralResp>> nuevoAsocio(AsocioAdd request)
        {
            Response<GeneralResp> salida = new Response<GeneralResp>();
            GeneralResp generalResp = new GeneralResp();


            // validaciones 
            foreach (userAdd user in request.users)
            {
                if (user.Usrnombre == "" || String.IsNullOrEmpty(user.Usrnombre))
                {
                    salida.ok = false;
                    salida.msg = "El nombre del usuario es obligatorio.";
                    return salida;
                }

                if (user.Usrapp == "" || String.IsNullOrEmpty(user.Usrapp))
                {
                    salida.ok = false;
                    salida.msg = "El apellido paterno es obligatorio.";
                    return salida;
                }

                if (user.Usremail == "" || String.IsNullOrEmpty(user.Usremail))
                {
                    salida.ok = false;
                    salida.msg = "El email del usuario es obligatorio.";
                    return salida;
                }

                if (!_generalRepository.ValidaEmail(user.Usremail))
                {
                    salida.ok = false;
                    salida.msg = "El formato del email es incorrecto.";
                    return salida;
                }

                var existUser = await _dataBaseContext.Usuario.Where(x => x.Usremail == user.Usremail).AnyAsync();

                if (existUser)
                {
                    salida.ok = false;
                    salida.msg = "Este email ya esta en uso por otro socio.";
                    return salida;
                }
            }


            // Registro a tabala membrecia socio
            var mem = await _dataBaseContext.Membresia.Where(x => x.Memid == request.Memid).FirstOrDefaultAsync();


            if (mem is null)
            {
                salida.ok = false;
                salida.msg = "No existe ninguna membresia.";
                return salida;
            }

            MembresiaSocio membresiaSocio = new MembresiaSocio();

            try
            {
                membresiaSocio.FechaRegistro = DateTime.Now.Date;
                membresiaSocio.FechaInicio = DateTime.Now.Date;
                membresiaSocio.FechaPago = DateTime.Now.Date.AddDays(mem.Memduracion);
                membresiaSocio.EstId = 8;
                membresiaSocio.DiasRestantes = mem.Memduracion;
                membresiaSocio.MembresiaId = mem.Memid;
                _dataBaseContext.MembresiaSocio.Add(membresiaSocio);
                _dataBaseContext.SaveChanges();
            }
            catch (Exception e)
            {
                salida.ok = false;
                salida.msg = "Error al intentar registrar." + e.Message;
                return salida;
            }


            // agregamos al usuario(s) y socio(s)

            foreach (userAdd user in request.users)
            {
                Usuario usuario = new Usuario();
                Socio socio = new Socio();
                try
                {
                    usuario.Usrnombre = user.Usrnombre;
                    usuario.Usrapp = user.Usrapp;
                    usuario.Usrapm = user.Usrapm;
                    usuario.Usrimagen = "";
                    usuario.Usremail = user.Usremail;
                    usuario.Usrtelefono = user.Usrtelefono;

                    usuario.Usrpassword = "";
                    usuario.GymId = request.Gymid;
                    usuario.RolId = 3;
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


                try
                {
                    socio.Usrid = usuario.Usrid;
                    socio.MemSocid = membresiaSocio.Memsocid;
                    _dataBaseContext.Socio.Add(socio);
                    _dataBaseContext.SaveChanges();
                }
                catch (Exception e)
                {
                    salida.ok = false;
                    salida.msg = "Error al intentar registrar el socio." + e.Message;
                    return salida;
                }
            }


            salida.ok = true;
            salida.msg = "Asocio Correcto";
            generalResp.Details = "Membresia asociada correctamente";
            salida.data = generalResp;
            return salida;
        }
    }
}
