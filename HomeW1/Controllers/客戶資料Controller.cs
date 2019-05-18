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
using HomeW1.ViewModel;

namespace HomeW1.Controllers
{
    public class 客戶資料Controller : Controller
    {
        //private ClientDataEntities db = new ClientDataEntities();
        客戶資料Repository repoClientInfo;

        public 客戶資料Controller()
        {
            repoClientInfo = RepositoryHelper.Get客戶資料Repository();
        }
        // GET: 客戶資料
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.TaxIDSortParm = sortOrder == "TaxID" ? "taxID_desc" : "TaxID";
            ViewBag.PhoneSortParm = sortOrder == "Phone" ? "phone_desc" : "Phone";
            ViewBag.FaxSortParm = sortOrder == "Fax" ? "fax_desc" : "Fax";
            ViewBag.AddrSortParm = sortOrder == "Addr" ? "addr_desc" : "Addr";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "email_desc" : "Email";
            ViewBag.ClientClassSortParm = sortOrder == "ClientClass" ? "clientClass_desc" : "ClientClass";

            var data = repoClientInfo.SelectData();

            if (!String.IsNullOrEmpty(searchString))
            {
                data = repoClientInfo.ViewDataFilter(data, searchString);
            }


            repoClientInfo.SortData(ref data, sortOrder);
            ClientDataViewModel viewData = new ClientDataViewModel
            {
                ClientData = data.ToList(),
            };
            return View(viewData);
        }

        // GET: 客戶資料/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 ClientData = repoClientInfo.Find(id.Value);
            if (ClientData == null)
            {
                return HttpNotFound();
            }
            return View(ClientData);
        }

        // GET: 客戶資料/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: 客戶資料/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,是否已刪除,客戶分類")] 客戶資料 ClientData)
        {
            if (ModelState.IsValid)
            {
                repoClientInfo.Add(ClientData);
                repoClientInfo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(ClientData);
        }

        // GET: 客戶資料/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 ClientData = repoClientInfo.Find(id.Value);
            if (ClientData == null)
            {
                return HttpNotFound();
            }
            return View(ClientData);
        }

        // POST: 客戶資料/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,是否已刪除,客戶分類")] 客戶資料 ClientData)
        {
            if (ModelState.IsValid)
            {
                repoClientInfo.UnitOfWork.Context.Entry(ClientData).State = EntityState.Modified;
                repoClientInfo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            return View(ClientData);
        }

        // GET: 客戶資料/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 ClientData = repoClientInfo.Find(id.Value);
            if (ClientData == null)
            {
                return HttpNotFound();
            }
            return View(ClientData);
        }

        // POST: 客戶資料/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 ClientData = repoClientInfo.Find(id);
            ClientData = repoClientInfo.Remove(ClientData);
            repoClientInfo.UnitOfWork.Context.Entry(ClientData).State = EntityState.Modified;
            //repoClientInfo.Delete(ClientData);
            repoClientInfo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repoClientInfo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult DownloadXlsx()
        {
            var data = repoClientInfo.All().Select(x => new { x.客戶名稱, x.統一編號, x.電話, x.傳真,x.地址,x.Email,x.客戶分類 });
            if (!data.Any())
            {
                return View("Index", repoClientInfo.SelectData());
            }
            var table = CommonMethod.CreateDataTable(data);
            System.IO.MemoryStream stream = CommonMethod.ExportExcelFromDataTable(table, "客戶資料");
            FileContentResult fResult = new FileContentResult(stream.ToArray(), "application/x-xlsx");
            fResult.FileDownloadName = "客戶資料.xlsx";
            return fResult;
        }
    }
}
