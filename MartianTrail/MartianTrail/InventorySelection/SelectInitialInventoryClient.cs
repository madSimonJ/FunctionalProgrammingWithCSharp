using MartianTrail.Entities;
using MartianTrail.PlayerInteraction;

namespace MartianTrail.InventorySelection
{
    public class SelectInitialInventoryClient : IInventorySelection
    {

        private static void DisplayInventory(IPlayerInteraction playerInteraction, InventorySelectionState state) =>
            playerInteraction.WriteMessage(
                "Batteries: " + state.NumberOfBatteries,
                "Food Packs: " + state.Food,
                "Laser Charges: " + state.LaserCharges,
                "Atmosphere Suits: " + state.AtmosphereSuits,
                "MediPacks: " + state.MediPacks,
                "Remaining Credits: " + state.Credits

            );

        private readonly IEnumerable<InventoryConfiguration> _inventorySelections = new[]
        {
            new InventoryConfiguration("Batteries", 50, (q, oldState) => 
                oldState with { NumberOfBatteries = q}),
            new InventoryConfiguration("Food Packs", 50, (q, oldState) => 
                oldState with { Food = q}),
            new InventoryConfiguration("Laser Charges ", 50, (q, oldState) => 
                oldState with { LaserCharges = q}),
            new InventoryConfiguration("Atmosphere Suits", 50, (q, oldState) => 
                oldState with { AtmosphereSuits = q}),
            new InventoryConfiguration("MediPacks", 50, (q, oldState) => 
                oldState with { MediPacks = q})
        };

        private static InventorySelectionState MakeInventorySelection(
            IPlayerInteraction playerInteraction,
            InventorySelectionState oldState,
            string name,
            int costPerItem,
            Func<int, InventorySelectionState, InventorySelectionState> updateFunc)
        {
            var numberAffordable = oldState.Credits / costPerItem;
            bool ValidateUserChoice(int x) => x >= 0 && x <= numberAffordable;

            var userAttempt = playerInteraction.GetInput($"{name} Selection.  They cost {costPerItem} per item.  How many would you like?  You can't afford more than {numberAffordable}");

            var validUserInput = userAttempt.IterateUntil(
                x =>
                {
                    var userMessage = userAttempt switch
                    {
                        IntegerInput { IntegerFromUser: < 0 } => "That was less than zero",
                        IntegerInput i when (i.IntegerFromUser * costPerItem) > oldState.Credits => "You can't accord that many!",
                        IntegerInput _ => "Thank you",
                        EmptyInput => "You have to enter a value",
                        TextInput => "That wasn't an integer value",
                        UserInputError e => "An error occurred: " + e.ExceptionRaised.Message
                    };

                    playerInteraction.WriteMessage(userMessage);
                    
                    return x is IntegerInput ii && ValidateUserChoice(ii.IntegerFromUser)
                        ? x
                        : playerInteraction.GetInput("Please try again...");
                }, x => x is IntegerInput ii && ValidateUserChoice(ii.IntegerFromUser));

            var numberOfItemsBought = (validUserInput as IntegerInput).IntegerFromUser;

            var updatedInventory = updateFunc(numberOfItemsBought, oldState) with
            {
                Credits = oldState.Credits - (numberOfItemsBought * costPerItem)
            };

            return updatedInventory;
        }

        private static InventorySelectionState UpdateUserIsHappyStatus(IPlayerInteraction playerInteraction,
            InventorySelectionState oldState)
        {
            var yes = new[]
            {
                "Y",
                "YES",
                "YEP",
                "WHY NOT",
            };

            var no = new[]
            {
                "N",
                "NO",
                "NOPE",
                "ARE YOU JOKING??!??",
            };

            DisplayInventory(playerInteraction, oldState);

            bool GetPlayerResponse(string message)
            {
                var playerResponse = playerInteraction.GetInput(message);
                var validatedPlayerResponse = playerResponse switch
                {
                    TextInput ti when yes.Contains(ti.TextFromUser.ToUpper()) => true,
                    TextInput ti when no.Contains(ti.TextFromUser.ToUpper()) => false,
                    _ => GetPlayerResponse("I didn't understand that, could you try again?")
                };
                return validatedPlayerResponse;
            };

            return (oldState with
            {
                PlayerIsHappyWithSelection = GetPlayerResponse("Are you happy with these purchases?")
            }).Map(x => x with {Credits = x.PlayerIsHappyWithSelection ? x.Credits : 1000});
        }



        public InventoryState SelectInitialInventory(IPlayerInteraction playerInteraction)
        {
            var initialState = new InventorySelectionState
            {
                Credits = 1000
            };

            var finalState = initialState.IterateUntil(x => 
                    this._inventorySelections.Aggregate(x, (acc, y) =>
                            MakeInventorySelection(playerInteraction, acc, y.Name, y.CostPerItem, y.UpdateFunc)
                    ).Map(y => UpdateUserIsHappyStatus(playerInteraction, y))
                , x => x.PlayerIsHappyWithSelection);

            var returnValue = new InventoryState
            {
                NumberOfBatteries = finalState.NumberOfBatteries,
                Food = finalState.Food,
                LaserCharges = finalState.LaserCharges,
                AtmosphereSuits = finalState.AtmosphereSuits,
                MediPacks = finalState.MediPacks,
                Credits = finalState.Credits
            };
            return returnValue;
        }

        public abstract class InventorySelectionResult { }

        public class InventorySelectionInvalidInput { }

        public class InventorySelectionValueTooLow { }

        public class InventorySelectionValueTooHigh { }

        public class InventorySelectionValid
        {
            public int QuantitySelected { get; set; }
            public int UpdatedCreditsAmount { get; set; }
        }

        public struct InventoryConfiguration
        {
            public string Name { get; set; }
            public int CostPerItem { get; set; }
            public Func<int, InventorySelectionState, InventorySelectionState> UpdateFunc { get; set; }

            public InventoryConfiguration(string name, int costPerItem, Func<int, InventorySelectionState, InventorySelectionState> updateFunc)
            {
                Name = name;
                CostPerItem = costPerItem;
                UpdateFunc = updateFunc;
            }

        }



    }
}
