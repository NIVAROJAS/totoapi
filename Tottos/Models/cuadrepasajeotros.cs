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
    
    public partial class cuadrepasajeotros
    {
        public int id { get; set; }
        public int idCuadre { get; set; }
        public string nombre { get; set; }
        public string lugar { get; set; }
        public string concepto { get; set; }
        public decimal monto { get; set; }
    
        public virtual cuadre cuadre { get; set; }
    }
}
