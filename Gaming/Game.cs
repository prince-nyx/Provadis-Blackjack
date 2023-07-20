using BlackJack;
using BlackJack.Hubs;
using BlackJack.Pages;
using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.EntityFrameworkCore.Internal;
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

    private int betTime = 20;
    private int turnTime = 60;
    private int potLimit = 25;
    


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
		enableBet(betTime);
        Console.WriteLine("SPIEL GESTARTET");
        phase = GamePhase.BETTING;
        finishedBets = new bool[] {false,false,false,false,false,false,false};

        timer = new System.Timers.Timer(betTime * 1000);
        timer.Elapsed += new System.Timers.ElapsedEventHandler(startRound);
        timer.Start();

    }

    public void submitBet(Player player)
    {
        int slotid = getSlotId(player);
        if (!finishedBets[slotid]) { 
            finishedBets[slotid] = true;
            if (checkBetEnd())
                startRound(null, null);
        }
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
			markActivePlayer(7,30);
			DealerThinking();
        }
        else if (slots[currentSlotsTurn] != null && players.ContainsKey(slots[currentSlotsTurn]))
        {
            markActivePlayer(currentSlotsTurn, turnTime);
            startTurn(players[slots[currentSlotsTurn]]);

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
                endRound();
            else
            {
                DealerDrawsCard();
                DealerThinking();
            }
        });
    }

    public void playerBets(Player player, int amount)
    {
        if(player.wallet >= amount)
        {
            if((player.bet + amount) <= potLimit)
		    {
			    player.AddBet(amount);
		    } else
		    {
			    player.registerEvent(new FrontendEvent("console", "Es ist nicht möglich mehr als "+ potLimit+" zu setzen"));
                player.bet = potLimit;
            }
            setBet(getSlotId(player), player.getBet());
            setBalance(player, player.wallet);
        }
        else
            player.registerEvent(new FrontendEvent("console", "Zu wenig Geld"));
    }

    private void endRound()
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

						Console.WriteLine("WALLET UPDATE SUCCESS");
					}
                }
                else if (playerPoints > 21)
                { 
                    headline = "Du hast dich überkauft";
                    result = player.bet + "€ verloren";
					Console.WriteLine("WALLET UPDATE SUCCESS");

				} else if (dealerPoints > 21)
				{
					player.AddWallet(player.bet * 2);
					headline = "Dealer überkauft";
                    result = (player.bet * 2) + "€ gewonnen";

					Console.WriteLine("WALLET UPDATE SUCCESS");

				} else if(playerPoints > dealerPoints)
                {
                    player.AddWallet(player.bet * 2);
                    headline = "Du hast gewonnen";
                    result = player.bet + "€ gewonnen";

					
					Console.WriteLine("WALLET UPDATE SUCCESS");

				} else
				{
					headline = "Dealer gewinnt";
					result = player.bet + "€ verloren";
					Console.WriteLine("WALLET UPDATE SUCCESS");
				}

                player.hand.clear();
                player.resetBet();
                setBet(i, 0);
				setBalance(player, player.wallet);
                showResult(player, headline, result);
            }
        }
        phase = GamePhase.WAITING_FOR_PLAYERS;
        showStartButton(players[hostid]);
        dealerDeck.clear();
		currentSlotsTurn = -1;
	}

    

    private void DealerDrawsCard()
    {
        Card card = deck.drawCard();
        dealerDeck.addCard(card);
        addDealerCard(card);
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
                addCardToPlayer(i, card);
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
            int slotid = getSlotId(player);
            assignPlayer(slotid, player.username);
            markUserSlot(player, slotid);
            load(player);
            if (player.id.Equals(hostid))
                showStartButton(player);
            for (int i = 0; i < slots.Length; i++)
                if (slots[i] != null && slots[i] != player.id)
                    player.registerEvent(new FrontendEvent("assignPlayer", i.ToString(), players[slots[i]].username));               
        }
    }

    public void PlayerLeaves(Player player)
    {
        if (slots.Contains(player.id))
        {
            int slotid = getSlotId(player);
            unassignPlayer(slotid);
            slots[slotid] = null;
            players.Remove(player.id);
            player.resetBet();
            player.events.Clear();
            player.currentGameId = "";

            if(player.id.Equals(hostid))
            {
                lookingForNewHost();
                if (hostid != null) { 
                    if (phase == GamePhase.WAITING_FOR_PLAYERS)
                        showStartButton(players[hostid]);
                } else
                {
                    Program.app.gameManager.deleteGame(id);
                }
            }
            
		}
	}

    public void lookingForNewHost()
    {
        hostid = null;
		for (int i = 0; i < slots.Length; i++)
		{
			if (slots[i] != null)
			{
                hostid = slots[i];
				break;
			}
		}
	}

    private int getSlotId(Player player)
    {
		return Array.IndexOf(slots, player.id);
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
        addCardToPlayer(currentSlotsTurn, card);
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
	public void addCardToPlayer(int slotid, Card card)
    {
        foreach (Player player in players.Values)
        {
            player.registerEvent(new FrontendEvent("addCardToPlayer", slotid.ToString(), card.getName(), card.position.ToString()));
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
    public void addDealerCard(Card card)
    {
        foreach (Player player in players.Values)
        {
            if(card.position == 1)
				player.registerEvent(new FrontendEvent("addDealerCard", "", card.position.ToString()));
			else
                player.registerEvent(new FrontendEvent("addDealerCard", card.getName(), card.position.ToString()));
		}
    }

    //all
    public void enableBet(int betTime)
    {
        foreach (Player player in players.Values)
            player.registerEvent(new FrontendEvent("enableBet", betTime.ToString()));
    }

    //all
    public void disableBet()
    {
        foreach (Player player in players.Values)
            player.registerEvent(new FrontendEvent("disableBet"));
	}
    
    //all
	public void markActivePlayer(int slotid, int time)
	{
		foreach (Player player in players.Values)
			player.registerEvent(new FrontendEvent("markActivePlayer", slotid.ToString(), time.ToString()));
	}


	//all
	public void showDealerCards(String cardname)
    {
        foreach (Player player in players.Values)
            player.registerEvent(new FrontendEvent("showDealerCards", cardname));
    }

    //client
	public void load(Player player)
	{
		player.registerEvent(new FrontendEvent("load", player.wallet.ToString(), player.username, id));
	}

	//client
	public void endTurn(Player player, int slotid)
    {
        player.registerEvent(new FrontendEvent("endTurn", slotid.ToString()));
    }

    //client
    public void startTurn(Player player)
    {
        player.registerEvent(new FrontendEvent("startTurn"));
    }

    //client
    public void setBalance(Player player, double amount)
    {
        player.registerEvent(new FrontendEvent("setBalance", amount.ToString()));
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
