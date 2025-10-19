# Controle de Estoque – Dashboard

> Sistema completo para **gerenciamento de estoque**, **entrada e saída de produtos** e **cálculo automático de preços**, desenvolvido em **C# (Windows Forms)** com banco de dados **MySQL**.  
Ideal para lojas, comércios e pequenos negócios que precisam de um controle rápido e eficiente 
---

## Funcionalidades

 **Cadastro de produtos** com nome, categoria, estoque mínimo e preço  
 **Dashboard** com listagem e consulta dos produtos  
 **Controle de entradas** e  **saídas de estoque**  
 **Cálculo de preço de venda e lucro automático**  
 **Registro de vendas** integrado com o estoque  
 **Importação de produtos via planilha Excel (.xlsx)**  
 

---

## Tecnologias Utilizadas

| Tecnologia | Função |
|-------------|---------|
|  **C# / .NET Framework** | Desenvolvimento principal (Windows Forms) |
|  **MySQL** | Banco de dados relacional |
|  **ExcelDataReader** | Leitura e importação de planilhas Excel |
|  **ClickOnce** | Instalação em outras máquinas |
|  **Visual Studio 2022** | IDE de desenvolvimento |

---

## Estrutura do Banco de Dados

O sistema utiliza 4 tabelas principais:

- **usuarios** → dados de login e autenticação  
- **produtos** → informações sobre os produtos  
- **vendas** → histórico de vendas realizadas  
- **entrada_saida** → registros de movimentações no estoque  

> Cada operação de entrada, saída ou venda é registrada automaticamente no banco 

---

##Importação via Excel

Você pode cadastrar vários produtos de uma vez através de uma planilha `.xlsx`.  
O formato deve conter as seguintes colunas:

NomeProduto | Categoria | Quantidade | EstoqueMinimo | PrecoCusto | PrecoVenda | estoque_atual | data_cadastro


---

## 👨‍💻 Desenvolvedor

**Igor Vinicius Araujo Pece dos Santos**  
projeto pessoal – Sistema de Controle de Estoque  
Versão: `1.0.0`  
 

📫 Contato: [LinkedIn](www.linkedin.com/in/igor-vinicius-6bb571283) • [GitHub](https://github.com/IgorVinicin)  

---

⭐ *Se este projeto te ajudou, deixe uma estrela no repositório!* ⭐
