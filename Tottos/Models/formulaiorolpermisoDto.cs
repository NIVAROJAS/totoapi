using System;

namespace Tottos.Models.Dto
{
    public class formulaiorolpermisoDto
    {
        public int idformulario { get; set; }

        public int idrol { get; set; }

        public int idpermiso { get; set; }

        public formularioDto formulario { get; set; }

        public permisoDto permiso { get; set; }

        public rolDto rol { get; set; }

        public static formulaiorolpermisoDto FromModel(formulaiorolpermiso model)
        {
            return new formulaiorolpermisoDto()
            {
                idformulario = model.idformulario, 
                idrol = model.idrol, 
                idpermiso = model.idpermiso, 
                formulario = formularioDto.FromModel(model.formulario), 
                permiso = permisoDto.FromModel(model.permiso), 
                rol = rolDto.FromModel(model.rol), 
            }; 
        }

        public formulaiorolpermiso ToModel()
        {
            return new formulaiorolpermiso()
            {
                idformulario = idformulario, 
                idrol = idrol, 
                idpermiso = idpermiso, 
                formulario = formulario.ToModel(), 
                permiso = permiso.ToModel(), 
                rol = rol.ToModel(), 
            }; 
        }
    }
}