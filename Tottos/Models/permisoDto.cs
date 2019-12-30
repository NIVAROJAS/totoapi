using System;
using System.Collections.Generic;
using System.Linq;

namespace Tottos.Models.Dto
{
    public class permisoDto
    {
        public int id { get; set; }

        public string descripcion { get; set; }

        public ICollection<formulaiorolpermiso> formulaiorolpermisoes { get; set; }

        public static permisoDto FromModel(permiso model)
        {
            return new permisoDto()
            {
                id = model.id, 
                descripcion = model.descripcion, 
                formulaiorolpermisoes = model.formulaiorolpermiso, 
            }; 
        }

        public permiso ToModel()
        {
            return new permiso()
            {
                id = id, 
                descripcion = descripcion, 
                formulaiorolpermiso = formulaiorolpermisoes, 
            }; 
        }
    }
}