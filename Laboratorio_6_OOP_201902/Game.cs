using Laboratorio_6_OOP_201902.Cards;
using Laboratorio_6_OOP_201902.Enums;
using Laboratorio_6_OOP_201902.Static;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Laboratorio_6_OOP_201902
{
    public class Game
    {
        //Constantes
        private const int DEFAULT_CHANGE_CARDS_NUMBER = 3;

        //Atributos
        private Player[] players;
        private Player activePlayer;
        private List<Deck> decks;
        private List<SpecialCard> captains;
        private Board boardGame;
        internal int turn;

        //Constructor
        public Game()
        {
            Random random = new Random();
            decks = new List<Deck>();
            captains = new List<SpecialCard>();
            AddDecks();
            AddCaptains();
            players = new Player[2] { new Player(), new Player() };
            ActivePlayer = Players[random.Next(2)];
            boardGame = new Board();
            //Add board to players
            players[0].Board = boardGame;
            players[1].Board = boardGame;
            turn = 0;
        }
        //Propiedades
        public Player[] Players
        {
            get
            {
                return this.players;
            }
        }
        public Player ActivePlayer
        {
            get
            {
                return this.activePlayer;
            }
            set
            {
                activePlayer = value;
            }
        }
        public List<Deck> Decks
        {
            get
            {
                return this.decks;
            }
        }
        public List<SpecialCard> Captains
        {
            get
            {
                return this.captains;
            }
        }
        public Board BoardGame
        {
            get
            {
                return this.boardGame;
            }
        }
  

        //Metodos
        public bool CheckIfEndGame()
        {
            if (players[0].LifePoints == 0 || players[1].LifePoints == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public int GetWinner()
        {
            if (players[0].LifePoints == 0 && players[1].LifePoints > 0)
            {
                return 1;
            }
            else if (players[1].LifePoints == 0 && players[0].LifePoints > 0)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
        
        public void Play()
        {
            int userInput = 0;
            int rowUserInput;
            int[] compareArray = new int[2] {0,0};
            bool turnPassed = true;
            List<String> passOrPlay = new List<string> { "Play Card", "Pass Turn" };
            List<String> lineBuff = new List<string> {"Melee Row", "Range Row", "Long Range Row" };
            var rand = new Random();
            int firstOrSecondUser = ActivePlayer.Id == 0 ? 0 : 1;

            while (CheckIfEndGame())
            {
                //turno 0 o configuracion
                if (turn == 0)
                {
                    for (int _ = 0; _ < Players.Length; _++)
                    {
                        ActivePlayer = Players[firstOrSecondUser];
                        Visualization.ClearConsole();
                        //Mostrar mensaje de inicio
                        Visualization.ShowProgramMessage($"Player {ActivePlayer.Id + 1} select Deck and Captain:");
                        //Preguntar por deck
                        Visualization.ShowDecks(this.Decks);
                        userInput = Visualization.GetUserInput(this.Decks.Count - 1);
                        Deck deck = new Deck();
                        deck.Cards = new List<Card>(Decks[userInput].Cards);
                        ActivePlayer.Deck = deck;
                        //Preguntar por capitan
                        Visualization.ShowCaptains(Captains);
                        userInput = Visualization.GetUserInput(this.Captains.Count - 1);
                        ActivePlayer.ChooseCaptainCard(new SpecialCard(Captains[userInput].Name, Captains[userInput].Type, Captains[userInput].Effect));
                        //Asignar mano
                        ActivePlayer.FirstHand();
                        //Mostrar mano
                        Visualization.ShowHand(ActivePlayer.Hand);
                        //Mostar opciones, cambiar carta o pasar
                        Visualization.ShowListOptions(new List<string>() { "Change Card", "Pass" }, "Change 3 cards or ready to play:");
                        userInput = Visualization.GetUserInput(1);
                        if (userInput == 0)
                        {
                            Visualization.ClearConsole();
                            Visualization.ShowProgramMessage($"Player {ActivePlayer.Id + 1} change cards:");
                            Visualization.ShowHand(ActivePlayer.Hand);
                            for (int i = 0; i < DEFAULT_CHANGE_CARDS_NUMBER; i++)
                            {
                                Visualization.ShowProgramMessage($"Input the numbers of the cards to change (max {DEFAULT_CHANGE_CARDS_NUMBER}). To stop enter -1");
                                userInput = Visualization.GetUserInput(ActivePlayer.Hand.Cards.Count, true);
                                if (userInput == -1) break;
                                ActivePlayer.ChangeCard(userInput);
                                Visualization.ShowHand(ActivePlayer.Hand);
                            }
                        }
                        firstOrSecondUser = ActivePlayer.Id == 0 ? 1 : 0;
                    }
                    turn += 1;
                    Visualization.ClearConsole();
                }
                else
                {
                    
                    for (int _ = 0; _ < Players.Length; _++)
                    {
                        while (turnPassed)
                        {
                            Visualization.ShowProgramMessage("Turn: " + Convert.ToString(turn));
                            Visualization.ShowProgramMessage("");
                            activePlayer.DrawCard(rand.Next(0, activePlayer.Deck.Cards.Count));
                            Visualization.ShowBoard(BoardGame, activePlayer.Id, GetLifePoints(), GetTotalAttackPoints());
                            Visualization.ShowHand(activePlayer.Hand);
                            Visualization.ShowListOptions(passOrPlay, "Make your move player " + Convert.ToString(activePlayer.Id + 1));
                            userInput = Visualization.GetUserInput(1);
                            if (userInput == 0 && activePlayer.Hand.Cards.Count!=0)
                            {
                                Visualization.ShowProgramMessage("Choose a card by inputing the id. input -1 to cancel ");
                                userInput = Visualization.GetUserInput(activePlayer.Hand.Cards.Count);
                                if (userInput != -1)
                                {
                                    if (activePlayer.Hand.Cards[userInput].Type==EnumType.buff)
                                    {
                                        Visualization.ShowListOptions(lineBuff, "Choose a row to buff:");
                                        rowUserInput = Visualization.GetUserInput(2);
                                        switch (rowUserInput)
                                        {
                                            case 0:
                                                activePlayer.PlayCard(userInput,EnumType.buffmelee);
                                                break;
                                            case 1:
                                                activePlayer.PlayCard(userInput, EnumType.buffrange);
                                                break;
                                            case 2:
                                                activePlayer.PlayCard(userInput, EnumType.bufflongRange);
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        activePlayer.PlayCard(userInput);
                                    }
                                    
                                }

                            }
                            else
                            {
                                Visualization.ShowProgramMessage("Turn passed");
                                turnPassed = false;
                            }
                            Visualization.ClearConsole();
                        }
                        if (activePlayer.Id==0)
                        {
                            activePlayer = players[1];
                        }
                        else
                        {
                            activePlayer = players[0];
                        }
                        turnPassed = true;
                    }

                    int idPlayerRoundWinner = GetRoundWinner();
                    switch (idPlayerRoundWinner)
                    {
                        case 0:
                            Visualization.ShowProgramMessage("Player 1 WINS");
                            Players[1].LifePoints += -1;
                            break;
                        case 1:
                            Visualization.ShowProgramMessage("Player 2 WINS");
                            Players[0].LifePoints += -1;
                            break;
                        case -1:
                            Visualization.ShowProgramMessage("TIE");
                            Players[0].LifePoints += -1;
                            Players[1].LifePoints += -1;
                            break;
                    }
                    players[0].Board.DestroyCards();
                    players[1].Board.DestroyCards();
                    players[0].AttackPoints=0;
                    players[1 ].AttackPoints = 0;
                    turn += 1;
                    Thread.Sleep(3000);
                    Visualization.ClearConsole();
                }
            }
            Visualization.ShowProgramMessage("Player "+(GetWinner() + 1) + " is the WINNER");
            Visualization.ShowProgramMessage("GG");
            Thread.Sleep(3000);
        }
        public void AddDecks()
        {
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\Decks.txt";
            StreamReader reader = new StreamReader(path);
            int deckCounter = 0;
            List<Card> cards = new List<Card>();


            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string [] cardDetails = line.Split(",");

                if (cardDetails[0] == "END")
                {
                    decks[deckCounter].Cards = new List<Card>(cards);
                    deckCounter += 1;
                }
                else
                {
                    if (cardDetails[0] != "START")
                    {
                        if (cardDetails[0] == nameof(CombatCard))
                        {
                            cards.Add(new CombatCard(cardDetails[1], (EnumType) Enum.Parse(typeof(EnumType),cardDetails[2]), cardDetails[3], Int32.Parse(cardDetails[4]), bool.Parse(cardDetails[5])));
                        }
                        else
                        {
                            cards.Add(new SpecialCard(cardDetails[1], (EnumType)Enum.Parse(typeof(EnumType), cardDetails[2]), cardDetails[3]));
                        }
                    }
                    else
                    {
                        decks.Add(new Deck());
                        cards = new List<Card>();
                    }
                }

            }
            
        }
        public void AddCaptains()
        {
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\Captains.txt";
            StreamReader reader = new StreamReader(path);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] cardDetails = line.Split(",");
                captains.Add(new SpecialCard(cardDetails[1], (EnumType)Enum.Parse(typeof(EnumType), cardDetails[2]), cardDetails[3]));
            }
        }

        public int GetRoundWinner()
        {
            int[] attackPointOfPlayer = new int[2];
            attackPointOfPlayer = GetTotalAttackPoints();
            if (attackPointOfPlayer[0]> attackPointOfPlayer[1])
            {
                return 0;
            }
            else
            {
                if (attackPointOfPlayer[0]< attackPointOfPlayer[1])
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
        }
        public int[] GetLifePoints()
        {
            int[] totalLife = new int[] {0,0};
            totalLife[0]=Players[0].LifePoints;
            totalLife[1] = Players[1].LifePoints;
            return totalLife;
        }

        public int[] GetTotalAttackPoints()
        {
            int[] totalAttack = new int[2];
            totalAttack[0]= Players[0].GetAttackPoints()[0] ;
            totalAttack[1] = Players[1].GetAttackPoints()[0];
            return totalAttack;
        }
    }
}
