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
    public partial class FrmSupplier : Form
    {
        public FrmSupplier()
        {
            InitializeComponent();
        }
        #region Register Supplier 
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateFields())
                {
                    MessageBox.Show("Por favor, preencha as informações importante de um fornecedor");
                    return;
                }

                if (txtCodeSupplier.Text != "")
                {
                    MessageBox.Show("Já existe um fornecedor com esse ID");
                    return;
                }

                Supplier supplier = CreateSupplier();

                SupplierDAO dao = new SupplierDAO();

                dao.RegisterSupplier(supplier);

                SupplierTable.DataSource = dao.ListSupplier();

                TabSupplier.SelectedTab = tabPage2;
                new Helpers().LimparTela(this);
                txtUf.SelectedIndex = -1;
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Erro ao preencher as informações: " + ex.Message);
            }
            catch (Exception)
            {
                MessageBox.Show("Todos os campos tem que ser preenchidos.");
            }
        }

        private bool ValidateFields()
        {
            // Example of simple validations
            if (string.IsNullOrWhiteSpace(txtNameClient.Text) ||
                string.IsNullOrWhiteSpace(txtCnpj.Text) ||
                string.IsNullOrWhiteSpace(txtEmailClient.Text) ||
                string.IsNullOrWhiteSpace(txtCep.Text))
            {
                return false;
            }
            return true;
        }

        private Supplier CreateSupplier()
        {
            return new Supplier
            {
                Name = txtNameClient.Text,
                Cnpj = txtCnpj.Text,
                Email = txtEmailClient.Text,
                Telephone = txtTelephoneClient.Text,
                Phone = txtPhoneClient.Text,
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

        #region  lIST SUPPLIER
        private void FrmSupplier_Load(object sender, EventArgs e)
        {
            SupplierTable.DefaultCellStyle.ForeColor = Color.Black;
            SupplierDAO dao = new SupplierDAO();
            SupplierTable.DataSource = dao.ListSupplier();
        }

        #endregion

        #region CLEAR INPUTS
        private void BtnNew_Click(object sender, EventArgs e)
        {
            new Helpers().LimparTela(this);
            txtUf.SelectedIndex = -1;
        }
        #endregion

        #region EDIT SUPPLIER
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                // method of storing data in the model

                Supplier supplier = ObterSupplierDoFormulario();

                //invoking the register method
                SupplierDAO dao = new SupplierDAO();
                dao.EditSupplier(supplier);

                // change page
                TabSupplier.SelectedTab = tabPage2;

                // to list table customer
                SupplierTable.DataSource = dao.ListSupplier();

                /// cleaning the inputs
                new Helpers().LimparTela(this);
                txtUf.SelectedIndex = -1;

            }
            catch (ValidationException ex)
            {
                MessageBox.Show(ex.Message, "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception)
            {
                MessageBox.Show("Todos os campos devem ser preenchidos");
            }
        }

        private Supplier ObterSupplierDoFormulario()
        {
            // Tentar converter e validar o Código do Cliente
            if (txtCodeSupplier.Text == null) MessageBox.Show("O Código do fornecedor deve ser um número válido.");
            if (txtCnpj.Text == null) MessageBox.Show("O CPF é obrigatorio.");
            if (txtEmailClient.Text == null) MessageBox.Show("O E-mail é obrigatorio.");
            if (txtNameClient.Text == null) MessageBox.Show("O é obrigatorio.");

            // Retornar o objeto Client preenchido
            return new Supplier
            {
                Code = int.Parse(txtCodeSupplier.Text),
                Name = txtNameClient.Text,
                Cnpj = txtCnpj.Text,
                Email = txtEmailClient.Text,
                Telephone = txtTelephoneClient.Text,
                Phone = txtPhoneClient.Text,
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

        #region TRANSFER DATA FROM DATA GRIDVIEW TO THE FORM
        private void SupplierTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the selected row is valid
            if (SupplierTable.CurrentRow != null && SupplierTable.CurrentRow.Index >= 0)
            {
                FillDetailsFromTable(SupplierTable.CurrentRow);
                TabSupplier.SelectedTab = tabPage1;
            }
            //Update the phone number on the consultation page with the values from the input fields
            TabSupplier.SelectedTab = tabPage1;
        }

        private void FillDetailsFromTable(DataGridViewRow row)
        {
            // Assign the values from the table to the corresponding TextBoxes
            txtCodeSupplier.Text = SupplierTable.CurrentRow.Cells[0].Value.ToString();
            txtNameClient.Text = SupplierTable.CurrentRow.Cells[1].Value.ToString();
            txtCnpj.Text = SupplierTable.CurrentRow.Cells[2].Value.ToString();
            txtEmailClient.Text = SupplierTable.CurrentRow.Cells[3].Value.ToString();
            txtTelephoneClient.Text = SupplierTable.CurrentRow.Cells[4].Value.ToString();
            txtPhoneClient.Text = SupplierTable.CurrentRow.Cells[5].Value.ToString();
            txtCep.Text = SupplierTable.CurrentRow.Cells[6].Value.ToString();
            txtAddress.Text = SupplierTable.CurrentRow.Cells[7].Value.ToString();
            txtNumberHome.Text = SupplierTable.CurrentRow.Cells[8].Value.ToString();
            txtComplement.Text = SupplierTable.CurrentRow.Cells[9].Value.ToString();
            txtNeighborhood.Text = SupplierTable.CurrentRow.Cells[10].Value.ToString();
            txtCity.Text = SupplierTable.CurrentRow.Cells[11].Value.ToString();
            txtUf.Text = SupplierTable.CurrentRow.Cells[12].Value.ToString();
        }

        #endregion

        #region DELETE SUPPLIER
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Validação do campo de código do cliente
                if (string.IsNullOrEmpty(txtCodeSupplier.Text) || !int.TryParse(txtCodeSupplier.Text, out int supplierCode))
                {
                    MessageBox.Show("O código do fornecedor deve ser preenchido e numérico.");
                    return;
                }

                Supplier obj = new Supplier
                {
                    Code = supplierCode
                };

                SupplierDAO dao = new SupplierDAO();

                dao.DeleteSupplier(obj);

                SupplierTable.DataSource = dao.ListSupplier();

                TabSupplier.SelectedTab = tabPage2;

                new Helpers().LimparTela(this);
                txtUf.SelectedIndex = -1;
            }
            catch (Exception)
            {
                MessageBox.Show("Todos os campos devem ser preenchidos");
            }
        }
        #endregion

        #region SEARCH CEP IN API-CEP
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

        #region SERACH NAME SUPPLIER    
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtSearch.Text.Trim(); // Remove espaços em branco

                SupplierDAO dao = new SupplierDAO();

                // Verifica se o campo de busca não está vazio
                if (string.IsNullOrEmpty(name))
                {
                    MessageBox.Show("Por favor, insira um nome para busca.");
                    SupplierTable.DataSource = dao.ListSupplier();
                    return;
                }

                // Tenta buscar o cliente pelo nome
                var searchResult = dao.SearchSupplierByName(name);

                // Verifica se a busca retornou resultados
                if (searchResult != null && searchResult.Rows.Count > 0)
                {
                    SupplierTable.DataSource = searchResult;
                }
                else
                {
                    // Exibe uma mensagem informando que a busca não teve resultados
                    MessageBox.Show("Nenhum fornecedor encontrado com esse nome.");

                    // Exibe todos os clientes caso a busca falhe
                    SupplierTable.DataSource = dao.ListSupplier();
                }
            }
            catch (Exception ex)
            {
                // Mostra a mensagem de erro em caso de falha
                MessageBox.Show("Ocorreu um erro durante a busca: " + ex.Message);
            }
        }

        #endregion

        #region LIST SEARCH SUPPLIER 
        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar))
            {
                string name = "%" + txtSearch.Text.Trim() + "%";

                SupplierDAO dao = new SupplierDAO();

                SupplierTable.DataSource = dao.ListSupplierByName(name);
            }
        }
        #endregion
    }
}
