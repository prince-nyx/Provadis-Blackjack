using BlackJack;
using BlackJack.Hubs;
using Microsoft.AspNet.SignalR.Messaging;
using System;
using System.Linq;
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
    //public GameHub hub { get; set; }

    private int currentSlotsTurn = -1;

    private int totalBet = 0; // Total betting amount

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
            startTurn(players[slots[currentSlotsTurn]], 20);
        }
        else
            nextPlayer();
    }

    public void dealCard(Boolean hidden)
    {
        Card card = deck.drawCard();
        dealerDeck.addCard(card);
        addDealerCard(card.getName(),hidden);

        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i] != null && players.ContainsKey(slots[i]))
            {
                Player player = players[slots[i]];
                card = deck.drawCard();
                player.addCard(card);
                addCardToPlayer(i, card.getName());
            }
        }
    }



    public void addPlayerToGame(Player player)
    {
        for(int i = 0; i < slots.Length;i++)
        {
            if(slots[i] == null)
            {
                slots[i] = player.id;
                players.Add(player.id, player);
                break;
            }
        }
    }

    public void PlayerJoined(Player player)
    {
        if (slots.Contains(player.id))
        {
            int slotid = Array.IndexOf(slots, player.id);
            assignPlayer(slotid, player.username);
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

    
    public void hit(int slotid)
    {
        Player player = players[slots[slotid]];

        if (player != null)
        {
                
                Card card = deck.drawCard();
                player.addCard(card);
                addCardToPlayer(slotid,card.getName()); 
           }      

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


    //test
    public void console(String message)
    {
        foreach (Player player in players.Values)
        {
            player.registerEvent(new FrontendEvent("console", message));
        }

    }

    //all
    public void addCardToPlayer(int slotid, String cardname)
    {
        foreach (Player player in players.Values)
        {
            player.registerEvent(new FrontendEvent("addCardToPlayer", slotid.ToString(),  cardname));
        }
    }

    //all
    public void setCardSum(int slotid, int amount)
    {
        foreach (Player player in players.Values)
        {
            player.registerEvent(new FrontendEvent("setCardSum", slotid.ToString(), amount.ToString()));
        }
    }

    //all
    public void assignPlayer(int slotid, String username)
    {
        foreach (Player player in players.Values)
        {
            player.registerEvent(new FrontendEvent("assignPlayer", slotid.ToString(), username));
        }
    }

    //all
    public void unassignPlayer(int slotid)
    {
        foreach (Player player in players.Values)
        {
            player.registerEvent(new FrontendEvent("unassignPlayer", slotid.ToString()));
        }
    }

    //all
    public void setBet(int slotid, double amount)
    {
        foreach (Player player in players.Values)
        {
            player.registerEvent(new FrontendEvent("setBet", slotid.ToString(), amount.ToString()));
        }
    }

    //all
    public void addDealerCard(String cardname, Boolean hidden)
    {
        foreach (Player player in players.Values)
        {
            player.registerEvent(new FrontendEvent("addDealerCard", cardname, hidden.ToString()));
        }
    }

    //client
    public void endTurn(Player player, int slotid)
    {
        player.registerEvent(new FrontendEvent("endTurn", slotid.ToString()));
    }

    //client
    public void startTurn(Player player, int time)
    {
        player.registerEvent(new FrontendEvent("startTurn", time.ToString()));
    }

    //client
    public void setbBalance(Player player, int amount)
    {
        player.registerEvent(new FrontendEvent("setbBalance", amount.ToString()));
    }

    //client
    public void enableBet(Player player)
    {
        player.registerEvent(new FrontendEvent("enableBet"));
    }

    //client
    public void disableBet(Player player)
    {
        player.registerEvent(new FrontendEvent("disableBet"));
    }


    //client
    public void showResult(Player player, int amount, String result)
    {
        player.registerEvent(new FrontendEvent("showResult",  amount.ToString(), result));
        totalBet += amount;
    }

    //client
    public void showStartButton(Player player)
    {
        player.registerEvent(new FrontendEvent("showStartButton"));
    }

    //Chipauswahl abrufen un den return Wert aufaddieren
    public void setBetBackend(string chipName)
    {
        int amount = GetChipAmount(chipName);

        totalBet += amount;
    }

}
