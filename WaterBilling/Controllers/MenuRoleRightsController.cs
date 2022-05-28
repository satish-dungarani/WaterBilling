using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterBillingDA;
using WaterBilling.Models;

namespace WaterBilling.Controllers
{
    public class MenuRoleRightsController : BaseController
    {
        clsMenuRoleRights _objMenuRoleRights = new clsMenuRoleRights();
        //
        // GET: /MenuRoleRights/
        public ActionResult Index()
        {
            SelectList _ObjUserRole = clsCommonUI.fillMasterValue((int)clsCommonUI.MasterType.UserRole);
            ViewData["fillUserRole"] = _ObjUserRole;

            return View();
        }

        public ActionResult Save(List<MenuRoleRightsModel> _objParam)
        {
            bool retval = false;
            try
            {
                if (Convert.ToBoolean(_objMenuRoleRights.deleteMenuRoleRights(_objParam[0].RefRoleId)))
                {
                    foreach (var _obj in _objParam)
                    {
                        if (_obj.CanView == true)
                        {
                            _obj.InsUser = clsCommonUI._User;
                            _obj.InsTerminal = clsCommonUI._Terminal;

                            retval = Convert.ToBoolean(_objMenuRoleRights.saveMenuRoleRights(_obj.RefRoleId, _obj.RefMenuId, _obj.CanInsert, _obj.CanUpdate
                                , _obj.CanDelete, _obj.CanView, _obj.InsUser, _obj.InsTerminal, _obj.UpdUser, _obj.UpdTerminal));
                        }
                    }
                }
                if (retval)
                {
                    //TempData["Success"] = "Rights successfully allocated!";
                    return Select(_objParam[0].RefRoleId);
                }
                else
                {
                    TempData["Error"] = "There was some server error. Please try again later!";
                    return View("Index");
                }
            }
            catch (Exception)
            {

                return View("Index");
            }
        }

        public ActionResult Select(int pRefRoleId)
        {
            try
            {
                List<MenuRoleRightsModel> _objMenuRightsList = new List<MenuRoleRightsModel>();
                if (pRefRoleId != 0)
                {
                    foreach (var _obj in _objMenuRoleRights.getMenuRoleRights(pRefRoleId))
                    {
                        _objMenuRightsList.Add(new MenuRoleRightsModel
                        {
                            RefRoleId = _obj.RefRoleId.HasValue ? _obj.RefRoleId.Value : 0,
                            RefMenuId = _obj.RefMenuId,
                            MenuName = _obj.MenuName,
                            CanInsert = _obj.CanInsert,
                            CanUpdate = _obj.CanUpdate,
                            CanDelete = _obj.CanDelete,
                            CanView = _obj.CanView,
                            InsUser = _obj.InsUser.HasValue ? _obj.InsUser.Value : 0,
                            InsDate = _obj.InsDate,
                            InsTerminal = _obj.InsTerminal,
                            UpdUser = _obj.UpdUser.HasValue ? _obj.UpdUser.Value : 0,
                            UpdDate = _obj.UpdDate,
                            UpdTerminal = _obj.UpdTerminal
                        });
                    }
                }
                return PartialView("LoadMenuRoleRightsPartial", _objMenuRightsList);

            }
            catch (Exception)
            {

                return PartialView("LoadMenuRoleRightsPartial");
            }
        }
    }
}