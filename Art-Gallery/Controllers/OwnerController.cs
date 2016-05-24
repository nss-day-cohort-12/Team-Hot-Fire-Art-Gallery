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
                                AskingPrice = (float)(double)ip.Price,
                                Sold = ip.Sold
                            }).ToList();

            InventoryViewModel inventory = new InventoryViewModel
            {
                Pieces = pieceList
            };

            return View(inventory);
        }

        public ActionResult Sales()
        {
            DataStoreContext db = new DataStoreContext();
            var soldPieces = (from ip in db.IndividualPiece
                              where ip.Sold == true
                              join aw in db.ArtWork
                              on ip.ArtWorkId equals aw.ArtWorkId
                              select new PieceViewModel
                              {
                                  Title = aw.Title,
                                  Cost = (float)(double)ip.Cost,
                                  AskingPrice = (float)(double)ip.Price,
                                  Sold = ip.Sold
                              }).ToList();
            InventoryViewModel inventory = new InventoryViewModel
            {
                Pieces = soldPieces,
                TotalProfit = 0,
                TotalSales = 0
            };

            foreach (var piece in inventory.Pieces)
            {
                inventory.TotalSales += piece.AskingPrice;
                inventory.TotalProfit += (piece.AskingPrice - piece.Cost);
            }
            return View(inventory);
        }

    }
}