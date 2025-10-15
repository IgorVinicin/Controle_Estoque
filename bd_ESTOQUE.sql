create database ChrisCell;
use ChrisCell;


CREATE TABLE usuario (
    id_usuario INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    senha VARCHAR(255) NOT NULL,
    tipo ENUM('admin', 'funcionario') DEFAULT 'funcionario',
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

Create table forma_pagamento(
	id_pagamento int auto_Increment primary key,
    NomeForma varchar(70) not null
);

CREATE TABLE produtos (
    id_produto INT AUTO_INCREMENT PRIMARY KEY,
    NomeProduto VARCHAR(100) NOT NULL,
    Quantidade int,
    precoCusto DECIMAL(10,2) NOT NULL,
    precoVenda DECIMAL(10,2) NOT NULL,
    EstoqueMinimo INT DEFAULT 0,
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE entrada_saida (
    id_movimento INT AUTO_INCREMENT PRIMARY KEY,
    id_produto INT NOT NULL,
    id_usuario INT NOT NULL,
    id_pagamento int not null,
    tipo ENUM('entrada', 'saida') NOT NULL,
    quantidade INT NOT NULL,
    data_movimento TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    observacao VARCHAR(255),
    FOREIGN KEY (id_produto) REFERENCES produtos(id_produto),
    foreign key (id_pagamento) references forma_pagamento(id_pagamento),
    FOREIGN KEY (id_usuario) REFERENCES usuario(id_usuario)
);





INSERT INTO usuario (nome, email, senha, tipo)
VALUES ('Operador 1', 'Operador@chriscell.com', '12345', 'admin');

INSERT INTO forma_pagamento (NomeForma) 
VALUES 
('Débito'),
('Crédito'),
('Pix'),
('Dinheiro');

