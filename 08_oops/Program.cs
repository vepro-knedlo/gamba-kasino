using System.Text;

namespace oops
{
	class Program
	{
		static void Main(string[] args)
		{
            Console.OutputEncoding = Encoding.UTF8;

            Bank player1Bank = new();

            string volba;
            int pujcka = 0;
            int kolSplaceno = 0;
            bool islandInvite = false;
            int card;
            int suit;
            bool played = false;

            Console.WriteLine("Začáteční šekely (1000 min, 5000 max): ");
            player1Bank.zustatek = UserInput.IntCheck("₪ vloženo.");
            player1Bank.zustatek = UserInput.ValCheck(player1Bank.zustatek, "₪ vloženo.", 1000, 5000);

            while (true)
            {
                if (played && player1Bank.roundsPaid != player1Bank.loanLength)
                {
                    player1Bank.PaymentSubstract();
                }
                else if (player1Bank.roundsPaid == player1Bank.loanLength && player1Bank.roundsPaid != 0)
                {
                    player1Bank.roundsPaid = 0;
                    player1Bank.loanLength = 0;
                    player1Bank.loanAmount = 0;
                    player1Bank.paymentAmount = 0;
                    Console.WriteLine("Půjčka splacena!");
                }
                played = false;


                if (player1Bank.zustatek < 0)
                {
                    Console.WriteLine("\n\nŠvorc.");
                    Console.WriteLine("Zůstatek peněz: " + player1Bank.zustatek + "₪");
                    Console.WriteLine("konec hry");
                    break;
                }
                Console.WriteLine("\n\n\n\n\nVýtejte v kasínu הבה נגילה \n\nMAIN MENU");
                Console.WriteLine("Zůstatek peněz: " + player1Bank.zustatek + "₪");
                Console.WriteLine("\n1 new game");
                Console.WriteLine("2 banka");
                Console.WriteLine("3 exit game");
                if (player1Bank.zustatek > 10000)
                {
                    Console.WriteLine("4 Little Saint James DLC");
                    islandInvite = true;
                }
                volba = Console.ReadLine();

                if (volba == "2")
                {
                    if (player1Bank.zustatek < 0 || player1Bank.loanAmount > 0)
                    {
                        Console.WriteLine("\nUž jste zadlužený!");
                        continue;
                    }
                    Console.WriteLine("\n\n\nVýtejte v bance Maxwell & co.");
                    Console.WriteLine("Kolik si půjčíte? (min 500₪ max 2000₪)");
                    player1Bank.loanAmount = UserInput.IntCheck("₪ půjčeno.");
                    player1Bank.loanAmount = UserInput.ValCheck(player1Bank.loanAmount, "₪ půjčeno.", 500, 2000);
                    player1Bank.PaymentCalc();
                    Console.WriteLine("splátka: " + player1Bank.paymentAmount + "₪ za kolo po dobu " + player1Bank.loanLength + " kol.");
                    Console.WriteLine("");
                    player1Bank.zustatek += player1Bank.loanAmount;
                }
                else if (volba == "1")
                {
                    if (player1Bank.zustatek > 200)
                    {
                        Console.WriteLine("Sázka (min. 200₪): ");
                        player1Bank.sazka = UserInput.IntCheck("₪ vsazeno.");
                        player1Bank.sazka = UserInput.ValCheck(player1Bank.sazka, "₪ vsazeno.", 200, player1Bank.zustatek);
                        Console.WriteLine("\n\n\nHra začala!\n\n\n");

                        played = true;

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

                        playerHand.cardSum = playerHand.Hand.Sum(c => c.value);
                        dealerHand.cardSum = dealerHand.Hand.Sum(c => c.value);

                        while (playerHand.Game())
                        {
                            deck.DealCard(out card, out suit);              //hráč tahá znovu
                            card = playerHand.MasterHandle(card);
                            playerHand.Hand.Add(new Card(card, suit));
                            Console.WriteLine("Vaše karta:" + card);
                            playerHand.cardSum = playerHand.Hand.Sum(c => c.value);
                        }
                        if (playerHand.bust)
                        {
                            Console.WriteLine("Bust!");
                            player1Bank.zustatek -= player1Bank.sazka;
                            continue;
                        }

                        while (dealerHand.Game())
                        {
                            deck.DealCard(out card, out suit);              //dealer tahá znovu
                            card = dealerHand.MasterHandle(card);
                            dealerHand.Hand.Add(new Card(card, suit));
                            Console.WriteLine("Dealerova karta:" + card);
                            dealerHand.cardSum = dealerHand.Hand.Sum(c => c.value);
                        }
                        if (dealerHand.bust)
                        {
                            Console.WriteLine("Dealer bust!");
                            player1Bank.zustatek += player1Bank.sazka;
                            continue;
                        }
                        else if (dealerHand.cardSum > playerHand.cardSum)
                        {
                            Console.WriteLine("Dealer má víc!");
                            player1Bank.zustatek -= player1Bank.sazka;
                            continue;
                        }
                        else if (playerHand.cardSum > dealerHand.cardSum)
                        {
                            Console.WriteLine("Hráč má víc!");
                            player1Bank.zustatek += player1Bank.sazka;
                            continue;
                        }
                        else if (playerHand.cardSum == dealerHand.cardSum)
                        {
                            Console.WriteLine("Stand off! Šul nul.");
                            continue;
                        }


                    }
                    else
                    {
                        Console.WriteLine("\nJseš broke gój, o co se snažíš ");
                    }
                }
                else if (volba == "4" && islandInvite)
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
    class BlackjackHand(bool player)
    {
        public bool player = player;
        public bool bust = false;
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
        public bool Game()
        {
            if (player)
            {
                if (cardSum > 21)
                {
                    bust = true;
                    return false;
                }
                int input = 0;
                Console.WriteLine("1 hit");
                Console.WriteLine("2 stand");
                input = UserInput.IntCheck(" zvoleno.");
                while (input != 1 && input != 2)
                {
                    Console.WriteLine("1 nebo 2. Znova:");
                    input = UserInput.IntCheck(" zvoleno.");
                }
                if (input == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (cardSum > 21)
                {
                    bust = true;
                    return false;
                }
                else if (cardSum < 18)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
    class Bank()
    {
        public int sazka = 0;
        public int zustatek = 0;

        public int loanAmount = 0;
        public int roundsPaid = 0;
        public int loanLength = 0;
        public int paymentAmount = 0;
        public void PaymentCalc()
        {
            if (loanAmount > zustatek)
            {
                loanLength = 4;
                paymentAmount = loanAmount / 4;
            }
            else if (loanAmount < zustatek)
            {
                loanLength = 3;
                paymentAmount = loanAmount / 3;
            }
        }
        public void PaymentSubstract() 
        {
            zustatek -= paymentAmount;
            roundsPaid++;
        }
    }
}