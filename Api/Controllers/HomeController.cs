﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(int id = 1)
        {
            ViewBag.UserId = id;
            return View();
        }

        [Route("viva")]
        public IActionResult VivaGraph(int id = 1)
        {
            ViewBag.UserId = id;
            return View();
        }

        [Route("NoServerNoServer")]
        public IActionResult NoServer()
        {
            return View();
        }
    }
}