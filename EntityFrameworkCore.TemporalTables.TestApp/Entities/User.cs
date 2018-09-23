namespace EntityFrameworkCore.TemporalTables.TestApp.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
        
        public string IsDeleted { get; set; }
    }
}
