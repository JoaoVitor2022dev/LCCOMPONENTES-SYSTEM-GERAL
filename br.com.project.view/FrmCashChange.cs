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
    public partial class FrmCashChange : Form
    {
        decimal sale_change;
        public FrmCashChange(decimal sale_change)
        {
            InitializeComponent();
            this.sale_change = sale_change;
        }

        private void FrmCashChange_Load(object sender, EventArgs e)
        {
            texchangeMoney.Text = sale_change.ToString();
        }
    }
}
