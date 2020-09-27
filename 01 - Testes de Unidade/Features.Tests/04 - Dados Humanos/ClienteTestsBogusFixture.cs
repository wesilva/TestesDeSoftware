using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Bogus.DataSets;
using Xunit;

namespace Features.Tests
{
    [CollectionDefinition(nameof(ClienteBogusCollection))]
    public class ClienteBogusCollection : ICollectionFixture<ClienteTestsBogusFixture>
    { }

    public class ClienteTestsBogusFixture : IDisposable
    {
        public Cliente GerarClienteValido()
        {
            return GerarClientes(1, true).FirstOrDefault();
        }

        public IEnumerable<Cliente> ObterClientesVariados()
        {
            var clientes = new List<Cliente>();

            clientes.AddRange(GerarClientes(50, true).ToList());
            clientes.AddRange(GerarClientes(50, false).ToList());

            return clientes;
        }

        public IEnumerable<Cliente> GerarClientes(int quantidade, bool ativo)
        {
            var genero = new Faker().PickRandom<Name.Gender>();

            //var email = new Faker().Internet.Email("Wellington", "Carvalho", "gmail");
            //var clienteFaker = new Faker<Cliente>();
            //clienteFaker.RuleFor(c => c.Nome, (f, c) => f.Name.FirstName());

            var cliente = new Faker<Cliente>("pt_BR")
                .CustomInstantiator(f => new Cliente(
                    Guid.NewGuid(), 
                    f.Name.FirstName(genero),
                    f.Name.LastName(genero),
                    f.Date.Past(2, DateTime.Now.AddYears(-18)),
                    "",
                    ativo,
                    DateTime.Now))
                .RuleFor(c => c.Email, (f, c) =>
                    f.Internet.Email(c.Nome.ToLower(), c.Sobrenome.ToLower()));

            return cliente.Generate(quantidade);
        }

        public Cliente GerarClienteInvalido()
        {
            var genero = new Faker().PickRandom<Name.Gender>();

            var cliente = new Faker<Cliente>("pt_BR")
                .CustomInstantiator(f => new Cliente(
                    Guid.NewGuid(),
                    f.Name.FirstName(genero),
                    f.Name.LastName(genero),
                    f.Date.Past(15, DateTime.Now.AddYears(-1)),
                    "",
                    false,
                    DateTime.Now));

            return cliente;
        }

        public void Dispose()
        {
        }
    }
}