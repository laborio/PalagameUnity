[System.Serializable]
public class Palagon
{
    public string palagonName;
    public int cost;
    public string primaryTrait;
    public string secondaryTrait;
    public int goldGeneration;
    public int xpGeneration;

    public Palagon(string name, int cost, string primary, string secondary, int goldGen, int xpGen)
    {
        palagonName = name;
        this.cost = cost;
        primaryTrait = primary;
        secondaryTrait = secondary;
        goldGeneration = goldGen;
        xpGeneration = xpGen;
    }
}