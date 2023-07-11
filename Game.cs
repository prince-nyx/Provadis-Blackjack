using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

public class Game
{

    private CardDeck deck;
    private Player[] players = new Player[7];
    private CardDeck dealerDeck;

    public Game()
    {
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
                player.addCard(deck.drawCard());
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
}
