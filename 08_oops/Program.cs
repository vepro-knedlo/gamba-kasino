using System.Text;

namespace Oops
{
	class Program
	{
		static void Main(string[] args)
		{
            Console.OutputEncoding = Encoding.UTF8;

            int sazka;
            int zustatek;
            string volba;
            int pujcka = 0;
            int kolSplaceno = 0;
            bool islandInvite = false;
            int card;
            int suit;

            Console.WriteLine("Začáteční šekely (1000 minimum): ");
            zustatek = UserInput.IntCheck("₪ vloženo.");
            zustatek = UserInput.ValCheck(zustatek, "₪ vloženo.", 1000, 5000);

            while (true)
            {
                if (kolSplaceno == 3)
                {
                    pujcka = 0;
                    Console.WriteLine("Půjčka splacena!");
                    kolSplaceno = 0;
                }
                if (zustatek < 0)
                {
                    Console.WriteLine("\n\nŠvorc.");
                    Console.WriteLine("Zůstatek peněz: " + zustatek + "₪");
                    Console.WriteLine("konec hry");
                    break;
                }
                Console.WriteLine("\n\n\n\n\nVýtejte v kasínu הבה נגילה \n\nMAIN MENU");
                Console.WriteLine("Zůstatek peněz: " + zustatek + "₪");
                Console.WriteLine("\n1 new game");
                Console.WriteLine("2 banka");
                Console.WriteLine("3 exit game");
                if (zustatek > 10000)
                {
                    Console.WriteLine("4 Little Saint James DLC");
                    islandInvite = true;
                }
                volba = Console.ReadLine();

                if (volba == "2")
                {
                    if (zustatek < 0 || pujcka > 0)
                    {
                        Console.WriteLine("\nUž jste zadlužený!");
                        continue;
                    }
                    Console.WriteLine("\n\n\nVýtejte v bance Maxwell & co.");
                    Console.WriteLine("Kolik si půjčíte? (min 500₪ max 2000₪)");
                    pujcka = UserInput.IntCheck("₪ půjčeno.");
                    pujcka = UserInput.ValCheck(pujcka, "₪ půjčeno.", 500, 2000);
                    Console.WriteLine("splátka: " + pujcka / 2 + "₪ za kolo po dobu 3 kol.");
                    Console.WriteLine("");
                    zustatek += pujcka;
                }
                else if (volba == "1")
                {
                    if (zustatek > 200)
                    {
                        Console.WriteLine("Sázka (min. 200₪): ");
                        sazka = UserInput.IntCheck("₪ vsazeno.");
                        sazka = UserInput.ValCheck(sazka, "₪ vsazeno.", 200, zustatek);
                        Console.WriteLine("\n\n\nHra začala!\n\n\n");

                        Deck deck = new();
                        BlackjackHand playerHand = new(true);

                        deck.DealCard(out card, out suit);              //tahá hráč
                        card = playerHand.MasterHandle(card);
                        playerHand.Hand.Add(new Card(card, suit));
                        Console.WriteLine("Vaše karta:" + card);

                        BlackjackHand dealerHand = new(false);

                        deck.DealCard(out card, out suit);              //tahá dealer
                        card = dealerHand.MasterHandle(card);
                        dealerHand.Hand.Add(new Card(card, suit));
                        Console.WriteLine("Dealerova karta:" + card);

                        deck.DealCard(out card, out suit);              //hráč tahá znovu
                        card = playerHand.MasterHandle(card);
                        playerHand.Hand.Add(new Card(card, suit));
                        Console.WriteLine("Vaše karta:" + card);


                    }
                    else
                    {
                        Console.WriteLine("\nJseš broke gój, o co se snažíš ");
                    }
                }
                else if (volba == "5" && islandInvite)
                {
                    Console.WriteLine("\n\nŠalom, já jsem Jeffrey a zvu vás na můj ostrov.");
                }
                else if (volba == "3")
                {
                    break;
                }
            }
        }
	}
	class Deck
	{
        private int[,] cards = new int[4, 13]
		{
			{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 },	//hertze
            { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 },	//káry
            { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 },	//piky
            { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }	//kříže
        };
        private int[,] discardedCards = new int[4, 13]
        {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },	//hertze
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },	//káry
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },	//piky
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }	//kříže
        };
        public void DealCard(out int card, out int suit)
		{
            Random rnd = new();
			card = rnd.Next(1, 14);
			suit = rnd.Next(1, 5);
			while (cards[suit-1, card-1] == 0)
			{
                card = rnd.Next(1, 14);
                suit = rnd.Next(1, 5);
            }
			cards[suit-1, card-1] = 0;
        }
        public void DiscardCard(int suit, int card)
        {
            discardedCards[suit, card] = card;
        }
        public void TurnOver()
        {
            cards = (int[,])discardedCards.Clone();
        }
    }
    class UserInput
    {
        public static int IntCheck(string matter = "")
        {
            string input;
            int output;
            while (true)
            {
                input = Console.ReadLine();

                try
                {
                    output = Convert.ToInt32(input);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error reading input " + input + ": NaN. Try again:");
                    continue;
                }
                Console.WriteLine(input + matter);
                break;
            }
            return output;
        }
        public static int ValCheck(int input, string matter, int? lowerLimit = null, int? upperLimit = null)
        {
            while ((lowerLimit.HasValue && input < lowerLimit) || (upperLimit.HasValue && input > upperLimit))
            {
                Console.WriteLine("Neplatná hodnota. Znova:");
                input = IntCheck(matter);
            }
            return input;
        }
    }
    class Card
    {
        public int value;
        public int suit;

        public Card(int value, int suit)
        {
            this.value = value;
            this.suit = suit;
        }
    }
    class BlackjackHand
    {
        public bool player;
        public List<Card> Hand = new();
        public int cardSum;
        public int MasterHandle(int card)
        {
            if (card == 1 && player)
            {
                Console.WriteLine("Eso! Za kolik bude? (1/11)");
                card = UserInput.IntCheck(" eso.");
                while (card != 11 && card != 1)
                {
                    Console.WriteLine("1 nebo 11. Znova:");
                    card = UserInput.IntCheck(" eso.");
                }
            }
            else if (card == 1 && !player)
            {
                if (cardSum < 11)
                {
                    card = 11;
                }
            }
            else if (card > 10)
            {
                card = 10;
            }
            return card;
        }

        public BlackjackHand(bool player)
        {
            this.player = player;
        }
    }
}