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
        public ActionResult Index(string artistString, string mediumString, string priceString)
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
                                    Price = piece.Price,
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

            var PriceList = new List<string>();
            PriceList.Add("Less than $250");
            PriceList.Add("Less than $500");
            PriceList.Add("Less than $1000");
            PriceList.Add("Greater than $1000");
            ViewData["priceString"] = new SelectList(PriceList);


            //Allows dropbox selection to filter results
            if (!string.IsNullOrEmpty(artistString))
            {
                ArtInventory = ArtInventory.Where(a => a.ArtistName.Contains(artistString));
            }

            if (!string.IsNullOrEmpty(mediumString))
            {
                ArtInventory = ArtInventory.Where(m => m.Medium == mediumString);
            }

            if (!string.IsNullOrEmpty(priceString))
            {
                switch (priceString)
                {
                    case "Less than $250":
                        ArtInventory = ArtInventory.Where(p => p.Price <= 250);
                        break;
                    case "Less than $500":
                        ArtInventory = ArtInventory.Where(p => p.Price <= 500);
                        break;
                    case "Less than $1000":
                        ArtInventory = ArtInventory.Where(p => p.Price <= 1000);
                        break;
                    case "Greater than $1000":
                        ArtInventory = ArtInventory.Where(p => p.Price > 1000);
                        break;
                    default:
                        ArtInventory = ArtInventory.Where(p => p.Price > 0);
                        break;
                }
            };

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
                                    Price = piece.Price,
                                    HasSold = piece.Sold,
                                    Location = piece.Location
                                });

            CustomerPieceViewModel SelectedPiece = new CustomerPieceViewModel
            {
                AllPieces = SelectionInfo.ToList()
            };

            return View(SelectedPiece);
        }

        public ActionResult Shows()
        {
            DataStoreContext db = new DataStoreContext();

            var ShowInfo = db.ArtShow.ToList();

            ArtshowViewModel allShows = new ArtshowViewModel()
            {
                ShowList = ShowInfo
            };

            return View(allShows);
        }
    }
}