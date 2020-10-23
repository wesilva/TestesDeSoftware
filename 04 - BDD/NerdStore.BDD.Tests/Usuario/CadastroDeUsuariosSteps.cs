﻿using NerdStore.BDD.Tests.Config;
using TechTalk.SpecFlow;
using Xunit;

namespace NerdStore.BDD.Tests.Usuario
{
    [Binding]
    [CollectionDefinition(nameof(AutomacaoWebFixtureCollection))]
    public class CadastroDeUsuariosSteps
    {
        private readonly CadastroDeUsuarioTela _cadastroDeUsuarioTela;
        private readonly AutomacaoWebTestsFixture _testsFixture;

        public CadastroDeUsuariosSteps(AutomacaoWebTestsFixture testsFixture)
        {
            _testsFixture = testsFixture;
            _cadastroDeUsuarioTela = new CadastroDeUsuarioTela(testsFixture.BrowserHelper);
        }

        [When(@"Ele clicar em registrar")]
        public void QuandoEleClicarEmRegistrar()
        {
            // Act
            _cadastroDeUsuarioTela.ClicarNoLinkRegistrar();

            // Assert
            Assert.Contains(_testsFixture.Configuration.RegisterUrl, _cadastroDeUsuarioTela.ObterUrl());
        }
        
        [When(@"Preencher os dados do formulário")]
        public void QuandoPreencherOsDadosDoFormulario(Table table)
        {
            // Arrange
            _testsFixture.GerarDadosUsuario();
            var usuario = _testsFixture.Usuario;

            // Act
            _cadastroDeUsuarioTela.PreencherFormularioRegistro(usuario);

            // Assert
            Assert.True(_cadastroDeUsuarioTela.ValidarPreenchimentoFormularioRegistro(usuario));
        }
        
        [When(@"Clicar no botão registrar")]
        public void QuandoClicarNoBotaoRegistrar()
        {
            _cadastroDeUsuarioTela.ClicarNoBotaoRegistrar();
        }
        
        [When(@"Preencher os dados do formulário com uma senha sem maiusculas")]
        public void QuandoPreencherOsDadosDoFormularioComUmaSenhaSemMaiusculas(Table table)
        {
           // Arrange
           _testsFixture.GerarDadosUsuario();
           var usuario = _testsFixture.Usuario;
           usuario.Senha = "teste123@";

           // Act
           _cadastroDeUsuarioTela.PreencherFormularioRegistro(usuario);

           // Assert
           Assert.True(_cadastroDeUsuarioTela.ValidarPreenchimentoFormularioRegistro(usuario));
        }
        
        [When(@"Preencher os dados do formulário com uma senha sem caractere especial")]
        public void QuandoPreencherOsDadosDoFormularioComUmaSenhaSemCaractereEspecial(Table table)
        {
            // Arrange
            _testsFixture.GerarDadosUsuario();
            var usuario = _testsFixture.Usuario;
            usuario.Senha = "Teste123";

            // Act
            _cadastroDeUsuarioTela.PreencherFormularioRegistro(usuario);

            // Assert
            Assert.True(_cadastroDeUsuarioTela.ValidarPreenchimentoFormularioRegistro(usuario));
        }

        [Then(@"Ele receberá uma mensagem de erro que a senha precisa conter uma letra maiuscula")]
        public void EntaoEleReceberaUmaMensagemDeErroQueASenhaPrecisaConterUmaLetraMaiuscula()
        {
            Assert.True(_cadastroDeUsuarioTela
                .ValidarMensagemDeErroFormulario("Passwords must have at least one uppercase ('A'-'Z')"));
        }
        
        [Then(@"Ele receberá uma mensagem de erro que a senha precisa conter um caractere especial")]
        public void EntaoEleReceberaUmaMensagemDeErroQueASenhaPrecisaConterUmCaractereEspecial()
        {
            Assert.True(_cadastroDeUsuarioTela
                .ValidarMensagemDeErroFormulario("Passwords must have at least one non alphanumeric character"));
        }
    }
}
