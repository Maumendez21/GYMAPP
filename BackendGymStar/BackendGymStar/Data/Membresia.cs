﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BackendGymStar.Data
{
    public partial class Membresia
    {
        public Membresia()
        {
            MembresiaSocio = new HashSet<MembresiaSocio>();
        }

        public int Memid { get; set; }
        public string Memnombre { get; set; }
        public string Memdescripcion { get; set; }
        public double Memprecio { get; set; }
        public int Memduracion { get; set; }
        public int Mempersonas { get; set; }
        public long EstId { get; set; }
        public long GymId { get; set; }
        public DateTime MemfecReg { get; set; }
        public bool Memactivo { get; set; }
        public int MemusrReg { get; set; }

        public virtual Estatus Est { get; set; }
        public virtual Gimnasio Gym { get; set; }
        public virtual ICollection<MembresiaSocio> MembresiaSocio { get; set; }
    }
}