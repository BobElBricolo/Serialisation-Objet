namespace Devoir2;

public class TestRecursif
{
    public int nombreR;
    public string nomR;
    public int[][] tableau;
    
    public TestRecursif(int nombreR, string nomR)
    {
        this.nombreR = nombreR;
        this.nomR = nomR;
        tableau = new [] { new int[]{ 1, 2, 3 }, new int[] { 4, 5, 6 } };
    }
    public TestRecursif(int nombreR, string nomR, int[][] tableau)
    {
        this.nombreR = nombreR;
        this.nomR = nomR;
        this.tableau = tableau;
    }
    
    public string ToString()
    {
        return "TestRecursif: Nombre: " + nombreR + "   -   Nom: " + nomR + "   -   Tableau: " + tableau[1][1];
    }
}