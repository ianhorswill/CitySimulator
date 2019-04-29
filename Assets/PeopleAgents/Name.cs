using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class NameManager : MonoBehaviour
{
    public string assetDir = "/Assets/PeopleAgents";
    private string path;
    
    public enum sex
    {
        female, male, other
    };

    // Start is called before the first frame update
    void Start()
    {
        path = Directory.GetCurrentDirectory();
    }

    public string getName(sex sex)
    {
        return getSurname() + " " + getFirstname(sex);
    }

    public string getSurname()
    {
        string[] surnames = File.ReadAllLines(path + assetDir + "/names/english_surnames.txt");
        System.Random rand = new System.Random();
        return surnames[rand.Next(surnames.Length)];
    }

    public string getFirstname(sex sex)
    {
        string[] firstnames;
        if (sex == NameManager.sex.female)
        {
            firstnames = File.ReadAllLines(path + assetDir + "/names/feminine_names.txt");
        } else if (sex == sex.male)
        {
            firstnames = File.ReadAllLines(path + assetDir + "/names/masculine_names.txt");
        }
        else
        {
            firstnames = File.ReadAllLines(path + assetDir + "/names/masculine_names.txt");
        }
        System.Random rand = new System.Random();
        return firstnames[rand.Next(firstnames.Length)];
    }
}
