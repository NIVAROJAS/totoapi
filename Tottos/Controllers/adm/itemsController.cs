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
    public class itemsController : ApiController
    {
        private repositoryEntities db = new repositoryEntities();

        // GET: api/items
        public IQueryable<item> Getitem()
        {
            return db.item;
        }

        // GET: api/items/5
        [ResponseType(typeof(item))]
        public async Task<IHttpActionResult> Getitem(int id)
        {
            item item = await db.item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(itemDto.FromModel(item));
        }

        public ResponceDto<itemDto> Getitem(int draw, int start, int length, string search)
        {
            if (search == null) search = "";
            var total = db.item.Where(x => x.descripcion.Contains(search)).Count();
            var s = db.item.Where(x => x.descripcion.Contains(search)).OrderBy(x => x.id).Skip(start).Take(length).ToList();

            var result = new ResponceDto<itemDto>
            {
                draw = draw,
                recordsFiltered = total,
                recordsTotal = total,
                data = s.Select(a => itemDto.FromModel(a)).ToList()
            };

            return result;
        }


        // PUT: api/items/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putitem(int id, item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != item.id)
            {
                return BadRequest();
            }

            db.Entry(item).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!itemExists(id))
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

        // POST: api/items
        [ResponseType(typeof(item))]
        public async Task<IHttpActionResult> Postitem(item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.item.Add(item);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = item.id }, item);
        }

        // DELETE: api/items/5
        [ResponseType(typeof(item))]
        public async Task<IHttpActionResult> Deleteitem(int id)
        {
            item item = await db.item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            db.item.Remove(item);
            await db.SaveChangesAsync();

            return Ok(item);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool itemExists(int id)
        {
            return db.item.Count(e => e.id == id) > 0;
        }
    }
}