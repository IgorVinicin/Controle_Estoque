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
    public partial class CadastrarProd : Form
    {
        public CadastrarProd()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            frmDashBoard dashboard = new frmDashBoard();
            dashboard.Show();
            this.Close();
        }

        private void btnCadastrarProd_Click(object sender, EventArgs e)
        {
            string nmProduto = "";
            string categoria = "";
            int quantidade = 0;
            int estoquemin = 0;
            decimal precoCusto = 0;
            decimal precoVenda = 0;



            nmProduto= txtNomeProd.Text;
            categoria = txtCategoriaProd.Text;
            quantidade = int.Parse(txtQuantProd.Text);
            estoquemin = int.Parse(txtEstoqProd.Text);
            precoCusto = decimal.Parse(txtPrecoCusto.Text);
            precoVenda = decimal.Parse(txtPrecoVenda.Text);


            try
            {
                string conexaoString = "server=localhost;user=root;password=;database=ChrisCell";
                using (MySqlConnection conexao = new MySqlConnection(conexaoString))
                    {
                    conexao.Open();
                    string query = "INSERT INTO produtos (NomeProduto, Categoria, Quantidade, EstoqueMinimo, PrecoCusto, PrecoVenda) " +
                                   "VALUES (@NomeProduto, @Categoria, @Quantidade, @EstoqueMinimo, @PrecoCusto, @PrecoVenda)";
                    using (MySqlCommand comando = new MySqlCommand(query, conexao))
                    {
                        comando.Parameters.AddWithValue("@NomeProduto", nmProduto);
                        comando.Parameters.AddWithValue("@Categoria", categoria);
                        comando.Parameters.AddWithValue("@Quantidade", quantidade);
                        comando.Parameters.AddWithValue("@EstoqueMinimo", estoquemin);
                        comando.Parameters.AddWithValue("@PrecoCusto", precoCusto);
                        comando.Parameters.AddWithValue("@PrecoVenda", precoVenda);
                        int linhasAfetadas = comando.ExecuteNonQuery();
                        if (linhasAfetadas > 0)
                        {
                            MessageBox.Show("Produto cadastrado com sucesso!");
                        }
                        else
                        {
                            MessageBox.Show("Falha ao cadastrar o produto.");
                        }
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
