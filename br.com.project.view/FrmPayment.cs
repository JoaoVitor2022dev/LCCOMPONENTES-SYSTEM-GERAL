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
    public partial class FrmPayment : Form
    {

        #region VARIAVEIS DA VENDA

        Customer custumer = new Customer();
        DataTable shoppingCart = new DataTable();
        DateTime currentDate;

        #endregion

        public FrmPayment(Customer custumer, DataTable shoppingCart, DateTime currentDate)
        {
            InitializeComponent();
            this.custumer = custumer;
            this.shoppingCart = shoppingCart;
            this.currentDate = currentDate;
        }

        #region BTN PAYMENT 
        private void btnPayment_Click(object sender, EventArgs e)
        {
            try
            {
                // botão de finalizar 

                decimal sale_money, sale_cart, sale_change, totalPaymented, total;

                int stock_qty, purchased_qty, updated_stock;

                int initialValue = 0;

                txtCard.Text = initialValue.ToString();

                ProductDAO produtoADao = new ProductDAO();

                sale_money = decimal.Parse(txtMoney.Text);
                sale_cart = decimal.Parse(txtCard.Text);
                total = decimal.Parse(txtTotal.Text);

                // calcular o total pago 

                totalPaymented = sale_money + sale_cart;

                if (totalPaymented < total)
                {
                    MessageBox.Show("O total pago é menor que o valor total da venda");
                }
                else
                {
                    sale_change = totalPaymented - total;

                    Sale sale = new Sale();

                    sale.customer_id = custumer.Code;
                    sale.Date_sales = currentDate;
                    sale.Total_sales = total;
                    sale.observation = txtObservation.Text;

                    SaleDAO vdao = new SaleDAO();

                    texchangeMoney.Text = sale_change.ToString();

                    vdao.RegisterSales(sale);

          

                    // cadastrar item de venda 
                    foreach (DataRow linha in shoppingCart.Rows)
                    {
                        ItemSale Item = new ItemSale();

                        Item.Sales_Id = vdao.returnsLastIdSale();

                        // 1 - pegando o codigo de barra 
                        string barcode = linha["Codigo"].ToString();


                        // buscar o id do produto baseado no barcode
                        ProductDAO product = new ProductDAO();
                        Item.Product_id = product.ReturnsProductIdByBarCode(barcode);


                        // 2 - pegando a quantidade de item selecionado pelo usuario
                        Item.Qtd = int.Parse(linha["Qtd"].ToString());

                        Item.Subtotal = decimal.Parse(linha["Subtotal"].ToString());


                        // baixa de estoque 
                        stock_qty = produtoADao.ReturningCurrentStockProducts(Item.Product_id);

                        purchased_qty = Item.Qtd;

                        if (stock_qty == 0)
                        {
                            MessageBox.Show("Esse produto está zerado no nosso estoque!");
                            return;
                        }

                        if (purchased_qty > stock_qty)
                        {
                            MessageBox.Show("Não temos essa quatidade desse produto no estoque!");
                            return;
                        }

                        updated_stock = stock_qty - purchased_qty;

                        produtoADao.LowerStock(Item.Product_id, updated_stock);

                        ItemSaleDAO itemDAO = new ItemSaleDAO();

                        itemDAO.RegisteringSalesItem(Item);

                        this.Dispose();

                        MessageBox.Show("Venda registrada com sucesso.");
                    }


                    // contrutor de troco
                    FrmCashChange cashChange = new FrmCashChange(sale_change);

                    cashChange.ShowDialog();
                }
                new FrmSale().ShowDialog();
            }
            catch (Exception err)
            {
                MessageBox.Show("Erro: " + err.Message);
            }
        }
        #endregion

        #region EXIT PAYMENT
        private void button1_Click(object sender, EventArgs e)
        {
            FrmSale Tela_vendas = new FrmSale();
            this.Hide();
            Tela_vendas.ShowDialog();
            this.Close();
        }
        #endregion
    }
}
