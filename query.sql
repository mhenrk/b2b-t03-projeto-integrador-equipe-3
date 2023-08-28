CREATE DATABASE todolist;

USE todolist; 

CREATE TABLE Users (
  user_id INT PRIMARY KEY IDENTITY(1,1),
  name VARCHAR(50) NOT NULL,
  email VARCHAR(50) UNIQUE NOT NULL,
  password VARCHAR(255) NOT NULL
);

CREATE TABLE Tasks (
  task_id INT PRIMARY KEY IDENTITY(1,1),
  title VARCHAR(100) NOT NULL,
  description VARCHAR(255) NOT NULL,
  status VARCHAR(20) NOT NULL,
  user_id INT NOT NULL,
  FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

INSERT INTO Users (name, email, password)
VALUES 
  ('Jo�o', 'joao@email.com', 'senha123'),
  ('Maria', 'maria@email.com', 'senha456'),
  ('Pedro', 'pedro@email.com', 'senha789'),
  ('Ana', 'ana@email.com', 'senha012');


INSERT INTO Tasks (title, description, status, user_id)
VALUES 
  ('Criar p�gina de login', 'Desenvolver p�gina de login para o sistema', 'Em andamento', 1),
  ('Implementar autentica��o', 'Adicionar autentica��o ao sistema', 'Conclu�do', 2),
  ('Criar banco de dados', 'Criar modelo de banco de dados para o sistema', 'Em andamento', 3),
  ('Desenvolver API', 'Criar API RESTful para o sistema', 'Planejado', 4),
  ('Refatorar c�digo', 'Refatorar c�digo para melhorar desempenho', 'Em andamento', 1),
  ('Adicionar novas funcionalidades', 'Adicionar novas funcionalidades ao sistema', 'Planejado', 2),
  ('Testar sistema', 'Realizar testes unit�rios e de integra��o', 'Em andamento', 3),
  ('Implementar sistema de notifica��es', 'Adicionar sistema de notifica��es ao sistema', 'Planejado', 4),
  ('Otimizar consultas SQL', 'Otimizar consultas SQL para melhorar desempenho', 'Em andamento', 1),
  ('Corrigir bugs', 'Corrigir bugs e erros no sistema', 'Conclu�do', 2);


SELECT u.name, t.*
FROM Users u
JOIN Tasks t ON t.user_id = u.user_id;
