using general_software_model.br.com.project.dao;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace general_software_model.br.com.project.view
{
    public partial class FrmDetailSale : Form
    {
        int sale_id;
        public FrmDetailSale(int sale_id)
        {
            this.sale_id = sale_id;
            InitializeComponent();
        }

        #region LISTAR DETALHES DE VENDA
        private void FrmDetailSale_Load(object sender, EventArgs e)
        {
            // Carrega tela de detalhes 
            ItemSaleDAO dao = new ItemSaleDAO();

            ItemSaleTable.DefaultCellStyle.BackColor = Color.White;
            ItemSaleTable.DataSource = dao.ListAllItemsForSale(sale_id);
        }
        #endregion
    }
}
