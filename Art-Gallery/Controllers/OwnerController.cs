using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;
using Art_Gallery.ViewModels;
using Art_Gallery.Models;

namespace Art_Gallery.Controllers
{
    public class OwnerController : Controller
    {
        public ActionResult Index()
        {
            DataStoreContext db = new DataStoreContext();
            var pieceList = (from aw in db.ArtWork
                            join ip in db.IndividualPiece
                            on aw.ArtWorkId equals ip.ArtWorkId
                            select new PieceViewModel
                            {
                                Title = aw.Title,
                                Cost = (float)(double)ip.Cost,
                                AskingPrice = (float)(double)ip.Price
                            }).ToList();

            InventoryViewModel inventory = new InventoryViewModel
            {
                Pieces = pieceList
            };

            return View(inventory);
        }

    }
}