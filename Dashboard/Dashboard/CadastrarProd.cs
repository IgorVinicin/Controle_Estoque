using ExcelDataReader;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



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

        private void btnImportar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1️⃣ Abre o seletor de arquivos
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Arquivos Excel (*.xlsx;*.xls)|*.xlsx;*.xls";
                ofd.Title = "Selecione a planilha de produtos";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string caminhoArquivo = ofd.FileName;
                    MessageBox.Show("Arquivo selecionado:\n" + caminhoArquivo);

                    // 2️⃣ Lê o Excel
                    using (var stream = File.Open(caminhoArquivo, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            var result = reader.AsDataSet();
                            DataTable tabela = result.Tables[0]; // primeira planilha

                            // 3️⃣ Conecta ao banco
                            string connectionString = "server=localhost;user=root;password=;database=ChrisCell";
                            using (MySqlConnection conexao = new MySqlConnection(connectionString))
                            {
                                conexao.Open();

                                // 4️⃣ Percorre as linhas do Excel (ignorando o cabeçalho)
                                for (int i = 1; i < tabela.Rows.Count; i++)
                                {
                                    string nmProduto = tabela.Rows[i][0].ToString();
                                    string categoria = tabela.Rows[i][1].ToString();
                                    decimal quantidade = Convert.ToDecimal(tabela.Rows[i][2]);
                                    decimal estoquemin = Convert.ToDecimal(tabela.Rows[i][3]);
                                    decimal precoCusto = Convert.ToDecimal(tabela.Rows[i][4]);
                                    decimal precoVenda = Convert.ToDecimal(tabela.Rows[i][5]);

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

                                        comando.ExecuteNonQuery();
                                    }
                                }

                                MessageBox.Show("Produtos cadastrados com sucesso!");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao importar produtos: " + ex.Message);
            }
        }
    }
}
