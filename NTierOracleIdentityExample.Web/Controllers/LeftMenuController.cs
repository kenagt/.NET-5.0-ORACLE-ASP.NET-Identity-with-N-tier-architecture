using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTierOracleIdentityExample.Bll.Services.Abstract;

namespace NTierOracleIdentityExample.Web.Controllers
{
    [Authorize(Policy = "RequiredBasic")]
    public class LeftMenuController : Controller
    {
        #region Fields
        private readonly ILogService _logService;
        #endregion

        #region Constructors
        public LeftMenuController(ILogService logService)
        {
            _logService = logService;
        }

        #endregion

        #region Get Methods

        public IActionResult LeftMenu()
        {
            return PartialView("LeftMenu.cshtml");
        }

        #endregion
    }
}

