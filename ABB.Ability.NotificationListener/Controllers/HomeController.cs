using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ABB.Ability.NotificationListener.Core;

namespace ABB.Ability.NotificationListener.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["SignalRHost"] = NotificationListenerConfiguration.SignalRHost;
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
