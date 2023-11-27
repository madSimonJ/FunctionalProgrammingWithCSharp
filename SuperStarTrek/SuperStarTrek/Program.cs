using ConsoleGame.Common;
using ConsoleGame.Common.RandomNumbers;
using SuperStarTrek;

var rnd = new RandomNumberGenerator();


var gameEngine = new GameEngine<SuperStarTrekGameState>(rnd);


var fnr = (int x) => rnd.BetweenZeroAnd((int)(x * 7.98M + 1.01M));

var finalState = new GameEngine<SuperStarTrekGameState>(rnd)
    .SetUpInteraction(x => x.Console)
    .InitialInventory(rnd => new SuperStarTrekGameState
    {
        T = rnd.Between(2000, 4000),
        T9 = rnd.Between(25, 35),
        E = 3000,
        P = 10,
        S9 = 200,
        B9 = 2,
        XString = "",
        X0String = " IS ",
        Q1 = fnr(1),
        Q2 = fnr(1),
        S1 = fnr(1),
        S2 = fnr(1),
        C = Enumerable.Repeat(Enumerable.Repeat(0, 2), 9),
        K3 = rnd.BetweenZeroAnd(100) switch
        {
            > 98 => 3,
            > 95 => 2,
            > 80 => 1,
            _ => 0
        },
        B3 = rnd.BetweenZeroAnd(100) switch
        {
            > 96 => 1,
            _ => 0
        }

    }.Map(x => x with
    {
        T0 = x.T,
        E0 = x.E,
        P0 = x.P,
        K9 = x.K3,
        B9 = x.B3
    }))
    .IntroductionMessage(
        x => x.WriteBlankLines(11),
        x => x.WriteMessage(
                "                                    ,------*------,", 
                            "                    ,-------------   '---  ------'", 
                            "                     '-------- --'      / /", 
                            "                         ,---' '-------/ /--,", 
                            "                          '----------------'", 
                            string.Empty, 
                            "                    THE USS ENTERPRISE --- NCC-1701"),
        x => x.WriteBlankLines(5)
        )
    //.TakeTurn(x => { })
    //.EndGame(x => { })
    .Play();

