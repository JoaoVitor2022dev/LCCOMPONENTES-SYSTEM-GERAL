using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace general_software_model.br.com.project.model
{
    internal class Sale
    {
        public int Id { get; set; }
        public int customer_id { get; set; }
        public DateTime Date_sales { get; set; }
        public decimal Total_sales { get; set; }
        public string observation { get; set; }
    }
}
