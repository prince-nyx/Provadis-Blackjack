using BlackJack;
using BlackJack.Hubs;
using System;

public class Player
{
	public String id { get; }
    public String currentGameId { get; set; }
    public double wallet { get; set; }
    public String username { get; }
    private CardDeck hand = new CardDeck();

	public Player(String username, double wallet)
    {
        this.id = Program.GenerateRandomString(8);
        this.username = username;
		this.wallet = wallet;
        this.currentGameId = "";
    }

	public int getPoints()
	{
		return hand.BlackJackSum();
	}

	public String ToString()
	{
		return username + " - Karten ("+hand.ToString()+") Punkte: "+getPoints();
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


}
