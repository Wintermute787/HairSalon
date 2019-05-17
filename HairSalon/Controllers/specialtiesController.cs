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
    }
}