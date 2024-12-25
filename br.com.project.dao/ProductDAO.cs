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
    internal class ProductDAO
    {
        private MySqlConnection connection;
        public ProductDAO()
        {
            this.connection = new ConnectionFactory().GetConnection();
        }
        #region Register Product 
        public void RegisterProduct(Product obj)
        {
            if (obj == null)
            {
                MessageBox.Show("Dados do produto não podem ser nulos.");
                return;
            }

            try
            {
                string sql = "INSERT INTO tb_products (description, barcode, price, column_, stock_qty, supplier_id ) VALUES (@description, @barcode, @price, @colunm, @stock_qty , @supplier_id);";

                using (MySqlCommand executacmd = new MySqlCommand(sql, connection))
                {
                    executacmd.Parameters.AddWithValue("@description", obj.Description);
                    executacmd.Parameters.AddWithValue("@barcode", obj.Barcode);
                    executacmd.Parameters.AddWithValue("@price", obj.Price);
                    executacmd.Parameters.AddWithValue("@colunm", obj.Column);
                    executacmd.Parameters.AddWithValue("@stock_qty", obj.StockQuantity);
                    executacmd.Parameters.AddWithValue("@supplier_id", obj.Supplier_id);

                    connection.Open();
                    executacmd.ExecuteNonQuery();
                }

                MessageBox.Show("Produto cadastrado com sucesso!");
            }
            catch (MySqlException dbErro)
            {
                MessageBox.Show($"Erro ao cadastrar produto: {dbErro.Message}");
            }
            catch (Exception erro)
            {
                MessageBox.Show($"Aconteceu um erro {erro}");
            }
            finally
            {
                // Fechando a conexão no finally, caso esteja aberta
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        #endregion

        #region list products
        public DataTable listProducts()
        {
            DataTable ProductTable = new DataTable();

            try
            {
                string sql = @"SELECT tb_products.id AS ""Código"", tb_products.description AS ""Descrição"", tb_products.barcode AS ""Código de Barra"",  tb_products.price AS ""Preço"", tb_products.column_ AS ""Coluna"",tb_products.stock_qty AS ""Qtd Estoque"", tb_suppliers.name AS ""Fornecedor""
                               FROM 
                               tb_products
                               JOIN 
                               tb_suppliers ON tb_products.supplier_id = tb_suppliers.id;";


                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                using (MySqlCommand executacmd = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(executacmd))
                    {
                        dataAdapter.Fill(ProductTable);
                    }
                }
            }
            catch (MySqlException dbErro)
            {
                MessageBox.Show($"Erro ao Listar produtos: {dbErro.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao metodo de listar productos: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                // Fechar a conexão se necessário
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return ProductTable;
        }
        #endregion

        #region Edit Product
        public void EdictProduct(Product obj)
        {
            try
            {
                if (obj == null)
                {
                    MessageBox.Show("Os dados do produto não pode ser nullo!");
                    return;
                }

                string sql = "UPDATE tb_products SET description = @description, barcode = @barcode, price = @price, column_ = @colunm ,stock_qty = @stock_qty, supplier_id = @supplier_id WHERE id = @id";


                using (MySqlCommand executacmd = new MySqlCommand(sql, connection))
                {
                    executacmd.Parameters.AddWithValue("@description", obj.Description);
                    executacmd.Parameters.AddWithValue("@price", obj.Price);
                    executacmd.Parameters.AddWithValue("@barcode", obj.Barcode);
                    executacmd.Parameters.AddWithValue("@colunm", obj.Column);
                    executacmd.Parameters.AddWithValue("@stock_qty", obj.StockQuantity);
                    executacmd.Parameters.AddWithValue("@supplier_id", obj.Supplier_id);
                    executacmd.Parameters.AddWithValue("@id", obj.Id);

                    connection.Open();
                    executacmd.ExecuteNonQuery();
                }

                MessageBox.Show("Produto Alterado com sucesso!");
            }
            catch (MySqlException dbErro)
            {
                MessageBox.Show("Erro ao editar as informações do produto: " + dbErro.Message);
            }
            catch (Exception erro)
            {
                MessageBox.Show("Aconteceu um erro " + erro);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        #endregion

        #region Delete Product 
        public void DeleteProduct(Product obj)
        {

            if (obj == null)
            {
                MessageBox.Show("O Id do Produto não pode ser nullo!");
                return;
            }

            try
            {
                string sql = "DELETE FROM tb_products WHERE id = @id";

                using (MySqlCommand executacmd = new MySqlCommand(sql, connection))
                {
                    executacmd.Parameters.AddWithValue("@id", obj.Id);

                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    executacmd.ExecuteNonQuery();

                    MessageBox.Show("Produto Deletado com sucesso!");
                }
            }
            catch (MySqlException dbErro)
            {
                MessageBox.Show("Erro ao cadastrar produto: " + dbErro.Message);
            }
            catch (Exception erro)
            {
                MessageBox.Show("Aconteceu um erro " + erro);
            }
        }
        #endregion

        #region Search Products By Name
        public DataTable SearchProductsByName(string name)
        {
            DataTable producTable = new DataTable();

            try
            {
                string sql = @"SELECT 
                            tb_products.id AS ""Código"", 
                            tb_products.description AS ""Descrição"",
                            tb_products.barcode AS ""Código de Barra"",
                            tb_products.stock_qty AS ""Qtd Estoque"", 
                            tb_products.price AS ""Preço"",
                            tb_suppliers.name AS ""Fornecedor""
                            FROM 
                            tb_products
                            JOIN 
                            tb_suppliers ON tb_products.supplier_id = tb_suppliers.id  
                            WHERE tb_products.description = @name;";

                using (MySqlCommand executacmd = new MySqlCommand(sql, connection))
                {
                    executacmd.Parameters.AddWithValue("@name", name);

                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(executacmd))
                    {
                        dataAdapter.Fill(producTable);
                    }
                }
            }
            catch (MySqlException dbErro)
            {
                MessageBox.Show("Erro ao listar os Product: " + dbErro.Message);
            }
            catch (Exception error)
            {
                MessageBox.Show("Error ao executar o comando sql: " + error);
                return null;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return producTable;
        }
        #endregion

        #region List Products By Name
        public DataTable ListProductsByName(string name)
        {
            DataTable ProductTable = new DataTable();

            try
            {
                string sql = @"SELECT tb_products.id AS ""Id"",
                                      tb_products.description AS ""Descrição"", 
                                      tb_products.barcode AS ""Código-Barra"", 
                                      tb_products.price AS ""Preço"", 
                                      tb_products.stock_qty ""Qtd"", 
                                      tb_suppliers.name ""Fornecedor""
                                      FROM
                                      tb_products JOIN tb_suppliers ON tb_products.supplier_id = tb_suppliers.id  
                                      WHERE 
                                      tb_products.description LIKE @description;";

                using (MySqlCommand executacmd = new MySqlCommand(sql, connection))
                {
                    executacmd.Parameters.AddWithValue("@description", "%" + name + "%");

                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(executacmd))
                    {
                        dataAdapter.Fill(ProductTable);
                    }
                }
            }
            catch (MySqlException dbErro)
            {
                MessageBox.Show("Erro ao listar os Product: " + dbErro.Message);
            }
            catch (Exception error)
            {
                MessageBox.Show("Erro ao executar o comando SQL: " + error.Message);
                return null;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return ProductTable;
        }
        #endregion

        #region returns product by BarCode
        public Product ReturnsProductByBarCode(string barcode)
        {
            try
            {
                string sql = "SELECT * FROM tb_products WHERE barcode = @barcode";

                using (MySqlCommand executacmd = new MySqlCommand(sql, connection))
                {
                    executacmd.Parameters.AddWithValue("@barcode", barcode);

                    connection.Open();

                    using (var reader = executacmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Product
                            {
                                Id = reader.GetInt32("id"),
                                Description = reader.GetString("description"),
                                Price = reader.GetDecimal("price"),
                                Barcode = reader.GetString("barcode")
                            };
                        }
                        else
                        {
                            MessageBox.Show("Nenhum produto encontrado com esse código.");
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro ao buscar o produto: " + ex.Message);
                return null;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        #endregion

        #region Retorna id do produto by barcode
        public int ReturnsProductIdByBarCode(string barcode)
        {
            try
            {
                string sql = "SELECT id FROM tb_products WHERE barcode = @barcode";

                using (MySqlCommand executacmd = new MySqlCommand(sql, connection))
                {
                    executacmd.Parameters.AddWithValue("@barcode", barcode);

                    connection.Open();

                    using (var reader = executacmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int id = reader.GetInt32("id");
                            return id;
                        }
                        else
                        {
                            MessageBox.Show("Nenhum produto encontrado com esse código de barras.");
                            return 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro ao buscar o produto: " + ex.Message);
                return 0;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        #endregion

        #region Method of returning the current stock of products
        public int ReturningCurrentStockProducts(int idproduto)
        {
            int qtd_estoque = 0;

            try
            {
                string sql = "SELECT stock_qty FROM tb_products WHERE id = @id";

                using (var executacmd = new MySqlCommand(sql, connection))
                {
                    executacmd.Parameters.AddWithValue("@id", idproduto);

                    connection.Open();

                    using (var reader = executacmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            qtd_estoque = reader.GetInt32("stock_qty");
                        }
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show($"Aconteceu um erro: {err.Message}");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return qtd_estoque;
        }
        #endregion

        #region Method to lower stock
        public void LowerStock(int idproduto, int qtdestoque)
        {
            try
            {
                string sql = "UPDATE tb_products SET stock_qty = @qtd WHERE id = @id";

                using (var executacmd = new MySqlCommand(sql, connection))
                {
                    executacmd.Parameters.AddWithValue("@id", idproduto);
                    executacmd.Parameters.AddWithValue("@qtd", qtdestoque);

                    connection.Open();
                    executacmd.ExecuteNonQuery();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show($"Aconteceu um erro: {err.Message}");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        #endregion
    }
}
