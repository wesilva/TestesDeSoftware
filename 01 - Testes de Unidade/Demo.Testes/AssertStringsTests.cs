using Xunit;

namespace Demo.Testes
{
    public class AssertStringsTests
    {
        [Fact]
        public void StringsTools_UnirNomes_RetornarNomeCompleto()
        {
            var stringsTools = new StringsTools();

            var nomeCompleto = stringsTools.Unir("Wellington", "Carvalho");

            Assert.Equal("Wellington Carvalho", nomeCompleto);
        }

        [Fact]
        public void StringsTools_UnirNomes_DeveIgnorarCase()
        {
            var stringsTools = new StringsTools();

            var nomeCompleto = stringsTools.Unir("Wellington", "Carvalho");

            Assert.Equal("WELLINGTON CARVALHO", nomeCompleto, true);
        }

        [Fact]
        public void StringsTools_UnirNomes_DeveConterTrecho()
        {
            var stringsTools = new StringsTools();

            var nomeCompleto = stringsTools.Unir("Wellington", "Carvalho");

            Assert.Contains("gton Car", nomeCompleto);
        }

        [Fact]
        public void StringsTools_UnirNomes_DeveComecarCom()
        {
            var stringsTools = new StringsTools();

            var nomeCompleto = stringsTools.Unir("Wellington", "Carvalho");

            Assert.StartsWith("Well", nomeCompleto);
        }

        [Fact]
        public void StringsTools_UnirNomes_DeveAcabarCom()
        {
            var stringsTools = new StringsTools();

            var nomeCompleto = stringsTools.Unir("Wellington", "Carvalho");

            Assert.EndsWith("lho", nomeCompleto);
        }

        [Fact]
        public void StringsTools_UnirNomes_ValidarExpressaRegular()
        {
            var stringsTools = new StringsTools();

            var nomeCompleto = stringsTools.Unir("Wellington", "Carvalho Silva");

            Assert.Matches("[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+", nomeCompleto);
        }
    }
}