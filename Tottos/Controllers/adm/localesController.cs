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
    public class localesController : ApiController
    {
        private repositoryEntities db = new repositoryEntities();

        // GET: api/locales
        public List<localDto> Getlocal()
        {
            var x = (from a in db.local
                     select a).ToList();


            return (from a in x select localDto.FromModel(a)).ToList();
        }

        // GET: api/locales/5
        [ResponseType(typeof(local))]
        public async Task<IHttpActionResult> Getlocal(int id)
        {
            local local = await db.local.FindAsync(id);
            if (local == null)
            {
                return NotFound();
            }

            return Ok(localDto.FromModel(local));
        }


        public ResponceDto<localDto> Getlocal(int draw, int start, int length, string search)
        {
            if (search == null) search = "";
            var total = db.local.Where(x => x.nombre.Contains(search)).Count();
            var s = db.local.Where(x => x.nombre.Contains(search)).OrderBy(x => x.id).Skip(start).Take(length).ToList();

            var result = new ResponceDto<localDto>
            {
                draw = draw,
                recordsFiltered = total,
                recordsTotal = total,
                data = s.Select(a => localDto.FromModel(a)).ToList()
            };

            return result;
        }


        // PUT: api/locales/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putlocal(int id, local local)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != local.id)
            {
                return BadRequest();
            }

            db.Entry(local).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!localExists(id))
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

        // POST: api/locales
        [ResponseType(typeof(local))]
        public async Task<IHttpActionResult> Postlocal(local local)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.local.Add(local);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = local.id }, local);
        }

        // DELETE: api/locales/5
        [ResponseType(typeof(local))]
        public async Task<IHttpActionResult> Deletelocal(int id)
        {
            local local = await db.local.FindAsync(id);
            if (local == null)
            {
                return NotFound();
            }

            db.local.Remove(local);
            await db.SaveChangesAsync();

            return Ok(local);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool localExists(int id)
        {
            return db.local.Count(e => e.id == id) > 0;
        }
    }
}