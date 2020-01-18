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
    public class asistenciasController : ApiController
    {
        private repositoryEntities db = new repositoryEntities();

        // GET: api/asistencias/5
        [ResponseType(typeof(asistencia))]
        public async Task<IHttpActionResult> Getasistencia(int id)
        {
            asistencia asistencia = await db.asistencia.FindAsync(id);
            if (asistencia == null)
            {
                return NotFound();
            }

            return Ok(asistenciaDto.FromModel(asistencia));
        }

        // GET: api/asistencias/5
        [ResponseType(typeof(asistencia))]
        public async Task<IHttpActionResult> Getlastasistencia(int id, int last)
        {
            asistencia asistencia = await db.asistencia.FirstAsync(x => x.fechaSalida == null && x.idPersonal == id);

            if (asistencia == null)
            {
                return NotFound();
            }

            return Ok(asistenciaDto.FromModel(asistencia));
        }

        public ResponceDto<asistenciaDto> Getasistencias(int draw, int start, int length, string search)
        {
            if (search == null) search = "";
            var total = db.asistencia.Where(x => x.usuario.nombre.Contains(search)).Count();
            var s = db.asistencia.Where(x => x.usuario.nombre.Contains(search)).OrderBy(x => x.id).Skip(start).Take(length).ToList();

            var result = new ResponceDto<asistenciaDto>
            {
                draw = draw,
                recordsFiltered = total,
                recordsTotal = total,
                data = s.Select(a => asistenciaDto.FromModel(a)).ToList()
            };

            return result;
        }



        public ResponceDto<asistenciaDto> GetResumenAsistencia(int draw, int start, int length, string desde, string hasta)
        {


            DateTime fdesde = DateTime.Now;
            DateTime fhasta = DateTime.Now;

            fdesde = DateTime.ParseExact(desde, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            fhasta = DateTime.ParseExact(hasta, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            string strQuery = @"select a.id, a.idPersonal, u.nombre, a.fechaIngreso, a.fechaSalida
                                from asistencia a
                                inner join usuario u
                                on a.idPersonal = u.id
                                where a.fechaIngreso between @desde and @hasta
                                order by a.idPersonal, a.fechaIngreso
                                ";

            var listaResumen = db.Database.SqlQuery<asistenciaDto>(strQuery, new MySql.Data.MySqlClient.MySqlParameter("@desde", fdesde), new MySql.Data.MySqlClient.MySqlParameter("@hasta", fhasta)).ToList();

            var result = new ResponceDto<asistenciaDto>
            {
                draw = draw,
                recordsFiltered = listaResumen.Count(),
                recordsTotal = listaResumen.Count(),
                data = listaResumen.Skip(start).Take(length).ToList()
            };

            return result;

        }

        // PUT: api/asistencias/5
        [ResponseType(typeof(asistenciaDto))]
        public async Task<IHttpActionResult> Putasistencia(int id, asistencia asistencia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != asistencia.id)
            {
                return BadRequest();
            }

            asistencia.fechaSalida = DateTime.Now;

            db.Entry(asistencia).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!asistenciaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(asistenciaDto.FromModel(asistencia));
        }

        // POST: api/asistencias
        [ResponseType(typeof(asistencia))]
        public async Task<IHttpActionResult> Postasistencia(asistencia asistencia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            asistencia.fechaIngreso = DateTime.Now;

            db.asistencia.Add(asistencia);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = asistencia.id }, asistencia);
        }

        // DELETE: api/asistencias/5
        [ResponseType(typeof(asistencia))]
        public async Task<IHttpActionResult> Deleteasistencia(int id)
        {
            asistencia asistencia = await db.asistencia.FindAsync(id);
            if (asistencia == null)
            {
                return NotFound();
            }

            db.asistencia.Remove(asistencia);
            await db.SaveChangesAsync();

            return Ok(asistencia);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool asistenciaExists(int id)
        {
            return db.asistencia.Count(e => e.id == id) > 0;
        }
    }
}