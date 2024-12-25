using general_software_model.br.com.project.model;
using general_software_model.br.com.projet.connection;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace general_software_model.br.com.project.dao
{
    internal class ItemSaleDAO
    {
        private MySqlConnection Connection;
        public ItemSaleDAO()
        {
            this.Connection = new ConnectionFactory().GetConnection();
        }

        #region REGISTER SALES ITEM
        public void RegisteringSalesItem(ItemSale obj)
        {
            try
            {
                string sql = @"INSERT INTO tb_sales_items (sale_id, product_id, quantity,subtotal)
                               VALUES (@sale_id, @product_id, @quantity, @subtotal)";

                using (MySqlCommand executacmd = new MySqlCommand(sql, Connection))
                {
                    executacmd.Parameters.AddWithValue("@sale_id", obj.Sales_Id);
                    executacmd.Parameters.AddWithValue("@product_id", obj.Product_id);
                    executacmd.Parameters.AddWithValue("@quantity", obj.Qtd);
                    executacmd.Parameters.AddWithValue("@subtotal", obj.Subtotal);

                    Connection.Open();
                    executacmd.ExecuteNonQuery();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show($"Erro ao executar o comando SQL: {err}");
            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }
            }

        }
        #endregion

        #region REGISTER LIST SALE ITEM
        public DataTable ListAllItemsForSale(int tb_sales_items)
        {

            DataTable TabelaItens = new DataTable();

            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                string sql = @"SELECT tb_sales_items.id AS 'Código',
                                      tb_products.description AS 'Nome do Produto', 
                                      tb_sales_items.quantity AS 'QTDE', 
                                      tb_products.price AS 'Preço',
                                      tb_sales_items.subtotal
                               FROM 
                                      tb_sales_items 
                               JOIN 
                                      tb_products 
                                 ON 
                                      tb_sales_items.product_id = tb_products.id
                              WHERE 
                                      tb_sales_items.sale_id = @tb_sales_items;";


                using (MySqlCommand executecmdsql = new MySqlCommand(sql, Connection))
                {
                    executecmdsql.Parameters.AddWithValue("@tb_sales_items", tb_sales_items);

                    using (MySqlDataAdapter da = new MySqlDataAdapter(executecmdsql))
                    {
                        da.Fill(TabelaItens);
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show($"Erro ao executar o comando SQL: {err}");
                return null;
            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }
            }

            return TabelaItens;
        }
        #endregion
    }
}
