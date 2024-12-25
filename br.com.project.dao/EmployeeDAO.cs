using general_software_model.br.com.project.model;
using general_software_model.br.com.project.view;
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
    internal class EmployeeDAO
    {
        private MySqlConnection Connection;
        public EmployeeDAO()
        {
            this.Connection = new ConnectionFactory().GetConnection();
        }

        #region Register Employee
        public void RegisterEmployee(Employee obj)
        {
            try
            {
                // Validação básica de dados
                if (obj == null)
                {
                    MessageBox.Show("Dados do funcionário não podem ser nulos.");
                    return;
                }

                string sql = "INSERT INTO tb_employees (name, rg, cpf, email, password, role, access_level, phone, mobile, zip_code, address, number, complement, district, city, state) " +
                             "VALUES (@name, @rg, @cpf, @email, @password, @role, @access_level, @phone, @mobile, @zip_code, @address, @number, @complement, @district, @city, @state);\r\n";

                // Organize the sql command   
                using (MySqlCommand executacmd = new MySqlCommand(sql, Connection))
                {
                    executacmd.Parameters.AddWithValue("@name", obj.Name);
                    executacmd.Parameters.AddWithValue("@rg", obj.Rg);
                    executacmd.Parameters.AddWithValue("@cpf", obj.Cpf);
                    executacmd.Parameters.AddWithValue("@email", obj.Email);
                    executacmd.Parameters.AddWithValue("@password", obj.Password);
                    executacmd.Parameters.AddWithValue("@role", obj.Position);
                    executacmd.Parameters.AddWithValue("@access_level", obj.AccessLevel);
                    executacmd.Parameters.AddWithValue("@phone", obj.Telephone);
                    executacmd.Parameters.AddWithValue("@mobile", obj.Phone);
                    executacmd.Parameters.AddWithValue("@zip_code", obj.Cep);
                    executacmd.Parameters.AddWithValue("@address", obj.Address);
                    executacmd.Parameters.AddWithValue("@number", obj.Number);
                    executacmd.Parameters.AddWithValue("@complement", obj.Complement);
                    executacmd.Parameters.AddWithValue("@district", obj.Neighborhood);
                    executacmd.Parameters.AddWithValue("@city", obj.City);
                    executacmd.Parameters.AddWithValue("@state", obj.State);

                    // Execute the sql command
                    Connection.Open();
                    executacmd.ExecuteNonQuery();
                }

                MessageBox.Show("Funcionário cadastrado com sucesso!");
            }
            catch (MySqlException dbErro)
            {
                // Tratamento específico para erros de banco de dados
                MessageBox.Show("Erro ao cadastrar funcioário: " + dbErro.Message);
            }
            catch (Exception erro)
            {
                MessageBox.Show("Ocorreu um error: " + erro.Message);
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

        #region list employee
        public DataTable ListEmployee()
        {
            // Instanciar o objeto table 
            DataTable EmployeeTable = new DataTable();
            string sql = @" SELECT 
                            id AS 'Código',
                            name AS 'Nome',
                            rg AS 'RG',
                            cpf AS 'CPF',
                            email AS 'E-mail',
                            password AS 'Senha',
                            role AS 'Cargo',
                            access_level AS 'Nivel de Acesso',
                            phone AS 'Telefone',
                            mobile AS 'WhatsApp',
                            zip_code AS 'CEP',
                            address AS 'Endeço',
                            number AS 'Número',
                            complement AS 'Complemento',
                            district AS 'Bairro',
                            city AS 'Cidade',
                            state AS 'Estado'
                            FROM tb_employees; ";

            try
            {
                // validação de estado de conexão 
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                // 2 - organizar o comando sql no executar 
                using (MySqlCommand executacmd = new MySqlCommand(sql, Connection))
                {
                    // Preencher os dados no DataTable usando MySqlDataAdapter
                    using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(executacmd))
                    {
                        dataAdapter.Fill(EmployeeTable);
                    }
                }
            }
            catch (Exception error)
            {
                // Tratar a exceção de forma clara
                MessageBox.Show($"Erro ao listar clientes: {error.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                // Fechar a conexão se necessário
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }
            }

            return EmployeeTable;
        }
        #endregion

        #region Edit employee
        public void EditEmployee(Employee obj)
        {
            try
            {
                //  validfação de dados 
                if (obj == null)
                {
                    MessageBox.Show("Dados do funcionário não podem ser nulos.");
                    return;
                }

                // string de conexão 
                string sql = @"UPDATE tb_employees
                              SET name=@name, rg=@rg, cpf=@cpf, email=@email, password=@password, 
                              role=@role, access_level=@access_level, phone=@phone, 
                              mobile=@mobile, zip_code=@zip_code, address=@address, number=@number, 
                              complement=@complement, district=@district, city=@city, state=@state WHERE id=@id";

                using (MySqlCommand executacmd = new MySqlCommand(sql, Connection))
                {
                    executacmd.Parameters.AddWithValue("@name", obj.Name);
                    executacmd.Parameters.AddWithValue("@rg", obj.Rg);
                    executacmd.Parameters.AddWithValue("@cpf", obj.Cpf);
                    executacmd.Parameters.AddWithValue("@email", obj.Email);
                    executacmd.Parameters.AddWithValue("@password", obj.Password);
                    executacmd.Parameters.AddWithValue("@role", obj.Position);
                    executacmd.Parameters.AddWithValue("@access_level", obj.AccessLevel);
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

                    if (Connection.State == ConnectionState.Closed)
                    {
                        Connection.Open();
                    }

                    executacmd.ExecuteNonQuery();
                }

                MessageBox.Show("Cliente Alterado com sucesso!");
            }
            catch (MySqlException dbErro)
            {
                MessageBox.Show("Erro ao ediatr o funcioário: " + dbErro.Message);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Ocorreu um error: {error}");
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

        #region Delete Employee
        public void DeleteEmployee(Employee obj)
        {
            try
            {
                // validação de dados 
                if (obj == null)
                {
                    MessageBox.Show("Dados do funcionário não podem ser nulos.");
                    return;
                }

                // validação de estado de conexão 
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                // string de comando SQL 
                string sql = "DELETE FROM tb_employees WHERE id = @id";

                using (MySqlCommand executacmd = new MySqlCommand(sql, Connection))
                {
                    executacmd.Parameters.AddWithValue("@id", obj.Code);

                    executacmd.ExecuteNonQuery();
                }

                MessageBox.Show("Funcionário deletado com sucesso com sucesso!");
            }
            catch (MySqlException DbError)
            {
                MessageBox.Show($"Erro ao cadastrar funcioário: {DbError.Message}");
            }
            catch (Exception error)
            {
                MessageBox.Show($"Ocorreu um error: {error.Message}");
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

        #region Search Employee By Name
        public DataTable SearchEmployeeByName(string name)
        {
            // 1 - Criação do DataTable
            DataTable tabelaCliente = new DataTable();
            string sql = "SELECT * FROM tb_employees WHERE name = @name;";

            // Validação da entrada
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("O nome não pode estar vazio ou nulo.");
                return null;
            }

            try
            {
                // 2 - Usando a variável de conexão já existente (Connection)
                using (MySqlCommand executacmd = new MySqlCommand(sql, Connection))
                {
                    // Adicionando o parâmetro
                    executacmd.Parameters.AddWithValue("@name", name);

                    // Abrindo a conexão se estiver fechada
                    if (Connection.State == ConnectionState.Closed)
                        Connection.Open();

                    // 3 - Executar a leitura dos dados com ExecuteReader
                    using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(executacmd))
                    {
                        // Preenchendo o DataTable com o resultado da consulta
                        dataAdapter.Fill(tabelaCliente);
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Erro ao executar o comando SQL: " + error.Message);
            }
            finally
            {
                // Fechando a conexão se ela estiver aberta
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
            }

            return tabelaCliente;
        }
        #endregion

        #region List Employee By Name
        public DataTable ListEmployeeByName(string name)
        {
            try
            {
                // 1 - Criação do DataTable
                DataTable tabelaCliente = new DataTable();
                string sql = "SELECT * FROM tb_employees WHERE name LIKE @name;";

                // Validação de entrada
                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("O nome não pode estar vazio ou nulo.");
                    return null;
                }

                // 2 - Organizar o comando SQL no executacmd com a variável de conexão existente
                using (MySqlCommand executacmd = new MySqlCommand(sql, Connection))
                {
                    // Adicionando o parâmetro com wildcard para busca parcial
                    executacmd.Parameters.AddWithValue("@name", "%" + name + "%");

                    // Abrindo a conexão se estiver fechada
                    if (Connection.State == ConnectionState.Closed)
                        Connection.Open();

                    // 3 - Criando MySqlDataAdapter para preencher o DataTable
                    using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(executacmd))
                    {
                        dataAdapter.Fill(tabelaCliente);
                    }
                }

                // Retornando o DataTable preenchido
                return tabelaCliente;
            }
            catch (Exception error)
            {
                // Mensagem de erro
                MessageBox.Show("Erro ao executar o comando SQL: " + error.Message);
                return null;
            }
            finally
            {
                // Garante que a conexão será fechada, se ainda estiver aberta
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
        }
        #endregion

        #region Login method
        public bool LoginMethod(string email, string password)
        {
            try
            {
                string sql = "SELECT name, access_level FROM tb_employees WHERE email = @Email AND password = @password";

                using (MySqlCommand executacmd = new MySqlCommand(sql, Connection))
                {
                    executacmd.Parameters.AddWithValue("@Email", email);
                    executacmd.Parameters.AddWithValue("@password", password);

                    Connection.Open(); // Conexão já está definida na variável conexao

                    using (MySqlDataReader reader = executacmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string access_level = reader["access_level"].ToString();
                            string name = reader["name"].ToString();

                            FrmMenu TelaMenu = new FrmMenu();

                            TelaMenu.txtMenuUser.Text = name;

                            if (access_level.Equals("Administrador"))
                            {
                                MessageBox.Show($"Bem vindo Administrador: {name}");
                                TelaMenu.Show();
                            }
                            else if (access_level.Equals("Usuário"))
                            {
                                MessageBox.Show($"Bem vindo Usuário: {name}");
                                TelaMenu.MenuEmployee.Visible = false; // Oculta opções para o nível "Usuário"
                                TelaMenu.Show();
                            }

                            return true;
                        }
                        else
                        {
                            MessageBox.Show("E-mail ou senha incorretos!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Log de erro pode ser implementado aqui
                MessageBox.Show($"Erro ao se conectar ao banco de dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                // Log de erro pode ser implementado aqui
                MessageBox.Show($"Ocorreu um erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close(); // Garante que a conexão seja fechada
                }
            }
        }
        #endregion
    }
}
