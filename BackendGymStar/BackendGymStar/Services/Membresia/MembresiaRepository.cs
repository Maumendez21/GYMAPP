using AutoMapper;
using BackendGymStar.Data;
using BackendGymStar.DTO;
using BackendGymStar.DTO.General;
using BackendGymStar.DTO.Salidas;
using Microsoft.EntityFrameworkCore;

namespace BackendGymStar.Services.Membresia
{
    public class MembresiaRepository : IMembresiaRepository
    {
        private readonly DataBaseContext _dataBaseContext;
        private readonly IMapper _mapper;

        public MembresiaRepository(DataBaseContext dataBaseContext, IMapper mapper)
        {
            this._dataBaseContext = dataBaseContext;
            this._mapper = mapper;
        }


       public async Task<Response<List<MembresiaDTO>>> ListMembresias(long idGym)
        {
            Response<List<MembresiaDTO>> salida = new Response<List<MembresiaDTO>>();

            var existGym = await _dataBaseContext.Gimnasio.Where(g => g.Gymid == idGym).AnyAsync();

            if (!existGym)
            {
                salida.ok = false;
                salida.msg = "El gimansio no existe.";
                return salida;
            }


            List<MembresiaDTO> membresias = _mapper.Map<List<MembresiaDTO>>(await (from m in _dataBaseContext.Membresia
                                             where m.GymId == idGym
                                             select new BackendGymStar.Data.Membresia
                                             {
                                                 Memid = m.Memid,
                                                 Memnombre = m.Memnombre,
                                                 Memdescripcion = m.Memdescripcion,
                                                 Memprecio = m.Memprecio,
                                                 Memduracion = m.Memduracion,
                                                 Mempersonas = m.Mempersonas,
                                                 EstId = m.EstId
                                             }).ToListAsync());


            if (membresias == null)
            {
                salida.ok = false;
                salida.msg = "No hay membresias.";
                return salida;
            }

            salida.ok = true;
            salida.msg = "";
            salida.data = membresias;
            return salida;
        }


        public async Task<Response<SalidasResponse>> desHabMembresia(DesHabDelMem model)
        {
            // accion 1: deshabilitar, habilitar. accion = 2: borrar
            Response<SalidasResponse> salida = new Response<SalidasResponse>();

            Data.Membresia membresia = await _dataBaseContext.Membresia.Where(m => m.Memid == model.idMem).FirstOrDefaultAsync();

            if (membresia is null)
            {
                salida.ok = false;
                salida.msg = "La membresia solicitada no existe";
                return salida;
            }


            if (membresia.MembresiaSocio.Count > 0)
            {
                salida.ok = false;
                salida.msg = "Esta Membresia no se puede deshabilitar o eliminar, esta siendo utilizada.";
                return salida;
            }


            if (model.accion == 1)
            {
                if (membresia.EstId == 6)
                {
                    // deshabilitar
                    membresia.EstId = 7;
                    _dataBaseContext.SaveChanges();
                    salida.ok = true;
                    salida.msg = "Membresia deshabilitada";
                }
                else
                {
                    // habilitar
                    membresia.EstId = 6;
                    _dataBaseContext.SaveChanges();
                    salida.ok = true;
                    salida.msg = "Membresia habilitada";
                }
            }
            else if (model.accion == 2)
            {
                // eliminar

                try
                {
                    _dataBaseContext.Membresia.Remove(membresia);
                    _dataBaseContext.SaveChanges();
                    salida.ok = true;
                    salida.msg = "Membresia eliminada.";
                }
                catch (Exception e)
                {
                    salida.ok = false;
                    salida.msg = "Ocurrio un error " + e.Message;
                    return salida;
                }
                
            }
            return salida;
        }


        public async Task<Response<MembresiaDTO>> ActionsMembership(MembresiaDTO membresiaRequest) 
        {
            Response<MembresiaDTO> salida = new Response<MembresiaDTO>();

            Data.Membresia membresiaAction = new Data.Membresia();

            

            if (membresiaRequest.Memnombre == "" || String.IsNullOrEmpty(membresiaRequest.Memnombre))
            {
                salida.ok = false;
                salida.msg = "El nombre de la membresia es obligatorio.";
                return salida;
            }
            
            if (membresiaRequest.Memprecio == 0)
            {
                salida.ok = false;
                salida.msg = "El precio de la membresia es obligatorio.";
                return salida;
            }
          
            
            if (membresiaRequest.Memduracion == 0)
            {
                salida.ok = false;
                salida.msg = "La duración de la membresia es obligatoria.";
                return salida;
            }
            
            if (membresiaRequest.Mempersonas == 0)
            {
                salida.ok = false;
                salida.msg = "La cantidad de personas de la membresia es obligatoria.";
                return salida;
            }
            
            if (membresiaRequest.MemusrReg == 0)
            {
                salida.ok = false;
                salida.msg = "El usuario que va a registrar es obligatorio.";
                return salida;
            }
            
            if (membresiaRequest.GymId == 0)
            {
                salida.ok = false;
                salida.msg = "El gimnasio que va a registrar es obligatorio.";
                return salida;
            }


            if (membresiaRequest.Memid != 0)
            {
                membresiaAction = await _dataBaseContext.Membresia.Where(m => m.Memid == membresiaRequest.Memid).FirstOrDefaultAsync();

                if (membresiaAction is null)
                {
                    salida.ok = false;
                    salida.msg = "La membresia a actualizar no existe.";
                    return salida;
                }
            }


            try
            {
                membresiaAction.Memnombre = membresiaRequest.Memnombre;
                membresiaAction.Memdescripcion = membresiaRequest.Memdescripcion;
                membresiaAction.Memprecio = membresiaRequest.Memprecio;
                membresiaAction.Memduracion = membresiaRequest.Memduracion;
                membresiaAction.Mempersonas = membresiaRequest.Mempersonas;

                membresiaAction.GymId = membresiaRequest.GymId;
                membresiaAction.MemusrReg = membresiaRequest.MemusrReg;

                membresiaAction.MemfecReg = DateTime.Now;
                membresiaAction.Memactivo = true;
                membresiaAction.EstId = 6;

                if (membresiaRequest.Memid != 0)
                {
                    _dataBaseContext.SaveChanges();
                    salida.ok = true;
                    salida.msg = $"Membresia actualizada.";
                    salida.data = membresiaRequest;
                }else
                {
                    _dataBaseContext.Membresia.Add(membresiaAction);
                    _dataBaseContext.SaveChanges();
                    salida.ok = true;
                    salida.msg = $"Membresia {membresiaAction.Memnombre} registrada.";
                    salida.data = membresiaRequest;
                }

            }
            catch (Exception e)
            {
                salida.ok = false;
                salida.msg = "La membresia a actualizar no existe.";
            }

            return salida;
        }
     
    }
}
