using general_software_model.br.com.project.dao;
using general_software_model.br.com.project.model;
using MySqlX.XDevAPI;
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
    public partial class FrmSale : Form
    {
        #region VARIAVEIS DE VENDA 

        Customer custumer = new Customer();
        CustomerDAO cdao = new CustomerDAO();

        Product product = new Product();
        ProductDAO pdao = new ProductDAO();

        int qtd;
        decimal price;
        decimal subtotal, total;
        DataTable ShoppingCart = new DataTable();

        #endregion

        public FrmSale()
        {
            InitializeComponent();

            // CABEÇALHO DO DATAGRIDVIEW DO CARRINHO DE COMPRAS 
            ShoppingCart.Columns.Add("Codigo", typeof(string));
            ShoppingCart.Columns.Add("Produto", typeof(string));
            ShoppingCart.Columns.Add("Qtd", typeof(int));
            ShoppingCart.Columns.Add("Preço", typeof(decimal));
            ShoppingCart.Columns.Add("Subtotal", typeof(decimal));

            ProductTable.DataSource = ShoppingCart;
        }

        #region DATA ATUAL
        private void FrmSale_Load(object sender, EventArgs e)
        {
            txtTimeNow.Text = DateTime.Now.ToShortDateString();
        }
        #endregion

        #region SERACH CUSTUMER WITH CPF 
        private void txtCpf_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string cpf = txtCpf.Text.Trim();

                custumer = cdao.ReturnCustomersCPF(cpf);

                if (custumer != null)
                {
                    txtNameClient.Text = custumer.Name;
                }
                else
                {
                    MessageBox.Show("Cliente não encontrado. Verifique o CPF informado.");
                    txtCpf.Clear();
                    txtCpf.Focus();
                }
            }
        }
        #endregion

        #region SEARCH BARCODE
        private void txtCodeBar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string code = txtCodeBar.Text.Trim();

                product = pdao.ReturnsProductByBarCode(code);

                if (product != null)
                {
                    txtDescription.Text = product.Description;
                    txtPrice.Text = product.Price.ToString();
                }
                else
                {
                    MessageBox.Show("Produto não encontrado.");
                    txtCodeBar.Clear();
                    txtCodeBar.Focus();
                }
            }
        }
        #endregion

        #region ADICIONAR PRODUTO NO CARRIHO 
        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtStockQuantity.Text, out int qtd) || qtd <= 0)
                {
                    MessageBox.Show("Quantidade inválida. Insira um número inteiro positivo.");
                    txtStockQuantity.Focus();
                    return;
                }

                if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
                {
                    MessageBox.Show("Preço inválido. Insira um valor decimal positivo.");
                    txtPrice.Focus();
                    return;
                }

                string codeBar = txtCodeBar.Text.Trim();

                if (string.IsNullOrEmpty(codeBar))
                {
                    MessageBox.Show("Código de barra do produto é inválido.");
                    txtCodeBar.Focus();
                    return;
                }

                string description = txtDescription.Text.Trim();

                if (string.IsNullOrEmpty(description))
                {
                    MessageBox.Show("A descrição do produto não pode estar vazia.");
                    txtDescription.Focus();
                    return;
                }

                decimal subtotal = qtd * price;
                total += subtotal;

                ShoppingCart.Rows.Add(codeBar, description, qtd, price, subtotal);

                txtTotal.Text = total.ToString();

                txtCodeBar.Clear();
                txtDescription.Clear();
                txtStockQuantity.Clear();
                txtPrice.Clear();
                txtCodeBar.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro inesperado: {ex.Message}");
            }
        }
        #endregion

        #region REMOVER PRODUTO NO CARRiNHO
        private void btnRemoveProduct_Click(object sender, EventArgs e)
        {
            try
            {
                decimal subproduto = decimal.Parse(ProductTable.CurrentRow.Cells[4].Value.ToString());

                int index = ProductTable.CurrentRow.Index;

                DataRow linha = ShoppingCart.Rows[index];

                ShoppingCart.Rows.Remove(linha);

                ShoppingCart.AcceptChanges();

                total -= subproduto;

                txtTotal.Text = total.ToString();

                MessageBox.Show("Item Removido do carrinho com sucesso!");
            }
            catch (Exception err)
            {
                MessageBox.Show("Ocorreu um erro inesperado: " + err);
                throw;
            }
        }
        #endregion

        #region BTN PAYMENT
        private void btnPayment_Click(object sender, EventArgs e)
        {
            if (ShoppingCart.Rows.Count <= 0)
            {
                MessageBox.Show("Nenhum item selecionado para registrar venda.");
                return;
            }

            DateTime currentDate = DateTime.Parse(txtTimeNow.Text);

            FrmPayment TelaPaymanet = new FrmPayment(custumer, ShoppingCart, currentDate);

            TelaPaymanet.txtTotal.Text = total.ToString();

            this.Hide();

            TelaPaymanet.ShowDialog();

            this.Close();
        }

        #endregion

        #region Exit SALE'S PAGE
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion 
    }
}
