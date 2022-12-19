using Microsoft.EntityFrameworkCore;

namespace Cinetix_Api.Models
{
    public class User
    {
        public User() { }
        public User(int Id, string Fullname, string Email, string Password)
        {
            this.Id = Id;
            this.Fullname = Fullname;
            this.Email = Email;
            this.Password = Password;
        }
        public User(string Fullname, string Email, string Password)
        {
            this.Fullname = Fullname;
            this.Email = Email;
            this.Password = Password;
        }

        public long Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
