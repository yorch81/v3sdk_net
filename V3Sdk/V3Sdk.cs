using System;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

// V3Sdk  
//
// V3Sdk V3ctor WareHouse .NET SDK
//
// Copyright 2016 Jorge Alberto Ponce Turrubiates
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace V3Sdk
{
	/// <summary>
	/// V3ctor WareHouse .NET SDK.
	/// </summary>
	public class V3Sdk
	{
		/// <summary>
		/// Singleton Instance.
		/// </summary>
		private static V3Sdk INSTANCE = null;

		/// <summary>
		/// The URL.
		/// </summary>
		private string V3Url = "";

		/// <summary>
		/// The Access Key.
		/// </summary>
		private string V3Key = "";

		/// <summary>
		/// Initializes a new instance of the <see cref="V3Sdk.V3Sdk"/> class.
		/// </summary>
		/// <param name="url">URL.</param>
		/// <param name="key">Key.</param>
		private V3Sdk (string url = "", string key = "")
		{
			V3Url = url;
			V3Key = key;
		}
			
		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		/// <param name="url">URL.</param>
		/// <param name="key">Key.</param>
		public static V3Sdk GetInstance (string url = "", string key = "")
		{
			if (INSTANCE == null)
			{
				INSTANCE = new V3Sdk(url, key);
			}

			return INSTANCE;
		}

		/// <summary>
		/// Gets the URL.
		/// </summary>
		/// <returns>The URL.</returns>
		public string GetUrl ()
		{
			return V3Url;
		}

		/// <summary>
		/// Gets the key.
		/// </summary>
		/// <returns>The key.</returns>
		public string GetKey ()
		{
			return V3Key;
		}
			
		/// <summary>
		/// Gets the identifier.
		/// </summary>
		/// <returns>The identifier.</returns>
		/// <param name="v3Object">V3 object.</param>
		public static string GetId (JObject v3Object)
		{
			string id = "";

			try
			{
				JObject tmpJson = (JObject) v3Object["_id"];
				id = (string) tmpJson["$id"];
			}
			catch (Exception e) 
			{
				id = "";
			}

			return id;
		}

		/// <summary>
		/// Finds an object by Id
		/// </summary>
		/// <returns>V3ctor WareHouse object.</returns>
		/// <param name="entity">Entity.</param>
		/// <param name="_id">Identifier.</param>
		public JObject FindObject (string entity, string _id)
		{
			JObject retValue = new JObject();
			string url = V3Url + entity + "/" + _id + "?auth=" + V3Key;

			WebRequest request;
			HttpWebResponse response = null;

			try
			{
				request = HttpWebRequest.Create(url);
				response = (HttpWebResponse)request.GetResponse();

				if (response.StatusCode == HttpStatusCode.OK) 
				{
					StreamReader reader = new StreamReader(response.GetResponseStream());

					string strResponse = reader.ReadToEnd();

					retValue = JObject.Parse (strResponse);
				}
			}
			catch (Exception e) 
			{
				retValue = new JObject ();
			}
			finally 
			{
				if (response != null)
					response.Close ();
			}

			return retValue;
		}
			
		/// <summary>
		/// Execute Query.
		/// </summary>
		/// <param name="entity">Entity.</param>
		/// <param name="JsonQuery">Json query.</param>
		public JObject Query (string entity, JObject JsonQuery)
		{
			JObject retValue = new JObject();
			string url = V3Url + "query/" + entity + "?auth=" + V3Key;

			WebRequest request;
			HttpWebResponse response = null;

			try
			{
				request = HttpWebRequest.Create(url);
				request.ContentType = "text/json";
				request.Method = "POST";

				using (var streamWriter = new StreamWriter(request.GetRequestStream()))
				{
					string json = JsonQuery.ToString();

					streamWriter.Write(json);
					streamWriter.Flush();
					streamWriter.Close();
				}

				response = (HttpWebResponse)request.GetResponse();

				if (response.StatusCode == HttpStatusCode.OK) 
				{
					StreamReader reader = new StreamReader(response.GetResponseStream());

					string strResponse = reader.ReadToEnd();

					retValue = JObject.Parse(strResponse);
				}
			}
			catch (Exception e) 
			{
				retValue = new JObject ();
			}
			finally 
			{
				if (response != null)
					response.Close ();
			}

			return retValue;
		}

		/// <summary>
		/// Creates New Json Object.
		/// </summary>
		/// <returns>V3ctor WareHouse New object..</returns>
		/// <param name="entity">Entity.</param>
		/// <param name="JsonObject">Json object.</param>
		public JObject NewObject (string entity, JObject JsonObject)
		{
			JObject retValue = new JObject();
			string url = V3Url + entity + "?auth=" + V3Key;

			WebRequest request;
			HttpWebResponse response = null;

			// Remove _id
			JsonObject.Remove ("_id");

			try
			{
				request = HttpWebRequest.Create(url);
				request.ContentType = "text/json";
				request.Method = "POST";

				using (var streamWriter = new StreamWriter(request.GetRequestStream()))
				{
					string json = JsonObject.ToString();

					streamWriter.Write(json);
					streamWriter.Flush();
					streamWriter.Close();
				}

				response = (HttpWebResponse)request.GetResponse();

				if (response.StatusCode == HttpStatusCode.OK) 
				{
					StreamReader reader = new StreamReader(response.GetResponseStream());

					string strResponse = reader.ReadToEnd();

					retValue = JObject.Parse (strResponse);
				}
			}
			catch (Exception e) 
			{
				retValue = new JObject ();
			}
			finally 
			{
				if (response != null)
					response.Close ();
			}

			return retValue;
		}
			
		/// <summary>
		/// Updates the object.
		/// </summary>
		/// <returns>true if success false error</returns>
		/// <param name="entity">Entity.</param>
		/// <param name="_id">Identifier.</param>
		/// <param name="JsonObject">Json object.</param>
		public bool UpdateObject (string entity, string _id, JObject JsonObject)
		{
			bool retValue = false;
			string url = V3Url + entity + "/" + _id+ "?auth=" + V3Key;
			string ok = "{\"msg\":\"OK\"}";

			WebRequest request;
			HttpWebResponse response = null;

			// Remove _id
			JsonObject.Remove ("_id");

			try
			{
				byte[] data = System.Text.Encoding.UTF8.GetBytes(JsonObject.ToString());

				request = HttpWebRequest.Create(url);
				request.Method = "PUT";
				request.ContentType = "application/x-www-form-urlencoded";
				request.ContentLength = data.Length;

				using (var streamWriter = new StreamWriter(request.GetRequestStream()))
				{
					string json = JsonObject.ToString();

					streamWriter.Write(json);
					streamWriter.Flush();
					streamWriter.Close();
				}

				response = (HttpWebResponse)request.GetResponse();

				if (response.StatusCode == HttpStatusCode.OK) 
				{
					StreamReader reader = new StreamReader(response.GetResponseStream());

					string strResponse = reader.ReadToEnd();

					if (strResponse.Equals(ok))
						retValue = true;
				}
			}
			catch (Exception e) 
			{
				retValue = false;
			}
			finally 
			{
				if (response != null)
					response.Close ();
			}

			return retValue;
		}

		/// <summary>
		/// Deletes the object.
		/// </summary>
		/// <returns>true if success false error</returns>
		/// <param name="entity">Entity.</param>
		/// <param name="_id">Identifier.</param>
		public bool DeleteObject (string entity, string _id)
		{
			bool retValue = false;
			string url = V3Url + entity + "/" + _id+ "?auth=" + V3Key;

			WebRequest request;
			HttpWebResponse response = null;
			string ok = "{\"msg\":\"OK\"}";

			try
			{
				request = HttpWebRequest.Create(url);
				request.Method = "DELETE";

				response = (HttpWebResponse)request.GetResponse();

				if (response.StatusCode == HttpStatusCode.OK) 
				{
					StreamReader reader = new StreamReader(response.GetResponseStream());

					string strResponse = reader.ReadToEnd();

					if (strResponse.Equals(ok))
						retValue = true;
				}
			}
			catch (Exception e) 
			{
				retValue = false;
			}
			finally 
			{
				if (response != null)
					response.Close ();
			}

			return retValue;
		}

		/// <summary>
		/// Creates the entity.
		/// </summary>
		/// <returns><c>true</c>, if entity was created, <c>false</c> otherwise.</returns>
		/// <param name="EntityName">Entity name.</param>
		/// <param name="jsonConfig">Json config.</param>
		public bool CreateEntity(string EntityName, JObject jsonConfig)
		{
			return false;
		}
	}
}

