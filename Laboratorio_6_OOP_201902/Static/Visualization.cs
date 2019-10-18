using Laboratorio_6_OOP_201902.Cards;
using Laboratorio_6_OOP_201902.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratorio_6_OOP_201902.Static
{
    public static class Visualization
    {
        public static void ShowHand(Hand hand)
        {
            CombatCard combatCard;
            Console.WriteLine("Hand: ");
            for (int i = 0; i<hand.Cards.Count; i++)
            {
                if (hand.Cards[i] is CombatCard)
                {
                    combatCard = hand.Cards[i] as CombatCard;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"|({i}) {combatCard.Name} ({combatCard.Type}): {combatCard.AttackPoints} |");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"|({i}) {hand.Cards[i].Name} ({hand.Cards[i].Type}) |");
                }
                Console.ResetColor();
            }
            Console.WriteLine();
        }
        public static void ShowDecks(List<Deck> decks)
        {
            Console.WriteLine("Select one Deck:");
            for (int i = 0; i<decks.Count; i++)
            {
                Console.WriteLine($"({i}) Deck {i+1}");
            }
        }
        public static void ShowCaptains(List<SpecialCard> captains)
        {
            Console.WriteLine("Select one captain:");
            for (int i = 0; i < captains.Count; i++)
            {
                Console.WriteLine($"({i}) {captains[i].Name}: {captains[i].Effect}");
            }
        }
        public static int GetUserInput(int maxInput, bool stopper = false)
        {
            bool valid = false;
            int value;
            int minInput = stopper ? -1 : 0;
            while (!valid)
            {

                if (int.TryParse(Console.ReadLine(), out value))
                {
                    if (value >= minInput && value <= maxInput)
                    {
                        return value;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine($"The option ({value}) is not valid, try again");
                        Console.ResetColor();
                    }
                }
                else
                {
                    ConsoleError($"Input must be a number, try again");
                }
            }
            return -1;
        }
        public static void ConsoleError(string message)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public static void ShowProgramMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public static void ShowListOptions (List<string> options, string message = null)
        {
            if (message != null) Console.WriteLine($"{message}");
            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"({i}) {options[i]}");
            }
        }
        public static void ClearConsole()
        {
            Console.ResetColor();
            Console.Clear();
        }

        public static void ShowBoard(Board board, int player, int[] lifePoints, int[] attackPoints)
        {
            int opponent;
            string attackCardsLongRange = "";
            string attackCardsRange = "";
            string attackCardsMelee = "";
            string opponentAttackCardsLongRange = "";
            string opponentAttackCardsRange = "";
            string opponentAttackCardsMelee = "";
            if (player==0)
            {
                opponent = 1;
            }
            else
            {
                opponent = 0;
            }

            if (board.PlayerCards[opponent].ContainsKey(EnumType.longRange))
            {
                foreach (CombatCard card in board.PlayerCards[opponent][EnumType.longRange])
                {
                        
                    opponentAttackCardsLongRange = attackCardsLongRange + "|" +card.AttackPoints+ "|";
                } 
            }
            if (board.PlayerCards[player].ContainsKey(EnumType.longRange))
            {
                foreach (CombatCard card in board.PlayerCards[player][EnumType.longRange])
                {

                    attackCardsLongRange = attackCardsLongRange + "|" + card.AttackPoints + "|";
                }
            }

            if (board.PlayerCards[opponent].ContainsKey(EnumType.range))
            {
                foreach (CombatCard card in board.PlayerCards[opponent][EnumType.range])
                {

                    opponentAttackCardsRange = opponentAttackCardsRange + "|" + card.AttackPoints + "|";
                }
                
            }
            if (board.PlayerCards[player].ContainsKey(EnumType.range))
            {
                foreach (CombatCard card in board.PlayerCards[player][EnumType.range])
                {

                    attackCardsRange = attackCardsRange + "|" + card.AttackPoints + "|";
                }
            }


            if (board.PlayerCards[opponent].ContainsKey(EnumType.melee))
            {
                foreach (CombatCard card in board.PlayerCards[opponent][EnumType.melee])
                {

                    opponentAttackCardsMelee = opponentAttackCardsMelee + "|" + card.AttackPoints + "|";
                }
                
            }
            if (board.PlayerCards[player].ContainsKey(EnumType.melee))
            {
                foreach (CombatCard card in board.PlayerCards[player][EnumType.melee])
                {

                    attackCardsMelee = attackCardsMelee + "|" + card.AttackPoints + "|";
                }
            }




            string activeWeatherCard;
            if (board.WeatherCards.Count==0)
            {
                activeWeatherCard = "";
            }
            else
            {
                activeWeatherCard = "|"+board.WeatherCards[board.WeatherCards.Count-1].Name+ "|";
                
            }

            Console.WriteLine("Board:");
            Console.WriteLine();
            Console.WriteLine("Opponent - LifePoints:"+lifePoints[opponent]+" - AttackPoints:"+attackPoints[opponent]);
            Console.WriteLine("(Long Range) ["+board.GetAttackPoints(EnumType.longRange)[opponent]+"]:"+opponentAttackCardsLongRange);
            Console.WriteLine("(Range) [" + board.GetAttackPoints(EnumType.range)[opponent] + "]:"+ opponentAttackCardsRange);
            Console.WriteLine("(Melee) [" + board.GetAttackPoints(EnumType.longRange)[opponent] + "]:"+ opponentAttackCardsMelee);
            Console.WriteLine("");
            Console.Write("Weather Cards:");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(activeWeatherCard+"\n");
            Console.ResetColor();
            Console.WriteLine("");
            Console.WriteLine("You - LifePoints:" + lifePoints[player] + " - AttackPoints:" + attackPoints[player]);
            Console.WriteLine("(Long Range) [" + board.GetAttackPoints(EnumType.longRange)[player] + "]:" + attackCardsLongRange);
            Console.WriteLine("(Range) [" + board.GetAttackPoints(EnumType.range)[player] + "]:" + attackCardsRange);
            Console.WriteLine("(Melee) [" + board.GetAttackPoints(EnumType.longRange)[player] + "]:" + attackCardsMelee);
            Console.WriteLine("");
            Console.WriteLine("");
            
        }

    }
    
}
