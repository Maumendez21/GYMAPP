﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BackendGymStar.Data
{
    public partial class Usuario
    {
        public Usuario()
        {
            Socio = new HashSet<Socio>();
        }

        public int Usrid { get; set; }
        public string Usrnombre { get; set; }
        public string Usrapp { get; set; }
        public string Usrapm { get; set; }
        public string Usrimagen { get; set; }
        public string Usremail { get; set; }
        public string Usrtelefono { get; set; }
        public string Usrpassword { get; set; }
        public byte[] Usrhuella { get; set; }
        public long GymId { get; set; }
        public long RolId { get; set; }
        public long EstId { get; set; }
        public DateTime UsrfecReg { get; set; }
        public bool Usractivo { get; set; }

        public virtual Estatus Est { get; set; }
        public virtual Gimnasio Gym { get; set; }
        public virtual Rol Rol { get; set; }
        public virtual ICollection<Socio> Socio { get; set; }
    }
}