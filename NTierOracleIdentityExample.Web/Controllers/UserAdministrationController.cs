using NTierOracleIdentityExample.Bll.Services.Abstract;
using NTierOracleIdentityExample.Dll.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using NTierOracleIdentityExample.Web.Models;
using AutoMapper;

namespace NTierOracleIdentityExample.Web.Controllers
{
    [Authorize(Policy = "RequireAdministrator")]
    public class UserAdministration : Controller
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly ILogService _logService;
        private readonly ILogger<UserAdministration> _logger;
        private readonly IUserAdministrationService _userAdministrationService;
        private readonly IRoleAdministrationService _roleAdministrationService;
        private readonly IUserRoleAdministrationService _userRoleAdministrationService;
        #endregion

        #region Constructor
        public UserAdministration(IMapper mapper, ILogger<UserAdministration> logger, IUserAdministrationService userAdministrationService, IRoleAdministrationService roleAdministrationService, IUserRoleAdministrationService userRoleAdministrationService)
        {
            _mapper = mapper;
            _logger = logger;
            _userAdministrationService = userAdministrationService;
            _roleAdministrationService = roleAdministrationService;
            _userRoleAdministrationService = userRoleAdministrationService;
        }
        #endregion

        #region GET methods
        public IActionResult Index()
        {
            ViewBag.SelectedNavItem = "UserAdministration";

            //Roles
            var roleList = from o in _roleAdministrationService.GetAllRoles()
                                    select new
                                    {
                                        id = o.Id,
                                        name = o.Name
                                    };
            ViewData["roles"] = new SelectList(roleList, "id", "name");

            return View();
        }

        [HttpGet]
        public JsonResult GetUsers()
        {
            List<ApplicationUser> usersList = _userAdministrationService.GetAllUsers();

            //Load ViewData            
            var users = from us in usersList
                        select new
                        {
                            recid = us.Id,
                            firstName = us.FirstName,
                            lastName = us.LastName,
                            userName = us.UserName,
                            email = us.Email
                        };

            return Json(new { status = "success", records = users.OrderBy(us => us.recid).ToList(), total = users.Count() });
        }

        [HttpGet]
        public JsonResult GetRoles()
        {
            List<IdentityRole> rolesList = _roleAdministrationService.GetAllRoles();

            //Load ViewData            
            var roles = from us in rolesList
                        select new
                        {
                            recid = us.Id,
                            name = us.Name
                        };

            return Json(new { status = "success", records = roles.OrderBy(us => us.recid).ToList(), total = roles.Count() });
        }        

        [HttpGet]
        public JsonResult GetUserRoles()
        {
            List<ApplicationUser> usersList = _userAdministrationService.GetAllUsers();

            //Load ViewData            
            var userRoles = from us in usersList
                            select new
                            {
                                recid = us.Id,
                                firstName = us.FirstName,
                                lastName = us.LastName,
                                userName = us.UserName,
                                email = us.Email,
                                roles = string.Join(", ", _userRoleAdministrationService.GetUserRoles(us).Result),
                                rolesId = _userRoleAdministrationService.GetUserRoleId(_userRoleAdministrationService.GetUserRoles(us).Result).ToArray()
                            };

            return Json(new { status = "success", records = userRoles.OrderBy(us => us.recid).ToList(), total = userRoles.Count() });
        }

        public IActionResult GetAddRolePartial()
        {
            return PartialView("~/Views/UserAdministration/Partial/AddRole.cshtml", new RoleViewModel());
        }

        public IActionResult GetEditRolePartial(string recid)
        {
            IdentityRole model = _roleAdministrationService.GetRoleById(recid);
            RoleViewModel viewModel = _mapper.Map<RoleViewModel>(model);

            return PartialView("~/Views/UserAdministration/Partial/EditRole.cshtml", viewModel);
        }

        public IActionResult GetEmptyEditRolePartial()
        {
            return PartialView("~/Views/UserAdministration/Partial/EditRole.cshtml", new RoleViewModel());
        }

        public IActionResult GetAddUserPartial()
        {
            return PartialView("~/Views/UserAdministration/Partial/AddUser.cshtml", new ApplicationUserViewModel());
        }

        public IActionResult GetEditUserPartial(string recid)
        {
            ApplicationUser model = _userAdministrationService.GetUserById(recid);
            ApplicationUserViewModel viewModel = _mapper.Map<ApplicationUserViewModel>(model);

            return PartialView("~/Views/UserAdministration/Partial/EditUser.cshtml", viewModel);
        }

        public IActionResult GetEmptyEditUserPartial()
        {
            return PartialView("~/Views/UserAdministration/Partial/EditUser.cshtml", new ApplicationUserViewModel());
        }

        #endregion

        #region POST methods
        [HttpPost]
        public IActionResult InsertUser(ApplicationUserViewModel viewModel)
        {
            ApplicationUser model = _mapper.Map<ApplicationUser>(viewModel);

            if (null == _userAdministrationService.GetAllUsers().Where(b => b.UserName == model.UserName).FirstOrDefault())
            {
                model.Id = Guid.NewGuid().ToString().ToUpper();
                model.ConcurrencyStamp = Guid.NewGuid().ToString();
                _userAdministrationService.InsertUser(model);
                return Json(new { success = true, message = "User added!" });
            }
            else
            {
                return Json(new { success = false, message = "User already exist!" });
            }
        }

