using MartianTrail;
using MartianTrail.Communications.WebApi;
using MartianTrail.Entities;
using MartianTrail.GamePhases;
using MartianTrail.InventorySelection;
using MartianTrail.MiniGame;
using MartianTrail.PlayerInteraction;
using MartianTrail.RandomNumbers;
using MartianTrail.TimeService;

var playerInteraction = new PlayerInteractionClient(new ConsoleShim());

playerInteraction.WriteMessage(
"THE MARTIAN TRAIL",
    "_________________",
    "",
    "By Simon J. Painter",
    "",
    "A Sample Functional-style C# Game",
    "Inspired by \"Oregon Trail\" (1975 version) by Don Rawitsch",
    "Shared under a Creative Commons 3.0 Licence",
    ""
);

var wantInstructionsResponse = playerInteraction.GetInput("Would you like the instructions?");
var wantInstructions = wantInstructionsResponse is TextInput ti && ti.TextFromUser.ToUpper() == "YES";

playerInteraction.WriteMessageConditional(wantInstructions,
    "The year is 2147, and humanity has finally reached the planet Mars. Not only have we\r\ntraveled there, but settlement of the red planet is well underway. New cities, outposts,\r\nand trading posts are starting to spring up everywhere",
    "You and your family are among the latest batch of settlers to set down at the main\r\ntravel terminus, which is located in the colossal impact crater known as Hellas Basin.\r\nTravel time from Earth to Mars is far faster than it was back in the old days, but it’s\r\nstill a matter of weeks. You spent all that time planning your route from Hellas Basin\r\nto your plot of land up in Amazonis Planitia, which will involve a crossing of Tharsis\r\nRise along the way. It’s going to be a long, difficult, and dangerous journey.",
    "Not only is Mars a harsh environment, requiring everyone to wear atmosphere\r\nsuits the entire time you’re on the surface, but also it turns out there absolutely are\r\nMartians. Writers from the 20th century who took space exploration far less seriously\r\nthan they should have portrayed Martians as small, green-skinned creatures with no\r\nhair and antennae coming out of theirs heads. As it turns out, that’s precisely what\r\nthey look like. Who’d have thought it?",
    "Most Martians are fairly affable and don’t mind trading with the incoming Earthlings.\r\nHumanity could learn a lot from those folks. But some aren’t keen on what they see as\r\ntrespassers on their land, and those are the ones to look out for on the trail ahead",
    "For gathering food on the journey (it’ll last weeks, and you can’t carry that much with\r\nyou), you’ll have the chance to hunt a type of native martian fauna: Vrolids. They’re\r\nshort, stocky, and purple, and smell bad but taste good.",
    "For earning money, you can attempt to corner a herd of wild Lophroll, whose long,\r\nluxuriant fur is perfect for coats, or ’70s prog-rock style wigs for amateur guitarists\r\nand flutists. Prog rock had a resurgence in popularity in 2145, and there are even now\r\nalters to rock gods Ian Anderson and Steve Hackett on Earth’s capital city.2 Finally,\r\nyou’ll periodically be able to trade with outposts along the route for supplies, if you\r\nmake it that far!",
    "It’s going to take weeks of hard traveling by hover barge to get where you need to\r\nbe—over 16,000 kilometers away! Best of luck!"
);

var inventoryClient = new SelectInitialInventoryClient();
var initialInventory = inventoryClient.SelectInitialInventory(playerInteraction);

var mg = new PlayMiniGame(new RandomNumberGenerator(), playerInteraction, new TimeService());


var g = new Game();

g.Play(new GameState {Inventory = initialInventory }, playerInteraction, 
    new DisplayMartianWeather(new FetchWebApiDataClient(new HttpClientShim(new HttpClient()))),
    new SelectAction(new RandomNumberGenerator(), playerInteraction, new PlayMiniGame(new RandomNumberGenerator(), playerInteraction, new TimeService()))
    );

