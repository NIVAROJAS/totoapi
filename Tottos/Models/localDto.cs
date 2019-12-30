using System;
using System.Collections.Generic;
using System.Linq;

namespace Tottos.Models.Dto
{
    public class localDto
    {
        public int id { get; set; }

        public string nombre { get; set; }


        public static localDto FromModel(local model)
        {
            return new localDto()
            {
                id = model.id, 
                nombre = model.nombre, 
            }; 
        }

        public local ToModel()
        {
            return new local()
            {
                id = id, 
                nombre = nombre, 
            }; 
        }
    }
}