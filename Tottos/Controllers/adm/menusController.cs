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
    public class menusController : ApiController
    {
        private repositoryEntities db = new repositoryEntities();

        // GET: api/menus
        public IQueryable<menu> Getmenus()
        {
            return db.menu;
        }

        // GET: api/menus/5
        [ResponseType(typeof(menu))]
        public async Task<IHttpActionResult> Getmenu(int id)
        {
            menu menu = await db.menu.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }

            return Ok(menu);
        }


        [HttpGet]
        public IHttpActionResult GetUserMenu(int id)
        {
            try
            {
                var a = (
                            from f in db.formulario
                            where f.formulaiorolpermiso.Any(x => x.rol.usuariorol.Any(p => p.usuario.id == id))
                            select new {
                                Orden = f.menu.orden,
                                NombreMenu = f.menu.descripcion,
                                Icon = f.menu.icon,
                                Formularios = f
                            }
                        ).OrderBy(x => x.NombreMenu).ToList();

                List<menuDto> lista = new List<menuDto>();
                int indice = -1;
                string menuanterior = "";

                foreach (var item in a)
                {
                    if (item.NombreMenu != menuanterior)
                    {
                        indice++;
                        lista.Add(new menuDto());
                        lista[indice].descripcion = item.NombreMenu;
                        lista[indice].orden = item.Orden;
                        lista[indice].icon = item.Icon;
                        lista[indice].formulario = new List<formulario>();
                    } 
                    
                    lista[indice].formulario.Add(item.Formularios);
                    
                    menuanterior = item.NombreMenu;
                }


                var b = from x in lista
                        orderby x.orden
                        select new { Orden = x.orden, NombreMenu = x.descripcion, Icon = x.icon, Formularios = x.formulario.OrderBy(d => d.orden).Select(w => formularioDto.FromModel(w)).Distinct().ToList() };
                
                return Ok(b);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // PUT: api/menus/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putmenu(int id, menu menu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != menu.id)
            {
                return BadRequest();
            }

            db.Entry(menu).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!menuExists(id))
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

        // POST: api/menus
        [ResponseType(typeof(menu))]
        public async Task<IHttpActionResult> Postmenu(menu menu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.menu.Add(menu);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = menu.id }, menu);
        }

        // DELETE: api/menus/5
        [ResponseType(typeof(menu))]
        public async Task<IHttpActionResult> Deletemenu(int id)
        {
            menu menu = await db.menu.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }

            db.menu.Remove(menu);
            await db.SaveChangesAsync();

            return Ok(menu);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool menuExists(int id)
        {
            return db.menu.Count(e => e.id == id) > 0;
        }
    }
}