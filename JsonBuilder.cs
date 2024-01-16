using System.Reflection;
using System.Text.Json.Nodes;

namespace Devoir2;

public static class JsonBuilder
{
    /// <summary>
    /// Treat a property to serialize it in a JsonObject
    /// </summary>
    /// <param name="property">The property to serialize</param>
    /// <param name="obj">The object being serialized</param>
    /// <returns></returns>
    public static JsonObject TreatProperty(PropertyInfo property, object? obj)
    {
        // Treat arrays (first because they're classes)
        if (property.PropertyType.IsArray)
        {
            return new JsonObject
            {
                { "type", property.PropertyType.FullName },
                { "value", TreatArray(property.GetValue(obj) as Array) ?? null }
            };
        }
        
        // Treat classes
        if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
        {
            return Serialization.Serialize(property.GetValue(obj));
        }
        
        // Treat simple types
        return new JsonObject
        {
            { "type", property.PropertyType.FullName },
            { "value", property.GetValue(obj)?.ToString() }
        };
    }

    /// <summary>
    /// Treat a field to serialize it in a JsonObject
    /// </summary>
    /// <param name="field">The field to serialize</param>
    /// <param name="obj">The object being serialized</param>
    /// <returns></returns>
    public static JsonObject TreatField(FieldInfo field, object? obj)
    {
        // Treat arrays (have to do first because arrays are classes)
        if (field.FieldType.IsArray)
        {
            return new JsonObject
            {
                { "type", field.FieldType.FullName },
                { "value", TreatArray(field.GetValue(obj) as Array) ?? null }
            };
        }

        // Treat classes
        if (field.FieldType.IsClass && field.FieldType != typeof(string))
        {
            return Serialization.Serialize(field.GetValue(obj));
        }

        // Treat simple types
        return new JsonObject
        {
            { "type", field.FieldType.FullName },
            { "value", field.GetValue(obj)?.ToString() }
        };
    }

    /// <summary>
    /// Build a JsonArray from an Array, serializing it if needed
    /// </summary>
    /// <param name="array">The array</param>
    /// <returns>The json array or null if the array is null</returns>
    private static JsonArray? TreatArray(Array? array)
    {
        // If it's null, return null
        if (array == null)
            return null;

        // Else, treat it
        var json = new JsonArray();
        foreach (var value in array)
        {
            // If it's an array:
            if (value.GetType().IsArray)
            {
                json.Add(TreatArray((Array)value));
                continue;
            }

            // If it's a class:
            if (value.GetType().IsClass && value.GetType() != typeof(string))
            {
                json.Add(Serialization.Serialize(value));
                continue;
            }

            // If it's a simple type:
            json.Add(value.ToString());
        }

        return json;
    }
}