using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Codes.Institution
{
    // the InstitutionGenerator includes methods needed to generate an Institution

    public class Institution
    {
        private Person owner;
        private Plot location;
        private List<Person> employeeList;
        private string type;

        public Institution(Person owner, Plot location,string type, bool needBuild = true)
        {
//            if (needBuild)
//            {
//                ConstructCompanySite(location);
//            }
            
            this.owner = owner;
            this.location = location;
            this.type = type;
            employeeList = new List<Person>();

            Debug.Log("------------------------\n" + this.ToString());
//            Debug.Log("["+type+"] start hiring process...");
//            StartHiringProcess();
        }
        public Institution(Person owner, Plot location,string type)
        {
//            ConstructCompanySite(location);
            
            this.owner = owner;
            this.location = location;
            this.type = type;
            employeeList = new List<Person>();
            Debug.Log("------------------------\n" + this.ToString());
//            Debug.Log("["+type+"] start hiring process...");
//            StartHiringProcess();
        }

        public void StartHiringProcess()
        {
            Timer t = new Timer(Hiring,null,5000,10000);
        }

        private void Hiring(object state)
        {
            Hiring("someone");
        }

        public void Hiring(Person person)
        {
            // TODO : hiring process
            employeeList.Add(person);
            Debug.Log("\n----------HIRE----------\n["+type+ "] hires new employee ["+person+"]\n------------------------\n");
        }

        public void ConstructCompanySite(Plot location)
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
            return "Institution: " + type + "\nOwner: " + owner.name + "\nLocation: (" + location.x_pos +"," + location.y_pos+ ")\n";
        }
    }

    public class ConstructionCompany : Institution
    {
        public ConstructionCompany(Person owner, Plot location, string type, bool needBuild = true) : base(owner, location, type, needBuild)
        {
        }

        public ConstructionCompany(Person owner, Plot location, string type) : base(owner, location, type)
        {
        }

        public void Build(Plot loc)
        {
            Debug.Log("Building institution site...");
            Thread.Sleep(3000);
            Debug.Log("Construct building at location["+loc.x_pos +"," + loc.y_pos+"]");
        }
    }

    public class School : Institution
    {
        private List<Person> studentList;

        public School(Person owner, Plot location, string type) : base(owner, location, type)
        {
            studentList = new List<Person>();
        }

        public void EnrollStudent(Person student)
        {
            studentList.Add(student);
        }
    }

    public class Hospital : Institution
    {
        public Hospital(Person owner, Plot location, string type) : base(owner, location, type)
        {
        }

        public void TakePatient(Person patient)
        {
            
        }

        public Person BabyDelivery()
        {
            // TODO: baby delivery
            Person newBaby = new Person("baby", new List<Person>());
            return newBaby;
        }
    }

    public class ApartmentComplex : Institution
    {
        private List<Person> residents;

        public ApartmentComplex(Person owner, Plot location, string type) : base(owner, location, type)
        {
            residents = new List<Person>();
        }

        public void AddResidents(Person resident)
        {
            residents.Add(resident);
        }

        public void AddResidents(List<Person> rList)
        {
            foreach (var r in rList)
            {
                residents.Add(r);
            }
        }
    }

    public class LawFirm : Institution
    {
        public LawFirm(Person owner, Plot location, string type) : base(owner, location, type)
        {
        }

        public bool FireDivorce(string spouse1, string spouse2)
        {
            // TODO: fire divorce for a couple
            return true;
        }
    }
    


//    public class MainClass
//    {
//        public static void Main(string[] args)
//        {
//            Institution ins = InstitutionManager.GeneratorInstitution(new Person("brenda", new List<Person>()),
//                new Plot(2, 3));
////            string[] names = {"John", "Alice", "Nick", "Mike", "Sam"};
////            string[] locs = {"1, 2", "2, 8", "4, 6", "5, 9", "9, 5"};
////
////            for (int i = 0; i < 5; i++)
////            {
////                Institution ins = InstitutionManager.GeneratorInstitution(names[i], locs[i]);
////            }
//
//            Thread.Sleep(1000000);
//        }
//    }
    
}