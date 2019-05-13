using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Codes.Institution
{
    public class Institution
    {
        private Person owner;

        public Plot location;
        private List<Person> employeeList;
        private string type;
        public const string SUB_SYSTEM = "Institution";

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
            Logger.Log(SUB_SYSTEM, ToString());
        }
        public Institution(Person owner, Plot location,string type)
        {
//            ConstructCompanySite(location);
            
            this.owner = owner;
            this.location = location;
            this.type = type;
            employeeList = new List<Person>();
            Logger.Log(SUB_SYSTEM, ToString());
        }

        public IEnumerable<WaitForSeconds> StartHiringProcess()
        {
            yield return new WaitForSeconds(5f);
            Hiring(new Person("Someone", new List<Person>()));
        }

        public void Hiring(Person person)
        {
            // TODO : hiring process
            employeeList.Add(person);
            Logger.Log(SUB_SYSTEM,  ToString(), "hires", person.ToString());
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

        public void Build(Institution institution, Plot loc)
        {
            // TODO: build institution on certain plot
            Logger.Log(SUB_SYSTEM,  ToString(), "build", institution.ToString(), loc.ToString());
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
            Logger.Log(SUB_SYSTEM, ToString(), "enroll student", student.ToString());
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
            Logger.Log(SUB_SYSTEM, ToString(), "give birth to", newBaby.ToString());
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
}