using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using TesteIndigoNelsimarModel;
using System.Web.UI.HtmlControls;
using OpenQA.Selenium.Support.UI;
using System.IO;

namespace TesteIndigoNelsimar
{
    public partial class frmTesteIndigo : Form
    {
        static IWebDriver driver;
        Dictionary<string, string> enderecoTarefas = new Dictionary<string, string>();


        public frmTesteIndigo()
        {
            InitializeComponent();
        }

        private void btnTeste_Click(object sender, EventArgs e)
        {
            RealizarTarefas();
        }

        private void RealizarTarefas()
        {
            RealizaTarefa1();
            RealizaTarefa2();
            RealizaTarefa3();
            RealizaTarefa4();
            RealizaTarefa5();

            MessageBox.Show("Teste Finalizado! Muito Obrigado pela oportunidade!");
        }

        private void RealizaTarefa5()
        {
            //Acessa e realiza a primeira tarefa
            driver.Navigate().GoToUrl(enderecoTarefas["5"]);
            
            while (ExisteAlerta())
            {
                IAlert alerta = driver.SwitchTo().Alert();
                alerta.Accept();
            }

            IWebElement botaoProxima = driver.FindElement(By.Id("Submit"));
            botaoProxima.Click();

        }

        private bool ExisteAlerta()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        private void RealizaTarefa4()
        {
            //Acessa e realiza a primeira tarefa
            driver.Navigate().GoToUrl(enderecoTarefas["4"]);

            int numero1 = Convert.ToInt32(driver.FindElement(By.Id("numero1")).GetAttribute("value"));
            int numero2 = Convert.ToInt32(driver.FindElement(By.Id("numero2")).GetAttribute("value"));

            int soma = numero1 + numero2;
            int sub = numero1 - numero2;

            IWebElement resultadoSoma = driver.FindElement(By.Id("soma"));
            IWebElement resultadoSub = driver.FindElement(By.Id("subtracao"));

            resultadoSoma.SendKeys(soma.ToString());
            resultadoSub.SendKeys(sub.ToString());

            IWebElement botaoOperacao = driver.FindElement(By.Id("Operacao"));
            botaoOperacao.Click();

            IWebElement mensagem = driver.FindElement(By.Id("divResult"));

            MessageBox.Show(mensagem.Text);

            IWebElement botaoProxima = driver.FindElement(By.Id("Submit"));
            botaoProxima.Click();



        }

        private void RealizaTarefa3()
        {
            //Acessa e realiza a primeira tarefa
            driver.Navigate().GoToUrl(enderecoTarefas["3"]);

            IWebElement nome = driver.FindElement(By.Id("NomeCompleto"));
            nome.SendKeys("Nelsimar Marculino Freitas da Cruz");

            IWebElement email = driver.FindElement(By.Id("Email"));
            email.SendKeys("nelsimar.freitas@gmail.com");

            IWebElement divCheckBox = driver.FindElement(By.ClassName("checkbox"));
            List<IWebElement> checkBoxes = new List<IWebElement>(divCheckBox.FindElements(By.CssSelector("input[type='checkbox']")));

            divCheckBox.FindElement(By.XPath("//label[contains(text(),'Linux')]/input")).Click();
            divCheckBox.FindElement(By.XPath("//label[contains(text(),' MacOs X')]/input")).Click();
            divCheckBox.FindElement(By.XPath("//label[contains(text(),' IBM-DOS')]/input")).Click();

            divCheckBox.FindElement(By.XPath("//label[contains(text(),'Masculino')]/input")).Click();

            IWebElement select = driver.FindElement(By.Id("sel1"));
            List<IWebElement> opcoes = new List<IWebElement>(select.FindElements(By.TagName("option")));

            IWebElement opcaoMarcar = opcoes.First(x => x.Text == "Programador Pleno");

            IWebElement botao = driver.FindElement(By.Id("Submit"));
            botao.Click();
        }

        private void RealizaTarefa2()
        {
            //Acessa e realiza a primeira tarefa
            driver.Navigate().GoToUrl(enderecoTarefas["2"]);

            IWebElement frame = driver.FindElement(By.TagName("iframe"));
            driver.SwitchTo().Frame(frame);

            string texto = driver.FindElement(By.TagName("p")).Text;
            string citacao = driver.FindElement(By.TagName("cite")).Text;

            driver.SwitchTo().ParentFrame();

            IWebElement textarea = driver.FindElement(By.TagName("textarea"));
            textarea.SendKeys(texto);
            textarea.SendKeys("/r/n");
            textarea.SendKeys(" ");
            textarea.SendKeys(citacao);

            IWebElement botao = driver.FindElement(By.Id("Submit"));
            botao.Click();

        }

