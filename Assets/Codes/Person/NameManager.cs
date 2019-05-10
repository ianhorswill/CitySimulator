using System.Collections.Generic;
using System.IO;

public class NameManager
{
    private static readonly string assetDir = "/Assets/Codes/Person";
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
        if (parent_name == null || parent_name.Count == 0)
        {
            return Surnames.RandomElement();
        }
        else if (parent_name.Count == 1)
            return parent_name[0];
        else // return randomly from the parent name list
        {
            return parent_name.RandomElement();
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

        if (Random.Integer(100) > 98) {
            // choose masculine name from a rare name set 
            name = MiscellaneousMasculineName.RandomElement();
        }
        else
        {
            name = CommonMasculineName.RandomElement();
        }

        return name;
    }
    
    public static string getFemineName()
    {
        string name;
        if (Random.Integer(100) > 98)
        {
            // choose masculine name from a rare name set 
            name = MiscellaneousFeminineName.RandomElement();
        }
        else
        {
            name = CommonFeminineName.RandomElement();
        }
        return name;
    }
}
