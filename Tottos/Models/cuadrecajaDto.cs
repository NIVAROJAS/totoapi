using System;

namespace Tottos.Models.Dto
{
    public class cuadrecajaDto
    {
        public int id { get; set; }

        public Nullable<int> idCuadre { get; set; }

        public decimal efectivo { get; set; }

        public decimal visa { get; set; }

        public decimal egreso { get; set; }

        public decimal caja { get; set; }

        public cuadreDto cuadre { get; set; }

        public static cuadrecajaDto FromModel(cuadrecaja model)
        {
            return new cuadrecajaDto()
            {
                id = model.id, 
                idCuadre = model.idCuadre, 
                efectivo = model.efectivo, 
                visa = model.visa, 
                egreso = model.egreso, 
                caja = model.caja, 
                cuadre = cuadreDto.FromModel(model.cuadre), 
            }; 
        }

        public cuadrecaja ToModel()
        {
            return new cuadrecaja()
            {
                id = id, 
                idCuadre = idCuadre, 
                efectivo = efectivo, 
                visa = visa, 
                egreso = egreso, 
                caja = caja, 
                cuadre = cuadre.ToModel(), 
            }; 
        }
    }

    public class ResumenCuadrecajaDto
    {

        public Nullable<int> idCuadre { get; set; }

        public DateTime fecha { get; set; }

        public string local { get; set; }

        public string cajero { get; set; }

        public decimal efectivo { get; set; }

        public decimal visa { get; set; }

        public decimal egreso { get; set; }

        public decimal caja { get; set; }
        
        public decimal cierre { get; set; }


    }
         
}