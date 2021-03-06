﻿using Newtonsoft.Json;
using ProCienciaWeb.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text;

namespace ProCienciaWeb.API
{
    public class ApiService
    {
        public const string UrlServico = "https://apiprociencia.azurewebsites.net";
        public static HttpClient client = new HttpClient();

        public async Task<ObservableCollection<Projeto>> ObterProjetos()
        {
            string url = UrlServico + "/api/Projetos";
            var response = await client.GetStringAsync(url);

            ObservableCollection<Projeto> listaProjetos = JsonConvert.DeserializeObject<ObservableCollection<Projeto>>(response);

            return listaProjetos;
        }

        public async Task<Projeto> ObterProjeto(int projetoId)
        {
            string url = UrlServico + "/api/Projetos/" + projetoId.ToString();
            var response = await client.GetStringAsync(url);

            Projeto projeto = JsonConvert.DeserializeObject<Projeto>(response);

            return projeto;
        }

        public async Task IncluirProjeto(Projeto projeto)
        {
            string url = UrlServico + "/api/Projetos/";

            var json = JsonConvert.SerializeObject(projeto);
            var dados = new StringContent(json, Encoding.UTF8, "application/json");

            await client.PostAsync(url, dados);
        }

        public async Task<ObservableCollection<Area>> ObterAreas()
        {
            string url = UrlServico + "/api/Areas";
            var response = await client.GetStringAsync(url);

            ObservableCollection<Area> listaAreas = JsonConvert.DeserializeObject<ObservableCollection<Area>>(response);

            return listaAreas;
        }

        public async Task<ObservableCollection<SubArea>> ObterSubAreas()
        {
            string url = UrlServico + "/api/SubAreas";
            var response = await client.GetStringAsync(url);

            ObservableCollection<SubArea> listaSubAreas = JsonConvert.DeserializeObject<ObservableCollection<SubArea>>(response);

            return listaSubAreas;
        }

        public async Task<ObservableCollection<Instituicao>> ObterInstituicoes()
        {
            string url = UrlServico + "/api/Instituicoes";
            var response = await client.GetStringAsync(url);

            ObservableCollection<Instituicao> listaInstituicoes = JsonConvert.DeserializeObject<ObservableCollection<Instituicao>>(response);

            return listaInstituicoes;
        }
    }
}