        [HttpPost]
        public JsonResult InsertRole(RoleViewModel viewModel)
        {
            if (null == _roleAdministrationService.GetAllRoles().Where(b => b.Name == viewModel.Name).FirstOrDefault())
            {
                IdentityRole model = _mapper.Map<IdentityRole>(viewModel);
                model.Id = Guid.NewGuid().ToString().ToUpper();
                model.ConcurrencyStamp = Guid.NewGuid().ToString();

                _roleAdministrationService.InsertRole(model);

                return Json(new { success = true, message = "Role added!" });
            }
            else
            {
                return Json(new { success = false, message = "Role already exist!" });
            }
        }

        [HttpPost]
        public JsonResult InsertRoleClaim(string roleId, string ClaimType, string ClaimValue)
        {
            IdentityRole role = _roleAdministrationService.GetRoleById(roleId);
            Claim existingEntry = _roleAdministrationService.GetRoleClaimById(role);

            if (null == existingEntry)
            {
                Claim entry = new Claim(ClaimType, ClaimValue);
                _roleAdministrationService.InsertRoleClaim(role, entry);

                return Json(new { success = true, message = "Uspješno ste dodali novu privilegiju za rolu." });
            }
            else
            {
                return Json(new { success = false, message = "Role already exists!" });
            }
        }

        [HttpPost]
        public JsonResult EditUser(ApplicationUserViewModel viewModel)
        {
            viewModel.ConcurrencyStamp = Guid.NewGuid().ToString();
            ApplicationUser model = _userAdministrationService.GetUserById(viewModel.Id);
            _mapper.Map(viewModel, model);
            _userAdministrationService.EditUser(model);

            return Json(new { success = true, message = "User saved!" });
        }

        [HttpPost]
        public JsonResult EditRole(RoleViewModel viewModel)
        {
            IdentityRole model = _mapper.Map<IdentityRole>(viewModel);
            model.ConcurrencyStamp = Guid.NewGuid().ToString();

            _roleAdministrationService.InsertRole(model);

            return Json(new { success = true, message = "Role saved!" });
        }

        [HttpPost]
        public JsonResult EditUserRoles(string userRoleRecid, List<string> editUserRoles)
        {
            ApplicationUser user = _userAdministrationService.GetUserById(userRoleRecid);
            List<string> userRoleNames = _userRoleAdministrationService.GetUserRoles(user).Result;
            List<string> userRoleIds = _userRoleAdministrationService.GetUserRoleId(userRoleNames);

            if (editUserRoles != null && editUserRoles.Count > 0)
            {
                if(userRoleIds.Count == 0)
                {
                    foreach (var role in editUserRoles)
                    {
                        //Add all roles for user
                        _userRoleAdministrationService.AddUserToRole(user, _roleAdministrationService.GetAllRoles().Where(r => r.Id.Equals(role)).FirstOrDefault().Name);
                    }
                }
                else
                {
                    foreach (var role in editUserRoles)
                    {
                        if (!userRoleIds.Any(s => s.Contains(role)))
                        {
                            //Insert role
                            _userRoleAdministrationService.AddUserToRole(user, _roleAdministrationService.GetAllRoles().Where(r => r.Id.Equals(role)).FirstOrDefault().Name);
                        }
                    }
                    foreach (var role in userRoleIds)
                    {
                        if (editUserRoles.ToList().IndexOf(role) == -1)
                        {
                            //if null then delete
                            _userRoleAdministrationService.RemoveUserFromRole(user, _roleAdministrationService.GetAllRoles().Where(r => r.Id.Equals(role)).FirstOrDefault().Name);
                        }
                    }
                }
            }
            else
            {
                foreach (var role in userRoleNames)
                {
                    //Delete all roles for user
                    _userRoleAdministrationService.RemoveUserFromRole(user, _roleAdministrationService.GetAllRoles().Where(r => r.Name.Equals(role)).FirstOrDefault().Name);
                }
            }

            return Json(new { success = true, message = "Uspješno ste izmjenili korisnika!" });
        }        

        [HttpPost]
        public JsonResult EditRoleClaim(string recid, string name)
        {
            IdentityRole entry = _roleAdministrationService.GetAllRoles().Where(b => b.Id == recid).FirstOrDefault();
            entry.Name = name;
            entry.NormalizedName = name.ToUpper();
            entry.ConcurrencyStamp = Guid.NewGuid().ToString();

            _roleAdministrationService.EditRole(entry);

            return Json(new { success = true, message = "Uspješno ste izmjenili rolu!" });
        }

        [HttpPost]
        public JsonResult DeleteUser(string recid)
        {
            ApplicationUser entry = _userAdministrationService.GetAllUsers().Where(b => b.Id == recid).FirstOrDefault();
            _userAdministrationService.DeleteUser(entry);

            return Json(new { success = false, message = "Uspješno ste izbrisali korisnika!" });
        }

        [HttpPost]
        public JsonResult DeleteRole(string recid)
        {
            IdentityRole entry = _roleAdministrationService.GetRoleById(recid);
            _roleAdministrationService.DeleteRole(entry);

            return Json(new { success = false, message = "Uspješno ste izbrisali rolu!" });
        }

        [HttpPost]
        public JsonResult DeleteRoleClaim(string roleId)
        {
            IdentityRole role = _roleAdministrationService.GetRoleById(roleId);
            Claim existingEntry = _roleAdministrationService.GetRoleClaimById(role);

            _roleAdministrationService.DeleteRoleClaim(role, existingEntry);

            return Json(new { success = false, message = "Uspješno ste izbrisali privilegiju role!" });
        }

        #endregion

        #region Helper methods

        #endregion
    }
}