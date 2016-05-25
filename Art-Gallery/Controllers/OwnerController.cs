﻿using System;
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
                agent.Sales = totalSales;
                agent.Profit = totalProfit;
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

    }
}