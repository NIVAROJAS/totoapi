using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tottos.Models
{
    public class permisosrolDto
    {
        public int idformulario { get; set; }
        public string nombre { get; set; }
        public string nombreenmenu { get; set; }
        public Nullable<bool> Leer { get; set; }
        public Nullable<bool> Adicionar { get; set; }
        public Nullable<bool> Editar { get; set; }
        public Nullable<bool> Eliminar { get; set; }
    }
}