using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NTierOracleIdentityExample.Bll.Services.Abstract;
using NTierOracleIdentityExample.Dll.Entities;
using NTierOracleIdentityExample.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NTierOracleIdentityExmple.Web.Controllers
{
    public class HomeController : Controller
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly ILogService _logService;
        #endregion

        #region Constructor
        public HomeController(ILogService logService, IMapper mapper)
        {
            _logService = logService;
            _mapper = mapper;
        }
        #endregion

        #region GET methods
        public IActionResult Index()
        {
            ViewBag.SelectedNavItem = "Home";
            return View();
        }

        public JsonResult GetLog()
        {
            List<LogViewModel> logs = _mapper.Map<List<Log>, List<LogViewModel>>(_logService.SelectLog().Result);
        
            var logList = from l in logs
                          select new
                            {
                                Id = l.pk,
                                LogDate = l.Date,
                                LogValue = l.Value
                            };

            return Json(new { status = "success", records = logList.OrderBy(l => l.Id).ToList(), total = logList.Count() });
        }

        public JsonResult GetLogById(int pk)
        {
            LogViewModel log = _mapper.Map<Log, LogViewModel>(_logService.SelectLogById(pk).Result);

            return Json(new { status = "success", records = log });
        }
        #endregion

        #region POST methods
        [HttpPost]
        public JsonResult EditRule(LogViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                //Map viewModel to model
                Log model = _mapper.Map<Log>(viewModel);
                model.ModifiedBy = "DummyUser";
                model.ModifiedDate = DateTime.Now;
                _logService.UpdateLog(model);

                return Json(new
                {
                    success = true,
                    message = "Log saved!"
                });
            }
            else
            {
                return Json(new { success = false, message = "Error!" });
            }
        }

        [HttpPost]
        public JsonResult DeleteLog(int pk)
        {
            _logService.DeleteLog(pk);
            return Json(new { success = false, message = "Log deleted!" });
        }

        #endregion

        #region Helper methods

        #endregion
    }
}
