using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace HomeW1.Models
{   
	public  class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
	{
		public 客戶銀行資訊 Find(int id)
		{
			return this.All().Where(p => p.Id == id).FirstOrDefault();
		}
		public 客戶銀行資訊 Remove(客戶銀行資訊 data)
		{
			data.是否已刪除 = true;
			return data;
		}

        public void SortData(ref IEnumerable<客戶銀行資訊> inputData, string sortOrder)
        {
            switch (sortOrder)
            {
                case "name_desc":
                    inputData = inputData.OrderByDescending(s => s.銀行名稱);
                    break;
                case "Bank":
                    inputData = inputData.OrderBy(s => s.銀行代碼);
                    break;
                case "bank_desc":
                    inputData = inputData.OrderByDescending(s => s.銀行代碼);
                    break;
                case "Branch":
                    inputData = inputData.OrderBy(s => s.分行代碼);
                    break;
                case "branch_desc":
                    inputData = inputData.OrderByDescending(s => s.分行代碼);
                    break;
                case "AccountName":
                    inputData = inputData.OrderBy(s => s.帳戶名稱);
                    break;
                case "accountName_desc":
                    inputData = inputData.OrderByDescending(s => s.帳戶名稱);
                    break;
                case "AccountNum":
                    inputData = inputData.OrderBy(s => s.帳戶號碼);
                    break;
                case "accountNum_desc":
                    inputData = inputData.OrderByDescending(s => s.帳戶號碼);
                    break;
                case "Client":
                    inputData = inputData.OrderBy(s => s.客戶資料.客戶名稱);
                    break;
                case "client_desc":
                    inputData = inputData.OrderByDescending(s => s.客戶資料.客戶名稱);
                    break;
                default:
                    inputData = inputData.OrderBy(s => s.銀行名稱);
                    break;
            }
        }

        internal IEnumerable<客戶銀行資訊> SelectData()
        {
           return (IEnumerable<客戶銀行資訊>)this.Where(x => !x.是否已刪除).Include(客 => 客.客戶資料);
        }
    }

	public  interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
	{

	}
}