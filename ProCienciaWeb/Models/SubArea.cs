namespace ProCienciaWeb.Models
{
    public class SubArea
    {
        //Propriedades
        public int SubAreaId { get; set; }
        public string Nome { get; set; }

        //[ForeignKey("AreaId")]
        public virtual Area Area { get; set; }
        public int AreaId { get; set; }
    }
}