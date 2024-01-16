using System.Text.Json;

namespace Devoir2;

public class Deserialization
{
    
    /// <summary>
    /// Load an object from a json file
    /// </summary>
    /// <param name="filePath">The path to the json file</param>
    /// <returns>void</returns>
    public static void LoadObject(string filePath = "Objet.json")
    {
        // If the file path is not fully qualified, and the file doesn't exist in the current directory, try to find it in the parent directory
        if (!Path.IsPathFullyQualified(filePath) &&
            !File.Exists(Path.Combine(Directory.GetCurrentDirectory(), filePath)))
            filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../", filePath);

        // If the file path is not fully qualified, and the file doesn't exist in the parent directory, try to find it in the current directory
        if (!Path.IsPathFullyQualified(filePath))
            filePath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

        if (!File.Exists(filePath)) // If the file doesn't exist, throw an exception
            throw new FileNotFoundException("Le fichier n'existe pas", filePath);

        string jsonString = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), filePath));

        JsonDocument jsonDocument = JsonDocument.Parse(jsonString); 
        JsonElement root = jsonDocument.RootElement;

        // Deserialize the object
        Deserialize(root);
    }

    /// <summary>
    /// Deserialize a json element into an object
    /// </summary>
    /// <param name="element">The json element to deserialize</param>
    /// <returns>The deserialized object</returns>
    private static object? Deserialize(JsonElement element)
    {
        dynamic obj;

        // If type is null, return null
        Type? classe = Type.GetType(element.GetProperty("type").GetString() ?? string.Empty);

        if (classe == null)
        {
            Console.WriteLine("La classe n'existe pas");
            return null;
        }

        // List of objects to pass to the constructor
        List<Object> objets = new List<Object>();

        foreach (var property in element.EnumerateObject().Skip(1))
        {
            // Get the type of the property 
            Type? typeValue = Type.GetType(property.Value.GetProperty("type").GetString() ?? string.Empty);

            if (typeValue == null)
            {
                Console.WriteLine("Le type n'existe pas");
                return null;
            }

            // Treat arrays
            if (typeValue.IsArray)
            {
                // Not demanded in the homework
                continue;
            }

            // Treat classes
            if (typeValue.IsClass && typeValue != typeof(string))
            {
                objets.Add(Deserialize(property.Value));
                continue;
            }


            // Treat simple types
            string value = property.Value.GetProperty("value").GetString() ?? string.Empty;

            // Convert the value to the type of the property
            Object objetAjouter = Convert.ChangeType(value, typeValue);
            objets.Add(objetAjouter);
        }

        // Try to create the object
        try
        {
            obj = Activator.CreateInstance(classe, objets.ToArray()) ?? throw new InvalidOperationException();
            Console.WriteLine(obj.ToString());
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("Erreur dans la création de l'objet");
            return null;
        }

        return obj;
    }
}