using System;

namespace RabbitMqFileExport.Model
{
    public class Company
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string TaxNo { get; set; }
    }
}
