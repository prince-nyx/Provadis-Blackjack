using BlackJack;
using BlackJack.Hubs;
using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Timers;


public class Game
{
    private System.Timers.Timer timer;
    public String id { get; }
    private CardDeck deck;
    private String[] slots = new String[7];
    public String hostid { get; set; }
    private Dictionary<string, Player> players = new Dictionary<string, Player>();
    private CardDeck dealerDeck;

    private Boolean[] finishedBets;
    //public GameHub hub { get; set; }

    public int currentSlotsTurn{get; set;}

    private int totalBet = 0; // Total betting amount

    public GamePhase phase { get; set; }

    private int betTime = 10;
    private int turnTime = 60;
    


    public Game(string hostid)
    {
        this.hostid = hostid;
        this.id = Program.app.GenerateRandomString(4).ToUpper();
        Console.WriteLine("Game erstellt mit Code "+id+" und host "+hostid);
        dealerDeck = new CardDeck();
        deck = new CardDeck();
        deck.createBlackJackDeck();
        Console.WriteLine("Deck erstellt");
        deck.shuffle();
        Console.WriteLine("Deck gemischt");
        currentSlotsTurn = -1;
        phase = GamePhase.WAITING_FOR_PLAYERS;
        timer = new System.Timers.Timer();
    }


    public void startGame()
	{
		gamestarting();
		enableBet();
        Console.WriteLine("SPIEL GESTARTET");
        phase = GamePhase.BETTING;
        finishedBets = new bool[] {false,false,false,false,false,false,false};

        timer = new System.Timers.Timer(betTime * 1000);
        timer.Elapsed += new System.Timers.ElapsedEventHandler(startRound);
        timer.Start();

    }

    public void submitBet(String playerid)
    {
        int slotid = Array.IndexOf(slots, playerid);
        finishedBets[slotid] = true;
        if (checkBetEnd())
            startRound(null, null);
    }

    private Boolean checkBetEnd()
    {
        foreach(Boolean bet in finishedBets) {
            if(!bet)
            {
                return false;
            }
        }
        return true;
    }

    private void startRound(object source, System.Timers.ElapsedEventArgs e)
    {
        timer.Stop();
        disableBet();
        phase = GamePhase.PLAYING;
        DealerDrawsCard();
        dealCardsToPlayer();
        DealerDrawsCard();
        dealCardsToPlayer();
        nextPlayerTurn();
    }

    private void nextPlayerTurn()
    {
        currentSlotsTurn++;
        if (currentSlotsTurn >= slots.Length)
        {
            showDealerCards(dealerDeck.getCard(0).getName());
			markActivePlayer(7);
			DealerThinking();
        }
        else if (slots[currentSlotsTurn] != null && players.ContainsKey(slots[currentSlotsTurn]))
        {
            markActivePlayer(currentSlotsTurn);
            startTurn(players[slots[currentSlotsTurn]], turnTime);

            timer = new System.Timers.Timer(turnTime* 1000);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(endPlayerTurn);
            timer.Start();
        }
        else
            nextPlayerTurn();
    }

    private void endPlayerTurn(object source, System.Timers.ElapsedEventArgs e)
    {
        timer.Stop();
        if (slots[currentSlotsTurn] != null && players.ContainsKey(slots[currentSlotsTurn]))
        {
            endTurn(players[slots[currentSlotsTurn]], 20);
        }
        nextPlayerTurn();
    }
    private void DealerThinking()
    {
        Task.Factory.StartNew(() =>
        {
            System.Threading.Thread.Sleep(2000);
            if (dealerDeck.BlackJackSum() >= 17)
                endGame();
            else
            {
                DealerDrawsCard();
                DealerThinking();
            }
        });
    }

