using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DalSoft.Hosting.BackgroundQueue;
using Microsoft.AspNetCore.Mvc;

namespace Sol_Background_Worker.Controllers
{
    public class DemoController : Controller
    {
        private readonly BackgroundQueue backgroundQueue;

        public DemoController(BackgroundQueue backgroundQueue)
        {
            this.backgroundQueue = backgroundQueue;
        }

        #region Private Method

        private async Task LongRunningTask()
        {
            await Task.Run(async () =>
            {
                for (int counter = 0; counter <= 100; counter++)
                {
                    await Task.Delay(1000);
                }
            });
        }

        #endregion Private Method

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult OnPost()
        {
            backgroundQueue.Enqueue(async (cancelToken) =>
            {
                await LongRunningTask();
            });

            return RedirectToAction("Index", "Home");
        }
    }
}