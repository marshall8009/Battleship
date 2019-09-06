using BattleshipLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLibrary
{
    public static class GameLogic
    {
        public static void InitializeGrid(PlayerInfoModel model)
        {
            List<string> letters = new List<string>
            {
                "A",
                "B",
                "C",
                "D",
                "E"
            };

            List<int> numbers = new List<int>
            {
                1,
                2,
                3,
                4,
                5
            };

            foreach (string letter in letters)
            {
                foreach (int number in numbers)
                {
                    AddGridSpot(model, letter, number);
                }
            }
        }

        private static void AddGridSpot(PlayerInfoModel model, string letter, int number)
        {
            GridSpotModel spot = new GridSpotModel
            {
                SpotLetter = letter,
                SpotNumber = number,
                Status = GridSpotStatus.Empty
            };

            model.ShotGrid.Add(spot);
        }

        public static bool PlayerStillActive(PlayerInfoModel player)
        {
            bool isActive = false;

            foreach (var ship in player.ShipLocations)
            {
                if (ship.Status != GridSpotStatus.Sunk)
                {
                    isActive = true;
                }
            }

            return isActive;
            
        }

        public static bool PlaceShip(PlayerInfoModel model, string location)
        {
            bool output = false;

            (string row, int column) = SplitShotIntoRowAndColumn(location);

            bool isValidLocation = ValidatedGridLoction(model, row, column);
            bool isLocationOpen = ValidateShipLocation(model, row, column);

            if (isValidLocation && isLocationOpen)
            {
                model.ShipLocations.Add(new GridSpotModel
                {
                    SpotLetter = row.ToUpper(),
                    SpotNumber = column,
                    Status = GridSpotStatus.Ship
                });

                output = true;
            }

            return output;
        }

        private static bool ValidateShipLocation(PlayerInfoModel model, string row, int column)
        {
            bool isValidLocation = true;

            foreach (var ship in model.ShipLocations)
            {
                if (ship.SpotLetter == row.ToUpper() && ship.SpotNumber == column)
                {
                    isValidLocation = false;
                }
            }

            return isValidLocation;
        }

        private static bool ValidatedGridLoction(PlayerInfoModel model, string row, int column)
        {
            bool isValidLocation = false;

            foreach (var shot in model.ShotGrid)
            {
                if (shot.SpotLetter == row.ToUpper() && shot.SpotNumber == column)
                {
                    isValidLocation = true;
                }
            }

            return isValidLocation;
        }

        public static int GetShotCount(PlayerInfoModel player)
        {
            int count = 0;

            foreach (var shot in player.ShotGrid)
            {
                if (shot.Status != GridSpotStatus.Empty)
                {
                    count++;
                }
            }

            return count;
        }

        public static (string row, int column) SplitShotIntoRowAndColumn(string shot)
        {
            string row = "";
            int column = 0;

            if (shot.Length != 2)
            {
                throw new ArgumentException("Invalid shot type.", "shot");
            }

            char[] shotArray = shot.ToArray();

            row = shotArray[0].ToString();
            column = int.Parse(shotArray[1].ToString());

            return (row, column);
        }

        public static bool ValidateShot(PlayerInfoModel player, string row, int column)
        {
            bool isValidShot = false;

            foreach (var shot in player.ShotGrid)
            {
                if (shot.SpotLetter == row.ToUpper() && shot.SpotNumber == column)
                {
                    if (shot.Status == GridSpotStatus.Empty)
                    {
                        isValidShot = true;
                    }
                }
            }

            return isValidShot;
        }

        public static bool IdentifyShotResult(PlayerInfoModel opponent, string row, int column)
        {
            bool isAHit = false;

            foreach (var ship in opponent.ShipLocations)
            {
                if (ship.SpotLetter == row.ToUpper() && ship.SpotNumber == column)
                {
                    isAHit = true;
                }
            }

            return isAHit;
        }

        public static void MarkShotResult(PlayerInfoModel player, string row, int column, bool isAHit)
        {
            foreach (var shot in player.ShotGrid)
            {
                if (shot.SpotLetter == row.ToUpper() && shot.SpotNumber == column)
                {
                    if (isAHit)
                    {
                        shot.Status = GridSpotStatus.Hit;
                    }
                    else
                    {
                        shot.Status = GridSpotStatus.Miss;
                    }
                }
            }
        }
    }
}
