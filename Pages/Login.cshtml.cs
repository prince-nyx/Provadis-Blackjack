using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BlackJack.Pages
{
    public class LoginModel : PageModel
    {
		public string? Username { get; set; }
		public string? Password { get; set; }
		private readonly SqlConnection conn = new("Server=provadis-it-ausbildung.de;Database=BlackJack02;UID=BlackJackUser02;PWD=Pr@vadis_188_Pta;");
		private SqlCommand? cmd;
		private SqlDataReader? reader;

		public void OnPost()
        {
			this.Username = Request.Form["username"];
			this.Password = Request.Form["password"];

			try
            {
                if (this.IsLoginValid())
                {
                    Console.WriteLine("Valid");
                    //TODO: Session
                }
                else { Console.WriteLine("Invalid"); }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        private bool IsLoginValid()
        {
            Encrypt encrypt = new();

            bool isValid = false;
            string sql = "SELECT COUNT(*) FROM Benutzer WHERE Username ='" + this.Username + "' AND Passwort='" + encrypt.GetHashedPassword(this.Password) + "'";
            
            this.conn.Open();
            this.cmd = new SqlCommand(sql, this.conn);
            this.reader = this.cmd.ExecuteReader();

            while (this.reader.Read()) { isValid = Convert.ToInt32(this.reader.GetValue(0)) == 1 ? true : false; Console.WriteLine(this.reader.GetValue(0)); }

            this.reader.Close();
            this.cmd.Dispose();
            this.conn.Close();

            return isValid;
        }
    }
}