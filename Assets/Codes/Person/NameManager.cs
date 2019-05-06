using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class NameManager
{
    private static string assetDir = "/Assets/Codes/Person";
    private static readonly string path = Directory.GetCurrentDirectory();
    private static readonly string[] Surnames = File.ReadAllLines(path + assetDir + "/names/english_surnames.txt");
    private static readonly string[] MiscellaneousFeminineName = File.ReadAllLines(path + assetDir + "/names/feminine_names.txt");
    private static readonly string[] MiscellaneousMasculineName = File.ReadAllLines(path + assetDir + "/names/masculine_names.txt");
    private static readonly string[] CommonFeminineName = File.ReadAllLines(path + assetDir + "/names/female-names.txt");
    private static readonly string[] CommonMasculineName = File.ReadAllLines(path + assetDir + "/names/male-names.txt");
    
    public enum sex
    {
        female, male, other
    };

    /*
     Function: getName()
     Argument: enum NameManager.sex sex, int year, List<string> parent name
     Returns: name (in the format of "surname firstname")
     */
    public static string getSurname(List<string> parent_name)
    {
        System.Random rand = new System.Random();
        if (parent_name.Count == 0)
        {
            return Surnames[rand.Next(Surnames.Length)];
        }
        else if (parent_name.Count == 1)
            return parent_name[0];
        else // return randomly from the parent name list
        {
            return parent_name[rand.Next(parent_name.Count)];
        } 
    }

    public static string getFirstname(sex sex)
    {
        string firstname;
        switch (sex)
        {
            case sex.male:
                firstname = getMasculineName();
                break;
            case sex.female:
                firstname = getFemineName();
                break;
            default:
                firstname = getMasculineName();
                break;
        }
        return firstname;
    }

    public static string getMasculineName()
    {
        string name;
        System.Random rand = new System.Random();
        if (rand.Next(100) > 98) {
            // choose masculine name from a rare name set 
            name = MiscellaneousMasculineName[rand.Next(MiscellaneousMasculineName.Length)];
        }
        else
        {
            name = CommonMasculineName[rand.Next(CommonMasculineName.Length)];
        }

        return name;
    }
    
    public static string getFemineName()
    {
        string name;
        System.Random rand = new System.Random();
        if (rand.Next(100) > 98)
        {
            // choose masculine name from a rare name set 
            name = MiscellaneousFeminineName[rand.Next(MiscellaneousFeminineName.Length)];
        }
        else
        {
            name = CommonFeminineName[rand.Next(CommonFeminineName.Length)];
        }
        return name;
    }
}
