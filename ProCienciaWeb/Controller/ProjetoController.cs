using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProCienciaWeb.Models;

namespace ProCienciaWeb.Controller
{
    public class ProjetoController : ControllerBase
    {
        public const string UrlServico = "https://apiprociencia.azurewebsites.net";
        public static HttpClient client = new HttpClient();

        public static async Task<ObservableCollection<Projeto>> ObterProjetos()
        {
            string url = UrlServico + "/api/Projetos";
            var response = await client.GetStringAsync(url);

            ObservableCollection<Projeto> listaProjetos = JsonConvert.DeserializeObject<ObservableCollection<Projeto>>(response);

            return listaProjetos;
        }
    }
}
