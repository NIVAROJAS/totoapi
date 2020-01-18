using System;
using System.Linq;

namespace Tottos.Models.Dto
{
    public class asistenciaDto
    {
        public int id { get; set; }

        public int idPersonal { get; set; }

        public Nullable<System.DateTime> fechaIngreso { get; set; }

        public Nullable<System.DateTime> fechaSalida { get; set; }

        public string nombre { get; set; }
        public string observacion_ { get; set; }

        public usuarioDto usuario { get; set; }

        public static asistenciaDto FromModel(asistencia model)
        {
            return new asistenciaDto()
            {
                id = model.id, 
                idPersonal = model.idPersonal, 
                fechaIngreso = model.fechaIngreso, 
                fechaSalida = model.fechaSalida, 
                observacion_ = model.observacion_, 
                usuario = model.usuario != null? usuarioDto.FromModel(model.usuario) : null, 
            }; 
        }

        public asistencia ToModel()
        {
            return new asistencia()
            {
                id = id, 
                idPersonal = idPersonal, 
                fechaIngreso = fechaIngreso, 
                fechaSalida = fechaSalida, 
                observacion_ = observacion_, 
                usuario = usuario.ToModel(), 
            }; 
        }
    }
}