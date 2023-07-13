using BlackJack.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.AspNet.SignalR.Messaging;
using System;

public class Player
{
	private String currentGameId;
    private String username;
	private int wallet;
    private CardDeck hand = new CardDeck();

	public Player(String username, String currentGameId, int wallet)
	{
		this.username = username;
        this.currentGameId = currentGameId;
		this.wallet = wallet;
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
