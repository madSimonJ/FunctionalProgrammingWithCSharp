using ConsoleGame.Common;

namespace SuperStarTrek;

public record SuperStarTrekGameState : GameState
{
    public string ZString { get; set; }
    public string Z { get; set; }
    public string G { get; set; }
    public IEnumerable<IEnumerable<int>> C { get; set; }
    public string K { get; set; }
    public string N { get; set; }
    public string D { get; set; }
    public int T { get; set; }
    public int T0 { get; set; }
    public int T9 { get; set; }
    public int D0 { get; set; }
    public int E { get; set; }
    public int E0 { get; set; }
    public int P { get; set; }
    public int P0 { get; set; }
    public int S9 { get; set; }
    public int S { get; set; }
    public int B9 { get; set; }
    public int K9 { get; set; }
    public string XString { get; set; }
    public string X0String { get; set; }
    public int Q1 { get; set; }
    public int Q2 { get; set; }
    public int S1 { get; set; }
    public int S2 { get; set; }
    public int K3 { get; set; }
    public int B3 { get; set; }


    public override bool IsGameOver()
    {
        throw new NotImplementedException();
    }
}