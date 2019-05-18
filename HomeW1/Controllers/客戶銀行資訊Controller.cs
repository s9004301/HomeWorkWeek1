using HomeW1.Method;
using HomeW1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HomeW1.Controllers
{
    public class 客戶銀行資訊Controller : Controller
    {
        //private ClientDataEntities db = new ClientDataEntities();
        客戶銀行資訊Repository repoClientBankInfo;
        客戶資料Repository repoClientInfo;
        public 客戶銀行資訊Controller()
        {
            repoClientBankInfo = RepositoryHelper.Get客戶銀行資訊Repository();
            repoClientInfo = RepositoryHelper.Get客戶資料Repository();
        }
        // GET: 客戶銀行資訊
        public ActionResult Index(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.BankSortParm = sortOrder == "Bank" ? "bank_desc" : "Bank";
            ViewBag.BranchSortParm = sortOrder == "Branch" ? "branch_desc" : "Branch";
            ViewBag.AccountNameSortParm = sortOrder == "AccountName" ? "accountName_desc" : "AccountName";
            ViewBag.AccountNumSortParm = sortOrder == "AccountNum" ? "accountNum_desc" : "AccountNum";
            ViewBag.ClientSortParm = sortOrder == "Client" ? "client_desc" : "Client";

            var data = repoClientBankInfo.SelectData();
            repoClientBankInfo.SortData(ref data, sortOrder);
            return View(data.ToList());
        }

        // GET: 客戶銀行資訊/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 ClientBankInfo = repoClientBankInfo.Find(id.Value);
            if (ClientBankInfo == null)
            {
                return HttpNotFound();
            }
            return View(ClientBankInfo);
        }

        // GET: 客戶銀行資訊/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(repoClientInfo.All(), "Id", "客戶名稱");
            return View();
        }

        // POST: 客戶銀行資訊/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] 客戶銀行資訊 ClientBankInfo)
        {
            if (ModelState.IsValid)
            {
                repoClientBankInfo.Add(ClientBankInfo);
                repoClientBankInfo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(repoClientInfo.All(), "Id", "客戶名稱", ClientBankInfo.客戶Id);
            return View(ClientBankInfo);
        }

        // GET: 客戶銀行資訊/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 ClientBankInfo = repoClientBankInfo.Find(id.Value);
            if (ClientBankInfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(repoClientInfo.All(), "Id", "客戶名稱", ClientBankInfo.客戶Id);
            return View(ClientBankInfo);
        }

        // POST: 客戶銀行資訊/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] 客戶銀行資訊 ClientBankInfo)
        {
            if (ModelState.IsValid)
            {
                repoClientBankInfo.UnitOfWork.Context.Entry(ClientBankInfo).State = EntityState.Modified;
                repoClientBankInfo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(repoClientInfo.All(), "Id", "客戶名稱", ClientBankInfo.客戶Id);
            return View(ClientBankInfo);
        }

        // GET: 客戶銀行資訊/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 ClientBankInfo = repoClientBankInfo.Find(id.Value);
            if (ClientBankInfo == null)
            {
                return HttpNotFound();
            }
            return View(ClientBankInfo);
        }

        // POST: 客戶銀行資訊/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶銀行資訊 ClientBankInfo = repoClientBankInfo.Find(id);
            ClientBankInfo = repoClientBankInfo.Remove(ClientBankInfo);
            repoClientBankInfo.UnitOfWork.Context.Entry(ClientBankInfo).State = EntityState.Modified;
                //repoClientBankInfo.Delete(ClientBankInfo);
            repoClientBankInfo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repoClientBankInfo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult DownloadXlsx()
        {
            var data = repoClientBankInfo.All().Select(x => new { x.客戶Id, x.銀行名稱, x.銀行代碼, x.帳戶號碼, x.帳戶名稱, x.分行代碼 });
            if (!data.Any())
            {
                return View("Index", repoClientBankInfo.SelectData());
            }
            var table = CommonMethod.CreateDataTable(data);
            System.IO.MemoryStream stream = CommonMethod.ExportExcelFromDataTable(table, "客戶銀行資訊");
            FileContentResult fResult = new FileContentResult(stream.ToArray(), "application/x-xlsx");
            fResult.FileDownloadName = "客戶銀行資訊.xlsx";
            return fResult;
        }
    }
}
