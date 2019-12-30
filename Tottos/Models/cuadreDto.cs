using System;

namespace Tottos.Models.Dto
{
    public class cuadreDto
    {
        public int id { get; set; }

        public System.DateTime fecha { get; set; }

        public int cajero { get; set; }

        public int local { get; set; }

        public sbyte estado { get; set; }

        public usuarioDto usuario { get; set; }
        public string usuarionombre { get; set; }
        public string localnombre { get; set; }

        public static cuadreDto FromModel(cuadre model)
        {
            return new cuadreDto()
            {
                id = model.id,
                fecha = model.fecha,
                cajero = model.cajero,
                local = model.local,
                estado = model.estado,
                usuarionombre = model.usuario.nombre,
                localnombre = model.local1.nombre,
                usuario = usuarioDto.FromModel(model.usuario), 
            }; 
        }

        public cuadre ToModel()
        {
            return new cuadre()
            {
                id = id, 
                fecha = fecha, 
                cajero = cajero, 
                local = local, 
                estado = estado, 
                usuario = usuario.ToModel(), 
            }; 
        }
    }
}