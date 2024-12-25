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
    internal class SupplierDAO
    {
        private MySqlConnection connection;
        public SupplierDAO()
        {
            this.connection = new ConnectionFactory().GetConnection();
        }

        #region Register Supplier
        public void RegisterSupplier(Supplier obj)
        {
            try
            {
                // validação de dados supplier
                if (obj == null)
                {
                    MessageBox.Show("Dados do fornecedor não podem ser nulos.");
                    return;
                }

                // 1 - Definir o CMD sql - insert into
                string sql = @"INSERT INTO tb_suppliers (name, cnpj, email, phone, mobile, zip_code, address, number, complement, district, city, state) 
                VALUES (@name, @cnpj, @email, @phone, @mobile, @zip_code, @address, @number, @complement, @district, @city, @state);";


                // 2 -  Organizar o comando sql     

                using (MySqlCommand executacmd = new MySqlCommand(sql, connection))
                {
                    executacmd.Parameters.AddWithValue("@name", obj.Name);
                    executacmd.Parameters.AddWithValue("@cnpj", obj.Cnpj);
                    executacmd.Parameters.AddWithValue("@email", obj.Email);
                    executacmd.Parameters.AddWithValue("@phone", obj.Telephone);
                    executacmd.Parameters.AddWithValue("@mobile", obj.Phone);
                    executacmd.Parameters.AddWithValue("@zip_code", obj.Cep);
                    executacmd.Parameters.AddWithValue("@address", obj.Address);
                    executacmd.Parameters.AddWithValue("@number", obj.Number);
                    executacmd.Parameters.AddWithValue("@complement", obj.Complement);
                    executacmd.Parameters.AddWithValue("@district", obj.Neighborhood);
                    executacmd.Parameters.AddWithValue("@city", obj.City);
                    executacmd.Parameters.AddWithValue("@state", obj.State);

                    // 3 - executar o comando sql 
                    connection.Open();
                    executacmd.ExecuteNonQuery();
                }
                MessageBox.Show("Fornecedor cadastrado com sucesso!");
            }
            catch (MySqlException dbErro)
            {
                // Tratamento específico para erros de banco de dados
                MessageBox.Show("Erro ao cadastrar fornecedor: " + dbErro.Message);
            }
            catch (Exception erro)
            {
                MessageBox.Show("Ocorreu um error: " + erro);
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

        #region list Supplier
        public DataTable ListSupplier()
        {
            DataTable customerTable = new DataTable();
            string sql = @" SELECT 
                            id,
                            name AS 'Nome',
                            cnpj AS 'CNPJ',
                            email AS 'E-mail',
                            phone AS 'Telefone',
                            mobile AS 'WhatsApp',
                            zip_code AS 'CEP',
                            address AS 'Endeço',
                            number AS 'Número',
                            complement AS 'Complemento',
                            district AS 'Bairro',
                            city AS 'Cidade',
                            state AS 'Estado'
                            FROM tb_suppliers; ";

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                using (MySqlCommand executacmd = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(executacmd))
                    {
                        dataAdapter.Fill(customerTable);
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show($"Erro ao listar fornecedor: {error.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            return customerTable;
        }
        #endregion

        #region edit supplier 
        public void EditSupplier(Supplier obj)
        {
            try
            {
                if (obj == null)
                {
                    MessageBox.Show("Os dados do fornecedor são nulo.");
                    return;
                }

                string sql = @"UPDATE tb_suppliers 
                               SET name=@name, cnpj=@cnpj, email=@email,
                               phone=@phone, 
                               mobile=@mobile, zip_code=@zip_code, address=@address, number=@number, 
                               complement=@complement, district=@district, city=@city, state=@state 
                               WHERE id=@id";

                using (MySqlCommand executacmd = new MySqlCommand(sql, connection))
                {
                    executacmd.Parameters.AddWithValue("@name", obj.Name);
                    executacmd.Parameters.AddWithValue("@cnpj", obj.Cnpj);
                    executacmd.Parameters.AddWithValue("@email", obj.Email);
                    executacmd.Parameters.AddWithValue("@phone", obj.Telephone);
                    executacmd.Parameters.AddWithValue("@mobile", obj.Phone);
                    executacmd.Parameters.AddWithValue("@zip_code", obj.Cep);
                    executacmd.Parameters.AddWithValue("@address", obj.Address);
                    executacmd.Parameters.AddWithValue("@number", obj.Number);
                    executacmd.Parameters.AddWithValue("@complement", obj.Complement);
                    executacmd.Parameters.AddWithValue("@district", obj.Neighborhood);
                    executacmd.Parameters.AddWithValue("@city", obj.City);
                    executacmd.Parameters.AddWithValue("@state", obj.State);
                    executacmd.Parameters.AddWithValue("@id", obj.Code);

                    connection.Open();
                    executacmd.ExecuteNonQuery();
                }
                MessageBox.Show("Fornecedor Alterado com Sucesso!");

            }
            catch (MySqlException dbErro)
            {
                MessageBox.Show("Erro ao editar fornecedor: " + dbErro.Message);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Ocorreu um error: {error}");
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

        #region DELETE SUPPLIER
        public void DeleteSupplier(Supplier obj)
        {
            try
            {
                if (obj == null)
                {
                    MessageBox.Show("Erro no identificador de fornecedor.");
                    return;
                }

                // validação de estado de conexão 
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                string sql = @"DELETE FROM tb_suppliers WHERE id = @id";

                using (MySqlCommand executacmd = new MySqlCommand(sql, connection))
                {
                    executacmd.Parameters.AddWithValue("@id", obj.Code);
                    executacmd.ExecuteNonQuery();
                }
                MessageBox.Show("Fornecedor deletado com sucesso com sucesso!");
            }
            catch (MySqlException DbError)
            {
                MessageBox.Show($"Erro ao cadastrar fornecedor: {DbError.Message}");
            }
            catch (Exception erro)
            {
                MessageBox.Show("Ocorreu um error: " + erro);
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

        #region Search Customer By Name
        public DataTable SearchSupplierByName(string name)
        {
            DataTable customerTable = new DataTable();
            string sql = "SELECT * FROM tb_suppliers WHERE name = @name;";

            try
            {
                // 1 - Organizar o comando SQL
                using (MySqlCommand executacmd = new MySqlCommand(sql, connection))
                {
                    executacmd.Parameters.AddWithValue("@name", name);

                    // 2 - Abrir conexão se necessário
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    // 3 - Criar MySqlDataAdapter para preencher o DataTable
                    using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(executacmd))
                    {
                        dataAdapter.Fill(customerTable);
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Error while executing SQL command: " + error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                // Garantir que a conexão seja fechada
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return customerTable;
        }
        #endregion

        #region List Customer By Name
        public DataTable ListSupplierByName(string name)
        {
            DataTable customerTable = new DataTable();
            string sql = "SELECT * FROM tb_suppliers WHERE name LIKE @name;";

            try
            {
                // Organizar o comando SQL
                using (MySqlCommand executacmd = new MySqlCommand(sql, connection))
                {
                    executacmd.Parameters.AddWithValue("@name", "%" + name + "%");

                    // Abrir conexão se necessário
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    // Criar MySqlDataAdapter para preencher o DataTable
                    using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(executacmd))
                    {
                        dataAdapter.Fill(customerTable);
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Error while executing SQL command: " + error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                // Garantir que a conexão seja fechada
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return customerTable;
        }
        #endregion
    }
}
