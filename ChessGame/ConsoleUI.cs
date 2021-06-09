using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChessGame
{
    class ConsoleUI : IGameEngineUI, IGameUI
    {
        private const string Horizontal = "+--------";
        private const string Vertical = "| ";
        private const int Size = 8;
        public void DisplayCapturedPiece(Piece piece)
        {
            Console.WriteLine($"{piece.DisplayPieceInfo} was captured");
        }

        public void DisplayGameOver(GameState gamestate, Player player)
        {
            Console.WriteLine($"Checkmate! {player.Name} wins.");
        }

        public void DisplayInvalidMove(Player player, string message)
        {
            Console.WriteLine($"{player.Name} that was an invalid move due to: {message}. Try again.");
        }

        public void DisplayMoveSuccessful(GameState game, Player player, Coord start, Coord end)
        {
            DrawBoard(game.Board);
            Console.WriteLine($"{player.Name} successfully moved from {start} to {end}.");
        }
        public void DisplayNotificationCheckOccured(Player currentPlayerTurn, Player playerInCheck)
        {
            Console.WriteLine($"{currentPlayerTurn.Name} has put {playerInCheck.Name} in check");
   
        }
        public void DisplayReminderKingInCheck(Player currentPlayerTurn)
        {
            Console.WriteLine($"{currentPlayerTurn.Name} you are in check");

        }
        public void DisplayCoordsThatHaveKingInCheck(List<Coord> listOfCoordsTheHaveKingInCheck)
        {
            StringBuilder line = new StringBuilder();
            if(listOfCoordsTheHaveKingInCheck.Count == 1)
            {
                line.Append($"Check is happening due to the piece on coord {listOfCoordsTheHaveKingInCheck[0]}.");
            }
            else
            {
                line.Append($"Check is happening due to the pieces on coords:");
                for (int coord=0; coord<listOfCoordsTheHaveKingInCheck.Count-1; coord++)
                {
                    line.Append($" {listOfCoordsTheHaveKingInCheck[coord]},");
                }

                line.Append($" and {listOfCoordsTheHaveKingInCheck[listOfCoordsTheHaveKingInCheck.Count - 1]}.");
            }
            Console.WriteLine(line);
        }
        public void DrawBoard(IBoard board)
        {
            Console.WriteLine();
            Console.WriteLine("Board");
            Console.WriteLine("      0        1        2        3        4        5        6        7"); // column number
            for (int y = 0; y < Size; y++)
            {
                StringBuilder nextLine = new StringBuilder();
                nextLine.Append("  "); // move board left 2 spaces
                for (int x = 0; x < Size; x++)
                {
                    nextLine.Append(Horizontal); // write horizontal part of the cell
                }
                //Console.Write("+\n");  // add + at the end of row
                nextLine.Append("+\n");
                for (int x = 0; x < Size; x++)
                {
                    if (x == 0)
                    {

                        nextLine.Append(y + " "); //row number
                    }

                    char color = 'B';   // color of the celll
                    if ((y + x) % 2 == 0)
                    {
                        color = 'W';
                    }


                    Piece piece = board.GetPiece(new Coord(x, y));
                    //PieceCoordMap.TryGetValue(new Coord(x, y), out piece);

                    string cellInfo = PrintCell(x, y, color, piece);
                    nextLine.Append(Vertical + cellInfo + " ");
                }
                nextLine.Append("|");
                Console.WriteLine(nextLine);
            }
            // add bottom border
            StringBuilder border = new StringBuilder();
            border.Append("  ");

            for (int c = 0; c < Size; c++)
            {

                border.Append(Horizontal);
            }
            border.Append("+\n\n");
            Console.WriteLine(border);

        }
        private string PrintCell(int x, int y, char color, Piece piece = null)
        {
            if (piece == null)
            {
                return $" {color}(--)";
            }
            else
            {
                char pieceColor = piece.White ? 'W' : 'B';
                return $"{color}({pieceColor}-{piece.FirstLetter})";
            }
        }
        public void DrawCapturedList(Player player)
        {
            int count = player.CapturedList.Count();
            int numOfPawns = 0;
            StringBuilder line = new StringBuilder();
            line.Append("    "); // 
            line.Append(($"Piece's {player.Name} has captured: "));

            for (int i = 0; i < count; ++i)
            {
                if (player.CapturedList[i].IsPiecePromotable != null)
                {
                    numOfPawns++;
                }
                else
                {
                    line.Append($"{i -numOfPawns}. {player.CapturedList[i].DisplayPieceInfo} ");
                }

            }
            line.Append($" (Number of captured pawns {numOfPawns})");

            Console.WriteLine(line);
        }

        public Tuple<Coord, Coord> GetMove(GameState game, Player player)
        {
            Console.WriteLine($"{player.Name}'s turn!");

            DrawCapturedList(game.PlayerBlack);
           DrawBoard(game.Board);
            DrawCapturedList(game.PlayerWhite);

            Console.WriteLine("Type the start coordinate of the piece you want to move: \nExample type 0,6 ");
            var startCoord = Console.ReadLine();
            var IsStartCoord = true;
            Coord startCoordParsed = Menu.ParseMoveString(startCoord);
            var startCoordCantBeParsed = startCoordParsed.Equals(Coord.InvalidCoordinate);
            while (startCoordCantBeParsed || !game.Board.IsValidMoveCoord(startCoordParsed, IsStartCoord, game.TurnWhite))
            {
                Console.WriteLine($"{startCoordParsed} is not a valid input. Type the start coordinate of the piece you want to move: \nExample type 0,6");
                startCoord = Console.ReadLine();


                startCoordParsed = Menu.ParseMoveString(startCoord);
                startCoordCantBeParsed = startCoordParsed.Equals(Coord.InvalidCoordinate);
            }
            IsStartCoord = false;
            Console.WriteLine("Type the destination coordinate.\nExample type 0,5 ");
            var endCoord = Console.ReadLine();
            Coord endCoordParsed = Menu.ParseMoveString(endCoord);

            var endCoordCantBeParsed = endCoordParsed.Equals(Coord.InvalidCoordinate);
            while (endCoordCantBeParsed || !game.Board.IsValidMoveCoord(endCoordParsed, IsStartCoord, game.TurnWhite))
            {

                Console.WriteLine("Invalid input. Type the destination coordinate.\nExample type 0,5 ");
                endCoord = Console.ReadLine();
                endCoordParsed = Menu.ParseMoveString(endCoord);
                endCoordCantBeParsed = endCoordParsed.Equals(Coord.InvalidCoordinate);
            }

            return new Tuple<Coord, Coord>(startCoordParsed, endCoordParsed);

        }

        public Piece GetPieceToPromote(GameState gamestate, Player player, List<Piece> capturedPieces)
        {
           
                var pieceDict = new SortedDictionary<int, Piece>();
                foreach (var capPiece in capturedPieces)
                {
                    if (capPiece.IsPiecePromotable == null)
                    {
                        pieceDict.Add(pieceDict.Count + 1, capPiece);
                    }
                }
                Piece toReturn = null;
                while (toReturn == null)
                {
                    // int count = this.CapturedList.Count();
                    StringBuilder line = new StringBuilder();
                    line.Append("    "); // 

                    foreach (var kvp in pieceDict)
                    {
                        line.Append($"{kvp.Key}. {kvp.Value.DisplayPieceInfo} ");
                    }
                    Console.WriteLine(line);
                    Console.WriteLine("Pawn Promotion! Type the number of the piece you want to promote:");
                    var pieceNumberSelection = Console.ReadLine();
                    int pieceNumberParsed = Menu.ParsePromotionSelectionString(pieceNumberSelection);
                    if (!pieceDict.TryGetValue(pieceNumberParsed, out toReturn))
                    {
                        Console.WriteLine("Invalid selection");
                    }
                }
                return toReturn;
        }

        public UserAction GetUserAction(Player player, GameState game)
        {
           this.DrawBoard(game.Board);
            while (true)
            {
                Console.WriteLine(" Menue:");
                Console.WriteLine(" q-- quit   m-- move  n-- newGame s-- save l-- loadGame");
                var userInput = Console.ReadLine();
                if (userInput == "q")
                {
                    return UserAction.Quit;
                }
                else if (userInput == "m")
                {
                    return UserAction.PlayTurn;
                }
                else if (userInput == "n")
                {
                    return UserAction.NewGame;
                }
                else if (userInput == "s")
                {
                    return UserAction.SaveGame;
                }
                else if (userInput == "l")
                {
                    return UserAction.LoadGame;
                }
                Console.WriteLine("Invalid selection");
            }
        }

        public bool AskUserAboutCastling(Player player)
        {
            while (true)
            {
                Console.WriteLine($"{player.Name}, you have the option of castling.");
                Console.WriteLine($"If you would like to castle type 'c'.");
                Console.WriteLine($"Otherwise type 'n'.");
                var userInput = Console.ReadLine();
                if (userInput == "c")
                {
                    return true;
                }
                else if (userInput == "m")
                {
                    return false;
                }
                Console.WriteLine("Invalid selection");
            }
        }

        public  CastleCoord GetCastlingMove(List<CastleCoord> possibleCastlingOptions)
        {
            while (true)
            if(possibleCastlingOptions.Count == 1)
            {
                Console.WriteLine($"Would you like to execute castling with the King and Rook on coords {possibleCastlingOptions[0].KingCoord} and {possibleCastlingOptions[0].RookCoord}. Type 'y' to execute move, else type 'n' to perform other move");
                var userInput = Console.ReadLine();
                if (userInput == "y")
                {
                    return new CastleCoord(possibleCastlingOptions[0].KingCoord, possibleCastlingOptions[0].RookCoord);
                }
                else if (userInput == "n")
                    {
                        return null;
                    }
                    Console.WriteLine("Invalid selection");
                }
            else
            {
                Console.WriteLine($"You have 2 castling options.");
                Console.WriteLine($"Option 1 with the King and Rook on coords {possibleCastlingOptions[0].KingCoord}and {possibleCastlingOptions[0].RookCoord}. Type 'y1'. ");
                Console.WriteLine($"Option 2 with the King and Rook on coords {possibleCastlingOptions[1].KingCoord}and {possibleCastlingOptions[1].RookCoord}. Type '2'."); 
                Console.WriteLine($"Type '0' to exist castling and to perform other move.");

                    var userInput = Console.ReadLine();
                    if (userInput == "y1")
                    {
                        return new CastleCoord(possibleCastlingOptions[0].KingCoord, possibleCastlingOptions[0].RookCoord);
                    }
                    else if (userInput == "y2")
                    {
                        return new CastleCoord(possibleCastlingOptions[1].KingCoord, possibleCastlingOptions[1].RookCoord);
                    }
                    else if (userInput == "n")
                    {
                        return null;
                    }
                    Console.WriteLine("Invalid selection");
                }
        }

        public void SaveGame(GameState game)
        {
            //string json = JsonConvert.SerializeObject(game, Formatting.Indented, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });

            string json = JsonConvert.SerializeObject(game, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full,
              

            });

            string folder = Path.Combine(Environment.GetFolderPath(
                         Environment.SpecialFolder.LocalApplicationData),
                         "ChessSaves");
            Directory.CreateDirectory(folder);
            var invalidInput = true;
            while (invalidInput)
            {
                Console.WriteLine("Name your saved game:");
                var userInput = Console.ReadLine();
                var fileName = userInput + ".chess";
                string namedSavePath = Path.Combine(folder, fileName);

                if (!System.IO.File.Exists(namedSavePath))
                {
                    System.IO.File.WriteAllText(namedSavePath, json);
                    invalidInput = false;
                }
                else
                {
                    Console.WriteLine("Saved game with name \"{0}\" already exists. Enter a new name", userInput);
                }
            }
            //System.IO.File.WriteAllText("C:\\temp\\game2.chess", json);
            //String path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }
       public string LoadGame(GameState game)
        {
            var targetDirectory = Path.Combine(Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData),
                        "ChessSaves");
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            StringBuilder fileNames = new StringBuilder();
            var counter = 1;
            foreach (string fileName in fileEntries)
            {
                FileInfo fi = new FileInfo(fileName);
                fileNames.AppendLine($"{counter++}. {fi.Name}");
            }

            String selectedFile = null;
            var invalidInput = true;
            while (invalidInput)
            {
                Console.WriteLine(fileNames);
                Console.WriteLine("Select the saved game you wish to load by entering the corresponding number, i.e. '1'");
                var userSelection = Console.ReadLine();
                var indexPlusOne = Menu.ParseUserSavedGameSelection(userSelection);

                if (indexPlusOne > 0 && indexPlusOne <= fileEntries.Length)
                {
                    var index = indexPlusOne - 1;
                    selectedFile = fileEntries[index];
                    invalidInput = false;
                }
                else
                {
                    Console.WriteLine("Please enter a valid selection");
                }
            }



            string json = System.IO.File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), selectedFile));
            return json;

          


            //string json2 = System.IO.File.ReadAllText("C:\\temp\\game.chess");
            //game = JsonConvert.DeserializeObject<GameState>(json2, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });
        }
    }
    }

