using System;
using System.Collections.Generic;
using System.IO;

namespace Codes.Institution
{
    public static class InstitutionManager
    {
        // store all the institutions been constructed
        private static List<Institution> institutionList;
        // store the hardcode institution types
        private static string[] institutionTypeList;
        // store the construction companies
        private static List<ConstructionCompany> constructionCompanyList;
        
        static InstitutionManager()
        {
            institutionList = new List<Institution>();
            constructionCompanyList =  new List<ConstructionCompany>();
            institutionTypeList = File.ReadAllLines(Directory.GetCurrentDirectory() +"/Assets/Codes/Institution/institutionTypes.txt");
            
            // hard-codly assign one initial construction company
            ConstructionCompany cons = new ConstructionCompany(new Person("government", new List<Person>()),
                new Plot(0, 0), "ConstructionCompany", false);
            constructionCompanyList.Add(cons);
            institutionList.Add(cons);
        }
        
        // get a random type
        // TODO: based on current institution numbers and types and city scale
        public static string GetRandomType()
        {
            Random rnd = new Random();
            int index = rnd.Next(0, institutionTypeList.Length);
            return institutionTypeList[index];
        }
        
        
        // generate a new institution
        public static Institution GeneratorInstitution(Person owner, Plot location)
        {
            string type = GetRandomType();
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
            
            return newInstitution;
        }

        public static ConstructionCompany GetRandomConstructionCompany()
        {
            Random rnd = new Random();
            int index = rnd.Next(0, constructionCompanyList.Count);
            return constructionCompanyList[index];
        }
    }
}