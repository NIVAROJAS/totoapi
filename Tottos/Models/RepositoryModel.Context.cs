﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tottos.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class repositoryEntities : DbContext
    {
        public repositoryEntities()
            : base("name=repositoryEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<formulaiorolpermiso> formulaiorolpermiso { get; set; }
        public virtual DbSet<formulario> formulario { get; set; }
        public virtual DbSet<menu> menu { get; set; }
        public virtual DbSet<permiso> permiso { get; set; }
        public virtual DbSet<rol> rol { get; set; }
        public virtual DbSet<usuario> usuario { get; set; }
        public virtual DbSet<usuariorol> usuariorol { get; set; }
        public virtual DbSet<local> local { get; set; }
        public virtual DbSet<cuadre> cuadre { get; set; }
        public virtual DbSet<cuadrecaja> cuadrecaja { get; set; }
        public virtual DbSet<cuadrepasajeotros> cuadrepasajeotros { get; set; }
        public virtual DbSet<cuadreticket> cuadreticket { get; set; }
        public virtual DbSet<item> item { get; set; }
        public virtual DbSet<cuadreinventario> cuadreinventario { get; set; }
        public virtual DbSet<asistencia> asistencia { get; set; }
        public virtual DbSet<remuneraciones> remuneraciones { get; set; }
    }
}
