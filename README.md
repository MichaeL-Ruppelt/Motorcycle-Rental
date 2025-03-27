# MotorcicleRental

Essa é uma aplicação de alugueis de motocicletas, cadastros de motos e entregadores.

Foi desenvolvido como parte de um teste, porém inevitávelmente foi inserido alguns recursos das quais já tinha interesse em aplicar, porém o tempo não me permitiu evoluir como gostaria.

## Índice

- [Pré-requisitos](#pré-requisitos)
- [Executando a aplicação](#executando-a-aplicação)
  - [Subindo recursos com Docker Compose](#subindo-recursos-com-docker-compose)
  - [Executando a aplicação localmente](#executando-a-aplicação-localmente)
- [Controllers e Endpoints](#controllers-e-endpoints)
- [Observações do Desenvolvedor](#observações-do-desenvolvedor)

## Pré-requisitos

Certifique-se de ter os seguintes requisitos instalados:

- [Visual Studio](https://visualstudio.microsoft.com/pt-br/downloads/) ou [VsCode](https://code.visualstudio.com/download)
- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)
- [.NET SDK 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [PostgreSQL Client](https://www.postgresql.org/download/) (Recomendo o Dbeaver)


## Executando a aplicação

### Subindo recursos com Docker Compose

Para inicializar os serviços necessários (PostgreSQL e RabbitMQ), execute:

```sh
docker-compose up -d
```

Isso iniciará os serviços no background.

Verifique se os containers estão rodando corretamente:

```sh
docker ps
```

Se precisar parar os serviços, use:

```sh
docker-compose down
```

### Executando a aplicação localmente

Após subir os recursos necessários, execute a aplicação:

```sh
dotnet run --project MotorCycleRentail.Api --urls "http://localhost:5000;https://localhost:5001"
```
```sh
dotnet run --project MotorCycleRentail.Worker
```

A API estará disponível em: `http://localhost:5001` e `http://localhost:5001`.

Com Swagger Habilitado em: `https://localhost:5001/swagger/index.html`

Se executar a aplição direto pelo IIS Express, o navegador deverá abrir automáticamente. Caso não ocorra a porta padrão do iss é `https://localhost:44361/swagger/index.html`

### Executando os testes
Se quiser rodar os testes execute o comando abaixo na raiz do projeto de teste unitário:

```sh
dotnet test MotorCycleRentail.Test.Unit
```

## Controllers e Endpoints

Foi inserido no projeto  uma collection do postman com todos os endpoints.

A collection, constam com variáveis para otimizar algumas consultas, porém para testes repetidos será necessário atualizar as variáveis manualmente.

Cada request está numerado, seguindo uma ordem intuitíva de cada etapa para testar.



## Observações do Desenvolvedor

- Para facilitar a execução escolhi deixar o appsettings preenchido, me assegurei de não haver nenhum dado sensível nesse teste. (Obviamente em cenários reais, implementaria um KeyVault.)
- A aplicação executa automaticamente as migrations quando ela carrega, preenchendo também a tabela que requer dados com carga. (Planos de Alugueis e seus valores).
- Utilizei a ferramenta de monitoramento do RabbitMQ para verificar as filas: `http://localhost:15672` (usuário: guest, senha: guest).
- A aplicação faz uma verificação de Token de autenticação, porém configurei apenas para que ele ler e logar, não ví sentido em bloquear requisições nesse teste.
- Em caso de erro ao conectar no PostgreSQL, verifique se a string de conexão está correta no `appsettings.Development.json`.
- Observei que diversas regras de negócio são relativamente conflitante (como o fato de receber um identificar para a entidade que está sendo criada), escolhi manter o comportamente o mais próximo dos casos de uso e do Swagger de exemplo. 
- Talvez a implementação assuste um pouco, mas confesso que aproveitei para implementar alguns patterns e estruturas que queria a algum tempo, como a implementação dinâmica de seviços, repositories e casos de uso.
- Implementei o padrão soft-delete, de modo que nenhum registro é realmente apagado.

---

Caso tenha dúvidas, sugestões ou precise de suporte, entre em contato com o time de desenvolvimento!


