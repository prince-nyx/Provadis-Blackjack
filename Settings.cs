
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
        public double potLimit { get; set; }
        public double maxEinzahlung { get; set; }
        public double startguthaben { get; set; }

        public Settings()
        {
            betTime = 20;
            turnTime = 60;
            potLimit = 25;
            maxEinzahlung = 100;
            startguthaben = 100;
            load();
        }


        public void load()
        {

            string sql = "SELECT DISTINCT * FROM gamesettings";

            this.conn.Open();
            this.cmd = new SqlCommand(sql, this.conn);
            this.reader = this.cmd.ExecuteReader();
            Player player = null;
            while (this.reader.Read())
            {
                betTime = reader.GetInt32(0);
                turnTime = reader.GetInt32(1);
                potLimit = reader.GetDouble(2);
                maxEinzahlung = reader.GetDouble(3);
                startguthaben = reader.GetDouble(4);

            }


            this.reader.Close();
            this.cmd.Dispose();
            this.conn.Close();
        }



        public override string ToString()
        {
            return "Settings(betTime:" + betTime + " /turntime:" + turnTime + " /potlimit:" + potLimit + " /maxeinzahlung:" + maxEinzahlung+" /startguthaben:"+startguthaben+")";
        }

    }
}
