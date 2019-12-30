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

namespace Tottos.Controllers
{
    public class permisosController : ApiController
    {
        private repositoryEntities db = new repositoryEntities();

        // GET: api/permisos
        public IQueryable<permiso> Getpermiso()
        {
            return db.permiso;
        }

        // GET: api/permisos/5
        [ResponseType(typeof(permiso))]
        public async Task<IHttpActionResult> Getpermiso(int id)
        {
            permiso permiso = await db.permiso.FindAsync(id);
            if (permiso == null)
            {
                return NotFound();
            }

            return Ok(permiso);
        }

        // PUT: api/permisos/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putpermiso(int id, permiso permiso)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != permiso.id)
            {
                return BadRequest();
            }

            db.Entry(permiso).State = System.Data.Entity.EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!permisoExists(id))
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

        // POST: api/permisos
        [ResponseType(typeof(permiso))]
        public async Task<IHttpActionResult> Postpermiso(permiso permiso)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.permiso.Add(permiso);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = permiso.id }, permiso);
        }

        // DELETE: api/permisos/5
        [ResponseType(typeof(permiso))]
        public async Task<IHttpActionResult> Deletepermiso(int id)
        {
            permiso permiso = await db.permiso.FindAsync(id);
            if (permiso == null)
            {
                return NotFound();
            }

            db.permiso.Remove(permiso);
            await db.SaveChangesAsync();

            return Ok(permiso);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool permisoExists(int id)
        {
            return db.permiso.Count(e => e.id == id) > 0;
        }
    }
}