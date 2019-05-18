using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using HairSalon.Models;

namespace HairSalon.Controllers
{
    public class StylistsController : Controller
    {
        [HttpGet("/stylists")]
        public ActionResult Index()
        {
            List<Stylist> allStylists = Stylist.GetAll();
            return View(allStylists);
        }

        [HttpGet("/stylists/new")]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost("/stylists")]
        public ActionResult Create(string stylistName)
        {
            Stylist newStylist = new Stylist(stylistName);
            newStylist.Save();
            List<Stylist> allStylists = Stylist.GetAll();
            return View("Index", allStylists);
        }

        [HttpGet("/stylists/{id}")]
        public ActionResult Show(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Stylist selectedStylist = Stylist.Find(id);
            List<Specialty> specialtyDescription = Specialty.GetAll();
            List<Client> clientNames = selectedStylist.GetClients();
            model.Add("stylists", selectedStylist);
            model.Add("clients", clientNames);
            model.Add("specialtyDescription", specialtyDescription);
            return View(model);
        }

        [HttpPost("/stylists/{stylistId}/clients")]
        public ActionResult Create(int stylistId, string clientName)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Stylist foundStylist = Stylist.Find(stylistId);
            Client newClient = new Client(clientName, stylistId);
            newClient.Save();
            foundStylist.AddClient(newClient);
            List<Client> clientNames = foundStylist.GetClients();
            List<Specialty> specialtyDescription = Specialty.GetAll();
            model.Add("clients", clientNames);
            model.Add("stylists", foundStylist);
            model.Add("specialtyDescription", specialtyDescription);
            return View("Show", model);
        }

  

        [HttpPost("/stylists/{stylistId}/delete")]
        public ActionResult Delete(int stylisId)
        {
            Stylist stylist = Stylist.Find(stylisId);
            stylist.Delete();
            Dictionary<string, object> model = new Dictionary<string, object>();
            model.Add("stylists", stylist);
            return View(model);
        }
    }
}