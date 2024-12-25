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
    public partial class FrmCustomer : Form
    {
        public FrmCustomer()
        {
            InitializeComponent();
        }

        #region REGISTER CUSTUMER 
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateFields())
                {
                    MessageBox.Show("Por favor, preencha as informações do cpf e nome e número da casa!");
                    return;
                    ;
                }

                if (txtCodeClient.Text != "")
                {
                    MessageBox.Show("Já existe um fornecedor com esse ID");
                    return;
                }


                Customer customer = CreateCustomer();

                CustomerDAO dao = new CustomerDAO();

                dao.RegisterCustomer(customer);
                CustomerTable.DataSource = dao.ListCustomers(); 

                // Switch tab and clear fields
                TabClient.SelectedTab = tabPage2;
                new Helpers().LimparTela(this);
                txtUf.SelectedIndex = -1;
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Erro ao preencher as informações: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro no sistema: " + ex.Message);
            }
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtNameClient.Text) ||
                string.IsNullOrWhiteSpace(txtCpf.Text) ||
                string.IsNullOrWhiteSpace(txtRg.Text) ||
                !int.TryParse(txtNumberHome.Text, out _))
            {
                return false;
            }
            return true;
        }

        private Customer CreateCustomer()
        {
            return new Customer
            {
                Name = txtNameClient.Text,
                Rg = txtRg.Text,
                Cpf = txtCpf.Text,
                Email = txtEmailClient.Text,
                Telephone = txtTelephoneClient.Text,
                Phone = txtPhoneClient.Text,
                Zip_code = txtCep.Text,
                Address = txtAddress.Text,
                Number = int.TryParse(txtNumberHome.Text, out int number) ? number : 0,
                Complement = txtComplement.Text,
                Neighborhood = txtNeighborhood.Text,
                City = txtCity.Text,
                State = txtUf.Text
            };
        }
        #endregion

        #region  CLEAN THE INPUTS
        private void BtnNew_Click(object sender, EventArgs e)
        {
            new Helpers().LimparTela(this);
            txtUf.SelectedIndex = -1;
        }

        #endregion

        #region LISTER CUSTUMER
        private void FrmCustomer_Load(object sender, EventArgs e)
        {
            CustomerTable.DefaultCellStyle.ForeColor = Color.Black;

            CustomerDAO dao = new CustomerDAO();
            CustomerTable.DataSource = dao.ListCustomers();
        }
        #endregion

        #region TRANSFER TABLE DATA TO FORM TO EDIT OBJECT
        private void CustomerTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Ensure the selected row is valid
                if (CustomerTable.CurrentRow != null && CustomerTable.CurrentRow.Index >= 0)
                {
                    FillClientDetailsFromTable(CustomerTable.CurrentRow);
                    TabClient.SelectedTab = tabPage1;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void FillClientDetailsFromTable(DataGridViewRow row)
        {
            try
            {
                // Assign the values from the table to the corresponding TextBoxes
                txtCodeClient.Text = row.Cells[0].Value?.ToString() ?? string.Empty;
                txtNameClient.Text = row.Cells[1].Value?.ToString() ?? string.Empty;
                txtRg.Text = row.Cells[2].Value?.ToString() ?? string.Empty;
                txtCpf.Text = row.Cells[3].Value?.ToString() ?? string.Empty;
                txtEmailClient.Text = row.Cells[4].Value?.ToString() ?? string.Empty;
                txtTelephoneClient.Text = row.Cells[5].Value?.ToString() ?? string.Empty;
                txtPhoneClient.Text = row.Cells[6].Value?.ToString() ?? string.Empty;
                txtCep.Text = row.Cells[7].Value?.ToString() ?? string.Empty;
                txtAddress.Text = row.Cells[8].Value?.ToString() ?? string.Empty;
                txtNumberHome.Text = row.Cells[9].Value?.ToString() ?? string.Empty;
                txtComplement.Text = row.Cells[10].Value?.ToString() ?? string.Empty;
                txtNeighborhood.Text = row.Cells[11].Value?.ToString() ?? string.Empty;
                txtCity.Text = row.Cells[12].Value?.ToString() ?? string.Empty;
                txtUf.Text = row.Cells[13].Value?.ToString() ?? string.Empty;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        #endregion

        #region BTN CLEAR INPUTS
        private void BtnNew_Click_1(object sender, EventArgs e)
        {
            // Clear the inputs
            new Helpers().LimparTela(this);
            txtUf.SelectedIndex = -1;
        }


        #endregion

        #region EDIT CUSTUMER 
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCodeClient.Text == "")
                {
                    MessageBox.Show("Nenhum cliente está selecionado para editar as informações.");
                    return;
                }

                // Obter e validar os dados do formulário
                Customer client = ObterClienteDoFormulario();

                CustomerDAO dao = new CustomerDAO();
                dao.ChangeCustomer(client);

                // Atualizar a tabela de clientes após a alteração
                CustomerTable.DataSource = dao.ListCustomers();

                // Alterar a aba
                TabClient.SelectedTab = tabPage2;

                // Limpar os campos do formulário
                new Helpers().LimparTela(this);
                txtUf.SelectedIndex = -1;
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
        private Customer ObterClienteDoFormulario()
        {
            // Tentar converter e validar o Código do Cliente
            if (txtCodeClient.Text == null) MessageBox.Show("O Código do Cliente deve ser um número válido.");
            if (txtCpf.Text == null) MessageBox.Show("O CPF é obrigatorio.");
            if (txtEmailClient.Text == null) MessageBox.Show("O E-mail é obrigatorio.");
            if (txtNameClient.Text == null) MessageBox.Show("O é obrigatorio.");

            // Retornar o objeto Client preenchido
            return new Customer
            {
                Code = int.Parse(txtCodeClient.Text),
                Name = txtNameClient.Text,
                Rg = txtRg.Text,
                Cpf = txtCpf.Text,
                Email = txtEmailClient.Text,
                Telephone = txtTelephoneClient.Text,
                Phone = txtPhoneClient.Text,
                Zip_code = txtCep.Text,
                Address = txtAddress.Text,
                Number = int.Parse(txtNumberHome.Text),
                Complement = txtComplement.Text,
                Neighborhood = txtNeighborhood.Text,
                City = txtCity.Text,
                State = txtUf.Text
            };
        }



        #endregion

        #region DELETE CUSTUMER
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCodeClient.Text) || !int.TryParse(txtCodeClient.Text, out int clientCode))
                {
                    MessageBox.Show("O código do cliente deve ser preenchido e numérico.");
                    return;
                }

                Customer obj = new Customer
                {
                    Code = clientCode
                };

                CustomerDAO dao = new CustomerDAO();

                dao.DeleteCustomer(obj);

                CustomerTable.DataSource = dao.ListCustomers();

                TabClient.SelectedTab = tabPage2;

                new Helpers().LimparTela(this);
                txtUf.SelectedIndex = -1;
            }
            catch (FormatException)
            {
                MessageBox.Show("Formato inválido. Verifique se o código do cliente é numérico.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro: {ex.Message}");
            }
        }
        #endregion

        #region SEARCH CEP
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

                btnCep.Text = "Pesquisa";

            }
            catch (Exception)
            {
                MessageBox.Show("Endereço não encontrado, por favor digite manualmente.");
            }
        }
        #endregion

        #region BTN SEARCH
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtSearch.Text.Trim(); // Remove espaços em branco

                CustomerDAO dao = new CustomerDAO();

                // Verifica se o campo de busca não está vazio
                if (string.IsNullOrEmpty(name))
                {
                    CustomerTable.DataSource = dao.ListCustomers();
                    MessageBox.Show("Por favor, insira um nome para busca.");
                    return;
                }

                // Tenta buscar o cliente pelo nome
                var searchResult = dao.SearchCustomerByName(name);

                // Verifica se a busca retornou resultados
                if (searchResult != null && searchResult.Rows.Count > 0)
                {
                    CustomerTable.DataSource = searchResult;
                }
                else
                {
                    // Exibe uma mensagem informando que a busca não teve resultados
                    MessageBox.Show("Nenhum cliente encontrado com esse nome. Exibindo todos os clientes.");

                    // Exibe todos os clientes caso a busca falhe
                    CustomerTable.DataSource = dao.ListCustomers();
                }
            }
            catch (Exception ex)
            {
                // Mostra a mensagem de erro em caso de falha
                MessageBox.Show("Ocorreu um erro durante a busca: " + ex.Message);
            }
        }
        #endregion

        #region SEARCH LISTER CUSTUMER
        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verifica se a tecla pressionada é válida (não caractere de controle como Enter)
            if (!char.IsControl(e.KeyChar))
            {
                // Cria o padrão de busca usando o texto digitado
                string name = "%" + txtSearch.Text.Trim() + "%";

                // Instancia o objeto de acesso ao banco de dados
                CustomerDAO dao = new CustomerDAO();

                // Atualiza a tabela de clientes com os resultados da busca
                CustomerTable.DataSource = dao.ListCustomerByName(name);
            }
        }
        #endregion
    }
}
