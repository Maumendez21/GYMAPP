using AutoMapper;
using BackendGymStar.Data;
using BackendGymStar.DTO;
using BackendGymStar.DTO.Entradas;
using BackendGymStar.DTO.Salidas;
using Microsoft.EntityFrameworkCore;

namespace BackendGymStar.Services.Caja
{
    public class CajaRepository: ICajaRepository
    {
        private readonly DataBaseContext _baseContext;
        private readonly IMapper _mapper;

        public CajaRepository(DataBaseContext baseContext, IMapper mapper)
        {
            this._baseContext = baseContext;
            this._mapper = mapper;
        }

        public async Task<CajaDTO> cajaByIdGym(long indGym, long idUser)
        {
            CajaDTO salida = null;
            salida = _mapper.Map<CajaDTO>( await (from c in _baseContext.Caja where c.GymId == indGym && c.EstId == 10 && c.CajausrReg == idUser select c).FirstOrDefaultAsync());
            return salida;
        }
        
        
        public async Task<CajaDTO> cajaById(long idCaja)
        {
            CajaDTO salida = null;
            salida = _mapper.Map<CajaDTO>( await (from c in _baseContext.Caja where c.Cajaid == idCaja && c.EstId != 10 select c).FirstOrDefaultAsync());
            return salida;
        }

        public async Task<Response<List<DetalleCaja>>> pagosBycaja(int cajaId)
        {
            Response<List<DetalleCaja>> response = new Response<List<DetalleCaja>>();
            List<DetalleCaja> detalleCajas = new List<DetalleCaja>();

            var list = await (from c in _baseContext.Caja
                        where c.Cajaid == cajaId
                        select new Data.Caja
                        {
                            DetalleCaja = (from dt in _baseContext.DetalleCaja
                                           where dt.CajaId == c.Cajaid
                                           orderby dt.Detcajaid descending
                                           select new DetalleCaja {
                                           
                                               Detcajaid = dt.Detcajaid,
                                               Pago = (from p in _baseContext.Pago 
                                                       where p.Pagoid == dt.PagoId orderby p.Pagoid descending select p).FirstOrDefault()
                                           }).ToList()
                        }).FirstOrDefaultAsync();


            if (list is null)
            {
                response.ok = false;
                response.msg = "No existen pagos.";
                response.data = new List<DetalleCaja>();
                return response;
            }


            foreach (DetalleCaja item in list.DetalleCaja)
            {
                detalleCajas.Add(item);
            }

            response.ok = true;
            response.msg = "listado de pagos";
            response.data = detalleCajas;

            return response;
        }


        public async Task<Response<List<DetallePago>>> pagoDetail(int pagoid)
        {
            Response<List<DetallePago>> response = new Response<List<DetallePago>>();
            List<DetallePago> listDetail = new List<DetallePago>();

            listDetail = await (from dp in _baseContext.DetallePago where dp.PagoId == pagoid select dp).ToListAsync();

            if (listDetail is null)
            {
                response.ok = false;
                response.msg = "No hay detalles";
                response.data = new List<DetallePago>();
                return response;
            }

            response.ok = true;
            response.msg = "detalles de pago";
            response.data = listDetail;


            return response;
        }
        

        public async Task<Response<List<CajaDTO>>> historialCajaByGym(long idGym)
        {
            Response<List<CajaDTO>> response = new Response<List<CajaDTO>>();


            var listCajas = await (from c in _baseContext.Caja where c.GymId == idGym && c.EstId != 10 select c)
                .ToListAsync();

            if (listCajas is null)
            {
                response.ok = false;
                response.msg = "No hay ningún listado de cajas en este gym";
                return response;
            }

            List<CajaDTO> listSalida = new List<CajaDTO>();
            listSalida = _mapper.Map<List<CajaDTO>>(listCajas);

            response.ok = true;
            response.msg = "Historial de cajas";
            response.data = listSalida;
            return response;
        }
        

        public async Task<Response<CajaDTO>> abrirCaja(AbrirCajaAdd cajaOpen)
        {
            Response<CajaDTO> response = new Response<CajaDTO>();


            var existCaja = await _baseContext.Caja.Where(c => c.GymId == cajaOpen.idGym && c.EstId == 10 && c.CajausrReg == cajaOpen.idUserReg).AnyAsync();

            if (existCaja)
            {
                response.ok = false;
                response.msg = "Solo puede haber una caja registrada por usuario.";
                return response;
            }

            Data.Caja cajaAdd = new Data.Caja();
            cajaAdd.CajamontoApertura = cajaOpen.CajamontoApertura;
            cajaAdd.CajafechaApertura = DateTime.Now;
            cajaAdd.CajafecReg = DateTime.Now;
            cajaAdd.CajausrReg = (int)cajaOpen.idUserReg;
            cajaAdd.GymId = cajaOpen.idGym;
            cajaAdd.EstId = 10;
            cajaAdd.CajamontoActual = cajaOpen.CajamontoApertura;


            try
            {
                _baseContext.Caja.Add(cajaAdd);
                _baseContext.SaveChanges();
                CajaDTO cajaDTO = _mapper.Map<CajaDTO>(cajaAdd);
                response.ok = true;
                response.msg = $"Caja abierta a las ${cajaAdd.CajafechaApertura.ToString()}";
                response.data = cajaDTO;
            }
            catch (Exception e)
            {
                response.ok = false;
                response.msg = "ERROR: " + e.Message;
                return response;
            }
            return response;
        }


