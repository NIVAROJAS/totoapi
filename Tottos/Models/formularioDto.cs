using System;
using System.Collections.Generic;
using System.Linq;

namespace Tottos.Models.Dto
{
    public class formularioDto
    {
        public int id { get; set; }

        public string nombre { get; set; }

        public Nullable<int> idmenu { get; set; }

        public string nombremenu { get; set; }

        public string icon { get; set; }

        public Nullable<int> orden { get; set; }

        public Nullable<sbyte> activo { get; set; }

        public Nullable<sbyte> visible { get; set; }

        public static formularioDto FromModel(formulario model)
        {
            return new formularioDto()
            {
                id = model.id, 
                nombre = model.nombre, 
                idmenu = model.idmenu, 
                icon = model.icon,
                nombremenu = model.nombremenu, 
                orden = model.orden, 
                activo = model.activo, 
                visible = model.visible, 
            }; 
        }

        public formulario ToModel()
        {
            return new formulario()
            {
                id = id, 
                nombre = nombre, 
                idmenu = idmenu, 
                nombremenu = nombremenu, 
                orden = orden, 
                activo = activo, 
                visible = visible, 
            }; 
        }
    }
}