﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BackendGymStar.Data
{
    public partial class Socio
    {
        public int Socid { get; set; }
        public int Usrid { get; set; }
        public int? MemSocid { get; set; }

        public virtual MembresiaSocio MemSoc { get; set; }
        public virtual Usuario Usr { get; set; }
    }
}