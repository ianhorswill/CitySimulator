using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Institution
{
    // the InstitutionGenerator includes methods needed to generate an Institution
    public static class InstitutionGenerator
    {
        // store all the institutions been constructed
        private static List<Institution> institutionList;
        // store the hardcode institution types
        private static string[] institutionTypeList;
        
        static InstitutionGenerator()
        {
            institutionList = new List<Institution>();
            string path = Directory.GetCurrentDirectory();
            institutionTypeList = File.ReadAllLines(path +"/institutionTypes.txt");
        }
        
        // get a random type
        // TODO: based on current institution numbers and types and city scale
        private static string getRandomType()
        {
            Random rnd = new Random();
            int index = rnd.Next(0, institutionTypeList.Length);
            return institutionTypeList[index];
        }
        
        
        // generate a new institution
        public static Institution generatorInstitution(string owner, string location)
        {
            string type = getRandomType();
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
            
            institutionList.Add(newInstitution);
            
            return newInstitution;
        }
    }

    public class Institution
    {
        private string owner;
        private string location;
        private List<String> employeeList;
        private string type;

        public Institution(string owner, string location,string type)
        {
            this.owner = owner;
            this.location = location;
            this.type = type;
            employeeList = new List<string>();
        }

        public void Hiring(string person)
        {
            // TODO : hiring process
            employeeList.Add(person);
        }

        public void ConstructCompanySite(string location)
        {
            
        }

        public string getType()
        {
            return type;
        }
    }

    public class ConstructionCompany : Institution
    {
        public ConstructionCompany(string owner, string location, string type) : base(owner, location, type)
        {
        }

        public void Build(string loc)
        {
            
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

//    public class MainClass
//    {
//        public static void Main(string[] args)
//        {
//            Institution ins = InstitutionGenerator.generatorInstitution("", "");
//            Console.WriteLine(ins.getType());
//        }
//    }
    
}