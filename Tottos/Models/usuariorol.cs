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
    
    public partial class usuariorol
    {
        public int idusuario { get; set; }
        public int idrol { get; set; }
        public Nullable<sbyte> activo { get; set; }
    
        public virtual rol rol { get; set; }
        public virtual usuario usuario { get; set; }
    }
}
