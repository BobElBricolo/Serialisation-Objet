using Devoir2.Attribute;

namespace Devoir2;

public class Test
{
    public int nombre;
    public string nom;
    public TestRecursif testR;
    public int Chiffre { get; set; }

    [Exclude]
    public int excluded = 100;

    public Test()
    {
        nombre = 0;
        nom = "Bob";
        testR = new TestRecursif(1, "1");
    }
    
    
    public Test(int chiffre, int nombre, string nom, TestRecursif testR = null)
    {
        this.nombre = nombre;
        this.nom = nom;
        Chiffre = chiffre;
        this.testR = testR;
    }
   
    
    
    public string ToString()
    {
        return "Test: \n" + "Nombre: " + nombre + "   -   Nom: " + nom + "   -   Chiffre: " + Chiffre + 
               "\n" + testR?.ToString();
    }
}