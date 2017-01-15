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
    public class CodeController : ODataController
    {
        readonly SqliteContext _sqliteContext;

        public CodeController(SqliteContext sqliteContext)
        {
            _sqliteContext = sqliteContext;
        }

        [EnableQuery(PageSize = 10000)]
        public IHttpActionResult Get()
        {
            return Ok(_sqliteContext.CodeEntities.AsQueryable());
        }

        [EnableQuery(PageSize = 10000)]
        public IHttpActionResult Get([FromODataUri] string key)
        {
            return Ok(_sqliteContext.CodeEntities.SqlQuery("SELECT * FROM code_iata WHERE city LIKE '" + key + "'"));
        }

        [HttpGet]
        [ODataRoute("code_iata({key})")]
        [EnableQuery(PageSize = 10000)]
        public IHttpActionResult GetEventData([FromODataUri] string key)
        {
            return Ok(_sqliteContext.CodeEntities.SqlQuery("SELECT * FROM code_iata WHERE city LIKE '" + key + "'"));
        }
        [HttpPost]
        [ODataRoute("code_iata")]
        public async Task<IHttpActionResult> CreateEventData(Code_iata code)
        {
            if (code != null && !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _sqliteContext.CodeEntities.Add(code);
            await _sqliteContext.SaveChangesAsync();

            return Created(code);
        }
        
        [HttpPut]
        [ODataRoute("code_iata")]
        public async Task<IHttpActionResult> Put([FromODataUri] string key, Code_iata code)
        {
            if (code != null && !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_sqliteContext.CodeEntities.Any(t => t.City.Equals(key)))
            {
                return Content(HttpStatusCode.NotFound, "NotFound");
            }

            _sqliteContext.CodeEntities.AddOrUpdate(code);
            await _sqliteContext.SaveChangesAsync();

            return Updated(code);
        }
        

        [HttpPut]
        [ODataRoute("code_iata")]
        public async Task<IHttpActionResult> Patch([FromODataUri] string key, Delta<Code_iata> delta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_sqliteContext.CodeEntities.Any(t => t.City.Equals(key)))
            {
                return Content(HttpStatusCode.NotFound, "NotFound");
            }

            var code = _sqliteContext.CodeEntities.Single(t => t.City.Equals(key));
            delta.Patch(code);
            await _sqliteContext.SaveChangesAsync();

            return Updated(code);
        }

        [HttpDelete]
        [ODataRoute("code_iata")]
        public async Task<IHttpActionResult> Delete([FromODataUri] string key)
        {
            var entity = _sqliteContext.CodeEntities.FirstOrDefault(t => t.City.Equals(key));

            if (entity == null)
            {
                return Content(HttpStatusCode.NotFound, "NotFound");
            }
            
            _sqliteContext.CodeEntities.Remove(entity);
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
