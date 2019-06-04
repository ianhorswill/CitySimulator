using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Codes.Institution
{
    public class Institution
    {
        public Person owner;

        public Plot location;
        public List<Person> employeeList;
        public string type;
        public const string SUB_SYSTEM = "Institution";

        public Institution(Person owner, Plot location,string type)
        {
            this.owner = owner;
            this.location = location;
            this.type = type;
            employeeList = new List<Person>();
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
            float pay = 100; //Need to alter this based on the job / wage / position.  100 right now is arbitary.
            person.workStatus.getNewJob(this, pay);  //Tells the person that they are now employed with wage "pay".
            Logger.Log(SUB_SYSTEM,  type, "hires", person.name);
        }

        public string getType()
        {
            return type;
        }

        public override string ToString()
        {
            return type +  " Owner: " + owner.name + "， Location: （" + location.x_pos +"," + location.y_pos+ ")";
        }
    }

    public class ConstructionCompany : Institution
    {
        public ConstructionCompany(Person owner, Plot location, string type) : base(owner, location, type) { }

        public void Construct(Institution institution)
        {
            // TODO: build institution on certain plot
            Color color = InstitutionManager.colorMap[institution.type];
            institution.location.add_institution(institution);
            institution.location.set_color(color);
            Logger.Log(SUB_SYSTEM,  type, "build", institution.owner +":"+ institution.type, "("+location.x_pos+","+location.y_pos+")");
        }
    }

    public class School : Institution
    {
        private List<Person> students;

        public School(Person owner, Plot location, string type) : base(owner, location, type)
        {
            students = new List<Person>();
        }

        public void EnrollStudent(Person student)
        {
            students.Add(student);
            Logger.Log(SUB_SYSTEM, type, "enroll student", student.name);
        }
    }

    public class Hospital : Institution
    {
        private List<Person> patients;

        public Hospital(Person owner, Plot location, string type) : base(owner, location, type)
        {
            patients = new List<Person>();
        }

        public void TakePatient(Person patient)
        {
            patients.Add(patient);
        }

        // TODO: baby delivery - actions
        public Person BabyDelivery()
        {
            Person newBaby = new Person("baby", new List<Person>());
            Logger.Log(SUB_SYSTEM, type, "give birth to", newBaby.name);
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
        public LawFirm(Person owner, Plot location, string type) : base(owner, location, type) { }

        public bool FileDivorce(string spouse1, string spouse2)
        {
            // TODO: file divorce for a couple
            return true;
        }
    }
}