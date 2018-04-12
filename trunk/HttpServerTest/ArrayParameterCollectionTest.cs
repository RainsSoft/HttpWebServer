using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpServer;
using Xunit;

namespace HttpServerTest
{
	public class ArrayParameterCollectionTest
	{

		[Fact]
		public void Test()
		{
			ParameterCollection temp = new ParameterCollection();
			temp.Add("schedule[day][1][open]", "08:00");
			temp.Add("schedule[day][1][close]", "16:00");
			temp.Add("schedule[day][2][open]", "07:00");
			temp.Add("schedule[day][2][close]", "16:00");

			ArrayParameterCollection col = new ArrayParameterCollection(temp);
			Assert.Equal("07:00", col["schedule"]["day"]["2"]["open"].Value);
		}

	}
}
