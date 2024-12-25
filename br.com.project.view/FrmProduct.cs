using general_software_model.br.com.project.dao;
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
    public partial class FrmProduct : Form
    {
        public FrmProduct()
        {
            InitializeComponent();
        }
        #region LOAD
        private void FrmProduct_Load(object sender, EventArgs e)
        {
                SupplierDAO supplier = new SupplierDAO();

                cbSupplier.DataSource = supplier.ListSupplier();
                cbSupplier.DisplayMember = "Nome";
                cbSupplier.ValueMember = "id";

                ProductDAO dao = new ProductDAO();

                cbSupplier.SelectedIndex = -1;

                ProductTable.DataSource = dao.listProducts();
        }
        #endregion

        #region REGISTER PRODUCT
        private void BtnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtDescription.Text) ||
                    string.IsNullOrWhiteSpace(txtPrice.Text) ||
                    string.IsNullOrWhiteSpace(txtColunmProduct.Text) ||
                     string.IsNullOrWhiteSpace(txtStockQuantity.Text) ||
                     string.IsNullOrWhiteSpace(txtBarCode.Text) ||
                    cbSupplier.SelectedIndex == -1)
                {
                    MessageBox.Show("Todos os campos devem ser preenchidos!");
                    return;
                }

                if (txtCodeProduct.Text != "")
                {
                    MessageBox.Show("Já existe um Produto com esse Id");
                    return;
                }

                Product product = new Product
                {
                    Description = txtDescription.Text,
                    Price = decimal.Parse(txtPrice.Text),
                    Column = txtColunmProduct.Text,
                    StockQuantity = int.Parse(txtStockQuantity.Text),
                    Barcode = txtBarCode.Text,
                    Supplier_id = int.Parse(cbSupplier.SelectedValue.ToString())
                };

                ProductDAO dao = new ProductDAO();
                dao.RegisterProduct(product);

                new Helpers().LimparTela(this);

                ProductTable.DataSource = dao.listProducts();

                cbSupplier.SelectedIndex = -1;
                TabPorduct.SelectedTab = tabPage2;
            }
            catch (FormatException)
            {
                MessageBox.Show("Preencha todos os campos corretamente com valores válidos!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro: {ex.Message}");
            }
        }
        #endregion

        #region BTN CLEAN INPUT FORM
        private void BtnNew_Click(object sender, EventArgs e)
        {
            new Helpers().LimparTela(this);
            cbSupplier.SelectedIndex = -1;
        }

        #endregion

        #region BTN EDIT    
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtDescription.Text) ||
                    string.IsNullOrWhiteSpace(txtPrice.Text) ||
                    string.IsNullOrWhiteSpace(txtColunmProduct.Text) ||
                    string.IsNullOrWhiteSpace(txtStockQuantity.Text) ||
                    string.IsNullOrWhiteSpace(txtBarCode.Text) ||
                    string.IsNullOrWhiteSpace(cbSupplier.Text) ||
                   cbSupplier.SelectedIndex == -1)
                {
                    MessageBox.Show("Todos os campos devem ser preenchidos!");
                    return;
                }

                Product product = new Product()
                {
                    Description = txtDescription.Text,
                    Barcode = txtBarCode.Text,
                    Price = decimal.Parse(txtPrice.Text),
                    Column = txtColunmProduct.Text,
                    StockQuantity = int.Parse(txtStockQuantity.Text),
                    Supplier_id = int.Parse(cbSupplier.SelectedValue.ToString()),
                    Id = int.Parse(txtCodeProduct.Text)
                };

                ProductDAO dao = new ProductDAO();
                dao.EdictProduct(product);

                new Helpers().LimparTela(this);

                ProductTable.DataSource = dao.listProducts();
                TabPorduct.SelectedTab = tabPage2;
            }
            catch (FormatException)
            {
                MessageBox.Show("Preencha todos os campos corretamente com valores válidos!");
            }
            catch (Exception)
            {
                MessageBox.Show("Verifique se todos os campos estão preenchidos.");
            }         
        }
        private void ProductTable_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (ProductTable.CurrentRow != null && ProductTable.CurrentRow.Index >= 0)
                {
                    FillProductDetailsFromTable(ProductTable.CurrentRow);
                    TabPorduct.SelectedTab = tabPage1;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void FillProductDetailsFromTable(DataGridViewRow row)
        {
            txtCodeProduct.Text = row.Cells[0].Value?.ToString() ?? string.Empty;
            txtDescription.Text = row.Cells[1].Value?.ToString() ?? string.Empty;
            txtBarCode.Text = row.Cells[2].Value?.ToString() ?? string.Empty;
            txtPrice.Text = row.Cells[3].Value?.ToString() ?? string.Empty;
            txtColunmProduct.Text = row.Cells[4].Value?.ToString() ?? string.Empty;
            txtStockQuantity.Text = row.Cells[5].Value?.ToString() ?? string.Empty;
            cbSupplier.Text = row.Cells[6].Value?.ToString() ?? string.Empty;
        }

        #endregion

        #region DELETE PRODUCT 
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCodeProduct.Text) || !int.TryParse(txtCodeProduct.Text, out int productCode))
                {
                    MessageBox.Show("O código do cliente deve ser preenchido e numérico.");
                    return;
                }

                Product product = new Product()
                {
                    Id = productCode
                };

                ProductDAO dao = new ProductDAO();

                dao.DeleteProduct(product);

                new Helpers().LimparTela(this);

                ProductTable.DataSource = dao.listProducts();

                TabPorduct.SelectedTab = tabPage2;
            }
            catch (FormatException)
            {
                MessageBox.Show("Formato inválido. Verifique se o código do produto é numérico.");
            }
            catch (Exception ex)
            {
                // Exibe mensagem de erro com mais informações
                MessageBox.Show($"Ocorreu um erro: {ex.Message}");
            }
        }
        #endregion

        #region SEARCH PRODUCT
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string name = txtSearch.Text.Trim();
            ProductDAO dao = new ProductDAO();

            // Verifica se o campo de busca não está vazio
            if (string.IsNullOrEmpty(name))
            {
                ProductTable.DataSource = dao.listProducts();
                MessageBox.Show("Por favor, insira um nome para busca.");
                return;
            }

            var searchResult = dao.SearchProductsByName(name);

            if (searchResult != null && searchResult.Rows.Count > 0)
            {
                ProductTable.DataSource = searchResult;
            }
            else
            {
                MessageBox.Show("Nenhum cliente encontrado com esse nome. Exibindo todos os clientes.");

                ProductTable.DataSource = dao.listProducts();
            }
        }

        #endregion

        #region LIST PRODUCT 
        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar))
            {
                string name = "%" + txtSearch.Text.Trim() + "%";

                ProductDAO dao = new ProductDAO();

                ProductTable.DataSource = dao.ListProductsByName(name);
            }
        }
        #endregion
    }
}
