using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Devoir2.Attribute;

namespace Devoir2;

public static class Serialization
{
    /// <summary>
    /// Print the stats of an object
    /// </summary>
    /// <param name="obj">A serialisable object</param>
    public static void PrintStats(object obj)
    {
        Console.WriteLine($"Stats of {obj.GetType().Name}");
        Console.WriteLine($"Number of properties: {obj.GetType().GetProperties().Length}");
        Console.WriteLine($"Number of fields: {obj.GetType().GetFields().Length}");
        Console.WriteLine($"Number of methods: {obj.GetType().GetMethods().Length}");
        Console.WriteLine($"Number of events: {obj.GetType().GetEvents().Length}");
        Console.WriteLine($"Number of interfaces: {obj.GetType().GetInterfaces().Length}");
        Console.WriteLine($"Number of constructors: {obj.GetType().GetConstructors().Length}");
        Console.WriteLine($"Number of nested types: {obj.GetType().GetNestedTypes().Length}");
    }

    /// <summary>
    /// Serialize an object into a JsonObject
    /// </summary>
    /// <param name="obj">The object to serialize</param>
    /// <returns>The serialized object</returns>
    public static JsonObject Serialize(object? obj)
    {
        // If it's null, a null json is returned
        if (obj == null)
            return new JsonObject { { "type", "null" } };

        var json = new JsonObject { { "type", obj.GetType().FullName } };

        // Treat the properties
        foreach (var property in obj.GetType().GetProperties())
        {
            // Skip if it's an excluded property or if it's a calculated property
            if (property.GetCustomAttribute<Exclude>() != null || property.GetMethod == null)
                continue;

            json.Add(property.Name, JsonBuilder.TreatProperty(property, obj));
        }

        // Treat the fields
        foreach (var field in obj.GetType().GetFields())
        {
            if (field.GetCustomAttribute<Exclude>() != null)
                continue;

            json.Add(field.Name, JsonBuilder.TreatField(field, obj));
        }

        return json;
    }
}