        public async Task<Response<GeneralResp>> cerrarCaja(int idCaja)
        {
            Response<GeneralResp> response = new Response<GeneralResp>();
            GeneralResp generalResp = new GeneralResp();

            Data.Caja existCaja = await _baseContext.Caja.Where(c => c.Cajaid == idCaja && c.EstId == 10).FirstOrDefaultAsync();


            if (existCaja is null)
            {
                response.ok = false;
                response.msg = "No existe ninguna caja";
                return response;
            }

            try
            {
                existCaja.CajamontoCierre = existCaja.CajamontoActual;
                existCaja.EstId = 11;
                existCaja.CajafechaCierre = DateTime.Now;
                _baseContext.SaveChanges();
                response.ok = true;
                response.msg = "Caja cerrada.";
                generalResp.Details = "Caja cerrada correctamente " + existCaja.CajafechaCierre.ToString();
                response.data = generalResp;
            }
            catch (Exception e)
            {
                response.ok = false;
                response.msg = "ERROR: " + e.Message;
                return response;
            }

            return response;
        }



        public async Task<Response<GeneralResp>> realizaPago(PagoAdd pagoAdd)
        {
            Response<GeneralResp> salida = new Response<GeneralResp>();
            GeneralResp data = new GeneralResp();

            if (pagoAdd.DescripcionPago == "" || String.IsNullOrEmpty(pagoAdd.DescripcionPago))
            {
                salida.ok = false;
                salida.msg = "Debe haber alguna descripción del pago";
                return salida;
            }


            if (pagoAdd.detalle.Count == 0)
            {
                salida.ok = false;
                salida.msg = "No hay ningún detalle por registrar.";
                return salida;
            }

            // RealizaPago
            Pago pagoNew = new Pago();
            pagoNew.Descripcion = pagoAdd.DescripcionPago;
            pagoNew.Total = (float)pagoAdd.detalle.Sum(item => item.subtotal);
            pagoNew.UsuarioId = pagoAdd.usrPagoId;
            pagoNew.ConceptoId = pagoAdd.conceptoId;
            pagoNew.UsrReg = pagoAdd.usrReg;
            pagoNew.Gymid = pagoAdd.gymId;
            pagoNew.FechaPago = DateTime.Now;
            pagoNew.HoraPago = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            var countPagos = _baseContext.Pago.Where(p => p.Gymid == pagoAdd.gymId).Count();

            string refe = "REF" + pagoAdd.gymId;
            if (countPagos == 0 || countPagos == null)
            {
                refe += "001";
            }
            else if (countPagos.ToString().Trim().Length == 3)
                refe +=  countPagos.ToString().Trim();
            else if (countPagos.ToString().Trim().Length == 2)
                refe += "0"  + countPagos.ToString().Trim();
            else if (countPagos.ToString().Trim().Length == 1)
                refe += "00"  + countPagos.ToString().Trim();

            pagoNew.Referencia = refe;

            try
            {
                _baseContext.Pago.Add(pagoNew);
                _baseContext.SaveChanges();
            }
            catch (Exception e)
            {
                salida.ok = false;
                salida.msg = "Error a registrar el pago.";
                data.Details = "" + e.Message;
                salida.data = data;
                return salida;
            }

            //Registra Detalles de pago


            foreach (PagoDetalleAdd item in pagoAdd.detalle)
            {
                DetallePago detallePago = new DetallePago();

                detallePago.PagoId = pagoNew.Pagoid;
                detallePago.Descripcion = item.descripcionDetalle;
                detallePago.Subtotal = item.subtotal;
                detallePago.Cantidad = item.cantidad;

                try
                {
                    _baseContext.DetallePago.Add(detallePago);
                    _baseContext.SaveChanges();
                }
                catch (Exception e)
                {
                    data.Details += e.Message + " ";
                }

            }



            // agregamos el dealle de caja

            DetalleCaja detalleCaja = new DetalleCaja();
            detalleCaja.CajaId = pagoAdd.cajaId;
            detalleCaja.PagoId = pagoNew.Pagoid;

            try
            {
                _baseContext.DetalleCaja.Add(detalleCaja);
                _baseContext.SaveChanges();
            }
            catch (Exception e)
            {
                salida.ok = false;
                salida.msg = "Error a registrar el pago.";
                data.Details = "" + e.Message;
                salida.data = data;
                return salida;

            }


            // actualizamos la caja
            Data.Caja caja = await _baseContext.Caja.Where(c => c.Cajaid == pagoAdd.cajaId && c.EstId == 10).FirstOrDefaultAsync();

            if (caja is null)
            {
                salida.ok = false;
                salida.msg = "No existe una caja abierta.";
                return salida;
            }


            caja.CajamontoActual += pagoNew.Total;
            caja.CajafecReg = DateTime.Now;
            caja.CajausrReg = pagoAdd.usrReg;
            _baseContext.SaveChanges();


            salida.ok = true;
            salida.msg = "Pago realizado correctamente " + pagoNew.Referencia;
            data.Details = pagoNew.Total.ToString();
            salida.data = data;
            return salida;
        }







    }
}
