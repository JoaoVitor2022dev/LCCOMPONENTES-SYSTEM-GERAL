using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace general_software_model.br.com.project.model
{
    internal class ItemSale
    {
        public int Id { get; set; }
        public int Sales_Id { get; set; }
        public int Product_id { get; set; }
        public int Qtd { get; set; }
        public decimal Subtotal { get; set; }
    }
}
