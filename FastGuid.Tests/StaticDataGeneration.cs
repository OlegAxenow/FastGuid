using System;
using System.Text;
using NUnit.Framework;

namespace FastGuid.Tests
{
	[TestFixture]
	public class StaticDataGeneration
	{
		[Test, Explicit("Manual generation of HexDwords")]
		public void Generate_HexDwords()
		{
			// arrange
			var hexDwords = new uint[256];
			for (int i = 0; i < hexDwords.Length; i++)
			{
				var tuple = HexTuples[i];
				hexDwords[i] = (uint)(tuple.Item1) ^ ((uint)tuple.Item2 << 16);
			}

			var builder = new StringBuilder(3000);
			builder.Append("{");
			// act
			for (int i = 0; i < hexDwords.Length; i++)
			{
				if (i % 8 == 0)
					builder.AppendLine();
				else
					builder.Append(" ");

				builder.Append("0x").Append(((int)hexDwords[i]).ToString("x")).Append(",");
			}

			builder.AppendLine().Append("};");
			Console.WriteLine(builder.ToString());
		}

		[Test, Explicit("Manual generation of BitsFromHex")]
		public void Generate_BitsFromHex()
		{
			// arrange
			var bits = new ushort[256];

			for (int i = 0; i < bits.Length; i++)
			{
				bits[i] = 256;
			}

			for (ushort i = 0; i < HexTuplesForParsing.Length; i++)
			{
				var tuple = HexTuplesForParsing[i];
				bits[tuple.Item1] = i;
				bits[tuple.Item2] = i;
			}

			var builder = new StringBuilder(3000);
			builder.Append("{");
			// act
			for (int i = 0; i < bits.Length; i++)
			{
				if (i % 8 == 0)
					builder.AppendLine();
				else
					builder.Append(" ");

				if (bits[i] == 256)
				{
					builder.Append("(Bits)").Append(bits[i]);
				}
				else
				{
					builder.Append("new Bits(").Append(bits[i]).Append(")");
				}

				builder.Append(",");
			}

			builder.AppendLine().Append("};");
			Console.WriteLine(builder.ToString());
		}

		private static readonly ValueTuple<char, char>[] HexTuplesForParsing =
		{
			('0', '0'), ('1', '1'), ('2', '2'), ('3', '3'), ('4', '4'), ('5', '5'), ('6', '6'), ('7', '7'), ('8', '8'), ('9', '9'),
			('A', 'a'), ('B', 'b'), ('C', 'c'), ('D', 'd'), ('E', 'e'), ('F', 'f')
		};

