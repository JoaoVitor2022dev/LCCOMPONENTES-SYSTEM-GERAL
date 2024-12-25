using general_software_model.br.com.project.dao;
using general_software_model.br.com.project.model;
using general_software_model.Exeptions;
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
    public partial class FrmEmployee : Form
    {
        public FrmEmployee()
        {
            InitializeComponent();
        }

        #region REGISTER EMPLOYEE 
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateFields())
                {
                    MessageBox.Show("Por favor, preencha as informações: CPF, cargo, senha e acesso.");
                    return;
                }

                if (txtCodeEmployee.Text != "")
                {
                    MessageBox.Show("Já existe um fornecedor com esse ID");
                    return;
                }

                // method of storing data in the model
                Employee employee = EmployeeClient();

                //invoking the register method
                EmployeeDAO dao = new EmployeeDAO();

                dao.RegisterEmployee(employee);


                // trocar de pagina 
                TabEmployee.SelectedTab = tabPage2;
                EmployeeTable.DataSource = dao.ListEmployee();

                // Programmed method for clearing operating Inputs
                new Helpers().LimparTela(this);
                txtUf.SelectedIndex = -1;
                txtPosition.SelectedIndex = -1;
                txtAccessLevel.SelectedIndex = -1;
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Erro ao preencher as informações: " + ex.Message);
            }
            catch (Exception err)
            {
                MessageBox.Show("Error: " + err.Message);
            }
        }
        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtCpf.Text) ||
                string.IsNullOrWhiteSpace(txtAccessLevel.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                 string.IsNullOrWhiteSpace(txtPosition.Text))
            {
                return false;
            }
            return true;
        }

        private Employee EmployeeClient()
        {
            return new Employee
            {
                Name = txtName.Text,
                Rg = txtRg.Text,
                Cpf = txtCpf.Text,
                Email = txtEmail.Text,
                Password = txtPassword.Text,
                Position = txtPosition.Text,
                AccessLevel = txtAccessLevel.Text,
                Telephone = txtTelephone.Text,
                Phone = txtPhone.Text,
                Cep = txtCep.Text,
                Address = txtAddress.Text,
                Number = int.TryParse(txtNumberHome.Text, out int number) ? number : 0,
                Complement = txtComplement.Text,
                Neighborhood = txtNeighborhood.Text,
                City = txtCity.Text,
                State = txtUf.Text
            };
        }
        #endregion

        #region LIST EMPLOYEE RECORD
        private void FrmEmployee_Load(object sender, EventArgs e)
        {
            EmployeeTable.DefaultCellStyle.ForeColor = Color.Black;

            EmployeeDAO dao = new EmployeeDAO();
            EmployeeTable.DataSource = dao.ListEmployee();
        }
        #endregion

        #region DATA CLEANING METHOD
        private void BtnNew_Click(object sender, EventArgs e)
        {
            new Helpers().LimparTela(this);
            txtUf.SelectedIndex = -1;
            txtPosition.SelectedIndex = -1;
            txtAccessLevel.SelectedIndex = -1;
        }
        #endregion

        #region TRANSFER DATA FROM THE EMPLOYEE LIST TO THE REGISTRATION FORM
        private void EmployeeTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (EmployeeTable.CurrentRow != null && EmployeeTable.CurrentRow.Index >= 0)
            {
                FillEmployeeDetailsFromTable(EmployeeTable.CurrentRow);

                TabEmployee.SelectedTab = tabPage1;
            }
        }
        private void FillEmployeeDetailsFromTable(DataGridViewRow row)
        {
            // Assign the values from the table to the corresponding TextBoxes
            txtCodeEmployee.Text = row.Cells[0].Value?.ToString() ?? string.Empty;
            txtName.Text = row.Cells[1].Value?.ToString() ?? string.Empty;
            txtRg.Text = row.Cells[2].Value?.ToString() ?? string.Empty;
            txtCpf.Text = row.Cells[3].Value?.ToString() ?? string.Empty;
            txtEmail.Text = row.Cells[4].Value?.ToString() ?? string.Empty;
            txtPassword.Text = row.Cells[5].Value?.ToString() ?? string.Empty;
            txtPosition.Text = row.Cells[6].Value?.ToString() ?? string.Empty;
            txtAccessLevel.Text = row.Cells[7].Value?.ToString() ?? string.Empty;
            txtTelephone.Text = row.Cells[8].Value?.ToString() ?? string.Empty;
            txtPhone.Text = row.Cells[9].Value?.ToString() ?? string.Empty;
            txtCep.Text = row.Cells[10].Value?.ToString() ?? string.Empty;
            txtAddress.Text = row.Cells[11].Value?.ToString() ?? string.Empty;
            txtNumberHome.Text = row.Cells[12].Value?.ToString() ?? string.Empty;
            txtComplement.Text = row.Cells[13].Value?.ToString() ?? string.Empty;
            txtNeighborhood.Text = row.Cells[14].Value?.ToString() ?? string.Empty;
            txtCity.Text = row.Cells[15].Value?.ToString() ?? string.Empty;
            txtUf.Text = row.Cells[16].Value?.ToString() ?? string.Empty;
        }
        #endregion

        #region EDIT EMPLOYEE
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                // method of storing data in the model
                Employee employee = ObterClienteDoFormulario();

                //invoking the register method
                EmployeeDAO dao = new EmployeeDAO();
                dao.EditEmployee(employee);

                // to list table customer
                EmployeeTable.DataSource = dao.ListEmployee();

                // change page
                TabEmployee.SelectedTab = tabPage2;

                /// cleaning the inputs
                new Helpers().LimparTela(this);

                txtUf.SelectedIndex = -1;
                txtPosition.SelectedIndex = -1;
                txtAccessLevel.SelectedIndex = -1;

            }
            catch (ValidationException ex)
            {
                // Mensagem específica para erros de validação
                MessageBox.Show(ex.Message, "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception)
            {
                MessageBox.Show("Ocorreu um erro ao editar o cliente. Por favor, tente novamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Employee ObterClienteDoFormulario()
        {
            // Tentar converter e validar o Código do Cliente
            if (txtCodeEmployee.Text == null) MessageBox.Show("O Código do funcionario deve ser um número válido.");
            if (txtCpf.Text == null) MessageBox.Show("O CPF é obrigatorio.");
            if (txtEmail.Text == null) MessageBox.Show("O E-mail é obrigatorio.");
            if (txtPassword.Text == null) MessageBox.Show("A senha é obrigatoria.");
            if (txtPosition.Text == null) MessageBox.Show("O cargo é obrigatorio.");
            if (txtAccessLevel.Text == null) MessageBox.Show("O nível acesso é obrigatorio.");

            // Retornar o objeto Client preenchido
            return new Employee
            {
                Code = int.Parse(txtCodeEmployee.Text),
                Name = txtName.Text,
                Rg = txtRg.Text,
                Cpf = txtCpf.Text,
                Email = txtEmail.Text,
                Password = txtPassword.Text,
                Position = txtPosition.Text,
                AccessLevel = txtAccessLevel.Text,
                Telephone = txtTelephone.Text,
                Phone = txtPhone.Text,
                Cep = txtCep.Text,
                Address = txtAddress.Text,
                Number = int.Parse(txtNumberHome.Text),
                Complement = txtComplement.Text,
                Neighborhood = txtNeighborhood.Text,
                City = txtCity.Text,
                State = txtUf.Text
            };
        }

        #endregion

        #region DELETE EMPLOYEE
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Validação do campo de código do cliente
                if (string.IsNullOrEmpty(txtCodeEmployee.Text) || !int.TryParse(txtCodeEmployee.Text, out int EmployeeCode))
                {
                    MessageBox.Show("O código do funcionário deve está preenchido.");
                    return;
                }

                // deletar cliente 
                Employee obj = new Employee
                {
                    Code = EmployeeCode
                };

                EmployeeDAO dao = new EmployeeDAO();

                dao.DeleteEmployee(obj);

                // atualizar os dados do banco de dados 
                EmployeeTable.DataSource = dao.ListEmployee();

                TabEmployee.SelectedTab = tabPage2;

                /// cleaning the inputs
                new Helpers().LimparTela(this);
                txtUf.SelectedIndex = -1;
                txtPosition.SelectedIndex = -1;
                txtAccessLevel.SelectedIndex = -1;
            }
            catch (Exception)
            {
                MessageBox.Show("Verifica o indentificador do funcionário");
            }
        }
        #endregion

        #region SEARCH CEP IN API 
        private void btnCep_Click(object sender, EventArgs e)
        {
            try
            {
                string cep = txtCep.Text;

                // Sistema de carregamento
                btnCep.Text = "Carregando...";

                string xml = "https://viacep.com.br/ws/" + cep + "/xml/";

                DataSet dados = new DataSet();

                dados.ReadXml(xml);

                txtAddress.Text = dados.Tables[0].Rows[0]["logradouro"].ToString();
                txtNeighborhood.Text = dados.Tables[0].Rows[0]["bairro"].ToString();
                txtCity.Text = dados.Tables[0].Rows[0]["localidade"].ToString();
                txtUf.Text = dados.Tables[0].Rows[0]["uf"].ToString();

                btnCep.Text = "Pesquisar";
            }
            catch (Exception)
            {
                MessageBox.Show("Endereço não encontrado, por favor digite manualmente.");
            }
        }
        #endregion

        #region SEARCH EMPLOYEE
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtSearch.Text.Trim();
                EmployeeDAO dao = new EmployeeDAO();

                // Verifica se o campo de busca não está vazio
                if (string.IsNullOrEmpty(name))
                {
                    EmployeeTable.DataSource = dao.ListEmployee();
                    MessageBox.Show("Por favor, insira um nome para busca.");
                    return;
                }

                var searchResult = dao.SearchEmployeeByName(name);

                // Verifica se a busca retornou resultados
                if (searchResult != null && searchResult.Rows.Count > 0)
                {
                    EmployeeTable.DataSource = searchResult;
                }
                else
                {
                    // Exibe uma mensagem informando que a busca não teve resultados
                    MessageBox.Show("Nenhum cliente encontrado com esse nome. Exibindo todos os clientes.");

                    // Exibe todos os clientes caso a busca falhe
                    EmployeeTable.DataSource = dao.ListEmployee();
                }

            }
            catch (Exception ex)
            {
                // Mostra a mensagem de erro em caso de falha
                MessageBox.Show("Ocorreu um erro durante a busca: " + ex.Message);
            }
        }

        #endregion

        #region LIST EMPLOYEE SEARCH 
        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar))
            {
                string name = "%" + txtSearch.Text.Trim() + "%";

                EmployeeDAO dao = new EmployeeDAO();

                EmployeeTable.DataSource = dao.ListEmployeeByName(name); 
            }
        }
        #endregion
    }
}
