using BlackJack;
using BlackJack.Hubs;
using System;
using static System.Net.Mime.MediaTypeNames;

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

	public Player(String username, double wallet)
    {
        this.id = Program.app.GenerateRandomString(8);
        this.username = username;
		this.wallet = wallet;
        this.currentGameId = "";
        this.hand = new CardDeck();
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
        this.wallet -= bet;
    }

    public void resetBet()
    {
        bet = 0;
    }

    public void AddWallet(double amount)
    {
        this.wallet += amount;
    }


}
