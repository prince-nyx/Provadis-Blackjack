using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace BlackJack.Pages
{
    public class RegisterModel : PageModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Cpass { get; set; }
        public DateTime Birth { get; set; }
        public int? UserMoney = 100;
        private readonly SqlConnection conn = new("Server=provadis-it-ausbildung.de;Database=BlackJack02;UID=BlackJackUser02;PWD=Pr@vadis_188_Pta;");
        private SqlCommand? cmd;
        private SqlDataReader? reader;
        private readonly SqlDataAdapter? adapter = new SqlDataAdapter();

        public void OnPost()
        {
			this.Username = Request.Form["username"];
			this.Password = Request.Form["password"];
            this.Cpass = Request.Form["cpass"];
            this.Birth = Convert.ToDateTime(Request.Form["birth"]);

            if(this.IsUserCharValid() && this.IsPassEQCheckPass() && this.IsBirthValid() && this.IsUserNameValid() && this.IsPasswordValid()) { this.CreateNewUser(); }
            else { Console.WriteLine("Invalid User"); }
        }

        private bool IsUserCharValid()
        {
            return Regex.IsMatch(this.Username, "[*@:=\\/[?\\]|\\\\\"<>+;]") ? false : true;
        }

        private bool IsPassEQCheckPass()
        {
            return this.Password == this.Cpass ? true : false;
        }

        private bool IsBirthValid()
        {
            return this.Birth.CompareTo(DateTime.Today.AddYears(-18)) <= 0 ? true : false;
		}

        private bool IsUserNameValid() 
        {
            bool isValid = false;
            string sql = "SELECT COUNT(*) FROM Benutzer WHERE Username='" + this.Username + "'";

            this.conn.Open();
            this.cmd = new SqlCommand(sql, this.conn);
            this.reader = this.cmd.ExecuteReader();
            
            while (this.reader.Read()) { isValid = Convert.ToInt32(this.reader.GetValue(0)) < 1 ? true : false; }
            
            this.reader.Close();
            this.cmd.Dispose();
            this.conn.Close();
            
            return isValid;
        }

        private bool IsPasswordValid()
        {
            return this.Password.Length >= 8 ? true : false;
        }

        private void CreateNewUser()
		{
            try
            {
                Encrypt encrypt = new();
				
                string sql = "INSERT INTO Benutzer(Username, Passwort, GeldAnzahl, Geburtsdatum) VALUES('" + this.Username + "', '" + encrypt.GetHashedPassword(this.Password) + "', " + this.UserMoney + ", '" + this.Birth.ToShortDateString() + "')";

                this.conn.Open();
				this.cmd = new SqlCommand(sql, this.conn);
				this.adapter.UpdateCommand = new SqlCommand(sql, this.conn);
				
                this.adapter.UpdateCommand.ExecuteNonQuery();
				
                this.cmd.Dispose();
				this.conn.Close();
                
                Console.WriteLine("INSERT SUCCESS");
			}
            catch(Exception ex)
            {
                Console.WriteLine(ex);
			}
        }
	}
}