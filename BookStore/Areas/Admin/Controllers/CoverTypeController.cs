using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            CoverType coverType = new CoverType();
            if (id == null)
            {
                //this is for create
                return View(coverType);
            }
            //this is for edit
            coverType = _unitOfWork.CoverType.Get(id.GetValueOrDefault());
            if (coverType == null)
            {
                return NotFound();
            }
            return View(coverType);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                if (coverType.Id == 0)
                {
                    _unitOfWork.CoverType.Add(coverType);

                }
                else
                {
                    _unitOfWork.CoverType.Update(coverType);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(coverType);
        }





        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj =  _unitOfWork.CoverType.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            bool prodExist = _unitOfWork.Product.GetAll(x => x.CoverTypeId != id).Any();
            
            if (!prodExist)
            //var objFromDb = _unitOfWork.CoverType.Get(id);
            //var objFromDb1 = _unitOfWork.Product.Get(id);

            //if (objFromDb == null)
            //{
            //    return Json(new { success = false, message = "Error while deleting" });
            //}

            //else if (objFromDb.Id == objFromDb1.CoverTypeId)
            {
                var coverType = _unitOfWork.CoverType.Get(id);
                _unitOfWork.CoverType.Remove(coverType);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Delete Successful" });


            }
            else
            {

                return Json(new { success = false, message = "Error while deleting" });
            }

        }



        #endregion
    }
}