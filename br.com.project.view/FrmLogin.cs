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
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string senha = txtPassword.Text;

            EmployeeDAO dao = new EmployeeDAO();

            btnLogin.Text = "Carregando...";

            if (dao.LoginMethod(email, senha) == true)
            {
                this.Hide();
            }
        }
    }
}
