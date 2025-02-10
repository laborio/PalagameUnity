using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalagonDatabase : MonoBehaviour
{
    public static PalagonDatabase Instance;
    public List<Palagon> palagons = new List<Palagon>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        InitializePalagons();
    }

    void InitializePalagons()
    {
        palagons.Add(new Palagon("Palauno", 1, "Palawork", "Happy", 1, 2));
        palagons.Add(new Palagon("Paladoo", 1, "Palasleep", "Anxious", 1, 3));
        palagons.Add(new Palagon("Palami", 1, "Palamiam", "Sociable", 1, 4));
        palagons.Add(new Palagon("Palarun", 1, "Palasport", "Bored", 1, 2));
        palagons.Add(new Palagon("Palashine", 1, "Palaclean", "Happy", 1, 2));
        palagons.Add(new Palagon("Palapush", 2, "Palawork", "Angry", 3, 5));
        palagons.Add(new Palagon("Paladream", 2, "Palasleep", "Sociable", 2, 6));
        palagons.Add(new Palagon("Palamunch", 2, "Palamiam", "Happy", 2, 8));
        palagons.Add(new Palagon("Paladash", 2, "Palasport", "Bored", 3, 4));
        palagons.Add(new Palagon("Palanice", 2, "Palaclean", "Anxious", 3, 4));
        palagons.Add(new Palagon("Palaboss", 3, "Palawork", "Sociable", 6, 10));
        palagons.Add(new Palagon("Palanap", 3, "Palasleep", "Bored", 4, 12));
        palagons.Add(new Palagon("Palabite", 3, "Palamiam", "Happy", 5, 15));
        palagons.Add(new Palagon("Palastride", 3, "Palasport", "Angry", 6, 8));
        palagons.Add(new Palagon("Palafresh", 3, "Palaclean", "Sociable", 6, 8));
        palagons.Add(new Palagon("Palamaster", 4, "Palawork", "Bored", 10, 20));
        palagons.Add(new Palagon("Palasnore", 4, "Palasleep", "Happy", 8, 24));
        palagons.Add(new Palagon("Palaflex", 4, "Palasport", "Sociable", 10, 10));
        palagons.Add(new Palagon("Palaking", 5, "Palawork", "Angry", 15, 30));
        palagons.Add(new Palagon("Palachampion", 5, "Palasport", "Bored", 15, 20));
    }

    public Palagon GetPalagon(string name)
    {
        return palagons.Find(p => p.palagonName == name);
    }
}