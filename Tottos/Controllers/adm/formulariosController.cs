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

namespace Tottos.Controllers
{
    public class formulariosController : ApiController
    {
        private repositoryEntities db = new repositoryEntities();

        // GET: api/formularios
        public IQueryable<formulario> Getformulario()
        {
            return db.formulario;
        }

        // GET: api/formularios/5
        [ResponseType(typeof(formulario))]
        public async Task<IHttpActionResult> Getformulario(int id)
        {
            formulario formulario = await db.formulario.FindAsync(id);
            if (formulario == null)
            {
                return NotFound();
            }

            return Ok(formulario);
        }

        public ResponceDto<formularioDto> GetADM_Formulario(int draw, int start, int length, string search)
        {
            if (search == null) search = "";
            var total = db.formulario.Where(x => x.nombre.Contains(search)).Count();
            var s = db.formulario.Where(x => x.nombre.Contains(search)).OrderBy(x => x.id).Skip(start).Take(length).ToList();

            var result = new ResponceDto<formularioDto>
            {
                draw = draw,
                recordsFiltered = total,
                recordsTotal = total,
                data = s.Select(a => formularioDto.FromModel(a)).ToList()
            };

            return result;
        }

        // PUT: api/formularios/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putformulario(int id, formulario formulario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != formulario.id)
            {
                return BadRequest();
            }

            db.Entry(formulario).State = System.Data.Entity.EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!formularioExists(id))
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

        // POST: api/formularios
        [ResponseType(typeof(formulario))]
        public async Task<IHttpActionResult> Postformulario(formulario formulario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.formulario.Add(formulario);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = formulario.id }, formulario);
        }

        // DELETE: api/formularios/5
        [ResponseType(typeof(formulario))]
        public async Task<IHttpActionResult> Deleteformulario(int id)
        {
            formulario formulario = await db.formulario.FindAsync(id);
            if (formulario == null)
            {
                return NotFound();
            }

            db.formulario.Remove(formulario);
            await db.SaveChangesAsync();

            return Ok(formulario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool formularioExists(int id)
        {
            return db.formulario.Count(e => e.id == id) > 0;
        }
    }
}