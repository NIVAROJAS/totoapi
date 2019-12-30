﻿using System;
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
    public class remuneracionesController : ApiController
    {
        private repositoryEntities db = new repositoryEntities();

        // GET: api/remuneraciones
        public IQueryable<remuneraciones> Getremuneraciones()
        {
            return db.remuneraciones;
        }

        // GET: api/remuneraciones/5
        [ResponseType(typeof(remuneraciones))]
        public async Task<IHttpActionResult> Getremuneraciones(int id)
        {
            remuneraciones remuneraciones = await db.remuneraciones.FindAsync(id);
            if (remuneraciones == null)
            {
                return NotFound();
            }

            return Ok(remuneracionesDto.FromModel(remuneraciones));
        }

        public ResponceDto<remuneracionesDto> Getremuneraciones(int draw, int start, int length, string search)
        {
            if (search == null) search = "";
            var total = db.remuneraciones.Where(x => x.usuario.nombre.Contains(search)).Count();
            var s = db.remuneraciones.Where(x => x.usuario.nombre.Contains(search)).OrderBy(x => x.id).Skip(start).Take(length).ToList();

            var result = new ResponceDto<remuneracionesDto>
            {
                draw = draw,
                recordsFiltered = total,
                recordsTotal = total,
                data = s.Select(a => remuneracionesDto.FromModel(a)).ToList()
            };

            return result;
        }

        // PUT: api/remuneraciones/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putremuneraciones(int id, remuneraciones remuneraciones)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != remuneraciones.id)
            {
                return BadRequest();
            }

            db.Entry(remuneraciones).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!remuneracionesExists(id))
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

        // POST: api/remuneraciones
        [ResponseType(typeof(remuneraciones))]
        public async Task<IHttpActionResult> Postremuneraciones(remuneraciones remuneraciones)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.remuneraciones.Add(remuneraciones);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = remuneraciones.id }, remuneraciones);
        }

        // DELETE: api/remuneraciones/5
        [ResponseType(typeof(remuneraciones))]
        public async Task<IHttpActionResult> Deleteremuneraciones(int id)
        {
            remuneraciones remuneraciones = await db.remuneraciones.FindAsync(id);
            if (remuneraciones == null)
            {
                return NotFound();
            }

            db.remuneraciones.Remove(remuneraciones);
            await db.SaveChangesAsync();

            return Ok(remuneraciones);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool remuneracionesExists(int id)
        {
            return db.remuneraciones.Count(e => e.id == id) > 0;
        }
    }
}