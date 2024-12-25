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
    public partial class FrmMenu : Form
    {
        public FrmMenu()
        {
            InitializeComponent();
        }
        private void FrmMenu_Load(object sender, EventArgs e)
        {
            txtDateCurrent.Text = DateTime.Now.ToShortDateString();
        }

        private void MenuCustomerRegister_Click(object sender, EventArgs e)
        {
            FrmCustomer Tela = new FrmCustomer();
            Tela.ShowDialog();
        }

        private void MenuCustumerConsultation_Click(object sender, EventArgs e)
        {
            FrmCustomer Tela = new FrmCustomer();
            Tela.TabClient.SelectedTab = Tela.tabPage2;

            Tela.ShowDialog();
        }

        private void MenuRegisterEmployee_Click(object sender, EventArgs e)
        {
            FrmEmployee Tela = new FrmEmployee();
            Tela.ShowDialog();
        }

        private void MenuEmployeeConsultation_Click(object sender, EventArgs e)
        {
            FrmEmployee Tela = new FrmEmployee();
            Tela.TabEmployee.SelectedTab = Tela.tabPage2;
            Tela.ShowDialog();
        }

        private void MenuRegisterSupplier_Click(object sender, EventArgs e)
        {
            FrmSupplier Tela = new FrmSupplier();
            Tela.ShowDialog();
        }

        private void MenuSupplierConsultation_Click(object sender, EventArgs e)
        {
            FrmSupplier Tela = new FrmSupplier();
            Tela.TabSupplier.SelectedTab = Tela.tabPage2;
            Tela.ShowDialog();
        }

        private void MenuRegisterProduct_Click(object sender, EventArgs e)
        {
            FrmProduct Tela = new FrmProduct();
            Tela.ShowDialog();
        }

        private void MenuProductConsultation_Click(object sender, EventArgs e)
        {
            FrmProduct Tela = new FrmProduct();
            Tela.TabPorduct.SelectedTab = Tela.tabPage2;
            Tela.ShowDialog();
        }

        private void MenuNewSale_Click(object sender, EventArgs e)
        {
            FrmSale Tela = new FrmSale();
            Tela.ShowDialog();
        }

        private void MenuHistorySale_Click(object sender, EventArgs e)
        {
            FrmSalesHistory Tela = new FrmSalesHistory();
            Tela.ShowDialog();
        }

        private void MenuExitTheSystem_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("Você deseja sair?", "ATENÇÃO!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (Result == DialogResult.Yes)
            {
                // Apertou para sair 
                Application.Exit();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            txtCurrentTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }
    }
}
