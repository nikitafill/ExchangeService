using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeService.Domain.Models
{
    public class ExchangeRate
    {
        public DateTime Date { get; set; } 
        public string CurrencyCode { get; set; } 
        public decimal Rate { get; set; } 
    }

}
