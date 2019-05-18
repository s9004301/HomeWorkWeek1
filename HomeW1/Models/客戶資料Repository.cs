using System;
using System.Linq;
using System.Collections.Generic;
	
namespace HomeW1.Models
{   
	public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
	{
		public 客戶資料 Find(int id)
		{
			return this.All().Where(p => p.Id == id).FirstOrDefault();
		}
		public 客戶資料 Remove(客戶資料 data)
		{
			data.是否已刪除 = true;
			return data;
		}
        public IEnumerable<客戶資料> ViewDataFilter(IEnumerable<客戶資料> data,string searchString)
        {
           return data.Where(s => s.客戶分類.Contains(searchString));
        }

        public void SortData(ref IEnumerable<客戶資料> inputData, string sortOrder)
        {
            switch (sortOrder)
            {
                case "name_desc":
                    inputData = inputData.OrderByDescending(s => s.客戶名稱);
                    break;
                case "TaxID":
                    inputData = inputData.OrderBy(s => s.統一編號);
                    break;
                case "taxID_desc":
                    inputData = inputData.OrderByDescending(s => s.統一編號);
                    break;
                case "Phone":
                    inputData = inputData.OrderBy(s => s.電話);
                    break;
                case "phone_desc":
                    inputData = inputData.OrderByDescending(s => s.電話);
                    break;
                case "Fax":
                    inputData = inputData.OrderBy(s => s.傳真);
                    break;
                case "fax_desc":
                    inputData = inputData.OrderByDescending(s => s.傳真);
                    break;
                case "Addr":
                    inputData = inputData.OrderBy(s => s.地址);
                    break;
                case "addr_desc":
                    inputData = inputData.OrderByDescending(s => s.地址);
                    break;
                case "Email":
                    inputData = inputData.OrderBy(s => s.Email);
                    break;
                case "email_desc":
                    inputData = inputData.OrderByDescending(s => s.Email);
                    break;
                case "ClientClass":
                    inputData = inputData.OrderBy(s => s.客戶分類);
                    break;
                case "clientClass_desc":
                    inputData = inputData.OrderByDescending(s => s.客戶分類);
                    break;
                default:
                    inputData = inputData.OrderBy(s => s.客戶名稱);
                    break;
            }
        }

        internal IEnumerable<客戶資料> SelectData()
        {
           return (IEnumerable<客戶資料>)this.Where(x => !x.是否已刪除);
        }
    }

	public  interface I客戶資料Repository : IRepository<客戶資料>
	{

	}
}