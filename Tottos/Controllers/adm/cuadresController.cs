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
using Tottos.Models.Dto;

namespace Tottos.Controllers.adm
{
    public class cuadresController : ApiController
    {
        private repositoryEntities db = new repositoryEntities();

        // GET: api/cuadres
        public IQueryable<cuadre> Getcuadre()
        {
            return db.cuadre;
        }

        // GET: api/cuadres/5
        [ResponseType(typeof(cuadre))]
        public async Task<IHttpActionResult> Getcuadre(int id)
        {
            cuadre cuadre = await db.cuadre.FindAsync(id);
            if (cuadre == null)
            {
                return NotFound();
            }

            return Ok(cuadreDto.FromModel(cuadre));
        }

        public ResponceDto<cuadreDto> Getcuadre(int draw, int start, int length, string search)
        {
            var total = db.cuadre.Count();

            if (length == -1)
            {
                length = total;
            }

            var s = db.cuadre.OrderBy(x => x.id).Skip(start).Take(length).OrderByDescending(o => o.fecha).ToList();

            var result = new ResponceDto<cuadreDto>();
            result.draw = draw;
            result.recordsFiltered = total;
            result.recordsTotal = total;
            result.data = s.Select(a => cuadreDto.FromModel(a)).ToList();

            return result;
        }

        // PUT: api/cuadres/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putcuadre(int id, cuadre cuadre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cuadre.id)
            {
                return BadRequest();
            }

            db.Entry(cuadre).State = System.Data.Entity.EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!cuadreExists(id))
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

        // POST: api/cuadres
        [ResponseType(typeof(cuadre))]
        public async Task<IHttpActionResult> Postcuadre(cuadre cuadre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (cuadreExists(cuadre.fecha, cuadre.local)) {
                return Content(HttpStatusCode.Conflict, new { mensaje = "Ya existe un registro de cierre para la fecha especificada, verifique." });
            }

            if (cuadreExists(cuadre.local, cuadre.estado))
            {
                return Content(HttpStatusCode.Conflict, new { mensaje = "Existe un registro de cierre aun no consolidado, verifique." });
            }

            db.cuadre.Add(cuadre);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = cuadre.id }, cuadre);
        }

        // DELETE: api/cuadres/5
        [ResponseType(typeof(cuadre))]
        public async Task<IHttpActionResult> Deletecuadre(int id)
        {
            cuadre cuadre = await db.cuadre.FindAsync(id);
            if (cuadre == null)
            {
                return NotFound();
            }

            db.cuadre.Remove(cuadre);
            await db.SaveChangesAsync();

            return Ok(cuadre);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool cuadreExists(int id)
        {
            return db.cuadre.Count(e => e.id == id) > 0;
        }

        private bool cuadreExists(DateTime fecha, int local)
        {
            return db.cuadre.Count(e => e.fecha == fecha && e.local == local) > 0;
        }
        
        private bool cuadreExists(int local, int estado)
        {
            return db.cuadre.Count(e => e.local == local && e.estado == estado) > 0;
        }





    }
}