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

namespace Tottos.Controllers
{
    public class rolesController : ApiController
    {
        private repositoryEntities db = new repositoryEntities();

        // GET: api/roles
        public IQueryable<rol> Getroles()
        {
            return db.rol;
        }

        // GET: api/roles/5
        [ResponseType(typeof(rol))]
        public async Task<IHttpActionResult> Getrol(int id)
        {
            rol rol = await db.rol.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }

            return Ok(rolDto.FromModel(rol));
        }

        public ResponceDto<rolDto> Getrol(int draw, int start, int length, string search)
        {
            if (search == null) search = "";
            var total = db.rol.Where(x => x.descripcion.Contains(search)).Count();
            var s = db.rol.Where(x => x.descripcion.Contains(search)).OrderBy(x => x.id).Skip(start).Take(length).ToList();

            var result = new ResponceDto<rolDto>
            {
                draw = draw,
                recordsFiltered = total,
                recordsTotal = total,
                data = s.Select(a => rolDto.FromModel(a)).ToList()
            };

            return result;
        }

        // PUT: api/roles/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putrol(int id, rol rol)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rol.id)
            {
                return BadRequest();
            }

            db.Entry(rol).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!rolExists(id))
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

        // POST: api/roles
        [ResponseType(typeof(rol))]
        public async Task<IHttpActionResult> Postrol(rol rol)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.rol.Add(rol);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = rol.id }, rol);
        }

        // DELETE: api/roles/5
        [ResponseType(typeof(rol))]
        public async Task<IHttpActionResult> Deleterol(int id)
        {
            rol rol = await db.rol.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }

            db.rol.Remove(rol);
            await db.SaveChangesAsync();

            return Ok(rol);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool rolExists(int id)
        {
            return db.rol.Count(e => e.id == id) > 0;
        }
    }
}