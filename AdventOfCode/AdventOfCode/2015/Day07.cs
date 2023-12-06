using AdventOfCode.Common;

namespace AdventOfCode._2015
{
    public abstract class Instruction
    {
        public string WireToSendTo { get; set; }

    }

    public class SignalToWire : Instruction
    {
        public ushort Signal { get; set; }
    }

    public class WireToWire : Instruction
    {
        public string FromWire { get; set; }

    }

    public class BitwiseAnd : Instruction
    {
        public string WireOne { get; set; }
        public string WireTwo { get; set; }
    }

    public class OneAnd : Instruction
    {
        public string WireToAnd { get; set; }
    }

    public class BitwiseOr : Instruction
    {
        public string WireOne { get; set; }
        public string WireTwo { get; set; }
    }
    public class LeftShift : Instruction
    {
        public string WireToShift { get; set; }
        public ushort AmountToShift { get; set; }
    }

    public class RightShift : Instruction
    {
        public string WireToShift { get; set; }
        public ushort AmountToShift { get; set; }
    }

    public class BitwiseComplement : Instruction
    {
        public string WireToComplement { get; set; }

    }



    public class Day07
    {
        private static Instruction ParseInstructions(string input) =>
            input.Split(" ") switch
            {
                ["NOT", var wF, "->", var wT] => new BitwiseComplement {WireToComplement = wF, WireToSendTo = wT},
                [var wF, "LSHIFT", var a, "->", var wT] => new LeftShift { AmountToShift = ushort.Parse(a), WireToSendTo = wT, WireToShift = wF},
                [var wF, "RSHIFT", var a, "->", var wT] => new RightShift { AmountToShift = ushort.Parse(a), WireToSendTo = wT, WireToShift = wF},
                ["1", "AND", var wireToAnd, "->", var wD] => new OneAnd { WireToAnd = wireToAnd, WireToSendTo = wD},
                [var w1, "AND", var w2, "->", var wD] => new BitwiseAnd { WireOne = w1, WireTwo = w2, WireToSendTo = wD},
                [var w1, "OR", var w2, "->", var wD] => new BitwiseOr { WireOne = w1, WireTwo = w2, WireToSendTo = wD},
                [var signal, "->", var wire] when ushort.TryParse(signal, out var s) => new SignalToWire { Signal = s, WireToSendTo = wire},
                [var from, "->", var to] => new WireToWire { FromWire = from, WireToSendTo = to }
            };

        private static bool CanCarryOutInstruction(IDictionary<string, ushort> wires, Instruction i) =>
            i switch
            {
                SignalToWire _ => true,
                BitwiseAnd bwa => wires.ContainsKey(bwa.WireOne) && wires.ContainsKey(bwa.WireTwo),
                BitwiseOr bwo => wires.ContainsKey(bwo.WireOne) && wires.ContainsKey(bwo.WireTwo),
                LeftShift ls => wires.ContainsKey(ls.WireToShift),
                RightShift rs => wires.ContainsKey(rs.WireToShift),
                BitwiseComplement bwc => wires.ContainsKey(bwc.WireToComplement),
                WireToWire wtw => wires.ContainsKey(wtw.FromWire),
                OneAnd oa => wires.ContainsKey(oa.WireToAnd)
            };

        private static IDictionary<string, ushort> CarryOutInstruction(IDictionary<string, ushort> oldWires, Instruction i)
        {
            var newValue = Convert.ToUInt16(i switch
            {
                SignalToWire stw => stw.Signal,
                BitwiseAnd bwa => oldWires[bwa.WireOne] & oldWires[bwa.WireTwo],
                OneAnd oa => 1 & oldWires[oa.WireToAnd],
                BitwiseOr bwo => oldWires[bwo.WireOne] | oldWires[bwo.WireTwo],
                LeftShift ls => oldWires[ls.WireToShift] << ls.AmountToShift,
                RightShift rs => oldWires[rs.WireToShift] >> rs.AmountToShift,
                BitwiseComplement bwc => (ushort)~oldWires[bwc.WireToComplement],
                WireToWire wtw => oldWires[wtw.FromWire]
            });

            var returnValue = oldWires.Append(new KeyValuePair<string, ushort>(i.WireToSendTo, newValue));

            return returnValue.ToDictionary(x => x.Key, x => x.Value);
        }




        [Fact]
        public void Day07_Test01()
        {
            var instruction = ParseInstructions("123 -> x");
            instruction.Should().BeOfType<SignalToWire>();

            var signalInsruction = (instruction as SignalToWire);
            signalInsruction.Signal.Should().Be(123);
            signalInsruction.WireToSendTo.Should().Be("x");
        }

        [Fact]
        public void Day07_Test02()
        {
            var instruction = ParseInstructions("x AND y -> z");
            instruction.Should().BeOfType<BitwiseAnd>();

            var signalInsruction = (instruction as BitwiseAnd);
            signalInsruction.WireOne.Should().Be("x");
            signalInsruction.WireTwo.Should().Be("y");
            signalInsruction.WireToSendTo.Should().Be("z");
        }

