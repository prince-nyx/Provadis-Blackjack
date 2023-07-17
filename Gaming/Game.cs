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
    public String hostid { get; set; }
    private Dictionary<string, Player> players = new Dictionary<string, Player>(); 
    private CardDeck dealerDeck;
    public GameHub hub { get; set; }

    private int currentSlotsTurn = -1;

    private int total = 0; // Total betting amount

    public Game()
    {
        this.hostid = hostid;
        this.id = Program.GenerateRandomString(4).ToUpper();
        Console.WriteLine("Game erstellt mit Code "+id+" und host "+hostid);
        dealerDeck = new CardDeck();
        deck = new CardDeck();
        deck.createBlackJackDeck();
        Console.WriteLine("Deck erstellt");
        deck.shuffle();
        Console.WriteLine("Deck gemischt");
    }


    public void startGame()
    {
        currentSlotsTurn = -1;
        Console.WriteLine("SPIEL GESTARTET");
        dealCard(true);
        dealCard(false);
        nextPlayer();
    }

    public void nextPlayer()
    {
        currentSlotsTurn++;
        Console.WriteLine("Start turn");
        if (slots[currentSlotsTurn] != null && players.ContainsKey(slots[currentSlotsTurn]))
        {
            _ = getHub().startTurn(players[slots[currentSlotsTurn]], 20);
        }
        else
            nextPlayer();
    }

    public void dealCard(Boolean hidden)
    {
        Card card = deck.drawCard();
        dealerDeck.addCard(card);
        _ = getHub().addDealerCard(card.getName(),hidden);

        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i] != null && players.ContainsKey(slots[i]))
            {
                Player player = players[slots[i]];
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
                _ = getHub().assignPlayer(i, player.username);
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
        return hub;
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
    public void stand(int slotid)

    {
        Player player = players[slots[slotid]];
        if (player != null)
        {

        }
    }
}
