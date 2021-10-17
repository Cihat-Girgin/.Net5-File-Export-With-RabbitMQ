namespace RabbitMqFileExport.Model
{
    public class ExcelFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public EFileStatus status  { get; set; }
    }
}