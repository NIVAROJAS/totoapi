using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Tottos.Models;
using Tottos.Models.Dto;
using EntityState = System.Data.Entity.EntityState;

namespace Tottos.Controllers.adm
{
    public class cuadrecajasController : ApiController
    {
        private repositoryEntities db = new repositoryEntities();

        // GET: api/cuadrecajas
        public IQueryable<cuadrecaja> Getcuadrecaja()
        {
            return db.cuadrecaja;
        }

        // GET: api/cuadrecajas/5
        [ResponseType(typeof(cuadrecaja))]
        public async Task<IHttpActionResult> Getcuadrecaja(int id)
        {
            cuadrecaja cuadrecaja = await db.cuadrecaja.FindAsync(id);
            if (cuadrecaja == null)
            {
                return NotFound();
            }

            return Ok(cuadrecajaDto.FromModel(cuadrecaja));
        }

        public ResponceDto<cuadrecajaDto> Getcuadrecaja(int draw, int start, int length, int search)
        {

            var total = db.cuadrecaja.Where(x => x.idCuadre == search).Count();
            var s = db.cuadrecaja.Where(x => x.idCuadre == search).OrderBy(x => x.id).ToList();

            var result = new ResponceDto<cuadrecajaDto>
            {
                draw = draw,
                recordsFiltered = total,
                recordsTotal = total,
                data = s.Select(a => cuadrecajaDto.FromModel(a)).ToList()
            };

            return result;
        }


        public ResponceDto<ResumenCuadrecajaDto> GetResumenCuadrecaja(int draw, int start, int length, string desde, string hasta)
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

            var result = new ResponceDto<ResumenCuadrecajaDto>
            {
                draw = draw,
                recordsFiltered = listaResumen.Count(),
                recordsTotal = listaResumen.Count(),
                data = listaResumen
            };

            return result;

        }

        // PUT: api/cuadrecajas/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putcuadrecaja(int id, cuadrecaja cuadrecaja)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cuadrecaja.id)
            {
                return BadRequest();
            }

            db.Entry(cuadrecaja).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!cuadrecajaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/cuadrecajas
        [ResponseType(typeof(cuadrecaja))]
        public async Task<IHttpActionResult> Postcuadrecaja(cuadrecaja cuadrecaja)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.cuadrecaja.Add(cuadrecaja);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = cuadrecaja.id }, cuadrecaja);
        }

        // DELETE: api/cuadrecajas/5
        [ResponseType(typeof(cuadrecaja))]
        public async Task<IHttpActionResult> Deletecuadrecaja(int id)
        {
            cuadrecaja cuadrecaja = await db.cuadrecaja.FindAsync(id);
            if (cuadrecaja == null)
            {
                return NotFound();
            }

            db.cuadrecaja.Remove(cuadrecaja);
            await db.SaveChangesAsync();

            return Ok(cuadrecaja);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool cuadrecajaExists(int id)
        {
            return db.cuadrecaja.Count(e => e.id == id) > 0;
        }
    }
}