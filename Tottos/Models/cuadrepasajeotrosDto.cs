using System;
using System.Linq;

namespace Tottos.Models.Dto
{
    public class cuadrepasajeotrosDto
    {
        public int id { get; set; }

        public int idCuadre { get; set; }

        public string nombre { get; set; }

        public string lugar { get; set; }

        public string concepto { get; set; }

        public decimal monto { get; set; }

        public cuadreDto cuadre { get; set; }

        public static cuadrepasajeotrosDto FromModel(cuadrepasajeotros model)
        {
            return new cuadrepasajeotrosDto()
            {
                id = model.id, 
                idCuadre = model.idCuadre, 
                nombre = model.nombre, 
                lugar = model.lugar, 
                concepto = model.concepto, 
                monto = model.monto, 
                cuadre = cuadreDto.FromModel(model.cuadre), 
            }; 
        }

        public cuadrepasajeotros ToModel()
        {
            return new cuadrepasajeotros()
            {
                id = id, 
                idCuadre = idCuadre, 
                nombre = nombre, 
                lugar = lugar, 
                concepto = concepto, 
                monto = monto, 
                cuadre = cuadre.ToModel(), 
            }; 
        }
    }
}