using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AppAgenda.Clients
{
    class ApiAgendaHttpClient
    {
        private static Lazy<ApiAgendaHttpClient> _Lazy = new Lazy<ApiAgendaHttpClient>(() => new ApiAgendaHttpClient());
        public static ApiAgendaHttpClient Current { get => _Lazy.Value; }

        private ApiAgendaHttpClient()
        {
            _HttpClient = new HttpClient();
            _Dominio = "192.168.0.108";
        }

        private readonly HttpClient _HttpClient;
        private static string _Dominio;

        public async Task<Rootobject> BuscarProfissional(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    throw new InvalidOperationException("ID não informado");

                using (var response = await _HttpClient.GetAsync($"http://{_Dominio}/profissionais?id={id}"))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar o ID");

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar o ID");

                    return JsonConvert.DeserializeObject<Rootobject>(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Rootobject> BuscarProfissionais(string quantidade)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(quantidade))
                    throw new InvalidOperationException("Quantidade não informada");

                using (var response = await _HttpClient.GetAsync($"http://{_Dominio}/profissionais/{quantidade}"))//{_Dominio}
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");

                    return JsonConvert.DeserializeObject<Rootobject>(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Servicos>> BuscarServicos(int profissional)
        {
            try
            {
                using (var response = await _HttpClient.GetAsync($"http://{_Dominio}/servicos?id_profissional={profissional}"))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");

                    return JsonConvert.DeserializeObject<List<Servicos>>(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<string>> BuscarHorasLivres(int profissional, int servico, DateTime data)
        {
            string strData = data.ToString("yyyy-MM-dd");
            try
            {
                using (var response = await _HttpClient.GetAsync($"http://{_Dominio}/agenda?id_pessoa={profissional}&data={strData}&servico[]={servico}"))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");

                    return JsonConvert.DeserializeObject<List<string>>(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Resposta> Agendamento(Agenda agendamento)
        {
            string json = JsonConvert.SerializeObject(agendamento);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                using (var response = await _HttpClient.PostAsync($"http://{_Dominio}/agenda", content))
                {
                    if (!response.StatusCode.Equals("401"))
                    {
                        if (!response.IsSuccessStatusCode)
                            throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");
                    }
                    else
                    {
                        throw new InvalidOperationException("Senha incorreta!");
                    }

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");

                    return JsonConvert.DeserializeObject<Resposta>(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> validaLogin(User user)
        {
            string json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                using (var response = await _HttpClient.PostAsync($"http://{_Dominio}/validaLogin", content))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException("Ops, email ou senha incorreto.");

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Ops, uma falha impediu a atualização de seus dados, verifique sua coneção com a internet.");

                    return JsonConvert.DeserializeObject<bool>(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Resposta> InserirPessoa(Pessoa pessoa)
        {
            string json = JsonConvert.SerializeObject(pessoa);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                using (var response = await _HttpClient.PostAsync($"http://{_Dominio}/pessoas", content))
                {
                    if (!response.StatusCode.Equals("401"))
                    {
                        if (!response.IsSuccessStatusCode)
                            throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");
                    }
                    else
                    {
                        throw new InvalidOperationException("Senha incorreta!");
                    }

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");

                    return JsonConvert.DeserializeObject<Resposta>(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Cidade>> BuscarCidades()
        {
            try
            {
                using (var response = await _HttpClient.GetAsync($"http://{_Dominio}/cidades"))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");

                    return JsonConvert.DeserializeObject<List<Cidade>>(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> EmailUnico(string _email)
        {
            if (!ValidarEmail(_email))
            {
                return false;
            }
            else
            {
                try
                {
                    using (var response = await _HttpClient.GetAsync($"http://{_Dominio}/validaemail?email={_email}"))
                    {
                        if (!response.IsSuccessStatusCode)
                            throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");

                        var result = await response.Content.ReadAsStringAsync();

                        if (string.IsNullOrWhiteSpace(result))
                            throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");

                        return JsonConvert.DeserializeObject<bool>(result);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public bool ValidarEmail(string Email)
        {
            bool ValidEmail = false;
            int indexArr = Email.IndexOf("@");
            if (indexArr > 0)
            {
                if (Email.IndexOf("@", indexArr + 1) > 0)
                {
                    return ValidEmail;
                }

                int indexDot = Email.IndexOf(".", indexArr);
                if (indexDot - 1 > indexArr)
                {
                    if (indexDot + 1 < Email.Length)
                    {
                        string indexDot2 = Email.Substring(indexDot + 1, 1);
                        if (indexDot2 != ".")
                        {
                            ValidEmail = true;
                        }
                    }
                }
            }
            return ValidEmail;
        }

        public async Task<List<HorasDia>> BuscarHorasDia(int idPessoa, string dia)
        {
            try
            {
                using (var response = await _HttpClient.GetAsync($"http://{_Dominio}/horasdia?pessoa={idPessoa}&dia={dia}"))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");

                    return JsonConvert.DeserializeObject<List<HorasDia>>(result, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Resposta> InserirHorasDia(List<HorasDia> horasDias)
        {
            string json = JsonConvert.SerializeObject(horasDias);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                using (var response = await _HttpClient.PostAsync($"http://{_Dominio}/horasdia", content))
                {
                    if (!response.StatusCode.Equals("404"))
                    {
                        if (!response.IsSuccessStatusCode)
                            throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");
                    }

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");

                    return JsonConvert.DeserializeObject<Resposta>(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Resposta> DeletarHorasDia(HorasDia horasDias)
        {
            string json = JsonConvert.SerializeObject(horasDias);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {

                using (var response = await _HttpClient.DeleteAsync($"http://{_Dominio}/horasdia?pessoa={horasDias.id_pessoa}&dia={horasDias.dia_semana}"))
                {
                    if (!response.StatusCode.ToString().Equals("NotFound"))
                    {
                        if (!response.IsSuccessStatusCode)
                            throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");
                    }

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");

                    return JsonConvert.DeserializeObject<Resposta>(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DiaSemana>> BuscarDiasSemana(int idPessoa)
        {
            try
            {
                using (var response = await _HttpClient.GetAsync($"http://{_Dominio}/diasSemana?pessoa={idPessoa}"))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");

                    return JsonConvert.DeserializeObject<List<DiaSemana>>(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Pessoa>> BuscarPessoa(int id)
        {
            try
            {
                if (id == 0)
                    throw new InvalidOperationException("ID não informado");

                using (var response = await _HttpClient.GetAsync($"http://{_Dominio}/pessoas?id={id}"))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar o ID");

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar o ID");

                    return JsonConvert.DeserializeObject<List<Pessoa>>(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Pessoa>> BuscarPessoa(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    throw new InvalidOperationException("E-mail não informado");

                using (var response = await _HttpClient.GetAsync($"http://{_Dominio}/pessoas?email={email}"))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar o ID");

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar o ID");

                    return JsonConvert.DeserializeObject<List<Pessoa>>(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Pessoa>> BuscarProf(string tipo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tipo))
                    throw new InvalidOperationException("Tipo não informado");

                using (var response = await _HttpClient.GetAsync($"http://{_Dominio}/pessoas?tipo={tipo}"))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar.");

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar.");

                    return JsonConvert.DeserializeObject<List<Pessoa>>(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Agenda>> BuscarAgenda(int profissional, DateTime data)
        {
            string strData = data.ToString("yyyy-MM-dd");
            //strData = "2018-09-13";
            try
            {
                using (var response = await _HttpClient.GetAsync($"http://{_Dominio}/agendaProfissional?pessoa={profissional}&data={strData}"))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");

                    return JsonConvert.DeserializeObject<List<Agenda>>(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Agenda>> BuscarAgendaCliente(int cliente, int mes, int ano, int profissional)
        {
            try
            {
                using (var response = await _HttpClient.GetAsync($"http://{_Dominio}/agendaCliente?pessoa={cliente}&mes={mes}&ano={ano}&profissional={profissional}"))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException("Ops, uma falha impediu a atualização de seus dados.");

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Ops, uma falha impediu a atualização de seus dados, verifique sua coneção com a internet.");

                    return JsonConvert.DeserializeObject<List<Agenda>>(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Resposta> CancelarAgendamento(string id_agenda, string status)
        {
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("id_agenda", id_agenda));
            nvc.Add(new KeyValuePair<string, string>("status", status));
            var content = new FormUrlEncodedContent(nvc);
            try
            {
                using (var response = await _HttpClient.PutAsync($"http://{_Dominio}/cancelarAgendamento", content))
                {
                    if (!response.StatusCode.Equals("404"))
                    {
                        if (!response.IsSuccessStatusCode)
                            throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");
                    }

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");

                    return JsonConvert.DeserializeObject<Resposta>(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Meses>> agendaMeses(int cliente)
        {
            try
            {
                using (var response = await _HttpClient.GetAsync($"http://{_Dominio}/agendaMeses?cliente={cliente}"))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException("Ops, uma falha impediu a atualização de seus dados.");

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Ops, uma falha impediu a atualização de seus dados, verifique sua coneção com a internet.");

                    return JsonConvert.DeserializeObject<List<Meses>>(result, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Resposta> InserirServico(Servicos servico)
        {
            string json = JsonConvert.SerializeObject(servico);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                using (var response = await _HttpClient.PostAsync($"http://{_Dominio}/servicos", content))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException("Ops, uma falha impediu a atualização de seus dados.");

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Ops, uma falha impediu a atualização de seus dados, verifique sua coneção com a internet.");

                    return JsonConvert.DeserializeObject<Resposta>(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Resposta> AlterarServico(Servicos servico)
        {
            string json = JsonConvert.SerializeObject(servico);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                using (var response = await _HttpClient.PutAsync($"http://{_Dominio}/servicos", content))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException("Ops, uma falha impediu a atualização de seus dados.");

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Ops, uma falha impediu a atualização de seus dados, verifique sua coneção com a internet.");

                    return JsonConvert.DeserializeObject<Resposta>(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Resposta> DeletarServico(int servico)
        {
            try
            {

                using (var response = await _HttpClient.DeleteAsync($"http://{_Dominio}/servicos?id_servico={servico}"))
                {
                    if (!response.StatusCode.ToString().Equals("NotFound"))
                    {
                        if (!response.IsSuccessStatusCode)
                            throw new InvalidOperationException("Ops, uma falha impediu a exclusão de seus dados.");
                    }

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Ops, uma falha impediu a exclusão de seus dados, verifique sua coneção com a internet.");

                    return JsonConvert.DeserializeObject<Resposta>(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Cliente>> BuscarClientes(int id)
        {
            try
            {
                if (id == 0)
                    throw new InvalidOperationException("ID não informado");

                using (var response = await _HttpClient.GetAsync($"http://{_Dominio}/clientes?id={id}"))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException("Ops, uma falha impediu a atualização de seus dados.");

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Ops, uma falha impediu a atualização de seus dados, verifique sua coneção com a internet.");

                    return JsonConvert.DeserializeObject<List<Cliente>>(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Resposta> RecuperaSenha(string _email)
        {
            
            try
            {
                using (var response = await _HttpClient.GetAsync($"http://{_Dominio}/recuperaSenha?email={_email}"))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException("Ops, uma falha impediu a atualização de seus dados.");

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Ops, uma falha impediu a atualização de seus dados, verifique sua coneção com a internet.");

                    return JsonConvert.DeserializeObject<Resposta>(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }


    public class Pessoa
    {
        public int id_pessoa { get; set; }
        public string nome { get; set; }
        public string sobrenome { get; set; }
        public DateTime nascimento { get; set; }
        public string email { get; set; }
        public string telefone { get; set; }
        public string endereco { get; set; }
        public int numero { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public int id_cidade { get; set; }
        public string cep { get; set; }
        public string tipo { get; set; }
        public string senha { get; set; }
        public Cidade cidade { get; set; }
        public string nomeCompleto
        {
            get { return nome + " " + sobrenome; }
        }
        public string enderecoCompleto
        {
            get => endereco + ", " + bairro + ", " + cidade.nome;
        }
    }

    public class Cliente : Pessoa
    {
        public DateTime ultimo_agendamento { get; set; }
        public int qtd_agendamento { get; set; }
        public string dateUltAndQtd
        {
            get => string.Format("{0} agendamentos, ultimo {1:dd/MM/yyyy H:mm}", qtd_agendamento, ultimo_agendamento);
        }
    }

    public class Cidade
    {
        public int id_cidade { get; set; }
        public string nome { get; set; }
        public string uf { get; set; }
        public string pais { get; set; }
    }


    public class Rootobject
    {
        public List<Profissionais> profissionais { get; set; }
    }


    //public class Pessoa
    //{
    //    public int id_pessoa { get; set; }
    //    public string nome { get; set; }
    //    public string sobrenome { get; set; }
    //    public string nascimento { get; set; }
    //    public string email { get; set; }
    //    public string telefone { get; set; }
    //    public string endereco { get; set; }
    //    public int numero { get; set; }
    //    public string complemento { get; set; }
    //    public string bairro { get; set; }
    //    public int id_cidade { get; set; }
    //    public string cep { get; set; }
    //    public string tipo { get; set; }
    //    public string senha { get; set; }
    //}


    public class Profissionais
    {
        public string id_pessoa { get; set; }
        public string email { get; set; }
        public string telefone { get; set; }
        public string endereco { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string id_cidade { get; set; }
        public string cep { get; set; }
        public string senha { get; set; }
        public string nome { get; set; }
        public string sobrenome { get; set; }
        public string fantasia { get; set; }
        public Cidade cidade { get; set; }
        public string nomeCompleto
        {
            get { return nome + " " + sobrenome; }
        }
    }

    //public class Cidade
    //{
    //    public int id_cidade { get; set; }
    //    public string nome { get; set; }
    //    public string uf { get; set; }
    //    public string pais { get; set; }
    //}


    //public class RootServico
    //{
    //    public List<Servico> servicos { get; set; }
    //}
    ////não usar
    //public class Servico
    //{
    //    public string id_servico { get; set; }
    //    public string nome { get; set; }
    //    public string id_prof_serv { get; set; }
    //    public string id_profissional { get; set; }
    //    public string descricao { get; set; }
    //    public string valor { get; set; }
    //    public string duracao { get; set; }
    //}
    public class Agenda
    {
        public int id_agenda { get; set; }
        public int id_profissional { get; set; }
        public int id_cliente { get; set; }
        //[JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
        public DateTime datetime { get; set; }
        public List<Servicos> servico { get; set; }
        public Servicos servicos { get; set; }
        public Pessoa profissional { get; set; }
        public Pessoa cliente { get; set; }
        public string status { get; set; }
    }

    public class Servicos
    {
        public int id_servico { get; set; }
        public int id_profissional { get; set; }
        public string descricao { get; set; }
        public double valor { get; set; }
        public int duracao { get; set; }
        public string duracaoValor
        {
            get => Convert.ToString(duracao) + " min " + string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", valor);
    }
    }

    //public class Prof_serv
    //{
    //    public int id_prof_serv { get; set; }
    //}



    public class Resposta
    {
        public bool erro { get; set; }
        public int id { get; set; }
        public string msg { get; set; }
    }

    public class User
    {
        public string email { get; set; }
        public string senha { get; set; }
    }
    //return JsonConvert.DeserializeObject<List<HorasDia>>(result, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
    public class HorasDia
    {
        public HorasDia()
        {
            this.id_pessoa = 0;
            this.dia_semana = "";
            this.hora_inicial = new TimeSpan();
            this.hora_final = new TimeSpan();
        }
        public HorasDia(int idPessoa, string dia, TimeSpan horaInicial, TimeSpan horaFinal)
        {
            this.id_pessoa = idPessoa;
            this.dia_semana = dia;
            this.hora_inicial = horaInicial;
            this.hora_final = horaFinal;
        }
        public int id_pessoa { get; set; }
        public string dia_semana { get; set; }
        public TimeSpan hora_inicial { get; set; }
        public TimeSpan hora_final { get; set; }
    }


    public class DiaSemana
    {
        public string dia { get; set; }
        public bool status { get; set; }
    }


    public class Meses
    {
        public int mes { get; set; }
        public int ano { get; set; }
        public string nomeMes { get { return new DateTime(1900, mes, 1).ToString("MMMM", new CultureInfo("pt-BR")) +" "+ Convert.ToString(ano); } }
    }

}





