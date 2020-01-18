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


namespace Tottos.Controllers.adm
{
    public class cuadreinventarioController : ApiController
    {
        private repositoryEntities db = new repositoryEntities();

        // GET: api/cuadreinventario
        public IQueryable<cuadreinventario> Getcuadreinventario()
        {
            return db.cuadreinventario;
        }

        // GET: api/cuadreinventario/5
        [ResponseType(typeof(cuadreinventario))]
        public async Task<IHttpActionResult> Getcuadreinventario(int id)
        {
            cuadreinventario cuadreinventario = await db.cuadreinventario.FindAsync(id);
            if (cuadreinventario == null)
            {
                return NotFound();
            }

            return Ok(cuadreinventarioDto.FromModel(cuadreinventario));
        }


        public ResponceDto<cuadreinventarioDto> GetcuadreinventarioLista(int draw, int start, int length, int search)
        {

            // Actualizamos la lista de items
            string strQuery = @"insert into cuadreinventario (idCuadre, idItem, inicial, saldo)
                                select @id, id, ifnull(ant.saldo, 0) inicial, ifnull(ant.saldo, 0) saldo
                                from item left outer join 
                                 (select iditem, saldo from cuadreinventario  c
                                 inner join  cuadre b on c.idcuadre = b.id 
								 inner join cuadre d on b.local = d.local and d.id = @id
                                 where b.id = (
											select max(id)  from cuadre a where  a.local = d.local and a.fecha <= d.fecha and a.id <> @id
                                        )
                                ) ant on item.id = ant.iditem
                                where not exists (select * from cuadreinventario where item.id = cuadreinventario.iditem and cuadreinventario.idcuadre = @id)
                                and (select estado from cuadre where id = @id ) = 0
                                and item.activo = 1
                                ";

            var resultado = db.Database.ExecuteSqlCommand(strQuery, new MySql.Data.MySqlClient.MySqlParameter("@id", search));

            var total = db.cuadreinventario.Where(x => x.idCuadre == search).Count();
            var s = db.cuadreinventario.Where(x => x.idCuadre == search).OrderBy(x => x.id).ToList();

            var result = new ResponceDto<cuadreinventarioDto>
            {
                draw = draw,
                recordsFiltered = total,
                recordsTotal = total,
                data = s.Select(a => cuadreinventarioDto.FromModel(a)).ToList()
            };

            return result;
        }

        // PUT: api/cuadreinventario/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putcuadreinventario(int id, cuadreinventario cuadreinventario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cuadreinventario.id)
            {
                return BadRequest();
            }

            cuadreinventario.saldo = cuadreinventario.inicial + cuadreinventario.ingreso + cuadreinventario.otros - cuadreinventario.venta;

            db.Entry(cuadreinventario).State = System.Data.Entity.EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!cuadreinventarioExists(id))
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

        // POST: api/cuadreinventario
        [ResponseType(typeof(cuadreinventario))]
        public async Task<IHttpActionResult> Postcuadreinventario(cuadreinventario cuadreinventario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.cuadreinventario.Add(cuadreinventario);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = cuadreinventario.id }, cuadreinventario);
        }

        // DELETE: api/cuadreinventario/5
        [ResponseType(typeof(cuadreinventario))]
        public async Task<IHttpActionResult> Deletecuadreinventario(int id)
        {
            cuadreinventario cuadreinventario = await db.cuadreinventario.FindAsync(id);
            if (cuadreinventario == null)
            {
                return NotFound();
            }

            db.cuadreinventario.Remove(cuadreinventario);
            await db.SaveChangesAsync();

            return Ok(cuadreinventario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool cuadreinventarioExists(int id)
        {
            return db.cuadreinventario.Count(e => e.id == id) > 0;
        }
    }
}