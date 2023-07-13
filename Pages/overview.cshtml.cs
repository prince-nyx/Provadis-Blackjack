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
        
        public void OnPost()
        {
            int.Parse(code);
            int.Parse(GameID);

            
        }
        private Random randomZahl()
        {
            var rnd = new Random();
            rnd.Next();
            return rnd;
        }
        
        private int additionGameID()
        {
            
            return 
        }
    }
}

    
