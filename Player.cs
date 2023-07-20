using BlackJack;
using BlackJack.Hubs;
using System.Data.SqlClient;

public class Player
{
	public String id { get; }
    public String currentGameId { get; set; }
    public double wallet { get; set; }
    public String username { get; }
    public CardDeck hand { get; }

    public GameHub hub { get; set; }
    public String connectionId { get; set; }

    public List<FrontendEvent> events = new List<FrontendEvent>();

    public double bet;

    private SqlCommand? cmd;
    private SqlDataReader? reader;
    private readonly SqlDataAdapter? adapter = new SqlDataAdapter();
    private readonly SqlConnection conn = new("Server=provadis-it-ausbildung.de;Database=BlackJack02;UID=BlackJackUser02;PWD=Pr@vadis_188_Pta;");

    public Player(String username, double wallet)
    {
        this.id = Program.app.GenerateRandomString(8);
        this.username = username;
		this.wallet = wallet;
        this.currentGameId = "";
        this.hand = new CardDeck();
        bet = 0;
    }

	public int getPoints()
	{
		return hand.BlackJackSum();
	}

	public override String ToString()
	{
		return "Player(id:"+id+" / u:"+username+" / game: "+currentGameId+")";
	}


    public void addCard(Card card) {
		hand.addCard(card);
    }

	public int getHandSize()
	{
		return hand.size();
	}

    public override bool Equals(object obj)
    { 
        if (obj == null || !(obj is Player))
        {
            return false;
        }
        Player otherPlayer = (Player)obj;

        return this.username == otherPlayer.username;
    }

    public void registerEvent(FrontendEvent frontend)
    {
        Console.WriteLine("[EVENTS] "+username.ToUpper()+" registers "+ frontend.eventName+ "(" + string.Join(",", frontend.args) + ");");
        events.Add(frontend);
    }

    public double getBet()
    {
        return bet;
    }

    public void AddBet(double amount)
    {
        this.bet += amount;
        this.wallet -= amount;
        updateWallet();
    }

    public void resetBet()
    {
        bet = 0;
    }

    public List<>

    public void AddWallet(double amount)
    {
        this.wallet += amount;
        updateWallet();
    }
    private void updateWallet()
    {
        string sql = "UPDATE Benutzer SET GeldAnzahl = @GeldAnzahl WHERE username = @username";

        this.conn.Open();
        this.cmd = new SqlCommand(sql, this.conn);

        this.cmd.Parameters.AddWithValue("@username", username);
        this.cmd.Parameters.AddWithValue("@GeldAnzahl", wallet);

        this.cmd.ExecuteNonQuery();

        this.cmd.Dispose();
        this.conn.Close();
    }


}
