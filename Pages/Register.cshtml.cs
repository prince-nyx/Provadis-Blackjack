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

			if (this.IsUserCharValid() && this.IsPassEQCheckPass() && this.IsBirthValid() && this.IsUserNameValid() && this.IsPasswordValid()) { 
                this.CreateNewUser();
				return RedirectToPage("/login");


			}
            else {
				ModelState.AddModelError("", "Invalid username or password."); // Add a model error for displaying the error message on the page
				return Page();
				Console.WriteLine("Invalid User");
            }
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
            int age = DateTime.Today.Year - this.Birth.Year;

            if (age >= 18)
            {
                return true;
            }
            else
            {
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