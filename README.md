# V3ctor .NET Sdk #

## Description ##
V3ctor WareHouse .NET Sdk.

## Requirements ##
* [.NET Framework](http://www.microsoft.com/es-mx/download/details.aspx?id=30653)
* [Mono](http://www.mono-project.com/)
* [V3ctor WareHouse](https://github.com/yorch81/v3ctorwh)

## Developer Documentation ##
In the Code.

## Installation ##
Add nuget reference:
	PM> Install-Package V3Sdk.dll

## Examples ##
~~~

string url = "http://v3-yorch.rhcloud.com/";
string key = "lYltuNtYYbYRFC7QWwHn9b5aH2UJMk1234567890";

V3Sdk.V3Sdk v3ctor = V3Sdk.V3Sdk.GetInstance (url, key);

JObject json = new JObject ();
json.Add ("field", 1);
json.Add ("field2", "my value");

// Insert and get Objectc
JObject jsonResult = v3ctor.NewObject ("v3", json);

// Get string id
string id = V3Sdk.V3Sdk.GetId (jsonResult);

Console.WriteLine ("New ID: " + id);
Console.WriteLine ("field2: " + jsonResult["field2"]);

// Add field3
json.Add ("field3", "my value 3");

// Update Object
v3ctor.UpdateObject("v3", id, json);

// Find Object
jsonResult = v3ctor.FindObject ("v3", id);

Console.WriteLine ("ID: " + id);
Console.WriteLine ("field3: " + jsonResult["field3"]);

// Query 
jsonResult = v3ctor.Query("v3", json);

Console.WriteLine ("Query Result ...");
foreach (JToken i in jsonResult.Children()) {
	foreach (JObject objChild in i)
	{
		string myid = V3Sdk.V3Sdk.GetId(objChild);

		Console.WriteLine (myid);
		Console.WriteLine ("field3: " + objChild["field3"]);
	}
}

// Delete Object
v3ctor.DeleteObject("v3", id);
			
~~~

## Notes ##
For install dependencies, please run: Update-package -reinstall

## References ##
http://es.wikipedia.org/wiki/Singleton

P.D. Let's go play !!!




