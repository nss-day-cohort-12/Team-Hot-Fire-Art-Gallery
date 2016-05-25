using Art_Gallery.Models;
using Art_Gallery.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Art_Gallery.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index(string artistString, string mediumString)
        {
            DataStoreContext db = new DataStoreContext();

            //Main query that pulls back all needed data
            var ArtInventory = (from art in db.ArtWork
                                join piece in db.IndividualPiece
                                on art.ArtWorkId equals piece.ArtWorkId
                                where art.NumberInInventory >= 1 && piece.Sold == false
                                orderby art.Title
                                select new CustomerArtViewModel
                                {
                                    ArtworkId = art.ArtWorkId,
                                    IndividualId = piece.IndividualPieceId,
                                    ArtistName = art.Artist,
                                    Title = art.Title,
                                    Image = piece.Image,
                                    Medium = art.Medium,
                                    QtyInInventory = art.NumberInInventory,
                                    HasSold = piece.Sold
                                });

            //Query only artists for artist dropdown select 
            var ArtistQry = from art in db.ArtWork
                            orderby art.Artist
                            select art.Artist;

            var ArtistList = new List<string>();
            ArtistList.AddRange(ArtistQry.Distinct());
            ViewData["artistString"] = new SelectList(ArtistList);

            //Query mediums for dropdown
            var MediumsQry = from art in db.ArtWork
                            orderby art.Medium
                            select art.Medium;

            var MediumsList = new List<string>();
            MediumsList.AddRange(MediumsQry.Distinct());
            ViewData["mediumString"] = new SelectList(MediumsList);

            //Allows dropbox selection to filter results
            if (!string.IsNullOrEmpty(artistString))
            {
                ArtInventory = ArtInventory.Where(a => a.ArtistName.Contains(artistString));
            }

            if (!string.IsNullOrEmpty(mediumString))
            {
                ArtInventory = ArtInventory.Where(m => m.Medium == mediumString);
            }

            //Primary ViewModel of all art from primary query
            CustomerPieceViewModel AllArt = new CustomerPieceViewModel
            {
                AllPieces = ArtInventory.ToList()
            };

            return View(AllArt);
        }

        public ActionResult Selection(int ArtworkId)
        {
            DataStoreContext db = new DataStoreContext();

            var SelectionInfo = (from art in db.ArtWork
                                join piece in db.IndividualPiece
                                on art.ArtWorkId equals piece.ArtWorkId
                                 where art.ArtWorkId == ArtworkId
                                orderby art.Title
                                select new CustomerArtViewModel
                                {
                                    ArtworkId = art.ArtWorkId,
                                    IndividualId = piece.IndividualPieceId,
                                    ArtistName = art.Artist,
                                    Title = art.Title,
                                    Image = piece.Image,
                                    Medium = art.Medium,
                                    Dimensions = art.Dimensions,
                                    QtyInInventory = art.NumberInInventory,
                                    Price = (float)(double)piece.Price,
                                    HasSold = piece.Sold,
                                    Location = piece.Location
                                });

            CustomerPieceViewModel SelectedPiece = new CustomerPieceViewModel
            {
                AllPieces = SelectionInfo.ToList()
            };

            return View(SelectedPiece);
        }
    }
}