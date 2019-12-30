using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Tottos.Models;
using EntityState = System.Data.Entity.EntityState;

namespace Tottos.Controllers.adm
{
    public class asistenciasController : ApiController
    {
        private repositoryEntities db = new repositoryEntities();

        // GET: api/asistencias
        public IQueryable<asistencia> Getasistencia()
        {
            return db.asistencia;
        }

        // GET: api/asistencias/5
        [ResponseType(typeof(asistencia))]
        public async Task<IHttpActionResult> Getasistencia(int id)
        {
            asistencia asistencia = await db.asistencia.FindAsync(id);
            if (asistencia == null)
            {
                return NotFound();
            }

            return Ok(asistencia);
        }

        // PUT: api/asistencias/5
        [ResponseType(typeof(void))]
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

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/asistencias
        [ResponseType(typeof(asistencia))]
        public async Task<IHttpActionResult> Postasistencia(asistencia asistencia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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