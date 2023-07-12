using BlackJack.Hubs;
using Microsoft.AspNet.SignalR.Messaging;
using System;

public class Player
{
	String anzeigename;
	CardDeck hand = new CardDeck();

	public Player(String anzeigename)
	{
		this.anzeigename = anzeigename;
	}

	public int getPoints()
	{
		return hand.BlackJackSum();
	}

	public String ToString()
	{
		return anzeigename + " - Karten ("+hand.ToString()+") Punkte: "+getPoints();
	}


    public void addCard(Card card) {
		hand.addCard(card);
    }

	public int getHandSize()
	{
		return hand.size();
	}
}
