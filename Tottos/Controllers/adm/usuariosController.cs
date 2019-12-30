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
    public class usuariosController : ApiController
    {
        private repositoryEntities db = new repositoryEntities();

        // GET: api/usuarios
        public IQueryable<usuario> Getusuarios()
        {
            return db.usuario;
        }

        // GET: api/usuarios/5
        [ResponseType(typeof(usuario))]
        public async Task<IHttpActionResult> Getusuarios(int id)
        {
            usuario usuario = await db.usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuarioDto.FromModel(usuario));
        }

        // GET: api/Usuario
        public ResponceDto<usuarioDto> Getusuarios(int draw, int start, int length, string search)
        {
            if (search == null) search = "";

            var total = db.usuario.Where(n => n.nombre.Contains(search) || search == "").Count();
            var s = db.usuario.Where(n => n.nombre.Contains(search) || search == "").OrderBy(x => x.id).Skip(start).Take(length).ToList();

            var result = new ResponceDto<usuarioDto>();
            result.draw = draw;
            result.recordsFiltered = total;
            result.recordsTotal = total;
            result.data = s.Select(a => usuarioDto.FromModel(a)).ToList();

            return result;
        }


        // PUT: api/usuarios/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putusuario(int id, usuarioDto pusuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pusuario.id)
            {
                return BadRequest();
            }
                
            db.usuariorol.RemoveRange(db.usuariorol.Where(d => d.idusuario == id));
            

            usuario usuario = pusuario.ToModel();

            db.Entry(usuario).State = EntityState.Modified;

            for (int i = 0; i < pusuario.Roles.Count(); i++)
            {
                int rolId = pusuario.Roles[i];
                usuariorol rolUsuario = new usuariorol();
                rolUsuario.idrol = pusuario.Roles[i];
                rolUsuario.idusuario = id;

                db.usuariorol.Add(rolUsuario);
            }


            try
            {
                int x =  await db.SaveChangesAsync();
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

        // POST: api/usuarios
        [ResponseType(typeof(usuario))]
        public async Task<IHttpActionResult> Postusuario(usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.usuario.Add(usuario);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = usuario.id }, usuario);
        }

        // DELETE: api/usuarios/5
        [ResponseType(typeof(usuario))]
        public async Task<IHttpActionResult> Deleteusuario(int id)
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


        [HttpGet]
        [Route("api/usuarios/UsuarioAdministrador/{id}")]
        [ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> UsuarioAdministrador(int id)
        {
            var contador = await (from a in db.rol
                            where a.usuariorol.Count(u => u.idusuario == id) > 0 && a.administrador == 1
                            select a).CountAsync();

            return Ok(contador > 0);
        }


        [HttpPost]
        [Route("api/usuarios/validate")]
        [ResponseType(typeof(usuarioDto))]
        public IHttpActionResult validate(auth usuario)
        {
            usuario aDM_Usuario = db.usuario.Where(p => p.login == usuario.user && p.password == usuario.pass).FirstOrDefault();

            if (aDM_Usuario == null)
            {
                return NotFound();
            }

            aDM_Usuario.password = "********";

            return Ok(usuarioDto.FromModel(aDM_Usuario));
        }

        [HttpPost]
        [Route("api/usuarios/formpermissions")]
        [ResponseType(typeof(permisosrolDto))]
        public IHttpActionResult formpermissions(formQuery query)
        {

            var permisos = (from a in db.permiso
                            select a).ToList();

            string columnas = "";

            foreach (var item in permisos)
            {
                columnas += ", SUM(CASE WHEN (IdPermiso=" + item.id + ") THEN 1 ELSE 0 END) AS " + item.descripcion;
            }

            string strQuery = @"SELECT
                                formulario.nombre, 
                                formulario.nombremenu nombreenmenu, 
                                formulario.id IdFormulario " + columnas + @"
                                FROM usuario
                                     INNER JOIN usuarioRol ON usuario.id = usuarioRol.IdUsuario
                                     INNER JOIN formulaioRolPermiso ON FormulaioRolPermiso.IdRol = usuarioRol.IdRol
                                     INNER JOIN formulario ON formulaioRolPermiso.IdFormulario = formulario.Id
                                     INNER JOIN permiso ON formulaioRolPermiso.IdPermiso = permiso.Id
                                WHERE usuario.id = @IdUsuario AND
	                                    formulario.Id = @IdFormulario
                                GROUP BY 
                                formulario.Nombre, 
                                formulario.nombremenu, 
                                formulario.id
                                ";

            var listaPermisos = db.Database.SqlQuery<permisosrolDto>(strQuery, new MySql.Data.MySqlClient.MySqlParameter("@IdUsuario", query.userId), new MySql.Data.MySqlClient.MySqlParameter("@IdFormulario", query.formId)).ToList();

            if (listaPermisos.Count == 0)
            {
                return NotFound();
            }

            return Ok(listaPermisos[0]);
        }

        public class auth
        {
            public string user { get; set; }
            public string pass { get; set; }
        }


        public class formQuery
        {
            public int userId { get; set; }
            public int formId { get; set; }
        }
    }
}