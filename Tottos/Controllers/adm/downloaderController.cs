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
        public HttpResponseMessage Get(int rpt, string desde, string hasta)
        {
            if (rpt == 1) return ReporteCierre(desde, hasta);
            if (rpt == 2) return ReporteAsistencia(desde, hasta);
            if (rpt == 3) return ReporteRemuneraciones(desde, hasta);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        private HttpResponseMessage ReporteCierre(string desde, string hasta)
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

        private HttpResponseMessage ReporteAsistencia(string desde, string hasta)
        {
            DateTime fdesde = DateTime.Now;
            DateTime fhasta = DateTime.Now;

            fdesde = DateTime.ParseExact(desde, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            fhasta = DateTime.ParseExact(hasta, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            string strQuery = @"select a.idPersonal, u.nombre, a.fechaIngreso, a.fechaSalida
                                from asistencia a
                                inner join usuario u
                                on a.idPersonal = u.id
                                where a.fechaIngreso between @desde and @hasta
                                order by a.idPersonal, a.fechaIngreso
                                ";

            var listaResumen = db.Database.SqlQuery<asistenciaDto>(strQuery, new MySql.Data.MySqlClient.MySqlParameter("@desde", fdesde), new MySql.Data.MySqlClient.MySqlParameter("@hasta", fhasta)).ToList();


            HttpResponseMessage response = new HttpResponseMessage();

            using (ExcelPackage ep = new ExcelPackage())
            {
                ExcelWorksheet hoja = ep.Workbook.Worksheets.Add("Caja");

                hoja.Cells[1, 1].Value = "Id";
                hoja.Cells[1, 2].Value = "Nombre";
                hoja.Cells[1, 3].Value = "Fecha de Ingreso";
                hoja.Cells[1, 4].Value = "Fecha de Salida";

                hoja.Select("A1:D1");
                hoja.SelectedRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.SelectedRange.Style.Fill.BackgroundColor.SetColor(Color.Brown);
                hoja.SelectedRange.Style.Font.Color.SetColor(Color.White);

                if (listaResumen.Count() > 0)
                {
                    string filaFinal = (listaResumen.Count() + 1).ToString();
                    string filaFinalTotal = (listaResumen.Count() + 2).ToString();

                    hoja.Select("C2:C" + filaFinal);
                    hoja.SelectedRange.Style.Numberformat.Format = "yyyy-mm-dd hh:mm";

                    hoja.Select("D2:D" + filaFinal);
                    hoja.SelectedRange.Style.Numberformat.Format = "yyyy-mm-dd hh:mm";
                }

                hoja.Cells[2, 1].LoadFromCollection(from a in listaResumen 
                                                    select new { a.idPersonal, a.nombre, a.fechaIngreso, a.fechaSalida });

                response.Content = new ByteArrayContent(ep.GetAsByteArray());
            }

            response.StatusCode = HttpStatusCode.OK;
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            return response;
        }
        private HttpResponseMessage ReporteRemuneraciones(string desde, string hasta)
        {
            DateTime fdesde = DateTime.Now;
            DateTime fhasta = DateTime.Now;

            fdesde = DateTime.ParseExact(desde, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            fhasta = DateTime.ParseExact(hasta, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            string strQuery = @"SELECT idPersonal, aniopago, mespago, dia, concepto, importe, nombre 
                                FROM remuneraciones r
                                inner join usuario u
                                on r.idPersonal = u.id
                                where dia between  @desde and @hasta
                                order by dia";

            var listaResumen = db.Database.SqlQuery<remuneracionesDto>(strQuery, new MySql.Data.MySqlClient.MySqlParameter("@desde", fdesde), new MySql.Data.MySqlClient.MySqlParameter("@hasta", fhasta)).ToList();


            HttpResponseMessage response = new HttpResponseMessage();

            using (ExcelPackage ep = new ExcelPackage())
            {
                ExcelWorksheet hoja = ep.Workbook.Worksheets.Add("Caja");

                hoja.Cells[1, 1].Value = "Id";
                hoja.Cells[1, 2].Value = "Nombre";
                hoja.Cells[1, 3].Value = "Año";
                hoja.Cells[1, 4].Value = "Mes";
                hoja.Cells[1, 5].Value = "Dia";
                hoja.Cells[1, 6].Value = "Concepto";
                hoja.Cells[1, 7].Value = "Importe";

                hoja.Select("A1:G1");
                hoja.SelectedRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.SelectedRange.Style.Fill.BackgroundColor.SetColor(Color.Brown);
                hoja.SelectedRange.Style.Font.Color.SetColor(Color.White);

                if (listaResumen.Count() > 0)
                {
                    string filaFinal = (listaResumen.Count() + 1).ToString();
                    string filaFinalTotal = (listaResumen.Count() + 2).ToString();

                    hoja.Select("E2:E" + filaFinal);
                    hoja.SelectedRange.Style.Numberformat.Format = "yyyy-mm-dd hh:mm";
                }

                hoja.Cells[2, 1].LoadFromCollection(from a in listaResumen
                                                    select new { a.idPersonal, a.nombre, a.aniopago, a.mespago, a.dia, a.concepto, a.importe });

                response.Content = new ByteArrayContent(ep.GetAsByteArray());
            }

            response.StatusCode = HttpStatusCode.OK;
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            return response;
        }


    }
}
