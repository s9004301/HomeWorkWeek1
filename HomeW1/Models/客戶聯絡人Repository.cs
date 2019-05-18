using System;
using System.Linq;
using System.Collections.Generic;
	
namespace HomeW1.Models
{   
	public  class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
	{
		public 客戶聯絡人 Find(int id)
		{
			return this.All().Where(p => p.Id == id).FirstOrDefault();

		}

		public 客戶聯絡人 Remove(客戶聯絡人 data)
		{
			data.是否已刪除 = true;
			return data;
		}

		public bool CheckEmailIsDuplicate(客戶聯絡人 data)
		{
			return this.Where(x => x.客戶Id == data.客戶Id).Any(y=>y.Email == data.Email);
		}

		public void SortData(ref IEnumerable<客戶聯絡人> inputData, string sortOrder)
		{
			switch (sortOrder)
			{
				case "name_desc":
					inputData = inputData.OrderByDescending(s => s.姓名);
					break;
				case "Email":
					inputData = inputData.OrderBy(s => s.Email);
					break;
				case "email_desc":
					inputData = inputData.OrderByDescending(s => s.Email);
					break;
				case "Job":
					inputData = inputData.OrderBy(s => s.職稱);
					break;
				case "job_desc":
					inputData = inputData.OrderByDescending(s => s.職稱);
					break;
				case "Cellp":
					inputData = inputData.OrderBy(s => s.手機);
					break;
				case "cellp_desc":
					inputData = inputData.OrderByDescending(s => s.手機);
					break;
				case "Phone":
					inputData = inputData.OrderBy(s => s.電話);
					break;
				case "phone_desc":
					inputData = inputData.OrderByDescending(s => s.電話);
					break;
				case "Client":
					inputData = inputData.OrderBy(s => s.客戶資料.客戶名稱);
					break;
				case "client_desc":
					inputData = inputData.OrderByDescending(s => s.客戶資料.客戶名稱);
					break;
				default:
					inputData = inputData.OrderBy(s => s.姓名);
					break;
			}
		}

        internal IEnumerable<客戶聯絡人> ViewDataFilter(IEnumerable<客戶聯絡人> contactData, string searchString)
        {
            return contactData.Where(s => s.職稱.Contains(searchString));
        }

        internal IEnumerable<客戶聯絡人> SelectData()
        {
           return (IEnumerable<客戶聯絡人>)this.Where(x => !x.是否已刪除);
        }
    }

	public  interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
	{

	}
}