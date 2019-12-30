using System;
using System.Collections.Generic;
using System.Linq;

namespace Tottos.Models.Dto
{
    public class rolDto
    {
        public int id { get; set; }

        public string descripcion { get; set; }

        public Nullable<sbyte> activo { get; set; }

        public Nullable<sbyte> administrador { get; set; }

        public static rolDto FromModel(rol model)
        {
            return new rolDto()
            {
                id = model.id, 
                descripcion = model.descripcion, 
                activo = model.activo, 
                administrador = model.administrador, 
            }; 
        }

        public rol ToModel()
        {
            return new rol()
            {
                id = id, 
                descripcion = descripcion, 
                activo = activo, 
                administrador = administrador,
            }; 
        }
    }
}