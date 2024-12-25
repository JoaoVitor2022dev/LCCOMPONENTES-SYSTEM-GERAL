using general_software_model.br.com.project.model;
using general_software_model.br.com.projet.connection;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace general_software_model.br.com.project.dao
{
    internal class CustomerDAO
    {
        private MySqlConnection connection;
        public CustomerDAO()
        {
            this.connection = new ConnectionFactory().GetConnection();
        }

        #region Register Customer
        public void RegisterCustomer(Customer obj)
        {
            // Validação básica de dados
            if (obj == null)
            {
                MessageBox.Show("Dados do cliente não podem ser nulos.");
                return;
            }

            try
            {
                string sql = @"INSERT INTO tb_customer 
                   (name, rg, cpf, email, phone, mobile, zip_code, address, number, complement, district, city, state) 
                   VALUES (@name, @rg, @cpf, @email, @phone, @mobile, @zip_code, @address, @number, @complement, @district, @city, @state);";

                // Usando 'using' para garantir que o comando seja fechado adequadamente
                using (MySqlCommand executacmd = new MySqlCommand(sql, connection))
                {
                    // Definindo parâmetros de forma segura
                    executacmd.Parameters.AddWithValue("@name", obj.Name);
                    executacmd.Parameters.AddWithValue("@rg", obj.Rg);
                    executacmd.Parameters.AddWithValue("@cpf", obj.Cpf);
                    executacmd.Parameters.AddWithValue("@email", obj.Email);
                    executacmd.Parameters.AddWithValue("@phone", obj.Telephone);
                    executacmd.Parameters.AddWithValue("@mobile", obj.Phone);
                    executacmd.Parameters.AddWithValue("@zip_code", obj.Zip_code);
                    executacmd.Parameters.AddWithValue("@address", obj.Address);
                    executacmd.Parameters.AddWithValue("@number", obj.Number);
                    executacmd.Parameters.AddWithValue("@complement", obj.Complement);
                    executacmd.Parameters.AddWithValue("@district", obj.Neighborhood);
                    executacmd.Parameters.AddWithValue("@city", obj.City);
                    executacmd.Parameters.AddWithValue("@state", obj.State);

                    // Abrindo conexão
                    connection.Open();
                    // Executando o comando
                    executacmd.ExecuteNonQuery();
                }

                // Informando sucesso no cadastro
                MessageBox.Show("Cliente cadastrado com sucesso!");
            }
            catch (MySqlException dbErro)
            {
                // Tratamento específico para erros de banco de dados
                MessageBox.Show("Erro ao cadastrar cliente: " + dbErro.Message);
            }
            catch (Exception erro)
            {
                // Tratamento genérico para outras exceções
                MessageBox.Show("Ocorreu um erro: " + erro.Message);
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

        #region List Customers
        public DataTable ListCustomers()
        {
            // Criar DataTable para armazenar os dados
            DataTable customerTable = new DataTable();
            string sql = @" SELECT 
                            id AS 'Código',
                            name AS 'Nome',
                            rg AS 'Rg',
                            cpf AS 'CPF',
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
                            FROM tb_customer; ";
            try
            {
                // Abrir a conexão (supondo que 'connection' é uma variável já inicializada externamente)
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                // Preparar o comando SQL
                using (MySqlCommand executacmd = new MySqlCommand(sql, connection))
                {
                    // Preencher os dados no DataTable usando MySqlDataAdapter
                    using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(executacmd))
                    {
                        dataAdapter.Fill(customerTable);
                    }
                }
            }
            catch (Exception ex)
            {
                // Tratar a exceção de forma clara
                MessageBox.Show($"Erro ao listar clientes: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        #region Change Customer
        public void ChangeCustomer(Customer obj)
        {
            try
            {
                // 1 - Definir o CMD sql - UPDATE
                string sql = @"UPDATE tb_customer
                        SET name=@name, rg=@rg, cpf=@cpf, email=@email, phone=@phone, 
                        mobile=@mobile, zip_code=@zip_code, address=@address, number=@number, 
                        complement=@complement, district=@district, city=@city, state=@state 
                        WHERE id=@id";

                // 2 - Organizar o comando SQL
                using (MySqlCommand executacmd = new MySqlCommand(sql, connection))
                {
                    executacmd.Parameters.AddWithValue("@name", obj.Name);
                    executacmd.Parameters.AddWithValue("@rg", obj.Rg);
                    executacmd.Parameters.AddWithValue("@cpf", obj.Cpf);
                    executacmd.Parameters.AddWithValue("@email", obj.Email);
                    executacmd.Parameters.AddWithValue("@phone", obj.Telephone);
                    executacmd.Parameters.AddWithValue("@mobile", obj.Phone);
                    executacmd.Parameters.AddWithValue("@zip_code", obj.Zip_code);
                    executacmd.Parameters.AddWithValue("@address", obj.Address);
                    executacmd.Parameters.AddWithValue("@number", obj.Number);
                    executacmd.Parameters.AddWithValue("@complement", obj.Complement);
                    executacmd.Parameters.AddWithValue("@district", obj.Neighborhood);
                    executacmd.Parameters.AddWithValue("@city", obj.City);
                    executacmd.Parameters.AddWithValue("@state", obj.State);
                    executacmd.Parameters.AddWithValue("@id", obj.Code);

                    // 3 - Abrir conexão se necessário e executar o comando SQL
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    executacmd.ExecuteNonQuery();
                    MessageBox.Show("Cliente alterado com sucesso!");
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show("Ocorreu um erro: " + erro.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Garantir que a conexão seja fechada
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        #endregion

        #region Delete Customer
        public void DeleteCustomer(Customer obj)
        {
            try
            {
                // 1 - Definir o comando SQL para deletar
                string sql = @"DELETE FROM tb_customer WHERE id = @id";

                // 2 - Organizar o comando SQL
                using (MySqlCommand executacmd = new MySqlCommand(sql, connection))
                {
                    executacmd.Parameters.AddWithValue("@id", obj.Code);

                    // 3 - Abrir a conexão se necessário e executar o comando SQL
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    executacmd.ExecuteNonQuery();

                    MessageBox.Show("Cliente Deletado com sucesso!");
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show("An error occurred: " + erro.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Garantir que a conexão seja fechada
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        #endregion

        #region Search Customer By Name
        public DataTable SearchCustomerByName(string name)
        {
            DataTable customerTable = new DataTable();
            string sql = @"SELECT  
                            id AS 'Código',
                            name AS 'Nome',
                            rg AS 'Rg',
                            cpf AS 'CPF',
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
                           FROM tb_customer WHERE name = @name;";

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
        public DataTable ListCustomerByName(string name)
        {
            DataTable customerTable = new DataTable();
            string sql = @"SELECT  
                            id AS 'Código',
                            name AS 'Nome',
                            rg AS 'Rg',
                            cpf AS 'CPF',
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
                           FROM tb_customer WHERE name LIKE @name;";

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

        #region Return Customers CPF
        public Customer ReturnCustomersCPF(string cpf)
        {
            Customer customer = new Customer();
            string sql = @"SELECT * FROM tb_customer WHERE cpf = @cpf;";

            try
            {
                // Organizar o comando SQL
                using (MySqlCommand executedcmd = new MySqlCommand(sql, connection))
                {
                    executedcmd.Parameters.AddWithValue("@cpf", cpf);

                    // Abrir a conexão se necessário
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    // Executar o comando e ler o resultado
                    using (MySqlDataReader reader = executedcmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            customer.Code = reader.GetInt32("Id");
                            customer.Name = reader.GetString("name");
                        }
                        else
                        {
                            MessageBox.Show("Customer not found!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return null;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("An error occurred: " + error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            return customer;
        }
        #endregion
    }
}
