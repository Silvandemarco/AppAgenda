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

                using (var response = await _HttpClient.GetAsync($"http://192.168.0.105/profissionais/{quantidade}"))//192.168.0.106
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


    }

    public class Rootobject
    {
        public List<Profissionais> profissionais { get; set; }
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
        public string id_cidade { get; set; }
        public string nome { get; set; }
        public string uf { get; set; }
        public string pais { get; set; }
    }

}
