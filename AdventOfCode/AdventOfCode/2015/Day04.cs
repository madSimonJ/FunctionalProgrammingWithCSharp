using System.Text;
using AdventOfCode.Common;

namespace AdventOfCode._2015
{
    public class Day04
    {
        [Fact]
        public void Day04_Test01()
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            var input = "abcdef";

            var iter = new IndefiniteEnumerable<string>(i => $"{input}{i}"
                    .Map(x => Encoding.ASCII.GetBytes(x))
                    .Map(x => md5.ComputeHash(x))
                    .Map(x => BitConverter.ToString(x))
                    .Map(x => x.Replace("-", string.Empty)));
            var result = iter.findIndex(x => x.StartsWith("00000"));
            result.Should().Be(609043);

        }

        [Fact]
        public void Day04_Test02()
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            var input = "pqrstuv";

            var iter = new IndefiniteEnumerable<string>(i => $"{input}{i}"
                .Map(x => Encoding.ASCII.GetBytes(x))
                .Map(x => md5.ComputeHash(x))
                .Map(x => BitConverter.ToString(x))
                .Map(x => x.Replace("-", string.Empty)));
            var result = iter.findIndex(x => x.StartsWith("00000"));
            result.Should().Be(1048970);

        }

        [Fact]
        public void Day04_Part01()
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            var input = "ckczppom";

            var iter = new IndefiniteEnumerable<string>(i => $"{input}{i}"
                .Map(x => Encoding.ASCII.GetBytes(x))
                .Map(x => md5.ComputeHash(x))
                .Map(x => BitConverter.ToString(x))
                .Map(x => x.Replace("-", string.Empty)));
            var result = iter.findIndex(x => x.StartsWith("00000"));
            result.Should().Be(117946);

        }

        [Fact]
        public void Day04_Part02()
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            var input = "ckczppom";

            var iter = new IndefiniteEnumerable<string>(i => $"{input}{i}"
                .Map(x => Encoding.ASCII.GetBytes(x))
                .Map(x => md5.ComputeHash(x))
                .Map(x => BitConverter.ToString(x))
                .Map(x => x.Replace("-", string.Empty)));
            var result = iter.findIndex(x => x.StartsWith("000000"));
            result.Should().Be(3938038);

        }
    }
}
