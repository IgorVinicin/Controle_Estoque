using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using ClosedXML.Excel;



namespace Dashboard
{
    public partial class frmDashBoard : Form
    {
        public frmDashBoard()
        {
         
            InitializeComponent();
            RoundPanelCorners(panelVendas, 20);
            RoundPanelCorners(panelRei, 20);

        }

        private void RoundPanelCorners(Panel panel, int cornerRadius)
        {
            var path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, cornerRadius, cornerRadius), 180, 90);
            path.AddArc(new Rectangle(panel.Width - cornerRadius, 0, cornerRadius, cornerRadius), 270, 90);
            path.AddArc(new Rectangle(panel.Width - cornerRadius, panel.Height - cornerRadius, cornerRadius, cornerRadius), 0, 90);
            path.AddArc(new Rectangle(0, panel.Height - cornerRadius, cornerRadius, cornerRadius), 90, 90);
            path.CloseFigure();

            panel.Region = new Region(path);
        }

        private void btnEstoque_Click(object sender, EventArgs e)
        {
            listViewEstoque.Visible = true;
            listViewHistorico.Visible = false;
            panelVendas.Visible = false;
            btnExportarHistorico.Visible = false;
            btnExportarEstoque.Visible = true;
            btnApagarHistorico.Visible = false;

            CarregarEstoque();

        }

        private void btnVender_Click(object sender, EventArgs e)
        {
            listViewEstoque.Visible = false;
            btnExportarHistorico.Visible = false;
            panelVendas.Visible = true;

            btnApagarHistorico.Visible = false;
            btnExportarEstoque.Visible = false;
            listViewHistorico.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*Abrir form Cadastrar Prod*/
            CadastrarProd cadprod = new CadastrarProd();
            cadprod.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDashBoard_Load(object sender, EventArgs e)
        {
            /*select no cmbProd */
            LoadProductsIntoComboBox();
            LoadFormaPagamento();

            /*Configurações iniciais do listView*/
            if (listViewProd.Columns.Count == 0)
            {
                listViewProd.View = View.Details;
                listViewProd.Columns.Add("Produto", 100);
                listViewProd.Columns.Add("Quantidade", 110);
                listViewProd.Columns.Add("Forma Pagamento", 150);

            }
            if (listViewEstoque.Columns.Count == 0)
            {
                listViewEstoque.View = View.Details;
                listViewEstoque.Columns.Add("Produto", 400);
                listViewEstoque.Columns.Add("Quantidade", 100);
                listViewEstoque.Columns.Add("EstoqueMinimo", 100);
                listViewEstoque.Columns.Add("PrecoCusto", 100);
                listViewEstoque.Columns.Add("PrecoVenda", 100);
                listViewEstoque.Columns.Add("data_cadastro", 200);
            }

            if (listViewHistorico.Columns.Count == 0)
            {
                listViewHistorico.View = View.Details;
                listViewHistorico.Columns.Add("Produto", 400);
                listViewHistorico.Columns.Add("TotalVenda", 200);
                listViewHistorico.Columns.Add("Data", 200);
                listViewHistorico.Columns.Add("Forma de Pagamento", 200);
                listViewHistorico.Columns.Add("Observação", 200);
            }
        }

        private void CarregarEstoque()
        {
            listViewEstoque.Items.Clear();
            int itensBaixoEstoque = 0;
            int itensEsgotados = 0;
            int totalEmEstoque = 0;

            try
            {
                string connectionString = "server=localhost;user=root;password=;database=ChrisCell";
                using (MySqlConnection conexao = new MySqlConnection(connectionString))
                {
                    conexao.Open();
                    string query = "SELECT NomeProduto, Quantidade, EstoqueMinimo, PrecoCusto, PrecoVenda, data_cadastro FROM produtos";
                    using (MySqlCommand command = new MySqlCommand(query, conexao))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Adiciona os itens ao ListView
                                var item = new ListViewItem(reader["NomeProduto"].ToString());
                                int quantidade = Convert.ToInt32(reader["Quantidade"]);
                                int estoqueMinimo = Convert.ToInt32(reader["EstoqueMinimo"]);

                                item.SubItems.Add(reader["Quantidade"].ToString());
                                item.SubItems.Add(reader["EstoqueMinimo"].ToString());
                                item.SubItems.Add(reader["PrecoCusto"].ToString());
                                item.SubItems.Add(reader["PrecoVenda"].ToString());
                                item.SubItems.Add(Convert.ToDateTime(reader["data_cadastro"]).ToString("dd/MM/yyyy"));
                                listViewEstoque.Items.Add(item);

                                if (quantidade == 0)
                                {
                                    itensEsgotados++;
                                    item.BackColor = Color.Red; // Destaca itens esgotados em vermelho
                                }

                                else if (quantidade <= estoqueMinimo)
                                {
                                    itensBaixoEstoque++;
                                    item.BackColor = Color.Yellow; // Destaca itens com baixo estoque em amarelo
                                }

                                totalEmEstoque += quantidade;
                            }
                            label3.Text = $"{itensBaixoEstoque}";
                            label2.Text = $"{itensEsgotados}";
                            label5.Text = $"{totalEmEstoque}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar estoque: " + ex.Message);
            }
        }

        private void LoadProductsIntoComboBox()
        {
            try
            {
                string connectionString = "server=localhost;user=root;password=;database=ChrisCell";
                using (MySqlConnection conexao = new MySqlConnection(connectionString))
                {
                    conexao.Open();
                    string query = "SELECT NomeProduto FROM produtos";
                    using (MySqlCommand command = new MySqlCommand(query, conexao))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string produto = reader["NomeProduto"].ToString();
                                cmbProd.Items.Add(produto);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar produtos: " + ex.Message);
            }
        }

        private void LoadFormaPagamento()
        {
            try
            {
                string connectionString = "server=localhost;user=root;password=;database=ChrisCell";
                using (MySqlConnection conexao = new MySqlConnection(connectionString))
                {
                    conexao.Open();
                    string query = "SELECT NomeForma FROM forma_pagamento";
                    using (MySqlCommand command = new MySqlCommand(query, conexao))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string formaPagamento = reader["NomeForma"].ToString();
                                cmbFormaPagamento.Items.Add(formaPagamento);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar formas de pagamento: " + ex.Message);
            }
        }

        private void btnAddProd_Click(object sender, EventArgs e)
        {
            if (cmbProd.SelectedItem == null || string.IsNullOrWhiteSpace(txtQuant.Text) || cmbFormaPagamento.SelectedItem == null)
            {
                MessageBox.Show("Por favor, selecione um produto, a forma de pagamento e insira a quantidade.");
                return;
            }
            else
            {
                string produto = cmbProd.SelectedItem.ToString();
                string quantidade = txtQuant.Text;
                string formaPagamento = cmbFormaPagamento.SelectedItem.ToString();

                ListViewItem item = new ListViewItem(produto);
                item.SubItems.Add(quantidade);
                ListViewItem itemForma = new ListViewItem(formaPagamento);

                listViewProd.Items.Add(item);
                lblCount.Text = $"{listViewProd.Items.Count}";
            }

        }

        private void btn_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user=root;password=;database=ChrisCell";
            decimal totalVenda = 0;

            if (listViewProd.Items.Count == 0)
            {
                MessageBox.Show("Nenhum produto adicionado para venda.");
                return;
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (ListViewItem item in listViewProd.Items)
                    {
                        string nomeProduto = item.SubItems[0].Text;
                        int quantidadeVendida = int.Parse(item.SubItems[1].Text);

                        // Consulta o valor e o id do produto
                        string query = "SELECT id_produto, PrecoVenda, Quantidade FROM produtos WHERE NomeProduto = @nome";
                        int idProduto = 0;
                        decimal valorUnitario = 0;
                        int quantidadeEstoque = 0;

                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@nome", nomeProduto);

                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    idProduto = Convert.ToInt32(reader["id_produto"]);
                                    valorUnitario = Convert.ToDecimal(reader["PrecoVenda"]);
                                    quantidadeEstoque = Convert.ToInt32(reader["Quantidade"]);

                                    if (quantidadeVendida > quantidadeEstoque)
                                    {
                                        MessageBox.Show($"Estoque insuficiente para o produto: {nomeProduto}");
                                        return;
                                    }

                                    totalVenda += valorUnitario * quantidadeVendida;
                                }
                            }
                        }

                        AtualizarEstoque(nomeProduto, quantidadeVendida);

                        // Salva a venda na tabela entrada_saida
                        string insertQuery = @"INSERT INTO entrada_saida 
                            (id_produto, id_usuario, tipo, quantidade, data_movimento, observacao, id_pagamento) 
                            VALUES (@id_produto, @id_usuario, @tipo, @quantidade, @data_movimento, @observacao, @pagamento)";

                        using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                        {
                            insertCmd.Parameters.AddWithValue("@id_produto", idProduto);
                            insertCmd.Parameters.AddWithValue("@id_usuario", 1); // ajuste conforme seu sistema de usuários
                            insertCmd.Parameters.AddWithValue("@tipo", "saida");
                            insertCmd.Parameters.AddWithValue("@quantidade", quantidadeVendida);
                            insertCmd.Parameters.AddWithValue("@data_movimento", DateTime.Now);
                            insertCmd.Parameters.AddWithValue("@pagamento", cmbFormaPagamento.SelectedIndex + 1); // ajuste conforme seu sistema de pagamento
                            insertCmd.Parameters.AddWithValue("@observacao", $"Venda de {nomeProduto} ({quantidadeVendida} x R$ {valorUnitario:F2})");

                            insertCmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show($"Total da venda: R$ {totalVenda:F2}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao calcular o total da venda: " + ex.Message);
                return;
            }
        }

        private void AtualizarEstoque(string nomeProduto, int quantidadeVendida)
        {
            if (string.IsNullOrWhiteSpace(nomeProduto))
            {
                MessageBox.Show("O nome do produto não pode estar vazio.");
                return;
            }

            if (quantidadeVendida <= 0)
            {
                MessageBox.Show("A quantidade vendida deve ser maior que zero.");
                return;
            }

            try
            {
                string connectionString = "server=localhost;user=root;password=;database=ChrisCell";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE produtos SET Quantidade = Quantidade - @quantidade WHERE NomeProduto = @nome AND Quantidade >= @quantidade";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nome", nomeProduto);
                        command.Parameters.AddWithValue("@quantidade", quantidadeVendida);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show($"Estoque atualizado para o produto: {nomeProduto}");
                        }
                        else
                        {
                            MessageBox.Show($"Não foi possível atualizar o estoque. Verifique se há quantidade suficiente para o produto: {nomeProduto}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar o estoque: " + ex.Message);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            listViewProd.Items.Clear();
            lblCount.Text = $"{listViewProd.Items.Count}";
        }

        private void txtQuant_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Bloqueia a tecla
            }
        }

        private void CarregarHistoricoVendas()
        {
            listViewHistorico.Items.Clear();
            string connectionString = "server=localhost;user=root;password=;database=ChrisCell";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    // Busca apenas movimentos de saída (venda)
                    string query = @"SELECT es.id_movimento, p.NomeProduto, es.quantidade, es.data_movimento, es.observacao, fp.NomeForma
                                     FROM entrada_saida es
                                     JOIN produtos p ON es.id_produto = p.id_produto
                                     join forma_pagamento fp ON es.id_pagamento = fp.id_pagamento
                                     WHERE es.tipo = 'saida'
                                     ORDER BY es.data_movimento DESC";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new ListViewItem(reader["NomeProduto"].ToString());
                                item.SubItems.Add(reader["quantidade"].ToString());
                                item.SubItems.Add(Convert.ToDateTime(reader["data_movimento"]).ToString("dd/MM/yyyy HH:mm"));
                                item.SubItems.Add(reader["NomeForma"].ToString());
                                item.SubItems.Add(reader["observacao"].ToString());
                                listViewHistorico.Items.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar histórico de vendas: " + ex.Message);
            }
        }

        private void btnHistorico_Click(object sender, EventArgs e)
        {
        }

        private void btnHistorico_Click_1(object sender, EventArgs e)
        {
            listViewHistorico.Visible = true;
            listViewEstoque.Visible = false;
            btnExportarEstoque.Visible = false;
            panelVendas.Visible = false;
            btnExportarHistorico.Visible = true;
            btnApagarHistorico.Visible = true;
            CarregarHistoricoVendas();
        }

        private void cmbProd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true; // Bloqueia a tecla
            }
        }

        private void cmbFormaPagamento_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true; // Bloqueia a tecla
            }
        }

        private void btnApagarHistorico_Click(object sender, EventArgs e)
        {
            listViewHistorico.Items.Clear();
        }

        private void btnExportarEstoque_Click(object sender, EventArgs e)
        {
            ExportarEstoque();
        }

        private void ExportarEstoque()
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Estoque");

            // Cabeçalhos
            for (int i = 0; i < listViewEstoque.Columns.Count; i++)
                ws.Cell(1, i + 1).Value = listViewEstoque.Columns[i].Text;

            // Dados
            for (int i = 0; i < listViewEstoque.Items.Count; i++)
                for (int j = 0; j < listViewEstoque.Items[i].SubItems.Count; j++)
                    ws.Cell(i + 2, j + 1).Value = listViewEstoque.Items[i].SubItems[j].Text;

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                Title = "Estoque Atual"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                wb.SaveAs(saveFileDialog.FileName);
                MessageBox.Show("Exportação concluída com sucesso!", "Exportar Estoque", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExportarHistorico_Click(object sender, EventArgs e)
        {
            ExportarHistorico();
        }

        private void ExportarHistorico()
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Estoque");

            // Cabeçalhos
            for (int i = 0; i < listViewHistorico.Columns.Count; i++)
                ws.Cell(1, i + 1).Value = listViewEstoque.Columns[i].Text;

            // Dados
            for (int i = 0; i < listViewHistorico.Items.Count; i++)
                for (int j = 0; j < listViewHistorico.Items[i].SubItems.Count; j++)
                    ws.Cell(i + 2, j + 1).Value = listViewHistorico.Items[i].SubItems[j].Text;

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                Title = "Historico vendas"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                wb.SaveAs(saveFileDialog.FileName);
                MessageBox.Show("Exportação concluída com sucesso!", "Exportar Estoque", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
