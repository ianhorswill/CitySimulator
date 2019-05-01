using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Web;

public class NameManager : MonoBehaviour
{
    public string assetDir = "/Assets/PeopleAgents";
    private string path;
    private string[] surnames;
    private string[] miscellaneous_feminine_names;
    private string[] miscellaneous_masculine_names;
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
        miscellaneous_feminine_names = File.ReadAllLines(path + assetDir + "/names/feminine_names.txt");
        miscellaneous_masculine_names = File.ReadAllLines(path + assetDir + "/names/masculine_names.txt");
        names_by_decade = File.ReadAllText(path + assetDir + "/names/american_names_by_decade_with_fitted_probability_distributions.json");
    }
    
    /*
     Function: getName()
     Argument: enum NameManager.sex sex, int year, List<string> parent name
     Returns: name (in the format of "surname firstname")
     */
    public string getName(sex sex, int year, List<string> parent_name)
    {
        string firstname = getFirstname(sex, year);
        if (parent_name.Count == 0)
            return getSurname() + " " + firstname;
        else if (parent_name.Count == 1)
            return parent_name[0] + " " + firstname;
        else // return randomly from the parent name list
        {
            System.Random rand = new System.Random();
            return parent_name[rand.Next(parent_name.Count)] + " " + firstname;
        } 
    }

    public string getSurname()
    {
        System.Random rand = new System.Random();
        return surnames[rand.Next(surnames.Length)];
    }

    public string getFirstname(sex sex, int year)
    {
        string firstname;
        switch (sex)
        {
            case sex.male:
                firstname = getMasculineNameByDecades(year);
                break;
            case sex.female:
                firstname = getFemineNameByDecades(year);
                break;
            case sex.other:
                firstname = getFemineNameByDecades(year);
                break;
            default:
                firstname = getMasculineNameByDecades(year);
                break;
        }
        return firstname;
    }

    public string getMasculineNameByDecades(int year)
    {
        string name;
        if (year < 1880)
            year = 1880;
        int decade = (int)(Math.Floor(year / 10f) * 10f);
        System.Random rand = new System.Random();
        if (rand.Next(1) > 0.99f)
        {
            // choose masculine name from a rare name set 
            name = miscellaneous_masculine_names[rand.Next(miscellaneous_masculine_names.Length)];
        }
        else
        {
            name = getNameByDecade(decade, "M");
        }

        return name;
    }
    
    public string getFemineNameByDecades(int year)
    {
        string name;
        if (year < 1880)
            year = 1880;
        int decade = (int)(Math.Floor(year / 10f) * 10f);
        System.Random rand = new System.Random();
        if (rand.Next(1) > 0.99f)
        {
            // choose masculine name from a rare name set 
            name = miscellaneous_feminine_names[rand.Next(miscellaneous_feminine_names.Length)];
        }
        else
        {
            name = getNameByDecade(decade, "F");
        }
        return name;
    }

    public string getNameByDecade(int decade, string sex)
    {
        return "";
    }
}