    private void endGame()
    {
        int dealerPoints = dealerDeck.BlackJackSum();
        Boolean dealerBlackJack = dealerDeck.isBlackJack();

        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null && players.ContainsKey(slots[i]))
            {
                Player player = players[slots[i]];
                String headline = "";
                String result = "";
                int playerPoints = player.hand.BlackJackSum();

                if (player.hand.isLuckySeven())
                {
                    player.AddWallet(player.bet * 3);
                    headline = "LUCKY 7";
                    result = (player.bet * 3) + "€ gewonnen";
                }
                else if (dealerDeck.isBlackJack())
                {
                    if (player.hand.isBlackJack())
                    {
                        player.AddWallet(player.bet);
                        headline = "Der Dealer und du habt einen BlackJack";
                        result = "Einsatz von " + player.bet + "€ zurück";
                    }
                    else
                    {
                        headline = "Dealer hat einen Blackjack";
                        result = "Einsatz von " + player.bet + "€ verloren";
                    }
                }
                else if (playerPoints > 21)
                { 
                    headline = "Du hast dich überkauft";
                    result = player.bet + "€ verloren";
                } else if (dealerPoints > 21)
                {
                    player.AddWallet(player.bet * 2);
                    headline = "Dealer überkauft";
                    result = (player.bet * 2) + "€ gewonnen";
                } else
                {
                    player.AddWallet(player.bet * 2);
                    headline = "Du hast gewonnen";
                    result = player.bet + "€ gewonnen";
                }

                player.hand.clear();
                player.resetBet();
                setBet(i, 0);
                setbBalance(player, player.wallet);
                showResult(player, headline, result);
            }
        }
        phase = GamePhase.WAITING_FOR_PLAYERS;
    }

    

    private void DealerDrawsCard()
    {
        Card card = deck.drawCard();
        dealerDeck.addCard(card);
        addDealerCard(card.getName());
    }

    public void dealCardsToPlayer()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i] != null && players.ContainsKey(slots[i]))
            {
                Player player = players[slots[i]];
                Card card = deck.drawCard();
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
            markUserSlot(player, slotid);
            if (player.id.Equals(hostid))
                showStartButton(player);
            for (int i = 0; i < slots.Length; i++)
                if (slots[i] != null && slots[i] != player.id)
                    player.registerEvent(new FrontendEvent("assignPlayer", i.ToString(), players[slots[i]].username));               
        }
    }

    public Boolean isPlayersTurn(String playerid)
    {
        return slots[currentSlotsTurn].Equals(playerid);
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

    
    public void hit()
    {
        Player player = players[slots[currentSlotsTurn]];
        Card card = deck.drawCard();
        player.addCard(card);
        addCardToPlayer(currentSlotsTurn, card.getName());
        if(player.hand.isLuckySeven() || player.hand.BlackJackSum() > 21)
            endPlayerTurn(null, null);
    }
    
    public void stand()
    {
        endPlayerTurn(null, null);
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
	public void gamestarting()
	{
		foreach (Player player in players.Values)
		{
			player.registerEvent(new FrontendEvent("gamestarting"));
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
    public void addDealerCard(String cardname)
    {
        foreach (Player player in players.Values)
        {
            player.registerEvent(new FrontendEvent("addDealerCard", cardname));
        }
    }

    //all
    public void enableBet()
    {
        foreach (Player player in players.Values)
            player.registerEvent(new FrontendEvent("enableBet"));
    }

    //all
    public void disableBet()
    {
        foreach (Player player in players.Values)
            player.registerEvent(new FrontendEvent("disableBet"));
	}
    
    //all
	public void markActivePlayer(int slotid)
	{
		foreach (Player player in players.Values)
			player.registerEvent(new FrontendEvent("markActivePlayer", slotid.ToString()));
	}


	//all
	public void showDealerCards(String cardname)
    {
        foreach (Player player in players.Values)
            player.registerEvent(new FrontendEvent("showDealerCards", cardname));
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
    public void setbBalance(Player player, double amount)
    {
        player.registerEvent(new FrontendEvent("setbBalance", amount.ToString()));
    }


    //client
    public void showResult(Player player,String headline, String result)
    {
        player.registerEvent(new FrontendEvent("showResult", headline, result));
    }

    //client
    public void showStartButton(Player player)
    {
        player.registerEvent(new FrontendEvent("showStartButton"));
    }
	//client
	public void markUserSlot(Player player, int slotid)
	{
		player.registerEvent(new FrontendEvent("markUserSlot", slotid.ToString()));
	}


}
