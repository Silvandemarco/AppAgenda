using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        }

        private readonly HttpClient _HttpClient;

        public async Task<Rootobject> BuscarProfissional(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    throw new InvalidOperationException("ID não informado");

                using (var response = await _HttpClient.GetAsync($"http://192.168.0.106/profissionais?id={id}"))
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

                using (var response = await _HttpClient.GetAsync($"http://192.168.0.106/profissionais/{quantidade}"))//192.168.0.106
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

        public async Task<RootServico> BuscarServicos(string profissional)
        {
            try
            {
                using (var response = await _HttpClient.GetAsync($"http://192.168.0.106/servicos?id_profissional={profissional}"))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");

                    var result = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(result))
                        throw new InvalidOperationException("Algo de errado, não de deu certo ao consultar");

                    return JsonConvert.DeserializeObject<RootServico>(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<string>> BuscarHorasLivres(string profissional, string servico, DateTime data)
        {
            string strData = data.ToString("yyyy-MM-dd");
            try
            {
                using (var response = await _HttpClient.GetAsync($"http://192.168.0.106/agenda?id_pessoa={profissional}&data={strData}&servico[]={servico}"))
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
                using (var response = await _HttpClient.PostAsync($"http://192.168.0.106/agenda", content))
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
                using (var response = await _HttpClient.PostAsync($"http://192.168.0.106/validaLogin", content))
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

        public async Task<Resposta> InserirPessoa(Pessoa pessoa)
        {
            string json = JsonConvert.SerializeObject(pessoa);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                using (var response = await _HttpClient.PostAsync($"http://192.168.0.106/pessoas", content))
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
                using (var response = await _HttpClient.GetAsync($"http://192.168.0.106/cidades"))
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
                    using (var response = await _HttpClient.GetAsync($"http://192.168.0.106/validaemail?email={_email}"))
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
    }

    public class Rootobject
    {
        public List<Profissionais> profissionais { get; set; }
    }


    public class Pessoa
    {
        public int id_pessoa { get; set; }
        public string nome { get; set; }
        public string sobrenome { get; set; }
        public string nascimento { get; set; }
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
    }


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
    }

    public class Cidade
    {
        public int id_cidade { get; set; }
        public string nome { get; set; }
        public string uf { get; set; }
        public string pais { get; set; }
    }


    public class RootServico
    {
        public List<Servico> servicos { get; set; }
    }

    public class Servico
    {
        public string id_servico { get; set; }
        public string nome { get; set; }
        public string id_prof_serv { get; set; }
        public string id_profissional { get; set; }
        public string descricao { get; set; }
        public string valor { get; set; }
        public string duracao { get; set; }
    }
    public class Agenda
    {
        public int id_agenda { get; set; }
        public int id_profissional { get; set; }
        public int id_cliente { get; set; }
        public string datetime { get; set; }
        public List<Prof_serv> prof_serv { get; set; }
    }

    public class Prof_serv
    {
        public string id_prof_serv { get; set; }
    }



    public class Resposta
    {
        public bool erro { get; set; }
        public string id { get; set; }
        public string msg { get; set; }
    }

    public class User
    {
        public string email { get; set; }
        public string senha { get; set; }
    }
    /*
    public class Pessoa
    {
        public string id_pessoa { get; set; }
    }
    */
}





