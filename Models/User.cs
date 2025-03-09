namespace lessson1.Models;



    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }=string.Empty;
        public string? Permission{ get; set;}=string.Empty;
        public string Password { get; set; }=string.Empty;
        public string? Address { get; set; }=string.Empty;
        public string? Email {get; set; }=string.Empty;
        public string? Phone {get; set; }=string.Empty;
        public List<Jewel>? Cart { get; set; }

        
    }
