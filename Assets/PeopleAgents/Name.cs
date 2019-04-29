using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class NameManager : MonoBehaviour
{
    public string assetDir = "/Assets/PeopleAgents";
    private string path;
    private string[] surnames;
    private string[] feminine_names;
    private string[] masculine_names;
    private string names_by_decade;
    
    public enum sex
    {
        female, male, other
    };

    // Start is called before the first frame update
    void Start()
    {
        path = Directory.GetCurrentDirectory();
        surnames = File.ReadAllLines(path + assetDir + "/names/english_surnames.txt");
        feminine_names = File.ReadAllLines(path + assetDir + "/names/feminine_names.txt");
        masculine_names = File.ReadAllLines(path + assetDir + "/names/masculine_names.txt");
        names_by_decade = File.ReadAllText(path + assetDir + "/names/american_names_by_decade_with_fitted_probability_distributions");
    }

    public string getName(sex sex, int year)
    {
        return getSurname() + " " + getFirstname(sex, year);
    }

    public string getSurname()
    {
        System.Random rand = new System.Random();
        return surnames[rand.Next(surnames.Length)];
    }

    public string getFirstname(sex sex, int year)
    {
        string firstname;
        if (sex == NameManager.sex.female)
        {
            firstname = getMasculineNameByDecades(year);
        } else if (sex == sex.male)
        {
            firstname = getMasculineNameByDecades(year);
        }
        else
        {
            firstname = getMasculineNameByDecades(year);
        }
        return firstname;
    }

    public string getMasculineNameByDecades(int year)
    {
        if (year < 1880)
            year = 1880;
        int decade = (int)(Math.Floor(year / 10f) * 10f);
        System.Random rand = new System.Random();
        if (rand.Next(1) > 0.99f)
        {
            // choose masculine name from a rare name set 
            string name = masculine_names[rand.Next(masculine_names.Length)];
        }
        else
        {
            
        }

        return name;
    }

    public string getNameByDecade(int decade)
    {
        
    }
}
