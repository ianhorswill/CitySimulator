using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

    // the InstitutionGenerator includes methods needed to generate an Institution
public class InstitutionManager : SimulatorComponent
{
    // store all the institutions been constructed
    public static List<Institution> institutionList;

    // store the hardcode institution types
    private static string[] institutionTypeList;

    // the color map for each institution type
    public static Dictionary<String, Color> colorMap;

    // store the construction companies
    public static List<ConstructionCompany> constructionCompanyList;

    public static Dictionary<String, List<Institution>> InstitutionDictionary;

    private Space Space;

    public static InstitutionManager Singleton;

    static InstitutionManager()
    {
        institutionList = new List<Institution>();
        constructionCompanyList = new List<ConstructionCompany>();
        institutionTypeList = File.ReadAllLines(Directory.GetCurrentDirectory() +
                                                "/Assets/Codes/Institution/InstitutionData/institutionTypes.txt");

        string[] colorStr =
            File.ReadAllLines(Directory.GetCurrentDirectory() +
                              "/Assets/Codes/Institution/InstitutionData/institutionColorMap.txt");

        colorMap = new Dictionary<string, Color>();
        foreach (var line in colorStr)
        {
            String[] str = line.Split('|');
            Color color = new Color32(Convert.ToByte(str[1]), Convert.ToByte(str[2]), Convert.ToByte(str[3]), 255);
            colorMap[str[0]] = color;
        }

        InstitutionDictionary = new Dictionary<string, List<Institution>>();
        // hard-codly assign one initial construction company
        ConstructionCompany cons = new ConstructionCompany(Person.generateRandomPerson(), new Plot(0, 0),
            "ConstructionCompany", false);
        constructionCompanyList.Add(cons);
        institutionList.Add(cons);

        // add one school initially
        ////School school = new School(Person.generateRandomPerson(), new Plot(0, 1), "School");
        ////institutionList.Add(school);
        GeneratorInstitution(Person.generateRandomPerson(), "School", new Plot(0, 1));
        //initial apt complex
        GeneratorInstitution(Person.generateRandomPerson(), "ApartmentComplex", new Plot (0, 2));
    }

    public InstitutionManager(Space space)
    {
        Space = space;
        Singleton = this;
    }

    // get a random type
    // TODO: based on current institution numbers and types and city scale
    public static string GetRandomType()
    {
        return institutionTypeList.RandomElement();
    }


    // generate a new institution
    public static Institution GeneratorInstitution(Person owner, String type, Plot location)
    {
//            string type = GetRandomType();
        Institution newInstitution;
        switch (type)
        {
            case "ConstructionCompany":
                newInstitution = new ConstructionCompany(owner, location, type);
                break;
            case "School":
                newInstitution = new School(owner, location, type);
                break;
            case "Hospital":
                newInstitution = new Hospital(owner, location, type);
                break;
            case "LawFirm":
                newInstitution = new LawFirm(owner, location, type);
                break;
            case "ApartmentComplex":
                newInstitution = new ApartmentComplex(owner, location, type);
                break;
            default:
                newInstitution = new Institution(owner, location, type);
                break;
        }

        if (newInstitution.getType().Equals("ConstructionCompany"))
        {
            constructionCompanyList.Add(newInstitution as ConstructionCompany);
        }

        institutionList.Add(newInstitution);

        if (InstitutionDictionary.ContainsKey(type))
        {
            List<Institution> tempList = InstitutionDictionary[type];
            tempList.Add(newInstitution);
        }
        else
        {
            InstitutionDictionary[type] = new List<Institution> {newInstitution};
        }

        Logger.Log("Institution", type, owner.name, "(" + location.x_pos + "," + location.y_pos + ")");

        return newInstitution;
    }

    public static ConstructionCompany GetRandomConstructionCompany()
    {
        return constructionCompanyList.RandomElement();
    }

    /// <summary>
    /// Pick a random institution, or return null if there are currently no institutions.
    /// </summary>
    public static Institution RandomInstitutionIfAny()
    {
        if (institutionList.Count == 0)
            return null;
        return RandomInstitution();
    }

    /// <summary>
    /// Randomly chooses a currently existing institution
    /// </summary>
    public static Institution RandomInstitution()
    {
        return institutionList.RandomElement();
    }

    public static List<Institution> GetInstitutionOfType(string type)
    {
        if (InstitutionDictionary.ContainsKey(type) == false)
        {
            return new List<Institution>();
        }
        return InstitutionDictionary[type];
    }

    public Institution GetRandomInstitutionOfType(string type){
        if(InstitutionDictionary[type].Count > 0){
            return InstitutionDictionary[type].RandomElement();
        }
        return null;
    }

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Step()
    {
        //Institution institution =
        //    GeneratorInstitution(Person.generateRandomPerson(), GetRandomType(), Space.get_random_plot());
        //GetRandomConstructionCompany().Build(institution);
        //institution.Hiring(Person.generateRandomPerson());

        //Code for School enrollment
        foreach(School s in GetInstitutionOfType("School")){
            if (s.startDay == Simulator.CurrentTime.Day && s.startMonth == Simulator.CurrentTime.Month){
                s.EnrollEnteringClass();
            }
            if (s.endDay == Simulator.CurrentTime.Day && s.endMonth == Simulator.CurrentTime.Month){
                s.Graduate18YearOlds();
            }
        }        
    }

    public override void Visualize()
    {
        base.Visualize();
    }
}