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

        // **********************
        // ARTWORK INFORMATION -- CRUD
        // **********************

        public ActionResult Index()
        {
            DataStoreContext db = new DataStoreContext();
            var pieceList = (from aw in db.ArtWork
                            join ip in db.IndividualPiece
                            on aw.ArtWorkId equals ip.ArtWorkId
                            select new PieceViewModel
                            {
                                IndividualPieceId = ip.IndividualPieceId,
                                Title = aw.Title,
                                Cost = ip.Cost,
                                AskingPrice = ip.Price,
                                Sold = ip.Sold
                            }).ToList();

            InventoryViewModel inventory = new InventoryViewModel
            {
                Pieces = pieceList
            };

            return View(inventory);
        }

        // CREATE - GET
        [HttpGet]
        public ActionResult CreatePiece()
        {
            return View();
        }

        // CREATE - POST
        [HttpPost]
        public ActionResult CreatePiece(PieceViewModel pieces)
        {
            using (DataStoreContext db = new DataStoreContext())
            {
                if (ModelState.IsValid)
                {
                    var matchingArtWork = db.ArtWork.First(a => a.Title == pieces.Title).ArtWorkId;

                    if (matchingArtWork > 0)
                    {
                        IndividualPiece piece = new IndividualPiece
                        {
                            ArtWorkId = matchingArtWork,
                            Image = pieces.Image,
                            Price = pieces.AskingPrice,
                            Cost = pieces.Cost,
                            Sold = pieces.Sold,
                            EditionNumber = pieces.EditionNumber,
                            Location = pieces.Location,
                            InvoiceId = null
                        };
                        db.IndividualPiece.Add(piece);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else {
                        ViewBag.Title = "Our system currently only supports adding new prints of existing art.";
                    }
                }

                return View(pieces);
            }
        }

        // EDIT - GET
        [HttpGet]
        public ActionResult EditPiece(int IndividualPieceId)
        {
            using (DataStoreContext db = new DataStoreContext())
            {
                var pieces = (from aw in db.ArtWork
                              join ip in db.IndividualPiece
                              on aw.ArtWorkId equals ip.ArtWorkId
                              where ip.IndividualPieceId == IndividualPieceId
                              select new PieceViewModel
                              {
                                  IndividualPieceId = ip.IndividualPieceId,
                                  Title = aw.Title,
                                  Cost = ip.Cost,
                                  AskingPrice = ip.Price,
                                  Sold = ip.Sold
                              }).ToList();


                PieceViewModel pieceForEdit = new PieceViewModel
                {
                    Title = pieces.Select(p => p.Title).FirstOrDefault(),
                    Cost = pieces.Select(p => p.Cost).FirstOrDefault(),
                    AskingPrice = pieces.Select(p => p.AskingPrice).FirstOrDefault(),
                    Sold = pieces.Select(p => p.Sold).FirstOrDefault()
                };


                return View(pieceForEdit);
            }
        }

        // EDIT - POST
        [HttpPost]
        public ActionResult EditPiece(PieceViewModel pieces)
        {
            using (DataStoreContext db = new DataStoreContext())
            {
                var piece = db.IndividualPiece.Find(pieces.IndividualPieceId);

                if (ModelState.IsValid)
                {
                    piece.Cost = pieces.Cost;
                    piece.Price = pieces.AskingPrice;
                    piece.Sold = pieces.Sold;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(pieces);
            }
        }

        // DELETE
        public ActionResult DeletePiece(int IndividualPieceId)
        {
            if (IndividualPieceId != 0)
            {
                using (DataStoreContext db = new DataStoreContext())
                {
                    IndividualPiece piece = db.IndividualPiece.Find(IndividualPieceId);

                    db.IndividualPiece.Remove(piece);
                    db.SaveChanges();

                }
            }
            else
            {
                ViewBag.Title = "There was a problem";
            }
            return RedirectToAction("Index");
        }


        // **********************
        // VIEW SALES INFORMATION
        // **********************

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
                                  Cost = ip.Cost,
                                  AskingPrice = ip.Price,
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


        // **********************
        // AGENT INFORMATION -- view, create, edit
        // **********************

        public ActionResult Agents()
        {
            DataStoreContext db = new DataStoreContext();
            var agents = (from a in db.Agent
                          join i in db.Invoice
                          on a.AgentId equals i.AgentId
                          orderby a.Name
                          select new AgentViewModel
                          {
                              AgentId = a.AgentId,
                              Name = a.Name,
                              Location = a.Location,
                              Active = a.Active,
                              PieceSold = i.PieceSold
                          }).ToList();



            foreach (var agent in agents)
            {
                string[] pieces = agent.PieceSold.Split(',');
                double totalSales = 0;
                double totalProfit = 0;
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
                                     Price = ip.Price,
                                     Cost = ip.Cost
                                 }).ToList();
                    if (individualSale.Count > 0)
                    {
                        totalSales += individualSale[0].Price;
                        double profit = individualSale[0].Price - individualSale[0].Cost;
                        totalProfit += profit;
                    }
                }
                agent.Sales = Math.Round(totalSales, 2);
                agent.Profit = Math.Round(totalProfit, 2);
            }

            // brute force solution for displaying unique agents & profit/sales totals
            for (int i = 0; i < agents.Count; i++)
            {
                if ((i + 1 != agents.Count) && agents[i].Name == agents[i + 1].Name)
                {
                    if (agents[i + 2].Name == agents[i + 1].Name)
                    {
                        agents[i + 1].PieceSold += "," + agents[i + 2].PieceSold;
                        agents[i + 1].Sales += agents[i + 2].Sales;
                        agents[i + 1].Profit += agents[i + 2].Profit;
                        agents.Remove(agents[i + 2]);
                    } 
                    agents[i + 1].PieceSold += ", " + agents[i].PieceSold;
                    agents[i + 1].Sales += agents[i].Sales;
                    agents[i + 1].Profit += agents[i].Profit;
                    agents.Remove(agents[i]);
                }
            }


            // need to add new agents to this list
            // new agent = not represented on above agents list, 
            // will not have an invoice

            var potentialNewAgents = db.Agent.ToList();
            foreach (var potentialNewAgent in potentialNewAgents)
            {
                if (potentialNewAgent.AgentId > 7)
                {
                    AgentViewModel newAgent = new AgentViewModel
                    {
                        Name = potentialNewAgent.Name,
                        Location = potentialNewAgent.Location,
                        PieceSold = "none",
                        Active = potentialNewAgent.Active
                    };
                    agents.Add(newAgent);
                }
            }

            AgentSalesViewModel agentSales = new AgentSalesViewModel
            {
                Agents = agents
            };

            return View(agentSales);
        }

        // EDIT - GET
        [HttpGet]
        public ActionResult EditAgent(int AgentId)
        {
            using (DataStoreContext db = new DataStoreContext())
            {
                var agents = (from a in db.Agent
                              join i in db.Invoice
                              on a.AgentId equals i.AgentId
                              orderby a.Name
                              where a.AgentId == AgentId
                              select new AgentViewModel
                              {
                                  AgentId = a.AgentId,
                                  Name = a.Name,
                                  Location = a.Location,
                                  Active = a.Active,
                                  PieceSold = i.PieceSold
                              }).ToList();

                AgentViewModel agentsForEdit = new AgentViewModel
                {
                    Name = agents.Select(a => a.Name).FirstOrDefault(),
                    Location = agents.Select(a => a.Location).FirstOrDefault(),
                    Active = agents.Select(a => a.Active).FirstOrDefault()
                };

                return View(agentsForEdit);
            }
        }

       // EDIT - POST
       [HttpPost]
        public ActionResult EditAgent(AgentViewModel agents)
        {
            using (DataStoreContext db = new DataStoreContext())
            {
                var agent = db.Agent.Find(agents.AgentId);
                if (ModelState.IsValid)
                {
                    agent.Name = agents.Name;
                    agent.Location = agents.Location;
                    agent.Active = agents.Active;
                    db.SaveChanges();
                    return RedirectToAction("Agents");
                }

                return View(agents);
            }
        }

        // CREATE - GET
        [HttpGet]
        public ActionResult CreateAgent()
        {
            return View();
        }

        // CREATE - POST
        [HttpPost]
        public ActionResult CreateAgent(AgentViewModel agents)
        {
            using (DataStoreContext db = new DataStoreContext())
            {
                if (ModelState.IsValid)
                {
                    Agent agent = new Agent
                    {
                        Name = agents.Name,
                        Location = agents.Location,
                        Address = agents.Address,
                        PhoneNumber = agents.PhoneNumber,
                        Active = agents.Active
                    };
                    db.Agent.Add(agent);
                    db.SaveChanges();
                    return RedirectToAction("Agents");
                }

                return View(agents);
            }
        }







        // **********************
        // CREATE NEW ARTWORK FOR ARTWORK TABLE
        // **********************

        public ActionResult GetArtWork()
        {
            DataStoreContext db = new DataStoreContext();
            var artwork = (from aw in db.ArtWork
                             select new ArtWorkViewModel // adding new ArtWork below following ArtWorkViewModel properties
                             {
                                 ArtWorkId = aw.ArtWorkId,
                                 Artist = aw.Artist,
                                 Title = aw.Title,
                                 YearOriginalCreated = aw.YearOriginalCreated,
                                 Medium = aw.Medium,
                                 Dimensions = aw.Dimensions,
                                 NumberMade = aw.NumberMade,
                                 NumberInInventory = aw.NumberInInventory,
                                 NumberSold = aw.NumberSold
                             }).ToList();

           //var artists = db.ArtWork.Select(x => x.Artist);
           //var artistList = new List<string>();

           // //Query only artists for artist dropdown select 
           // var ArtistQry = from aw in db.ArtWork
           //                 orderby aw.Artist
           //                 select aw.Artist;

           // var ArtistList = new List<string>();
           // ArtistList.AddRange(ArtistQry.Distinct());
           // ViewData["artistString"] = new SelectList(ArtistList);

            InventoryViewModel inventoryArtWork = new InventoryViewModel
            {
                WorksOfArt = artwork // takes var artwork from above and adds to WorksOfArt list within InventoryViewModel
            };

            return View(inventoryArtWork);
        }


        // CREATE - GET
        [HttpGet] 
        public ActionResult CreateArtWork()
        {
            return View();
        }


        // this is how we submit the data from the form to the database
        [HttpPost] //must be here for page to work, sends data on submit button click
        public ActionResult CreateArtWork(ArtWorkViewModel artworkDetails)
        {
            using (DataStoreContext db = new DataStoreContext())
            {
                if (ModelState.IsValid)
                {
                    ArtWork artwork = new ArtWork
                    {
                        Artist = artworkDetails.Artist,
                        Title = artworkDetails.Title,
                        YearOriginalCreated = artworkDetails.YearOriginalCreated,
                        Medium = artworkDetails.Medium,
                        Dimensions = artworkDetails.Dimensions,
                        NumberMade = artworkDetails.NumberMade,
                        NumberInInventory = artworkDetails.NumberInInventory,
                        NumberSold = artworkDetails.NumberSold,
                    };
                    db.ArtWork.Add(artwork); //saves info to the context
                    db.SaveChanges(); //saves info to the databse
                    return RedirectToAction("Index");
                }
            }
            return View(artworkDetails);
        }










    }
}