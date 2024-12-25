using general_software_model.br.com.project.model;
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
    public partial class FrmSalesHistory : Form
    {
        public FrmSalesHistory()
        {
            InitializeComponent();
        }

        #region LISTAR AS VENDAS NO HISTORICO DE VENDAS
        private void FrmSalesHistory_Load(object sender, EventArgs e)
        {
            SaleDAO dao = new SaleDAO();

            SaleTable.DefaultCellStyle.ForeColor = Color.Black;
            SaleTable.DataSource = dao.ListSales();
        }
        #endregion

        #region SEARCH 
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime DateStart, DateEnd;

                DateStart = Convert.ToDateTime(txtDataStart.Value.ToString("yyyy-MM-dd"));
                DateEnd = Convert.ToDateTime(txtDataEnd.Value.ToString("yyyy-MM-dd"));


                if (DateStart > DateEnd)
                {
                    MessageBox.Show("A data de inicio do filtro é menor que a data fim.");
                    return;
                }

                SaleDAO dao = new SaleDAO();

                SaleTable.DataSource = dao.ListSalesPerPeriods(DateStart, DateEnd);
            }
            catch (Exception err)
            {
                MessageBox.Show("Error: " + err.Message);
            }
        }
        #endregion

        #region CLEAN LIST
        private void btnClear_Click(object sender, EventArgs e)
        {
            SaleDAO dao = new SaleDAO();

            SaleTable.DataSource = dao.ListSales();
        }

        #endregion

        #region VERIFICAR OS DADOS NO HISTORICO DE VENDAS 
        private void SaleTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int sale_id = int.Parse(SaleTable.CurrentRow.Cells[0].Value.ToString());

                FrmDetailSale Tela = new FrmDetailSale(sale_id);

                DateTime DataSale = Convert.ToDateTime(SaleTable.CurrentRow.Cells[1].Value.ToString());

                Tela.textDate.Text = DataSale.ToString("dd/MM/yyyy");
                Tela.txtClient.Text = SaleTable.CurrentRow.Cells[2].Value.ToString();
                Tela.textTotalSale.Text = SaleTable.CurrentRow.Cells[3].Value.ToString();
                Tela.txtObservation.Text = SaleTable.CurrentRow.Cells[4].Value.ToString();

                Tela.ShowDialog();
            }
            catch (Exception err)
            {
                MessageBox.Show("Erro: " + err.Message);
            }
        }
        #endregion
    }
}
