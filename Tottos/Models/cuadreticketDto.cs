using System;

namespace Tottos.Models.Dto
{
    public class cuadreticketDto
    {
        public int id { get; set; }

        public Nullable<int> idCuadre { get; set; }

        public Nullable<int> nroTicketsInicio { get; set; }

        public Nullable<int> nroTicketsFin { get; set; }

        public cuadreDto cuadre { get; set; }

        public static cuadreticketDto FromModel(cuadreticket model)
        {
            return new cuadreticketDto()
            {
                id = model.id, 
                idCuadre = model.idCuadre, 
                nroTicketsInicio = model.nroTicketsInicio, 
                nroTicketsFin = model.nroTicketsFin, 
                cuadre = cuadreDto.FromModel(model.cuadre), 
            }; 
        }

        public cuadreticket ToModel()
        {
            return new cuadreticket()
            {
                id = id, 
                idCuadre = idCuadre, 
                nroTicketsInicio = nroTicketsInicio, 
                nroTicketsFin = nroTicketsFin, 
                cuadre = cuadre.ToModel(), 
            }; 
        }
    }
}