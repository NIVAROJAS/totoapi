using System;
using System.Linq;

namespace Tottos.Models.Dto
{
    public class cuadreinventarioDto
    {
        public int id { get; set; }

        public int idCuadre { get; set; }

        public int idItem { get; set; }

        public decimal ingreso { get; set; }

        public decimal venta { get; set; }

        public decimal otros { get; set; }

        public string observacionesOtros { get; set; }

        public string descripcion { get; set; }

        public decimal inicial { get; set; }

        public decimal saldo { get; set; }

        public static cuadreinventarioDto FromModel(cuadreinventario model)
        {
            return new cuadreinventarioDto()
            {
                id = model.id, 
                idCuadre = model.idCuadre, 
                idItem = model.idItem, 
                ingreso = model.ingreso, 
                venta = model.venta, 
                otros = model.otros, 
                observacionesOtros = model.observacionesOtros, 
                inicial = model.inicial, 
                saldo = model.saldo,
                descripcion = model.item.descripcion
            }; 
        }

        public cuadreinventario ToModel()
        {
            return new cuadreinventario()
            {
                id = id, 
                idCuadre = idCuadre, 
                idItem = idItem, 
                ingreso = ingreso, 
                venta = venta, 
                otros = otros, 
                observacionesOtros = observacionesOtros, 
                inicial = inicial, 
                saldo = saldo, 
            }; 
        }
    }
}