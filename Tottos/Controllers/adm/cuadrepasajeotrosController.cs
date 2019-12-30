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
    public class cuadrepasajeotrosController : ApiController
    {
        private repositoryEntities db = new repositoryEntities();

        // GET: api/cuadrepasajeotros
        public IQueryable<cuadrepasajeotros> Getcuadrepasajeotros()
        {
            return db.cuadrepasajeotros;
        }

        // GET: api/cuadrepasajeotros/5
        [ResponseType(typeof(cuadrepasajeotros))]
        public async Task<IHttpActionResult> Getcuadrepasajeotros(int id)
        {
            cuadrepasajeotros cuadrepasajeotros = await db.cuadrepasajeotros.FindAsync(id);
            if (cuadrepasajeotros == null)
            {
                return NotFound();
            }

            return Ok(cuadrepasajeotrosDto.FromModel(cuadrepasajeotros));
        }

        public ResponceDto<cuadrepasajeotrosDto> Getcuadrepasajeotros(int draw, int start, int length, int search)
        {

            var total = db.cuadrepasajeotros.Where(x => x.idCuadre == search).Count();
            var s = db.cuadrepasajeotros.Where(x => x.idCuadre == search).OrderBy(x => x.id).ToList();

            var result = new ResponceDto<cuadrepasajeotrosDto>
            {
                draw = draw,
                recordsFiltered = total,
                recordsTotal = total,
                data = s.Select(a => cuadrepasajeotrosDto.FromModel(a)).ToList()
            };

            return result;
        }

        // PUT: api/cuadrepasajeotros/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putcuadrepasajeotros(int id, cuadrepasajeotros cuadrepasajeotros)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cuadrepasajeotros.id)
            {
                return BadRequest();
            }

            db.Entry(cuadrepasajeotros).State = System.Data.Entity.EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!cuadrepasajeotrosExists(id))
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

        // POST: api/cuadrepasajeotros
        [ResponseType(typeof(cuadrepasajeotros))]
        public async Task<IHttpActionResult> Postcuadrepasajeotros(cuadrepasajeotros cuadrepasajeotros)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.cuadrepasajeotros.Add(cuadrepasajeotros);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = cuadrepasajeotros.id }, cuadrepasajeotros);
        }

        // DELETE: api/cuadrepasajeotros/5
        [ResponseType(typeof(cuadrepasajeotros))]
        public async Task<IHttpActionResult> Deletecuadrepasajeotros(int id)
        {
            cuadrepasajeotros cuadrepasajeotros = await db.cuadrepasajeotros.FindAsync(id);
            if (cuadrepasajeotros == null)
            {
                return NotFound();
            }

            db.cuadrepasajeotros.Remove(cuadrepasajeotros);
            await db.SaveChangesAsync();

            return Ok(cuadrepasajeotros);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool cuadrepasajeotrosExists(int id)
        {
            return db.cuadrepasajeotros.Count(e => e.id == id) > 0;
        }
    }
}