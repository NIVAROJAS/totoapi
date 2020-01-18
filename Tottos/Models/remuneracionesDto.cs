using System;
using System.Linq;

namespace Tottos.Models.Dto
{
    public class remuneracionesDto
    {
        public int id { get; set; }

        public int idPersonal { get; set; }

        public int aniopago { get; set; }

        public int mespago { get; set; }

        public System.DateTime dia { get; set; }

        public string concepto { get; set; }

        public decimal importe { get; set; }

        public usuarioDto usuario { get; set; }

        public string nombre { get; set; }

        public static remuneracionesDto FromModel(remuneraciones model)
        {
            return new remuneracionesDto()
            {
                id = model.id, 
                idPersonal = model.idPersonal, 
                aniopago = model.aniopago, 
                mespago = model.mespago, 
                dia = model.dia, 
                concepto = model.concepto, 
                importe = model.importe, 
                usuario = usuarioDto.FromModel(model.usuario), 
            }; 
        }

        public remuneraciones ToModel()
        {
            return new remuneraciones()
            {
                id = id, 
                idPersonal = idPersonal, 
                aniopago = aniopago, 
                mespago = mespago, 
                dia = dia, 
                concepto = concepto, 
                importe = importe, 
                usuario = usuario.ToModel(), 
            }; 
        }
    }
}