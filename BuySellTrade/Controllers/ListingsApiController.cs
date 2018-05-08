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
using BuySellTrade.Models;

namespace BuySellTrade.Controllers
{
    /* Read only access to API */
    public class ListingsApiController : ApiController
    {
        private BuySellTradeDB db = new BuySellTradeDB();
        [Route("api/Listings")]
        // GET: api/Listings
        // GET: api/ListingsApi
        public IQueryable<Listing> GetListings()
        {
            return db.Listings;
        }

        [Route("api/Listings/{id:int}")]
        // GET: api/Listings/5
        // GET: api/ListingsApi/5
        [ResponseType(typeof(Listing))]
        public IHttpActionResult GetListing(int id)
        {
            Listing listing = db.Listings.Find(id);
            if (listing == null)
            {
                return NotFound();
            }

            return Ok(listing);
        }

        [Route("api/Listings/{title}")]
        // GET: api/Listings/Computer
        // GET: api/ListingsApi/Computer
        public IQueryable GetTitle(string title)
        {
            return db.Listings.Where(l => l.Title == title);
            
        }

    }

}