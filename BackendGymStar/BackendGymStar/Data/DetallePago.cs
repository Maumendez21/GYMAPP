﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BackendGymStar.Data
{
    public partial class DetallePago
    {
        public int Detpagoid { get; set; }
        public int PagoId { get; set; }
        public string Descripcion { get; set; }
        public double Subtotal { get; set; }
        public int Cantidad { get; set; }

        public virtual Pago Pago { get; set; }
    }
}