using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Linq;

public class Institution
{
    public Person owner;

        public Plot location;
        public List<Person> employeeList;
        public string type;
        public const string SUB_SYSTEM = "Institution";
        public int visitCount = 0;
        public float CUT_JOB_THRESHOLD = 0.01f;
        public int security_level = 0; // 0-100
        public float ROB_THRESHOLD = 20;

        public Institution(Person owner, Plot location,string type, bool needBuild = true)
        {
            this.owner = owner;
            this.location = location;
            this.type = type;
            employeeList = new List<Person>();
        }
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
        person.workStatus.getNewJob(this, pay); //Tells the person that they are now employed with wage "pay".
        Logger.Log(SUB_SYSTEM, type, "hires", person.name);
    }

        public bool Fire(Person person)
        {
            if (employeeList.Contains(person))
            {
                return employeeList.Remove(person);
            }
            else
            {
                return false;
            }
        }

        public void Visit()
        {
            visitCount++;
        }

        public void IncreaseSecurity()
        {
            security_level++;
        }

        public void BeRobbed()
        {
            Fire(employeeList.RandomElement());
        }

        public string getType()
        {
            return type;
        }
        

        public override string ToString()
        {
            //return type +  " Owner: " + owner.name + "， Location: （" + location.x_pos +"," + location.y_pos+ ")";
            return type;
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

        public void Build(Institution institution)
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
        private List<Person> studentList;
        public int startMonth;
        public int startDay;
        public int endMonth;
        public int endDay;
        private int capacity;

        public School(Person owner, Plot location, string type) : base(owner, location, type)
        {
            studentList = new List<Person>();
            startMonth = 1;
            startDay = 2;
            endMonth = 12;
            endDay = 30;
            capacity = 1000;
        }

        public void EnrollStudent(Person student)
        {
            studentList.Add(student);
            student.workStatus.getNewJob(this, 0);
            student.personalEducation.is_student = true;
            Logger.Log(SUB_SYSTEM, type, "enroll student", student.name);
        }

        public void GraduateStudent(Person student)
        {
            studentList.Remove(student);
            student.workStatus.loseJob();
            student.personalEducation.is_student = false;
            student.personalEducation.is_high_school_graduate = true;
            Logger.Log(SUB_SYSTEM, type, "graduate student", student.name);
        }

        public void EnrollEnteringClass()
        {
            var under18NotInSchool = from child in PersonTown.Singleton.aliveResidents
                                     where (child != null && child.age <= 18 && child.workStatus.workplace == null)
                                     select child;
            
            if (under18NotInSchool != null)
            {
                foreach (Person pc in under18NotInSchool)
                {
                    if(studentList.Count >= capacity) {return;}
                    else{
                        EnrollStudent(pc);
                    }
                }
            }
        }

        public void Graduate18YearOlds()
        {
            var students18 = from student in studentList
                             where (student != null && student.age == 18)
                             select student;

            if(students18 != null)
            {
                foreach (Person pc in students18)
                {
                    GraduateStudent(pc);
                }
            }
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
    public LawFirm(Person owner, Plot location, string type) : base(owner, location, type)
    {
    }

    public bool FireDivorce(string spouse1, string spouse2)
    {
        // TODO: fire divorce for a couple
        return true;
    }
}