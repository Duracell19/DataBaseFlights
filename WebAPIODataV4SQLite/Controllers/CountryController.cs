using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using WebAPIODataV4SQLite.DomainModel;

namespace WebAPIODataV4SQLite.Controllers
{
    public class CountryController : ODataController
    {
        readonly SqliteContext _sqliteContext;

        public CountryController(SqliteContext sqliteContext)
        {
            _sqliteContext = sqliteContext;
        }

        [EnableQuery(PageSize = 10000)]
        public IHttpActionResult Get()
        {
            return Ok(_sqliteContext.CountryEntities.AsQueryable());
        }

        [EnableQuery(PageSize = 10000)]
        public IHttpActionResult Get([FromODataUri] string key)
        {
            return Ok(_sqliteContext.CountryEntities.SqlQuery("SELECT * FROM code_iata WHERE country LIKE '" + key + "'"));
        }

        [HttpGet]
        [ODataRoute("country({key})")]
        [EnableQuery(PageSize = 10000)]
        public IHttpActionResult GetEventData([FromODataUri] string key)
        {
            return Ok(_sqliteContext.CountryEntities.SqlQuery("SELECT * FROM code_iata WHERE country LIKE '" + key + "'"));
        }
        [HttpPost]
        [ODataRoute("country")]
        public async Task<IHttpActionResult> CreateEventData(Countries code)
        {
            if (code != null && !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _sqliteContext.CountryEntities.Add(code);
            await _sqliteContext.SaveChangesAsync();

            return Created(code);
        }

        [HttpPut]
        [ODataRoute("country")]
        public async Task<IHttpActionResult> Put([FromODataUri] string key, Countries code)
        {
            if (code != null && !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_sqliteContext.CountryEntities.Any(t => t.Country.Equals(key)))
            {
                return Content(HttpStatusCode.NotFound, "NotFound");
            }

            _sqliteContext.CountryEntities.AddOrUpdate(code);
            await _sqliteContext.SaveChangesAsync();

            return Updated(code);
        }


        [HttpPut]
        [ODataRoute("country")]
        public async Task<IHttpActionResult> Patch([FromODataUri] string key, Delta<Countries> delta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_sqliteContext.CountryEntities.Any(t => t.Country.Equals(key)))
            {
                return Content(HttpStatusCode.NotFound, "NotFound");
            }

            var code = _sqliteContext.CountryEntities.Single(t => t.Country.Equals(key));
            delta.Patch(code);
            await _sqliteContext.SaveChangesAsync();

            return Updated(code);
        }

        [HttpDelete]
        [ODataRoute("country")]
        public async Task<IHttpActionResult> Delete([FromODataUri] string key)
        {
            var entity = _sqliteContext.CountryEntities.FirstOrDefault(t => t.Country.Equals(key));

            if (entity == null)
            {
                return Content(HttpStatusCode.NotFound, "NotFound");
            }

            _sqliteContext.CountryEntities.Remove(entity);
            await _sqliteContext.SaveChangesAsync();

            return Content(HttpStatusCode.NoContent, "Deleted");
        }

        protected override void Dispose(bool disposing)
        {
            _sqliteContext.Dispose();
            base.Dispose(disposing);
        }
    }
}