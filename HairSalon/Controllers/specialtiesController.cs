using Microsoft.AspNetCore.Mvc;
using HairSalon.Models;
using System.Collections.Generic;
using System;

namespace HairSalon.Controllers
{
    public class SpecialtiesController : Controller
    {
        [HttpGet("/specialties")]
        public ActionResult Index()
        {
            List<Specialty> allSpecialties = Specialty.GetAll();
            return View(allSpecialties);
        }

        [HttpGet("/specialties/new")]
        public ActionResult New(int specialtyId)
        {
            return View();
        }

        [HttpPost("/specialties")]
        public ActionResult Create(string description)
        {
            Specialty newSpecialty = new Specialty(description);
            newSpecialty.Save();
            List<Specialty> allSpecialties = Specialty.GetAll();
            return View("Index", allSpecialties);

        }

        [HttpGet("/specialties/{id}")]
        public ActionResult Show(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Specialty selectedSpecialty = Specialty.Find(id);
            List<Stylist> stylistName = selectedSpecialty.GetStylists();
            List<Stylist> allStylists = Stylist.GetAll();
            model.Add("selectedSpecialty", selectedSpecialty);
            model.Add("stylistName", stylistName);
            model.Add("allStylists", allStylists);
            return View(model);
        }

        [HttpPost("/specialties/{specialtyId}/stylists/new")]
        public ActionResult AddStylist(int specialtyId, int stylistId)
        {
            Specialty specialty = Specialty.Find(specialtyId);
            Stylist stylist = Stylist.Find(stylistId);
            specialty.AddStylist(stylist);
            return RedirectToAction("Show", new{id = specialtyId});

        }
    }
}