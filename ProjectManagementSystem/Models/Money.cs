using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsManagementSystem.Models
{
    public class Money
    {
        public int Id { get; set; }
        public string MoneysAmount { get; set; }
        public MoneyType MoneyType { get; set; }

    }
}
