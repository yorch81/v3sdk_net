using NUnit.Framework;
using System;
using Newtonsoft.Json.Linq;

namespace V3Sdk
{
	[TestFixture ()]
	public class V3SdkTest
	{
		private V3Sdk v3 = null;

		public V3SdkTest()
		{
			string url = "http://v3-yorch.rhcloud.com/";
			string key = "lYltuNtYYbYRFC7QWwHn9b5aH2UJMk1234567890";

			v3 = V3Sdk.GetInstance (url, key);
		}

		[Test]
		public void FindObjectTest()
		{
			string strJson = "{\"field\":\"value\"}";

			JObject json = JObject.Parse (strJson);

			JObject jsonNew = v3.NewObject ("demo", json);

			string id = V3Sdk.GetId (jsonNew); 

			json = v3.FindObject ("demo", id);

			string findid = V3Sdk.GetId (json); 

			Assert.AreEqual (id, findid);
		}

		[Test]
		public void QueryTest()
		{
			string strJson = "{\"field\":\"value\"}";

			JObject json = JObject.Parse (strJson);

			v3.NewObject ("demo", json);

			json = v3.Query ("demo", json);

			Assert.Greater (json.Count, 0);
		}

		[Test]
		public void NewObjectTest()
		{
			string expected = "expected";
			JObject json = new JObject ();
			json.Add ("field", expected);

			JObject jsonNew = v3.NewObject ("demo", json);

			string result = (string) jsonNew["field"];

			Assert.AreEqual (expected, result);
		}

		[Test]
		public void UpdateObjectTest()
		{
			string expected = "value2";
			JObject json = new JObject ();
			json.Add ("field", "value2");

			JObject jsonNew = v3.NewObject ("demo", json);
			string id = V3Sdk.GetId (jsonNew);

			json = new JObject ();
			json.Add ("field2", expected);

			v3.UpdateObject ("demo", id, json);

			json = v3.FindObject ("demo", id);

			string result = (string) json ["field2"];

			Assert.AreEqual (expected, result);
		}

		[Test]
		public void DeleteObjectTest()
		{
			JObject json = new JObject ();
			json.Add ("field", "value2");

			JObject jsonNew = v3.NewObject ("demo", json);
			string id = V3Sdk.GetId (jsonNew);

			bool result = v3.DeleteObject ("demo", id);

			Assert.AreEqual (true, result);
		}
	}
}

