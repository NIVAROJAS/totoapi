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
using EntityState = System.Data.Entity.EntityState;

namespace Tottos.Controllers.adm
{
    public class cuadreticketsController : ApiController
    {
        private repositoryEntities db = new repositoryEntities();

        // GET: api/cuadretickets
        public IQueryable<cuadreticket> Getcuadreticket()
        {
            return db.cuadreticket;
        }

        // GET: api/cuadretickets/5
        [ResponseType(typeof(cuadreticket))]
        public async Task<IHttpActionResult> Getcuadreticket(int id)
        {
            cuadreticket cuadreticket = await db.cuadreticket.FindAsync(id);
            if (cuadreticket == null)
            {
                return NotFound();
            }

            return Ok(cuadreticketDto.FromModel(cuadreticket));
        }


        public ResponceDto<cuadreticketDto> Getcuadretickets(int draw, int start, int length, int search)
        {

            var total = db.cuadreticket.Where(x => x.idCuadre == search).Count();
            var s = db.cuadreticket.Where(x => x.idCuadre == search).OrderBy(x => x.id).ToList();

            var result = new ResponceDto<cuadreticketDto>
            {
                draw = draw,
                recordsFiltered = total,
                recordsTotal = total,
                data = s.Select(a => cuadreticketDto.FromModel(a)).ToList()
            };

            return result;
        }



        // PUT: api/cuadretickets/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putcuadreticket(int id, cuadreticket cuadreticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cuadreticket.id)
            {
                return BadRequest();
            }

            db.Entry(cuadreticket).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!cuadreticketExists(id))
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

        // POST: api/cuadretickets
        [ResponseType(typeof(cuadreticket))]
        public async Task<IHttpActionResult> Postcuadreticket(cuadreticket cuadreticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.cuadreticket.Add(cuadreticket);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = cuadreticket.id }, cuadreticket);
        }

        // DELETE: api/cuadretickets/5
        [ResponseType(typeof(cuadreticket))]
        public async Task<IHttpActionResult> Deletecuadreticket(int id)
        {
            cuadreticket cuadreticket = await db.cuadreticket.FindAsync(id);
            if (cuadreticket == null)
            {
                return NotFound();
            }

            db.cuadreticket.Remove(cuadreticket);
            await db.SaveChangesAsync();

            return Ok(cuadreticket);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool cuadreticketExists(int id)
        {
            return db.cuadreticket.Count(e => e.id == id) > 0;
        }
    }
}