        [Fact]
        public void Day07_Test03()
        {
            var instruction = ParseInstructions("p LSHIFT 2 -> q");
            instruction.Should().BeOfType<LeftShift>();

            var signalInsruction = (instruction as LeftShift);
            signalInsruction.WireToShift.Should().Be("p");
            signalInsruction.AmountToShift.Should().Be(2);
            signalInsruction.WireToSendTo.Should().Be("q");
        }

        [Fact]
        public void Day07_Test04()
        {
            var instruction = ParseInstructions("NOT e -> f");
            instruction.Should().BeOfType<BitwiseComplement>();

            var signalInsruction = (instruction as BitwiseComplement);
            signalInsruction.WireToComplement.Should().Be("e");
            signalInsruction.WireToSendTo.Should().Be("f");
        }

        [Fact]
        public void Day07_Test05()
        {
            var instruction = ParseInstructions("123 -> x");
            var result = CarryOutInstruction(new Dictionary<string, ushort>(), instruction);
            result["x"].Should().Be(123);
            result.Count().Should().Be(1);
        }

        [Fact]
        public void Day07_Test06()
        {
            const string input = @"123 -> x
456 -> y
x AND y -> d";

            var instructions = input.Split("\r\n").Select(ParseInstructions);
            var result = instructions.Aggregate((new Dictionary<string, ushort>()) as IDictionary<string, ushort>, CarryOutInstruction);
            result["x"].Should().Be(123);
            result["y"].Should().Be(456);
            result["d"].Should().Be(72);
            result.Count().Should().Be(3);

        }

        [Fact]
        public void Day07_Test07()
        {
            const string input = @"123 -> x
456 -> y
x AND y -> d
x OR y -> e";

            var instructions = input.Split("\r\n").Select(ParseInstructions);
            var result = instructions.Aggregate((new Dictionary<string, ushort>()) as IDictionary<string, ushort>, CarryOutInstruction);
            result["x"].Should().Be(123);
            result["y"].Should().Be(456);
            result["d"].Should().Be(72);
            result["e"].Should().Be(507);
            result.Count().Should().Be(4);

        }

        [Fact]
        public void Day07_Test08()
        {
            const string input = @"123 -> x
456 -> y
x AND y -> d
x OR y -> e
x LSHIFT 2 -> f";

            var instructions = input.Split("\r\n").Select(ParseInstructions);
            var result = instructions.Aggregate((new Dictionary<string, ushort>()) as IDictionary<string, ushort>, CarryOutInstruction);
            result["x"].Should().Be(123);
            result["y"].Should().Be(456);
            result["d"].Should().Be(72);
            result["e"].Should().Be(507);
            result["f"].Should().Be(492);
            result.Count().Should().Be(5);

        }

        [Fact]
        public void Day07_Test09()
        {
            const string input = @"123 -> x
456 -> y
x AND y -> d
x OR y -> e
x LSHIFT 2 -> f
y RSHIFT 2 -> g";

            var instructions = input.Split("\r\n").Select(ParseInstructions);
            var result = instructions.Aggregate((new Dictionary<string, ushort>()) as IDictionary<string, ushort>, CarryOutInstruction);
            result["x"].Should().Be(123);
            result["y"].Should().Be(456);
            result["d"].Should().Be(72);
            result["e"].Should().Be(507);
            result["f"].Should().Be(492);
            result["g"].Should().Be(114);
            result.Count().Should().Be(6);

        }

        [Fact]
        public void Day07_Test10()
        {
            const string input = @"123 -> x
456 -> y
x AND y -> d
x OR y -> e
x LSHIFT 2 -> f
y RSHIFT 2 -> g
NOT x -> h
NOT y -> i";

            var instructions = input.Split("\r\n").Select(ParseInstructions);
            var result = instructions.Aggregate((new Dictionary<string, ushort>()) as IDictionary<string, ushort>, CarryOutInstruction);
            result["x"].Should().Be(123);
            result["y"].Should().Be(456);
            result["d"].Should().Be(72);
            result["e"].Should().Be(507);
            result["f"].Should().Be(492);
            result["g"].Should().Be(114);
            result["h"].Should().Be(65412);
            result["i"].Should().Be(65079);
            result.Count().Should().Be(8);

        }

       
        [Fact]
        public void Day07_Part01()
        {
            var input = File.ReadAllLines("./2015/Day07Input.txt");
            var instructions = input.Select(ParseInstructions);
            var result = new Dictionary<string, ushort>().IterateUntil(x => x.ContainsKey("a"),
                x => instructions.Aggregate(x, (agg, y) => (Dictionary<string, ushort>)(!agg.ContainsKey(y.WireToSendTo) &&  CanCarryOutInstruction(x, y)
                    ? CarryOutInstruction(agg, y)
                    : agg)));

            result["a"].Should().Be(956);
        }

        [Fact]
        public void Day07_Part02()
        {
            var input = File.ReadAllLines("./2015/Day07Input.txt");
            var instructions = input.Select(ParseInstructions);
            var result = new Dictionary<string, ushort>
            {
                {"b", 956}
            }.IterateUntil(x => x.ContainsKey("a"),
                x => instructions.Aggregate(x, (agg, y) => (Dictionary<string, ushort>)(!agg.ContainsKey(y.WireToSendTo) && CanCarryOutInstruction(x, y)
                    ? CarryOutInstruction(agg, y)
                    : agg)));

            result["a"].Should().Be(40149);
        }
    }
}
