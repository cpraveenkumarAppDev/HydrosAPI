using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HydrosApi.Models;

namespace HydrosApi.Controllers
{

    public class LayerActionController : ApiController
    {
        private SDEContext db = new SDEContext();

        [HttpGet]
        //public List<CD_LITHO_STRAT> Get()
        //{

           // return db.CD_LITHO_STRAT.ToList();

       // }
 
        public List<PlaceOfUsePolygonView> Get()
        {

            return db.PlaceOfUsePolygonView.ToList();
             

       }

        [Route("[action]")]
        [HttpGet]
        public IHttpActionResult GetShapes()
        {
            return Ok(db.PlaceOfUsePolygonView);
        }

        [Route("[action]/{id}")]
        [HttpGet]
        public IHttpActionResult GetShapeById(string id)
        {

            PlaceOfUsePolygonView pou = db.PlaceOfUsePolygonView.Find(id);

            if(pou==null)
            {
                return NotFound();
            }
            return Ok(pou);
        }

        //IMP-34-A17023011CBB-01

    }

    public class LayersController : ApiController
    {
        private OracleContext db = new OracleContext();

        // GET: api/Layers


        public IQueryable<LAYER> GetLAYERS()
        {
            return db.LAYERS;
        }

        // GET: api/Layers/5
        [ResponseType(typeof(LAYER))]
        public IHttpActionResult GetLAYER(long id)
        {
            LAYER lAYER = db.LAYERS.Find(id);
            if (lAYER == null)
            {
                return NotFound();
            }

            return Ok(lAYER.LOG_EVENTS);
        }

        // PUT: api/Layers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLAYER(long id, LAYER lAYER)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lAYER.ID)
            {
                return BadRequest();
            }

            db.Entry(lAYER).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LAYERExists(id))
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

        // POST: api/Layers
        [ResponseType(typeof(LAYER))]
        public IHttpActionResult PostLAYER(LAYER lAYER)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LAYERS.Add(lAYER);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (LAYERExists(lAYER.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = lAYER.ID }, lAYER);
        }

        // DELETE: api/Layers/5
        [ResponseType(typeof(LAYER))]
        public IHttpActionResult DeleteLAYER(long id)
        {
            LAYER lAYER = db.LAYERS.Find(id);
            if (lAYER == null)
            {
                return NotFound();
            }

            db.LAYERS.Remove(lAYER);
            db.SaveChanges();

            return Ok(lAYER);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LAYERExists(long id)
        {
            return db.LAYERS.Count(e => e.ID == id) > 0;
        }
    }
}