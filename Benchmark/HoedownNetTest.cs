using System;
using Hoedown;

namespace Benchmark
{
	public class HoedownNetTest : Test
	{
		Markdown md;
		public HoedownNetTest()
		{
			md = new Markdown(new HtmlRenderer());
			Name = "SundownNet";
		}

		public override string Transform(string str)
		{
			return md.Transform(str);
		}

		public override byte[] Transform(byte[] arr)
		{
			return md.Transform(arr);
		}
	}
}

