//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class cuadreinventario
    {
        public int id { get; set; }
        public int idCuadre { get; set; }
        public int idItem { get; set; }
        public decimal ingreso { get; set; }
        public decimal venta { get; set; }
        public decimal otros { get; set; }
        public string observacionesOtros { get; set; }
        public decimal inicial { get; set; }
        public decimal saldo { get; set; }
    
        public virtual cuadre cuadre { get; set; }
        public virtual item item { get; set; }
    }
}