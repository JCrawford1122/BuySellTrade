using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BuySellTrade.Models;
using System.IO;


namespace BuySellTrade.Controllers
{
    public class ListingsController : Controller
    {
        private BuySellTradeDB db = new BuySellTradeDB();

        // GET: Listings?Category=value&Title=value
        public ActionResult Index(string category, string title)
        {
            ViewBag.Category = new SelectList(CategoryList);
            // List of all listings
            var listings = db.Listings.ToList();
            // List listings in specfic category
            if (category != null)
            {
                // List listings with category and title
                if (title != null && title.Length > 0)
                {
                    listings = db.Listings.Where(l => l.Category == category).Where(
                        l => l.Title == title).ToList();
                }
                else
                {
                    listings = db.Listings.Where(l => l.Category == category).ToList();
                }
            }
            return View(listings);
        }

        // GET: Listings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Listing listing = db.Listings.Find(id);
            if (listing == null)
            {
                return HttpNotFound();
            }
            return View(listing);
        }

        // GET: Listings/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Category = new SelectList(CategoryList);
            return View();
        }

        // POST: Listings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Category,Title,Price,Description")] Listing listing, HttpPostedFileBase photo)
        {

            ViewBag.Category = new SelectList(CategoryList);
            if (ModelState.IsValid)
            {
                listing.Email = User.Identity.Name;
                if (photo != null && photo.ContentLength > 0)
                {
                    if (IsValidImage(photo))
                    {
                        using (var reader = new System.IO.BinaryReader(photo.InputStream))
                        {
                            listing.Photo = reader.ReadBytes(photo.ContentLength);
                        }
                    }
                    else
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                }
                db.Listings.Add(listing);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }  
            return View(listing);
        }

        // GET: Listings/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            ViewBag.Category = new SelectList(CategoryList);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Listing listing = db.Listings.Find(id);
            if (listing == null)
            {
                return HttpNotFound();
            }
            if (listing.Email == User.Identity.Name || User.IsInRole("Admin"))
            {
                return View(listing);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Listings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Category,Title,Price,Description")] Listing listing, HttpPostedFileBase photo)
        {
            ViewBag.Category = new SelectList(CategoryList);
            if (ModelState.IsValid)
            {
                listing.Email = User.Identity.Name;
                if (photo != null && photo.ContentLength > 0)
                {
                    if (IsValidImage(photo))
                    {
                        using (var reader = new System.IO.BinaryReader(photo.InputStream))
                        {
                            listing.Photo = reader.ReadBytes(photo.ContentLength);
                        }
                    }
                }
                db.Entry(listing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(listing);
        }

        // GET: Listings/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Listing listing = db.Listings.Find(id);
            if (listing == null)
            {
                return HttpNotFound();
            }
            if (listing.Email == User.Identity.Name || User.IsInRole("Admin"))
            {
                return View(listing);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: Listings/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Listing listing = db.Listings.Find(id);
            db.Listings.Remove(listing);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // AJAX-GET: Listings/SellerSearch?email=value&id=value
        
        public ActionResult SellerSearch(string email, int id)
        {
            var listings = db.Listings.Where(l => l.Email == email && l.Id != id);
            return PartialView("_SellerSearch", listings);
        }

        /* Return a list of distinct titles of current listings in JSON Format */
        public ActionResult TitleSearch(string term)
        {
            var listings = GetListings(term).Select(a => a.Title).Distinct();
            return Json(listings, JsonRequestBehavior.AllowGet);
        }

        /* Returns a list of listings that contain the searchString */
        private List<Listing> GetListings(string searchString)
        {

            return db.Listings.Where(l => l.Title.Contains(searchString)).ToList();
        }

        /* Checks the file extension of an uploaded file */
        private bool IsValidImage(HttpPostedFileBase photo)
        {
            if (Path.GetExtension(photo.FileName).ToLower() != ".jpg" &&
                Path.GetExtension(photo.FileName).ToLower() != ".png")
            {
                return false;
            }
            return true;
        }

        /* create the list used for the category dropdown */
        private static List<string> CategoryList = new List<string>(new[] {
            "Electronics", "Books", "Transportation", "Sporting Goods",
            "Music", "Video Games", "Collectibles" });

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
