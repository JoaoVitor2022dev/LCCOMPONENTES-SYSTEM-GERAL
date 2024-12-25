using general_software_model.br.com.projet.connection;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace general_software_model.br.com.project.model
{
    internal class SaleDAO
    {
        private MySqlConnection connection;
        public SaleDAO()
        {
            this.connection = new ConnectionFactory().GetConnection();
        }
        #region REGISTER SALES
        public void RegisterSales(Sale obj)
        {
            try
            {
                string sql = @" INSERT INTO tb_sales (customer_id, sale_date, total_sale, notes)
                                VALUES (@customer_id,@sale_date,@total_sale,@notes)";

                using (MySqlCommand executacmd = new MySqlCommand(sql, connection))
                {
                    executacmd.Parameters.AddWithValue("@customer_id", obj.customer_id);
                    executacmd.Parameters.AddWithValue("@sale_date", obj.Date_sales);
                    executacmd.Parameters.AddWithValue("@total_sale", obj.Total_sales);
                    executacmd.Parameters.AddWithValue("@notes", obj.observation);

                    connection.Open();
                    executacmd.ExecuteNonQuery();
                    connection.Clone();
                }
            }
            catch (MySqlException dbErro)
            {
                MessageBox.Show("Erro ao cadastrar cliente: " + dbErro.Message);
            }
        }
        #endregion

        #region RETURN ID LAST SALES
        public int returnsLastIdSale()
        {
            int idVenda = 0;
            string sql = @"SELECT MAX(id) id FROM tb_sales";

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                using (MySqlCommand executecmdsql = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = executecmdsql.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            idVenda = reader.GetInt32(0);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Ocorreu um erro: " + err);
                return 0;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return idVenda;
        }
        #endregion

        #region LIST SALES
        public DataTable ListSales()
        {
            DataTable HistoryTable = new DataTable();

            try
            {
                string sql = @"SELECT tb_sales.id as 'Código',
                               tb_sales.sale_date as 'Data da Venda',
                               tb_customer.name as 'Cliente',
                               tb_sales.total_sale as 'Total',
                               tb_sales.notes as 'Descrição'
                               FROM tb_sales JOIN tb_customer ON (tb_sales.customer_id = tb_customer.id)
                               ";

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                using (MySqlCommand executecmdsql = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(executecmdsql))
                    {
                        da.Fill(HistoryTable);
                    }
                }
            }
            catch (MySqlException dbbErr)
            {
                MessageBox.Show($"Erro ao executar o comando SQL: {dbbErr}");
                return null;
            }
            catch (Exception err)
            {
                MessageBox.Show($"Ocorreu um erro:  {err}");
                return null;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return HistoryTable;
        }
        #endregion

        #region LIST SALES PER PERIODS
        public DataTable ListSalesPerPeriods(DateTime startDate, DateTime endDate)
        {
            DataTable TabelaHistorico = new DataTable();

            try
            {
                string sql = @" SELECT tb_sales.id as 'Código',
                                tb_sales.sale_date as 'Data da venda',
                                tb_customer.name as 'Cliente',
                                tb_sales.total_sale as 'Total',
                                tb_sales.notes as 'Descrição' 
                                FROM tb_sales JOIN tb_customer ON (tb_sales.customer_id = tb_customer.id)

                                WHERE tb_sales.sale_date BETWEEN @startDate AND @endDate;";

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                using (MySqlCommand executecmdsql = new MySqlCommand(sql, connection))
                {
                    executecmdsql.Parameters.AddWithValue("@startDate", startDate);
                    executecmdsql.Parameters.AddWithValue("@endDate", endDate);

                    using (MySqlDataAdapter da = new MySqlDataAdapter(executecmdsql))
                    {
                        da.Fill(TabelaHistorico);
                    }
                }
            }
            catch (MySqlException dbbErr)
            {
                MessageBox.Show($"Erro ao executar o comando SQL: {dbbErr}");
                return null;
            }
            catch (Exception err)
            {
                MessageBox.Show($"Ocorreu um erro:  {err}");
                return null;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return TabelaHistorico;
        }
        #endregion


    }
}
