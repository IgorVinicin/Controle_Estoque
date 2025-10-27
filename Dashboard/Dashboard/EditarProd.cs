using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace Dashboard
{
    public partial class EditarProd : Form
    {
        public EditarProd()
        {
            InitializeComponent();

        }

        private void EditarProd_Load(object sender, EventArgs e)
        {

        }

        private void btnCadastrarProd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Por favor, insira o ID do produto.");
                return;
            }

            try
            {
                string conexaoString = "server=localhost;user=root;password=;database=ChrisCell";
                using(MySqlConnection conexao = new MySqlConnection(conexaoString))
                    {
                    conexao.Open();
                    string query = "UPDATE produtos SET NomeProduto = @NomeProduto, Categoria = @Categoria, Quantidade = @Quantidade, " +
                                   "EstoqueMinimo = @EstoqueMinimo, PrecoCusto = @PrecoCusto, PrecoVenda = @PrecoVenda WHERE id_produto = @idProduto";
                    using (MySqlCommand comando = new MySqlCommand(query, conexao))
                    {
                        comando.Parameters.AddWithValue("@NomeProduto", txtNomeProd.Text);
                        comando.Parameters.AddWithValue("@Categoria", txtCategoriaProd.Text);
                        comando.Parameters.AddWithValue("@Quantidade", int.Parse(txtQuantProd.Text));
                        comando.Parameters.AddWithValue("@EstoqueMinimo", int.Parse(txtEstoqProd.Text));
                        comando.Parameters.AddWithValue("@PrecoCusto", decimal.Parse(txtPrecoCusto.Text));
                        comando.Parameters.AddWithValue("@PrecoVenda", decimal.Parse(txtPrecoVenda.Text));
                        comando.Parameters.AddWithValue("@idProduto", int.Parse(textBox1.Text));
                        comando.ExecuteNonQuery();
                        MessageBox.Show("Produto atualizado com sucesso!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao conectar ao banco de dados: " + ex.Message);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int idProduto))
            {

                try
                {
                    string conexaoString = "server=localhost;user=root;password=;database=ChrisCell";
                    using (MySqlConnection conexao = new MySqlConnection(conexaoString))
                    {
                        conexao.Open();
                        string query = "SELECT * FROM produtos WHERE id_produto = @idProduto";

                        using (MySqlCommand comando = new MySqlCommand(query, conexao))
                        {

                            comando.Parameters.AddWithValue("@idProduto", idProduto);
                            using (MySqlDataReader leitor = comando.ExecuteReader())
                            {
                                if (leitor.Read())
                                {
                                    txtNomeProd.Text = leitor["NomeProduto"].ToString();
                                    txtCategoriaProd.Text = leitor["Categoria"].ToString();
                                    txtQuantProd.Text = leitor["Quantidade"].ToString();
                                    txtEstoqProd.Text = leitor["EstoqueMinimo"].ToString();
                                    txtPrecoCusto.Text = leitor["PrecoCusto"].ToString();
                                    txtPrecoVenda.Text = leitor["PrecoVenda"].ToString();
                                }
                                else
                                {
                                    MessageBox.Show("Produto não encontrado.");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Erro de Banco de Dados: " + ex.Message);
                }
            }
            else
            {

                MessageBox.Show("Por favor, digite um ID de produto válido (apenas números).");
                textBox1.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string conexaoString = "server=localhost;user=root;password=;database=ChrisCell";
                using (MySqlConnection conexao = new MySqlConnection(conexaoString))
                {
                    conexao.Open();
                    string query = "DELETE FROM produtos WHERE id_produto = @idProduto";
                    using (MySqlCommand comando = new MySqlCommand(query, conexao))
                    {
                        comando.Parameters.AddWithValue("@idProduto", int.Parse(textBox1.Text));
                        comando.ExecuteNonQuery();
                        MessageBox.Show("Produto excluído com sucesso!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao conectar ao banco de dados: " + ex.Message);
            }
        }
    }
}
