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

        public ActionResult Agents()
        {
            DataStoreContext db = new DataStoreContext();
            var agents = (from a in db.Agent
                          join i in db.Invoice
                          on a.AgentId equals i.AgentId
                          select new AgentViewModel
                          {
                              Name = a.Name,
                              Location = a.Location,
                              Active = a.Active,
                              PieceSold = i.PieceSold
                          }).ToList();

            

            foreach (var agent in agents)
            {
                string[] pieces = agent.PieceSold.Split(',');
                List<double> sales = new List<double>();
                foreach (var piece in pieces)
                {
                    var individualSale = (
                                 from ip in db.IndividualPiece
                                 where ip.Sold == true
                                 join aw in db.ArtWork
                                 on ip.ArtWorkId equals aw.ArtWorkId
                                 where aw.Title == piece
                                 select new PriceViewModel
                                 {
                                     Price = ip.Price
                                 }).ToList();
                    if (individualSale[0].Price > 0) sales.Add(individualSale[0].Price);
                }
                agent.Sales = sales;
            }

            AgentSalesViewModel agentSales = new AgentSalesViewModel
            {
                Agents = agents
            };

            return View(agentSales);
        }

    }
}