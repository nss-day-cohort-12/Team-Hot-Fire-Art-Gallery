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
        public ActionResult Index()
        {
            DataStoreContext db = new DataStoreContext();

            //List < IndividualPiece > Pieces = db.IndividualPiece.ToList();

            var ArtInventory = (from art in db.ArtWork
                                join piece in db.IndividualPiece
                                on art.ArtWorkId equals piece.ArtWorkId
                                orderby art.Title
                                select new CustomerArtViewModel
                                {
                                    ArtworkId = art.ArtWorkId,
                                    IndividualId = piece.IndividualPieceId,
                                    Title = art.Title,
                                    Image = piece.Image,
                                    HasSold = piece.Sold
                                }).ToList();

            CustomerPieceViewModel AllArt = new CustomerPieceViewModel
            {
                AllPieces = ArtInventory
            };

            return View(AllArt);
        }

        public ActionResult Selection()
        {
            return View();
        }
    }
}