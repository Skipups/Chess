using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ChessGame;
using Newtonsoft.Json;

namespace ChessGameWeb
{
    public class GameStateStore
    {
        private static string SaveDirectory = Path.Combine(Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData),
                        "ChessWebSaves");
        public void Save(GameState game)
        {
            EnsureDirectoryExists();
            var fileName = game.GameID.ToString() + ".chess";
            var filePath = Path.Combine(SaveDirectory, fileName);
            string json = JsonConvert.SerializeObject(game, Formatting.Indented, new JsonSerializerSettings
            {

                TypeNameHandling = TypeNameHandling.Auto,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full,

            });
            File.Delete(filePath);

            File.WriteAllText(filePath, json);

        }

        public GameState Load(Guid gameId)
        {
            EnsureDirectoryExists();
            var fileName = gameId.ToString() + ".chess";
            var filePath = Path.Combine(SaveDirectory, fileName);

            GameState toReturn = null;

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                toReturn = JsonConvert.DeserializeObject<GameState>(json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full,
                });

               toReturn.Board.PopulatePieceToIdMap(toReturn.PlayerWhite.CapturedList, toReturn.PlayerBlack.CapturedList);
            }

            return toReturn;
        }

        private void EnsureDirectoryExists()
        {
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }
        }

       
    }
}