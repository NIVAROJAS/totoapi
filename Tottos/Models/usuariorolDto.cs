using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tottos.Models.Dto;

namespace Tottos.Models
{
    public class usuariorolDto {

        public Nullable<int> IdUsuario { get; set; }

        public int IdRol { get; set; }

        public rolDto rol { get; set; }

        public usuarioDto usuario { get; set; }

        public string DescripcionRol { get; set; }


        //public static usuariorolDto FromModel(ADM_UsuarioRol model)
        //{
        //    return new usuariorolDto()
        //    {
        //        IdUsuario = model.IdUsuario,
        //        IdRol = model.IdRol,
        //        FechaCreacion = model.FechaCreacion,
        //        FechaModificacion = model.FechaModificacion,
        //        Usuario = model.Usuario,
        //    };
        //}

        //public usuariorol ToModel()
        //{
        //    return new usuariorol()
        //    {
        //        IdUsuario = IdUsuario.Value,
        //        IdRol = IdRol,
        //        FechaCreacion = FechaCreacion,
        //        FechaModificacion = FechaModificacion,
        //        Usuario = Usuario,
        //    };
        //}
    }
}