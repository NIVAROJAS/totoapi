using System;
using System.Collections.Generic;
using System.Linq;

namespace Tottos.Models.Dto
{
    public class usuarioDto
    {
        public int id { get; set; }

        public string nombre { get; set; }

        public string login { get; set; }

        public string password { get; set; }

        public Nullable<sbyte> activo { get; set; }

        public int[] Roles { get; set; }

        public static usuarioDto FromModel(usuario model)
        {
            return new usuarioDto()
            {
                id = model.id, 
                nombre = model.nombre, 
                login = model.login, 
                password = model.password, 
                activo = model.activo, 
            }; 
        }

        public usuario ToModel()
        {
            var x = new usuario();
            x.id = id;
            x.nombre = nombre;
            x.login = login;
            x.password = password;
            x.activo = activo;

            return x; 
        }

        public usuario ToModel(usuario x)
        {
            x.nombre = nombre;
            x.login = login;
            x.password = password;
            x.activo = activo;

            return x;
        }
    }
}