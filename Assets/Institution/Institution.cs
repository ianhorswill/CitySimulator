using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Institution
{
    // the InstitutionGenerator includes methods needed to generate an Institution
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
            institutionTypeList = File.ReadAllLines(Directory.GetCurrentDirectory() +"/institutionTypes.txt");
            
            // hard-codly assign one initial construction company
            ConstructionCompany cons = new ConstructionCompany("Government", "0, 0", "ConstructionCompany", false);
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
        public static Institution GeneratorInstitution(string owner, string location)
        {
            // the first institution have to be the ConstructionCompany
//            if (institutionList.Count == 0)
//            {
//                ConstructionCompany cons = new ConstructionCompany(owner, location, "ConstructionCompany");
//                constructionCompanyList.Add(cons);
//                institutionList.Add(cons);
//            }
            
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

    public class Institution
    {
        private string owner;
        private string location;
        private List<String> employeeList;
        private string type;

        public Institution(string owner, string location,string type, bool needBuild = true)
        {
            if (needBuild)
            {
                ConstructCompanySite(location);
            }
            
            this.owner = owner;
            this.location = location;
            this.type = type;
            employeeList = new List<string>();
            
            Console.WriteLine("------------------\n"+ this.ToString());
        }
        public Institution(string owner, string location,string type)
        {
            ConstructCompanySite(location);
            
            this.owner = owner;
            this.location = location;
            this.type = type;
            employeeList = new List<string>();
            Console.WriteLine("------------------\n"+ this.ToString());
        }

        public void Hiring(string person)
        {
            // TODO : hiring process
            employeeList.Add(person);
        }

        public void ConstructCompanySite(string location)
        {
            // randomly choose a construct company to construct a company site
            ConstructionCompany cons = InstitutionManager.GetRandomConstructionCompany();
            cons.Build(location);
        }

        public string getType()
        {
            return type;
        }

        public override string ToString()
        {
            return "Institution: " + type + "\nOwner: " + owner + "\nLocation: (" + location + ")\n";
        }
    }

    public class ConstructionCompany : Institution
    {
        public ConstructionCompany(string owner, string location, string type, bool needBuild = true) : base(owner, location, type, needBuild)
        {
        }
        
        public ConstructionCompany(string owner, string location, string type) : base(owner, location, type)
        {
        }

        public void Build(string loc)
        {
            Console.WriteLine("Construct building at "+loc);
        }
    }

    public class School : Institution
    {
        private List<string> studentList;
        public School(string owner, string location, string type) : base(owner, location, type)
        {
            studentList = new List<string>();
        }

        public void EnrollStudent(string student)
        {
            studentList.Add(student);
        }
    }

    public class Hospital : Institution
    {
        public Hospital(string owner, string location, string type) : base(owner, location, type)
        {
        }

        public void TakePatient(string patient)
        {
            
        }

        public string BabyDelivery()
        {
            string newBaby = "";
            return newBaby;
        }
    }

    public class ApartmentComplex : Institution
    {
        private List<string> residents;
        public ApartmentComplex(string owner, string location, string type) : base(owner, location, type)
        {
            residents = new List<string>();
        }

        public void AddResidents(string r)
        {
            residents.Add(r);
        }

        public void AddResidents(List<string> rList)
        {
            foreach (var r in rList)
            {
                residents.Add(r);
            }
        }
    }

    public class LawFirm : Institution
    {
        public LawFirm(string owner, string location, string type) : base(owner, location, type)
        {
        }

        public bool FireDivorce(string spouse1, string spouse2)
        {
            return true;
        }
    }
    
    public static class Helper {
        
    }

    public class MainClass
    {
        public static void Main(string[] args)
        {
            var name1 = "John";
            var location1 = "12, 7";

            var name2 = "Alice";
            var location2 = "3, 3";
            
            Institution ins = InstitutionManager.GeneratorInstitution(name1, location1);
            Institution ins2 = InstitutionManager.GeneratorInstitution(name2, location2);
        }
    }
    
}