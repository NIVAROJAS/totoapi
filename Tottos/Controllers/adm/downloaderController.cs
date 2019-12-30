using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Tottos.Models;
using Tottos.Models.Dto;

namespace Tottos.Controllers.adm
{
    public class downloaderController : ApiController
    {
        private repositoryEntities db = new repositoryEntities();
        public HttpResponseMessage Get(string desde, string hasta)
        {

            DateTime fdesde = DateTime.Now;
            DateTime fhasta = DateTime.Now;

            fdesde = DateTime.ParseExact(desde, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            fhasta = DateTime.ParseExact(hasta, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            string strQuery = @"select idCuadre, fecha, u.nombre cajero, l.nombre local, sum(cc.efectivo) efectivo, sum(cc.visa) visa, sum(egreso) egreso, sum(caja) caja,
                                sum(cc.efectivo) + sum(cc.visa) - sum(egreso) + sum(caja) cierre
                                from cuadrecaja cc 
                                inner join cuadre c
                                on cc.idCuadre = c.id
                                inner join usuario u
                                on c.cajero = u.id
                                inner join local l
                                on l.id = c.local
                                where c.fecha between @desde and @hasta
                                group by idCuadre, fecha, u.nombre, l.nombre
                                ";

            var listaResumen = db.Database.SqlQuery<ResumenCuadrecajaDto>(strQuery, new MySql.Data.MySqlClient.MySqlParameter("@desde", fdesde), new MySql.Data.MySqlClient.MySqlParameter("@hasta", fhasta)).ToList();


            HttpResponseMessage response = new HttpResponseMessage();

            using (ExcelPackage ep = new ExcelPackage())
            {
                ExcelWorksheet hoja = ep.Workbook.Worksheets.Add("Caja");

                hoja.Cells[1, 1].Value = "Id";
                hoja.Cells[1, 2].Value = "Fecha";
                hoja.Cells[1, 3].Value = "Local";
                hoja.Cells[1, 4].Value = "Cajero";
                hoja.Cells[1, 5].Value = "Efectivo";
                hoja.Cells[1, 6].Value = "Visa";
                hoja.Cells[1, 7].Value = "Egreso";
                hoja.Cells[1, 8].Value = "Caja";
                hoja.Cells[1, 9].Value = "Cierre";



                hoja.Select("A1:I1");
                hoja.SelectedRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.SelectedRange.Style.Fill.BackgroundColor.SetColor(Color.Brown);
                hoja.SelectedRange.Style.Font.Color.SetColor(Color.White);

                if (listaResumen.Count() > 0)
                {
                    string filaFinal = (listaResumen.Count() + 1).ToString();
                    string filaFinalTotal = (listaResumen.Count() + 2).ToString();

                    hoja.Select("B2:B" + filaFinal);
                    hoja.SelectedRange.Style.Numberformat.Format = "yyyy-mm-dd";

                    hoja.Cells["D" + filaFinalTotal].Value = "Total:";
                    hoja.Cells["D" + filaFinalTotal].Style.Font.Bold = true;

                    hoja.Select("A" + filaFinal + ":I" + filaFinal);
                    hoja.SelectedRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;

                    hoja.Cells["E" + filaFinalTotal].Formula = "SUM(E2:E" + filaFinal + ")";
                    hoja.Cells["F" + filaFinalTotal].Formula = "SUM(F2:F" + filaFinal + ")";
                    hoja.Cells["G" + filaFinalTotal].Formula = "SUM(G2:G" + filaFinal + ")";
                    hoja.Cells["H" + filaFinalTotal].Formula = "SUM(H2:H" + filaFinal + ")";
                    hoja.Cells["I" + filaFinalTotal].Formula = "SUM(I2:I" + filaFinal + ")";
                }

                hoja.Cells[2, 1].LoadFromCollection(listaResumen);

                response.Content = new ByteArrayContent(ep.GetAsByteArray());
            }

            response.StatusCode = HttpStatusCode.OK;
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            return response;
        }
    }
}
