using BlackJack.Hubs;
using System;

public class Player
{
	public String id { get; }
    public String currentGameId { get; set; }
    public int wallet { get; set; }
    public String username { get; }
    private CardDeck hand = new CardDeck();

	public Player(String id, String username, int wallet, String currentGameId)
    {
        this.id = id;
        this.username = username;
		this.wallet = wallet;
        this.currentGameId = currentGameId;
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

}
