using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
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
		
        public void OnGet()
        {
            Console.WriteLine("Check!");
        }

        public void OnPost()
        {
			this.Username = Request.Form["username"];
			this.Password = Request.Form["password"];
            this.Cpass = Request.Form["cpass"];
            this.Birth = Convert.ToDateTime(Request.Form["birth"]);
            Console.WriteLine("Username: " + IsUserCharValid() + " : " + IsUserNameValid());
            Console.WriteLine("Password: " + IsPassEQCheckPass() + " : " + IsPasswordValid());
            Console.WriteLine("Birth: " + IsBirthValid());
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
				string sql = "INSERT INTO Benutzer(Username, Passwort, GeldAnzahl, Geburtsdatum) VALUES('" + this.Username + "', '" + this.GetHashedPassword() + "', " + this.UserMoney + ", '" + this.Birth.ToShortDateString() + "')";
                Console.WriteLine(sql);
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

        private string GetHashedPassword()
        {
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(this.Password);
            byte[] passwordBytes = Encoding.UTF8.GetBytes("test");

            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
			byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);
			string encryptedResult = Convert.ToBase64String(bytesEncrypted);
            return encryptedResult;
		}

		private byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
		{
			byte[] encryptedBytes = null;
			byte[] saltBytes = new byte[] { 2, 1, 7, 3, 6, 4, 8, 5 };

			using (MemoryStream ms = new MemoryStream())
			{
				using (RijndaelManaged AES = new RijndaelManaged())
				{
					AES.KeySize = 256;
					AES.BlockSize = 128;

					var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
					AES.Key = key.GetBytes(AES.KeySize / 8);
					AES.IV = key.GetBytes(AES.BlockSize / 8);

					AES.Mode = CipherMode.CBC;

					using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
						cs.Close();
					}
					encryptedBytes = ms.ToArray();
				}
			}

			return encryptedBytes;
		}
	}
}