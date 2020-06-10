namespace ProCienciaWeb.Models
{
    public class Projeto
    {
        //Propriedades
        public int ProjetoId { get; set; }
        public string Titulo { get; set; }
        public string Resumo { get; set; }
        public string Autor { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }

        //[ForeignKey("AreaId")]
        public virtual Area Area { get; set; }
        public int AreaId { get; set; }

        //[ForeignKey("CodSubArea")]
        public virtual SubArea SubArea { get; set; }
        public int SubAreaId { get; set; }
    }
}