        private void RealizaTarefa1()
        {
            //Inicializa o objeto de conexão ao site
            driver = new ChromeDriver(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())) + "\\Selenium");
            //Acessa o site
            driver.Navigate().GoToUrl("http://indigo.rafson.com.br");

            //Encontra as propriedades de login
            IWebElement login = driver.FindElement(By.Id("login"));
            IWebElement password = driver.FindElement(By.Id("Password"));
            IWebElement button = driver.FindElement(By.Id("Submit"));

            // Realiza o login no site
            login.SendKeys("rafson.silva");
            password.SendKeys("indigo.2017");
            button.Submit();

            //Recupera a div contendo a lista de tarefas
            IWebElement lista = driver.FindElement(By.ClassName("list-group"));
            //Recupera a lista de tarefas a serem acessadas
            IWebElement[] listaTarefas = lista.FindElements(By.TagName("a")).ToArray();


            int numeroTarefa = 0;
            for (int i = 1; i <= 5; i++)
            {
                enderecoTarefas.Add(i.ToString(), listaTarefas[i].GetAttribute("href"));
            }

            //Acessa e realiza a primeira tarefa
            driver.Navigate().GoToUrl(listaTarefas[1].GetAttribute("href"));

            //REcupera a tabela inteira
            IWebElement tabelaTarefa1 = driver.FindElement(By.ClassName("tg"));


            List<IWebElement> rowsTabelaTarefa1 = new List<IWebElement>(driver.FindElements(By.TagName("tr")));
            List<TabelaTarefa1> listaTabela = new List<TabelaTarefa1>();

            foreach (var item in rowsTabelaTarefa1)
            {
                TabelaTarefa1 novoItem = new TabelaTarefa1();
                List<IWebElement> celulas = new List<IWebElement>(item.FindElements(By.TagName("td")));

                if (celulas.Count > 0)
                {
                    novoItem.Codigo = celulas[0].Text;
                    novoItem.Nome = celulas[1].Text;
                    novoItem.Apelido = celulas[2].Text;
                    novoItem.Trabalho = celulas[3].Text;
                    novoItem.Email = celulas[4].Text;

                    listaTabela.Add(novoItem);
                }

            }

            //Popula o campo quantidade
            IWebElement qtdeItensTabela = driver.FindElement(By.Id("login"));
            qtdeItensTabela.SendKeys(listaTabela.Count.ToString());

            // Encontra as textareas
            IWebElement[] textAreas = driver.FindElements(By.TagName("textarea")).ToArray();

            // Ordena Por Codigo
            listaTabela = listaTabela.OrderBy(x => x.Codigo).ToList();

            //Popula a primeira textarea
            foreach (var item in listaTabela)
            {
                HtmlTableCell celulaCodigo = new HtmlTableCell();

                textAreas[0].SendKeys(item.Codigo + "  ");
                textAreas[0].SendKeys(item.Nome + "  ");
                textAreas[0].SendKeys(item.Apelido + "  ");
                textAreas[0].SendKeys(item.Trabalho + "  ");
                textAreas[0].SendKeys(item.Email + "  ");
                textAreas[0].SendKeys(Environment.NewLine);
            }

            // Ordena Por Aelido
            listaTabela = listaTabela.OrderBy(x => x.Apelido).ToList();

            //Popula a primeira textarea
            foreach (var item in listaTabela)
            {
                HtmlTableCell celulaCodigo = new HtmlTableCell();

                textAreas[1].SendKeys(item.Codigo + "  ");
                textAreas[1].SendKeys(item.Nome + "  ");
                textAreas[1].SendKeys(item.Apelido + "  ");
                textAreas[1].SendKeys(item.Trabalho + "  ");
                textAreas[1].SendKeys(item.Email + "  ");
                textAreas[1].SendKeys(Environment.NewLine);
            }

            //Avança para próxima tarefa
            IWebElement botaoPrximaTarefa1 = driver.FindElement(By.Id("Submit"));
            botaoPrximaTarefa1.Click();
        }
    }
}
