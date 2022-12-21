namespace Cinetix_Api.Models
{
    public class Genre
    {
        public Genre() {}
        public Genre (string name)
        {
            Name = name;
        }
        public Genre(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
