using HomeW1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HomeW1.ViewModel
{
    public class ClientDataViewModel
    {
        public ClientDataViewModel()
        {
            ClientClassfication = new List<SelectListItem>
            {
                new SelectListItem{ Text = "VIP", Value = "VIP" ,Selected =false},
                new SelectListItem{ Text = "Diamond", Value = "Diamond" ,Selected =false},
                new SelectListItem{ Text = "Gold", Value = "Gold" ,Selected =false},
            };
        }
        public List<客戶資料> ClientData { get; set; }

        public List<SelectListItem> ClientClassfication { get; set; }
    }
}