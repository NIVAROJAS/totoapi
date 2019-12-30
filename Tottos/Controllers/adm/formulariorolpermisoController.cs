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
using EntityState = System.Data.Entity.EntityState;

namespace Tottos.Controllers
{
    public class formulariorolpermisoController : ApiController
    {
        private repositoryEntities db = new repositoryEntities();

        // GET: api/formulariorolpermiso
        public IQueryable<formulaiorolpermiso> Getformulaiorolpermiso()
        {
            return db.formulaiorolpermiso;
        }

        // GET: api/formulariorolpermiso/5
        [ResponseType(typeof(formulaiorolpermiso))]
        public async Task<IHttpActionResult> Getformulaiorolpermiso(int id)
        {
            formulaiorolpermiso formulaiorolpermiso = await db.formulaiorolpermiso.FindAsync(id);
            if (formulaiorolpermiso == null)
            {
                return NotFound();
            }

            return Ok(formulaiorolpermiso);
        }

        public ResponceDto<permisosrolDto> Getformulaiorolpermiso(int draw, int start, int length, string search, int id)
        {
            if (search == null) search = "";

            search = search.ToUpper();

            var permisos = (from a in db.permiso
                            select a).ToList();

            string columnas = "";

            foreach (var item in permisos)
            {
                columnas += ", SUM(CASE WHEN (IdPermiso=" + item.id +") THEN 1 ELSE 0 END) AS " + item.descripcion;
            }

            string strQuery = @"SELECT
                                formulario.nombre, 
                                formulario.nombremenu nombreenmenu, 
                                formulario.id IdFormulario " + columnas + @"
                                FROM formulario
                                LEFT OUTER JOIN formulaiorolpermiso ON formulaiorolpermiso.idformulario = formulario.Id
										                                 AND formulaiorolpermiso.idrol = @rol
                                LEFT OUTER JOIN rol ON formulaiorolpermiso.IdRol = rol.Id
                                LEFT OUTER JOIN permiso ON formulaiorolpermiso.IdPermiso = permiso.Id
                                GROUP BY 
                                formulario.Nombre, 
                                formulario.nombremenu, 
                                formulario.id
                                ";

            var listaPermisos = db.Database.SqlQuery<permisosrolDto>(strQuery, new MySql.Data.MySqlClient.MySqlParameter("@rol", id)).ToList();


            var total = listaPermisos.Count(x => x.nombreenmenu.ToUpper().Contains(search));
            var s = listaPermisos.Where(x => x.nombreenmenu.ToUpper().Contains(search)).OrderBy(x => x.nombreenmenu).Skip(start).Take(length).ToList();

            var result = new ResponceDto<permisosrolDto>
            {
                draw = draw,
                recordsFiltered = total,
                recordsTotal = total,
                data = s
            };

            return result;
        }

        public class dto
        {
            public int user { get; set; }
            public FormulariosPermisosDto FormulariosPermisos { get; set; }
        }

        public class FormulariosPermisosDto
        {
            public int IdRol { get; set; }
            public int IdFormulario { get; set; }

            public string NombreEnMenu { get; set; }

            public Nullable<bool> Leer { get; set; }

            public Nullable<bool> Adicionar { get; set; }

            public Nullable<bool> Editar { get; set; }

            public Nullable<bool> Eliminar { get; set; }
        }

        // POST: api/FormularioRolPermiso
        [HttpPost]
        [ResponseType(typeof(permisosrolDto))]
        public async Task<IHttpActionResult> PostFormularioRolPermiso(dto form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //1   Leer
            //2   Adicionar
            //3   Editar
            //4   Eliminar
            int idRol = form.FormulariosPermisos.IdRol;
            int IdFormulario = form.FormulariosPermisos.IdFormulario;

            var lstEnt = (from a in db.formulaiorolpermiso.Where(x => x.idrol == idRol && x.idformulario == IdFormulario)
                          select a)
                         .ToList();

            foreach (var item in lstEnt)
            {
                db.formulaiorolpermiso.Remove(item);
            }

            if (form.FormulariosPermisos.Leer.Value)
            {
                db.formulaiorolpermiso.Add(new formulaiorolpermiso()
                {
                    idformulario = form.FormulariosPermisos.IdFormulario,
                    idpermiso = 1,
                    idrol = form.FormulariosPermisos.IdRol
                });
            }

            if (form.FormulariosPermisos.Adicionar.Value)
            {
                db.formulaiorolpermiso.Add(new formulaiorolpermiso()
                {
                    idformulario = form.FormulariosPermisos.IdFormulario,
                    idpermiso = 2,
                    idrol = form.FormulariosPermisos.IdRol
                });
            }

            if (form.FormulariosPermisos.Editar.Value)
            {
                db.formulaiorolpermiso.Add(new formulaiorolpermiso()
                {
                    idformulario = form.FormulariosPermisos.IdFormulario,
                    idpermiso = 3,
                    idrol = form.FormulariosPermisos.IdRol
                });
            }

            if (form.FormulariosPermisos.Eliminar.Value)
            {
                db.formulaiorolpermiso.Add(new formulaiorolpermiso()
                {
                    idformulario = form.FormulariosPermisos.IdFormulario,
                    idpermiso = 4,
                    idrol = form.FormulariosPermisos.IdRol
                });
            }


            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ModelState);
            }

            return Ok(new permisosrolDto());
        }

        // PUT: api/formulariorolpermiso/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putformulaiorolpermiso(int id, formulaiorolpermiso formulaiorolpermiso)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != formulaiorolpermiso.idformulario)
            {
                return BadRequest();
            }

            db.Entry(formulaiorolpermiso).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!formulaiorolpermisoExists(id))
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

        // DELETE: api/formulariorolpermiso/5
        [ResponseType(typeof(formulaiorolpermiso))]
        public async Task<IHttpActionResult> Deleteformulaiorolpermiso(int id)
        {
            formulaiorolpermiso formulaiorolpermiso = await db.formulaiorolpermiso.FindAsync(id);
            if (formulaiorolpermiso == null)
            {
                return NotFound();
            }

            db.formulaiorolpermiso.Remove(formulaiorolpermiso);
            await db.SaveChangesAsync();

            return Ok(formulaiorolpermiso);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool formulaiorolpermisoExists(int id)
        {
            return db.formulaiorolpermiso.Count(e => e.idformulario == id) > 0;
        }
    }
}