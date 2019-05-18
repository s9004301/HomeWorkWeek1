using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HomeW1.Method;
using HomeW1.Models;

namespace HomeW1.Controllers
{
    public class 客戶聯絡人Controller : Controller
    {
        //private ClientDataEntities db = new ClientDataEntities();
        客戶聯絡人Repository repoClientContactData;
        客戶資料Repository repoClientInfo;

        public 客戶聯絡人Controller()
        {
            repoClientContactData = RepositoryHelper.Get客戶聯絡人Repository();
            repoClientInfo = RepositoryHelper.Get客戶資料Repository();
        }
        // GET: 客戶聯絡人
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "email_desc" : "Email";
            ViewBag.JobSortParm = sortOrder == "Job" ? "job_desc" : "Job";
            ViewBag.CellpSortParm = sortOrder == "Cellp" ? "cellp_desc" : "Cellp";
            ViewBag.PhoneSortParm = sortOrder == "Phone" ? "phone_desc" : "Phone";
            ViewBag.ClientSortParm = sortOrder == "Client" ? "client_desc" : "Client";
            var ContactData = repoClientContactData.SelectData();
            if (!String.IsNullOrEmpty(searchString))
            {
                ContactData = repoClientContactData.ViewDataFilter(ContactData, searchString);
            }
            repoClientContactData.SortData(ref ContactData, sortOrder);
            return View(ContactData.ToList());
        }

        // GET: 客戶聯絡人/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 ContactData = repoClientContactData.Find(id.Value);
            if (ContactData == null)
            {
                return HttpNotFound();
            }
            return View(ContactData);
        }

        // GET: 客戶聯絡人/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(repoClientInfo.All(), "Id", "客戶名稱");
            return View();
        }

        // POST: 客戶聯絡人/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 ContactData)
        {
            if (repoClientContactData.CheckEmailIsDuplicate(ContactData))
            {
                return View(ContactData);
            }

            if (ModelState.IsValid)
            {
                repoClientContactData.Add(ContactData);
                repoClientContactData.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(repoClientInfo.All(), "Id", "客戶名稱", ContactData.客戶Id);
            return View(ContactData);
        }

        // GET: 客戶聯絡人/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 ContactData = repoClientContactData.Find(id.Value);
            if (ContactData == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(repoClientInfo.All(), "Id", "客戶名稱", ContactData.客戶Id);
            return View(ContactData);
        }

        // POST: 客戶聯絡人/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 ContactData)
        {
            if (repoClientContactData.CheckEmailIsDuplicate(ContactData))
            {
                return View(ContactData);
            }

            if (ModelState.IsValid)
            {
                repoClientContactData.UnitOfWork.Context.Entry(ContactData).State = EntityState.Modified;
                repoClientContactData.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(repoClientInfo.All(), "Id", "客戶名稱", ContactData.客戶Id);
            return View(ContactData);
        }

        // GET: 客戶聯絡人/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 ContactData = repoClientContactData.Find(id.Value);
            if (ContactData == null)
            {
                return HttpNotFound();
            }
            return View(ContactData);
        }

        // POST: 客戶聯絡人/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶聯絡人 ContactData = repoClientContactData.Find(id);
            ContactData = repoClientContactData.Remove(ContactData);
            repoClientContactData.UnitOfWork.Context.Entry(ContactData).State = EntityState.Modified;
            //repoClientContactData.Delete(ContactData);
            repoClientContactData.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repoClientContactData.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult DownloadXlsx()
        {
            var data = repoClientContactData.All().Select(x => new { x.客戶Id, x.職稱, x.姓名, x.Email, x.手機, x.電話 });
            if (!data.Any())
            {
                return View("Index", repoClientContactData.SelectData());
            }
            var table = CommonMethod.CreateDataTable(data);
            System.IO.MemoryStream stream = CommonMethod.ExportExcelFromDataTable(table,"客戶聯絡人");
            FileContentResult fResult = new FileContentResult(stream.ToArray(), "application/x-xlsx");
            fResult.FileDownloadName = "客戶聯絡人.xlsx";
            return fResult;
        }
    }
}
