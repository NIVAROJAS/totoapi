using System;
using System.Collections.Generic;
using System.Linq;

namespace Tottos.Models.Dto
{
    public class itemDto
    {
        public int id { get; set; }

        public string descripcion { get; set; }

        public sbyte activo { get; set; }

        public static itemDto FromModel(item model)
        {
            return new itemDto()
            {
                id = model.id, 
                descripcion = model.descripcion, 
                activo = model.activo, 
            }; 
        }

        public item ToModel()
        {
            return new item()
            {
                id = id, 
                descripcion = descripcion, 
                activo = activo, 
            }; 
        }
    }
}