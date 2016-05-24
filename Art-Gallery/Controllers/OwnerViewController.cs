using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Art_Gallery.Models;
using Art_Gallery.ViewModels;

namespace Art_Gallery.Controllers
{
    public class OwnerViewController : Controller
    {
        DataStoreContext db = new DataStoreContext();
        // GET: OwnerView
        public ActionResult Index()
        {
            // on index, show inventory, asking price, and cost
            var pieces = (from piece in db.IndividualPiece
                        join art in db.ArtWork
                        on piece.ArtWorkId equals art.ArtWorkId
                        orderby art.ArtWorkId
                        select new PieceViewModel
                        {
                            Title = art.Title,
                            Cost = piece.Cost,
                            AskingPrice = piece.Price
                        }).ToList();
            InventoryViewModel inventory = new InventoryViewModel
            {
                Pieces = pieces
            };
            
            return View(inventory);
        }
    }
}