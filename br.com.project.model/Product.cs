using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace general_software_model.br.com.project.model
{
    internal class Product
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public decimal Price { get; set; }
        public string Column { get; set; }
        public int StockQuantity { get; set; }
        public int Supplier_id { get; set; }
    }
}
