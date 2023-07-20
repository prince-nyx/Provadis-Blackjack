using Azure.Identity;
using BlackJack.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using System.Data.SqlClient;
using System.Numerics;
using System.Text.RegularExpressions;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Mvc;

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
        private FrontendEvent frontend;
		private static readonly HttpClient client = new HttpClient();
		public void OnGet()
        {
            //START ACCESS CHECK
            String userid = Request.Cookies["userid"];
            String result = Program.app.checkAccess(userid);
            if (result.Equals("/index"))
            {
                Response.Cookies.Delete("userid");
            }
            else
            {
                Response.Redirect(result);
            }
            //END ACCESS CHECK
        }
        public IActionResult OnPost()
		{
			
			Console.WriteLine(Request.Cookies["userid"]);
			this.Username = Request.Form["username"];
			this.Password = Request.Form["password"];
            this.Cpass = Request.Form["cpass"];
            this.Birth = Convert.ToDateTime(Request.Form["birth"]);

			if (this.IsUsernameLengthValid() && this.IsUserCharValid() && this.IsPassEQCheckPass() && this.IsBirthValid() && this.IsUserNameValid() && this.IsPasswordValid()) { 
                this.CreateNewUser();
				return RedirectToPage("/login");


			}
            else {
				return Page();
            }
        }

        private bool IsUsernameLengthValid()
		{
			Boolean valid = (this.Username.Length >= 3);
			if (!valid)
				ModelState.AddModelError("username", "Benutzername zu kurz");
			return valid;

		}


		private bool IsUserCharValid()
        {
            Boolean valid = Regex.IsMatch(this.Username, "[*@:=\\/[?\\]|\\\\\"<>+;]");
            if(valid)
				ModelState.AddModelError("username", "Benutzername enthält ungültige Zeichen.");
            return !valid;
		}

        private bool IsPassEQCheckPass()
        {
			Boolean valid = this.Password == this.Cpass;
			if (!valid)
				ModelState.AddModelError("Cpass", "Passwörter stimmen nicht überein");
			return valid;
		}

        private bool IsBirthValid()
        {
            int age = DateTime.Today.Year - this.Birth.Year;

            if (age >= 18)
            {
                return true;
            }
            else
			{
				ModelState.AddModelError("birth", "zu jung.");
				return false;
            }
            
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
            
            if(!isValid)
				ModelState.AddModelError("username", "Benutzername bereits vergeben.");
			return isValid;
        }

        private bool IsPasswordValid()
        {
            Boolean valid = (this.Password.Length >= 8);
			if (!valid)
				ModelState.AddModelError("password", "Passwort zu kurz");
			return valid;
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