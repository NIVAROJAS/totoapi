using System;
using System.Collections.Generic;
using System.Linq;

namespace Tottos.Models.Dto
{
    public class menuDto
    {
        public int id { get; set; }

        public string descripcion { get; set; }

        public string orden { get; set; }

        public string icon { get; set; }

        public ICollection<formulario> formulario { get; set; }

        public static menuDto FromModel(menu model)
        {
            return new menuDto()
            {
                id = model.id, 
                descripcion = model.descripcion, 
                orden = model.orden, 
                icon = model.icon, 
                formulario = model.formulario, 
            }; 
        }

        public menu ToModel()
        {
            return new menu()
            {
                id = id, 
                descripcion = descripcion, 
                orden = orden, 
                icon = icon, 
                formulario = formulario, 
            }; 
        }
    }
}