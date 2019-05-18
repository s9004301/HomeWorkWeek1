using System;
using System.Linq;
using System.Collections.Generic;
	
namespace HomeW1.Models
{   
	public  class ClientViewRepository : EFRepository<ClientView>, IClientViewRepository
	{

	}

	public  interface IClientViewRepository : IRepository<ClientView>
	{

	}
}