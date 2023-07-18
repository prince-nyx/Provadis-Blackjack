using System;
using System.Linq.Expressions;

public class Card
{
    int wert { get; }
    int color { get; }

   public int position { set; get; }
    public Card(int wert, int color)
	{
        if (wert <= 0 || wert > 13)
            throw new ArgumentOutOfRangeException();
        if (color <= 0 || color > 4)
            throw new ArgumentOutOfRangeException();
        this.wert = wert;
        this.color = color;
	}

    public int getWert()
    {
        return wert < 10 ? wert : 10;
    }

    public string getName()
    {
        string swert;
        switch (wert)
        {
            case 1:
                swert = "ace";
                break;
            case 11:
                swert = "jack";
                break;
            case 12:
                swert = "queen";
                break;
            case 13:
                swert = "king";
                break;
            default:
                swert  = wert.ToString();
                break;
        }
        switch (color)
        {
            case 1:
                return swert + "_of_diamonds";
            case 2:
                return swert + "_of_hearts";
            case 3:
                return swert + "_of_spades";
            case 4:
                return swert + "_of_clubs";
            default:
                return wert+"-!-"+color;
        }
    }
}
