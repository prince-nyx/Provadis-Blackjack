
using System.Data.SqlClient;

namespace BlackJack
{

    public class Settings
    {

        private SqlCommand? cmd;
        private SqlDataReader? reader;
        private readonly SqlDataAdapter? adapter = new SqlDataAdapter();
        private readonly SqlConnection conn = new("Server=provadis-it-ausbildung.de;Database=BlackJack02;UID=BlackJackUser02;PWD=Pr@vadis_188_Pta;");
        public int betTime { get; set; }
        public int turnTime { get; set; }
        public int potLimit { get; set; }

        public Settings()
        {
            int betTime = 20;
            int turnTime = 60;
            int potLimit = 25;
        }

        public void load()
        {

            string sql = "SELECT DISTINCT * FROM Setting";

            this.conn.Open();
            this.cmd = new SqlCommand(sql, this.conn);
            this.reader = this.cmd.ExecuteReader();
            Player player = null;
            while (this.reader.Read())
            {

                
            }

            this.cmd.ExecuteNonQuery();

            this.cmd.Dispose();
            this.conn.Close();
        }

    }
}
