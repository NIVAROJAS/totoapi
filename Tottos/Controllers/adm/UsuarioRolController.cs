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

namespace Tottos.Controllers.adm
{
    public class UsuarioRolController : ApiController
    {
        private repositoryEntities db = new repositoryEntities();

        // GET: api/UsuarioRol
        public IQueryable<usuario> Getusuariorol()
        {
            return db.usuario;
        }

        // GET: api/UsuarioRol/5
        [ResponseType(typeof(usuario))]
        public async Task<IHttpActionResult> Getusuariorol(int id)
        {
            usuario usuario = await db.usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        public ResponceDto<usuariorolDto> GetADM_Rol(int id, int idUsuario)
        {

            var datos = (from u in db.rol
                         select new usuariorolDto()
                         {
                             IdRol = u.id,
                             IdUsuario = u.usuariorol.Where( d => d.idusuario == idUsuario ).Max(p => p.idusuario),
                             DescripcionRol = u.descripcion
                         }).ToList();

            //var datos = (from a in db.ADM_UsuarioRol
            //             join b in db.ADM_Rol
            //             on a.IdRol equals b.Id
            //             where a.IdUsuario.Equals(idUsuario)
            //                    && a.IdUsuario.Equals(idUsuario)
            //             select new ADM_UsuarioRolDto() {
            //                  IdRol = a.IdRol,
            //                  IdUsuario =  a.IdUsuario,
            //                  DescripcionRol = b.Descripcion
            //             }
            //             ).ToList();


            var total = datos.Count();

            var result = new ResponceDto<usuariorolDto>
            {
                draw = 1,
                recordsFiltered = total,
                recordsTotal = total,
                data = datos
            };

            return result;
        }

        // PUT: api/UsuarioRol/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putusuariorol(int id, usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuario.id)
            {
                return BadRequest();
            }

            db.Entry(usuario).State = System.Data.Entity.EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!usuarioExists(id))
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

        // POST: api/UsuarioRol
        [ResponseType(typeof(usuario))]
        public async Task<IHttpActionResult> Postusuariorol(usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.usuario.Add(usuario);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = usuario.id }, usuario);
        }

        // DELETE: api/UsuarioRol/5
        [ResponseType(typeof(usuario))]
        public async Task<IHttpActionResult> Deleteusuariorol(int id)
        {
            usuario usuario = await db.usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            db.usuario.Remove(usuario);
            await db.SaveChangesAsync();

            return Ok(usuario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool usuarioExists(int id)
        {
            return db.usuario.Count(e => e.id == id) > 0;
        }
    }
}