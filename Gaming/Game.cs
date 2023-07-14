using BlackJack;
using BlackJack.Hubs;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

public class Game
{
    private String id;
    private GameHub hub;
    private CardDeck deck;
    private Player[] slots = new Player[7];
    private CardDeck dealerDeck;

    public Game(GameHub hub)
    {
        this.id = Program.GenerateRandomString(4).ToUpper();
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
        foreach(Player player in slots)
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
        for(int i = 0; i < slots.Length;i++)
        {
            if(slots[i] == null)
            {
                slots[i] = player;
                break;
            }
        }
    }

    public Boolean containsPlayer(Player player)
    {

        foreach (Player p in slots)
            if (p.Equals(player))
                return true;
        return false;
    }

    public int getPlayerSize()
    {
        int amount = 0;
        foreach(Player player in slots)
            if (player != null)
                amount++;
        return amount;
           
    }

    public override String ToString()
    {
        return "Game(id:" + id + " / players:" + getPlayerSize() + ")";
    }


    public GameHub getHub()
    {
        return hub;
    }
    
    public void hitButton()
    {
        foreach (Player player in slots)
        {
        }
    }
}