		private static readonly ValueTuple<char, char>[] HexTuples =
		{
			('0', '0'), ('0', '1'), ('0', '2'), ('0', '3'), ('0', '4'), ('0', '5'), ('0', '6'), ('0', '7'), ('0', '8'), ('0', '9'),
			('0', 'a'), ('0', 'b'), ('0', 'c'), ('0', 'd'), ('0', 'e'), ('0', 'f'), ('1', '0'), ('1', '1'), ('1', '2'), ('1', '3'),
			('1', '4'), ('1', '5'), ('1', '6'), ('1', '7'), ('1', '8'), ('1', '9'), ('1', 'a'), ('1', 'b'), ('1', 'c'), ('1', 'd'),
			('1', 'e'), ('1', 'f'), ('2', '0'), ('2', '1'), ('2', '2'), ('2', '3'), ('2', '4'), ('2', '5'), ('2', '6'), ('2', '7'),
			('2', '8'), ('2', '9'), ('2', 'a'), ('2', 'b'), ('2', 'c'), ('2', 'd'), ('2', 'e'), ('2', 'f'), ('3', '0'), ('3', '1'),
			('3', '2'), ('3', '3'), ('3', '4'), ('3', '5'), ('3', '6'), ('3', '7'), ('3', '8'), ('3', '9'), ('3', 'a'), ('3', 'b'),
			('3', 'c'), ('3', 'd'), ('3', 'e'), ('3', 'f'), ('4', '0'), ('4', '1'), ('4', '2'), ('4', '3'), ('4', '4'), ('4', '5'),
			('4', '6'), ('4', '7'), ('4', '8'), ('4', '9'), ('4', 'a'), ('4', 'b'), ('4', 'c'), ('4', 'd'), ('4', 'e'), ('4', 'f'),
			('5', '0'), ('5', '1'), ('5', '2'), ('5', '3'), ('5', '4'), ('5', '5'), ('5', '6'), ('5', '7'), ('5', '8'), ('5', '9'),
			('5', 'a'), ('5', 'b'), ('5', 'c'), ('5', 'd'), ('5', 'e'), ('5', 'f'), ('6', '0'), ('6', '1'), ('6', '2'), ('6', '3'),
			('6', '4'), ('6', '5'), ('6', '6'), ('6', '7'), ('6', '8'), ('6', '9'), ('6', 'a'), ('6', 'b'), ('6', 'c'), ('6', 'd'),
			('6', 'e'), ('6', 'f'), ('7', '0'), ('7', '1'), ('7', '2'), ('7', '3'), ('7', '4'), ('7', '5'), ('7', '6'), ('7', '7'),
			('7', '8'), ('7', '9'), ('7', 'a'), ('7', 'b'), ('7', 'c'), ('7', 'd'), ('7', 'e'), ('7', 'f'), ('8', '0'), ('8', '1'),
			('8', '2'), ('8', '3'), ('8', '4'), ('8', '5'), ('8', '6'), ('8', '7'), ('8', '8'), ('8', '9'), ('8', 'a'), ('8', 'b'),
			('8', 'c'), ('8', 'd'), ('8', 'e'), ('8', 'f'), ('9', '0'), ('9', '1'), ('9', '2'), ('9', '3'), ('9', '4'), ('9', '5'),
			('9', '6'), ('9', '7'), ('9', '8'), ('9', '9'), ('9', 'a'), ('9', 'b'), ('9', 'c'), ('9', 'd'), ('9', 'e'), ('9', 'f'),
			('a', '0'), ('a', '1'), ('a', '2'), ('a', '3'), ('a', '4'), ('a', '5'), ('a', '6'), ('a', '7'), ('a', '8'), ('a', '9'),
			('a', 'a'), ('a', 'b'), ('a', 'c'), ('a', 'd'), ('a', 'e'), ('a', 'f'), ('b', '0'), ('b', '1'), ('b', '2'), ('b', '3'),
			('b', '4'), ('b', '5'), ('b', '6'), ('b', '7'), ('b', '8'), ('b', '9'), ('b', 'a'), ('b', 'b'), ('b', 'c'), ('b', 'd'),
			('b', 'e'), ('b', 'f'), ('c', '0'), ('c', '1'), ('c', '2'), ('c', '3'), ('c', '4'), ('c', '5'), ('c', '6'), ('c', '7'),
			('c', '8'), ('c', '9'), ('c', 'a'), ('c', 'b'), ('c', 'c'), ('c', 'd'), ('c', 'e'), ('c', 'f'), ('d', '0'), ('d', '1'),
			('d', '2'), ('d', '3'), ('d', '4'), ('d', '5'), ('d', '6'), ('d', '7'), ('d', '8'), ('d', '9'), ('d', 'a'), ('d', 'b'),
			('d', 'c'), ('d', 'd'), ('d', 'e'), ('d', 'f'), ('e', '0'), ('e', '1'), ('e', '2'), ('e', '3'), ('e', '4'), ('e', '5'),
			('e', '6'), ('e', '7'), ('e', '8'), ('e', '9'), ('e', 'a'), ('e', 'b'), ('e', 'c'), ('e', 'd'), ('e', 'e'), ('e', 'f'),
			('f', '0'), ('f', '1'), ('f', '2'), ('f', '3'), ('f', '4'), ('f', '5'), ('f', '6'), ('f', '7'), ('f', '8'), ('f', '9'),
			('f', 'a'), ('f', 'b'), ('f', 'c'), ('f', 'd'), ('f', 'e'), ('f', 'f')
		};
	}
}