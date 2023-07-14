using BlackJack;
using BlackJack.Hubs;
using System;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;


public class Game
{
    public String id { get; }
    private CardDeck deck;
    private String[] slots = new String[7];
    private String host;
    private Dictionary<string, Player> players = new Dictionary<string, Player>(); 
    private CardDeck dealerDeck;

    private int currentSlotsTurn = 0;

    private int total = 0; // Total betting amount


    public Game()
    {
        host = string.Empty;
        this.id = Program.GenerateRandomString(4).ToUpper();
        Console.WriteLine("Game erstellt mit Code "+id);
        dealerDeck = new CardDeck();
        deck = new CardDeck();
        deck.createBlackJackDeck();
        Console.WriteLine("Deck erstellt");
        deck.shuffle();
        Console.WriteLine("Deck gemischt");
    }


    public void start()
    {

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

    public Boolean containsPlayer(String playerid)
    {
        Console.WriteLine("gameid contains  " + playerid + ": " + players.ContainsKey(playerid));
       return players.ContainsKey(playerid);
    }

    public override String ToString()
    {
        return "Game(id:" + id + " / players:" + players.Count + ")";
    }


    public GameHub getHub()
    {
        return players[host].hub;
    }
    
    public void hit(int slotid)
    {
        Player player = players[slots[slotid]];

        if (player != null)
            {
                
                Card card = deck.drawCard();
                player.addCard(card);
            getHub().addCardToPlayer(slotid,card.getName()); 
           }
        
    }
    public void startGame()
    {
    }


    public int GetChipAmount(string chipName)
    {
        switch (chipName)
        {
            case "Pokerchip1.png":
                return 1;
            case "Pokerchip5.png":
                return 5;
            case "Pokerchip10.png":
                return 10;
            case "Pokerchip20.png":
                return 20;
            case "Pokerchip25.png":
                return 25;
            default:
                return 0;
        }
    }










































/*    public void setBet(int amount)
    {
        total += amount;
        Console.WriteLine("Total amount: " + total); // Display the total amount
    }

*/
}
