﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;


namespace WebApp2
{
    public class HomeController : Controller
    {
        //private AppDBContext db = new AppDBContext();

        public ActionResult Index()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}