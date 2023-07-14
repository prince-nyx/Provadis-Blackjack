using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace BlackJack.Pages
{
    public class Index1Model : PageModel
    {
        public string? code { get; set; }
        public string? GameID { get; set; }
        private readonly SqlConnection conn = new("Server=provadis-it-ausbildung.de;Database=BlackJack02;UID=BlackJackUser02;PWD=Pr@vadis_188_Pta;");
        private SqlCommand? cmd;
        private SqlDataReader? reader;
        private readonly SqlDataAdapter? adapter = new SqlDataAdapter();

        public void OnGet()
        {
            //START ACCESS CHECK
            String userid = Request.Cookies["userid"];
            String result = Program.app.checkAccess(userid);
            if (!result.Equals("/overview"))
            {
                Response.Redirect(result);
            }
            //END ACCESS CHECK
        }
        public void OnPost()
        {
            code = Request.Form["code"];
            GameID = Request.Form["GameID"];
            int.Parse(code);
        }
        public int randomZahl()
        {
            var rnd = new Random();
            this.code = rnd.Next(1,100000);
            return code;
        }
        public void CreateNewGameCode()
        {
            try
            {
                Encrypt encrypt = new();

                string sql = "INSERT INTO Game(Code,GameID) VALUES('" + this.code + ", '" + this.GameID + "')";

                this.conn.Open();
                this.cmd = new SqlCommand(sql, this.conn);
                this.adapter.UpdateCommand = new SqlCommand(sql, this.conn);

                this.adapter.UpdateCommand.ExecuteNonQuery();

            Console.WriteLine(Program.app.playerManager.getPlayer(Request.Cookies["userid"]).username);
            Console.WriteLine();


        }
        private Random randomZahl()
        {
            var rnd = new Random();
            rnd.Next();
            return rnd;
        }

    }
}

    
