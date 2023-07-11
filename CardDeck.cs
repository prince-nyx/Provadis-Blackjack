using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography;
using System.Collections.Generic;

public class CardDeck
{

	List<Card> cards = new List<Card>();
    RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();

    Random rnd = new Random();
    public CardDeck()
	{

	}

	public int BlackJackSum()
	{
		int ace = 0;
		int summe = 0;
		foreach(Card card in cards)
        {
            summe += card.getWert();
			if (card.getWert() == 1)
				ace++;
		}

		for(int i = 0;i<ace;i++)
		{
			if((summe+10) <= 21)
			{
				summe += 10;
			}
        }

        return summe;

	}

	public void createBlackJackDeck()
	{
		// 6 Decks erstellen
		for(int i=0;i<6;i++) {
			//Alle 4 Farben
			for(int color = 1; color < 5;color++) { 
				//Alle Karten
				for(int wert = 1; wert < 14;wert++)
                {
                    cards.Add(new Card(wert, color));
				}
			}
		}
	}

	public void shuffle()
	{
        int n = cards.Count;
        while (n > 1)
        {
            int k = rnd.Next(n + 1);
            n--;
            Card value = cards[k];
            cards[k] = cards[n];
            cards[n] = value;
        }
    }

	public Card drawCard()
	{
		int randomNumber = rnd.Next(0, cards.Count - 1);
        Card card = cards[randomNumber];
		cards.RemoveAt(randomNumber);
		Console.WriteLine("Karte gezogen. Karten im Deck: "+cards.Count);
		return card;
	}

	public void addCard(Card card) { cards.Add(card); }


    public string ToString()
	{ 
		Console.WriteLine("Heysho");
		return string.Join(",",cards.ConvertAll<string>(card => card.ToString()));
    }
}
