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
    
    public partial class cuadre
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public cuadre()
        {
            this.cuadrecaja = new HashSet<cuadrecaja>();
            this.cuadrepasajeotros = new HashSet<cuadrepasajeotros>();
            this.cuadreticket = new HashSet<cuadreticket>();
            this.cuadreinventario = new HashSet<cuadreinventario>();
        }
    
        public int id { get; set; }
        public System.DateTime fecha { get; set; }
        public int cajero { get; set; }
        public int local { get; set; }
        public sbyte estado { get; set; }
    
        public virtual local local1 { get; set; }
        public virtual usuario usuario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cuadrecaja> cuadrecaja { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cuadrepasajeotros> cuadrepasajeotros { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cuadreticket> cuadreticket { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cuadreinventario> cuadreinventario { get; set; }
    }
}