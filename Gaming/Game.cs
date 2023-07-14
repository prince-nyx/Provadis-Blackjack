using BlackJack;
using BlackJack.Hubs;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

public class Game
{
    private String id;
    private CardDeck deck;
    private String[] slots = new String[7];
    private String host;
    private Dictionary<string, Player> players = new Dictionary<string, Player>(); 
    private CardDeck dealerDeck;

    public Game()
    {
        this.id = Program.GenerateRandomString(4).ToUpper();
        dealerDeck = new CardDeck();
        deck = new CardDeck();
        deck.createBlackJackDeck();
        Console.WriteLine("Deck erstellt");
        deck.shuffle();
        Console.WriteLine("Deck gemischt");
    }


    public void dealCard()
    {
        Card card = deck.drawCard();
        dealerDeck.addCard(card);
        for (int i = 0; i < slots.Length; i++)
        {
            Player player = players[slots[i]];
            if(player != null)
            {
                card = deck.drawCard();
                player.addCard(card);
                _ = getHub().addCardToPlayer(i, card.getName());
            }
        }
    }



    public void addPlayer(Player player)
    {
        
        for(int i = 0; i < slots.Length;i++)
        {
            if(slots[i] == null)
            {
                slots[i] = player.id;
                players.Add(player.id, player);
                if (players.Count == 1)
                    host = player.id;
                break;
            }
        }
    }

    public Boolean containsPlayer(String id)
    {
       return players.ContainsKey(id);
    }

    public override String ToString()
    {
        return "Game(id:" + id + " / players:" + players.Count + ")";
    }


    public GameHub getHub()
    {
        return players[host].hub;
    }
}
