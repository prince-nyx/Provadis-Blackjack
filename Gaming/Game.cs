using BlackJack.Hubs;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

public class Game
{
    private GameHub hub;
    private CardDeck deck;
    private Player[] players = new Player[7];
    private CardDeck dealerDeck;

    public Game(GameHub hub)
    {
        this.hub = hub;
        dealerDeck = new CardDeck();
        deck = new CardDeck();
        deck.createBlackJackDeck();
        Console.WriteLine("Deck erstellt");
        deck.shuffle();
        Console.WriteLine("Deck gemischt");
    }


    public void dealCard()
    {
        foreach(Player player in players)
        {
            if(player != null)
            {
                Card card = deck.drawCard();
                player.addCard(card);
                _ = hub.fireEvent("SetCardImage", player.getHandSize().ToString(), card.ToString());
            }
        }
    }



    public void addPlayer(Player player)
    {
        for(int i = 0; i < players.Length;i++)
        {
            if(players[i] == null)
            {
                players[i] = player;
                break;
            }
        }
    }

    public GameHub getHub()
    {
        return hub;
    }
}
