using System;
using System.Collections.Generic;
using System.Linq;
using Codes.Institution;

public class Person
{
    public DateTime DateOfBirth;

    public int age
    {
        get
        {
            var today = Simulator.CurrentTime;
            var age = today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }
        set { }
    }

    /// <summary>
    /// The name of the person in the format of "firstname lastname"
    /// </summary>
    public string name
    {
        get { return firstName + " " + lastName; }
        set { }
    }
    
    public string firstName;
    public string GenerateRandomFirstName()
    {
        NameManager.sex sex = biologicalSex ? NameManager.sex.male : NameManager.sex.female;
        return NameManager.getFirstname(sex);
    }
    
    public string lastName;
    public string GenerateRandomLastName()
    {
        List<string> parentNames = new List<string>();
        if (parents != null)
        {
            for (int i = 0; i < parents.Length; i++)
            {
                if (parents[i] != null) parentNames.Add(parents[i].lastName);
            }
        }
        return NameManager.getSurname(parentNames);
    }

    public bool alive {get; set;}
    
    //Right now a person's location is inferred from the institution they are presently at.
    public Institution currentInstitution {get; set;}
    public Plot currentLocation{
        get { return currentInstitution.location; }  //Implies that Institution.location is public 
        set { }  // Right now this shouldn't ever be called because location is more strictly bound to institution
    }

    public Guid id {get; set;}

    // We will have things here eventually.
    // private var currentLocation;

    class Relationship
    {
        public int Charge;
        public int Spark;

        public Relationship(int c, int s)
        {
            Charge = c;
            Spark = s;
        }
    }
    private Dictionary<Person, Relationship> relationshipDict = new Dictionary<Person, Relationship>();
    public void updateRelationshipSpark(Person p, int amount)
    {
        if (!relationshipDict.ContainsKey(p)) 
            relationshipDict.Add(p,new Relationship(0,0));
        else
            relationshipDict[p].Spark += amount;
    }
    public void updateRelationshipCharge(Person p, int amount)
    {
        if (!relationshipDict.ContainsKey(p)) 
            relationshipDict.Add(p,new Relationship(0,0));
        else
            relationshipDict[p].Charge += amount;
    }

    public int getRelationshipSpark(Person p)
    {
        return relationshipDict[p].Spark;
    }
    public int getRelationshipCharge(Person p)
    {
        return relationshipDict[p].Charge;
    }
    
    public Person sigOther;
    public List<Person> siblings;
    public  List<Person> children;
    public Person[] parents;
    private static double conceptionRate = 1.0; // Should be lower, but setting higher for the sake of it.
    
    /// <summary>
    /// The gender of the person: if biologicalSex is true, the person is Male; if false, female.
    /// </summary>
    private bool biologicalSex;
    public bool isMale()
    {
        return biologicalSex;
    }
    public bool isFemale()
    {
        return !biologicalSex;
    }
    

    public Personality individualPersonality;
    public class Personality
    {
        public List<Tuple<string, int>> facets;
        private List<string> facet_names = new List<string>() { "LOVE_PROPENSITY", "HATE_PROPENSITY", "ENVY_PROPENSITY", "CHEER_PROPENSITY", "DEPRESSION_PROPENSITY", "ANGER_PROPENSITY",
                                                                "ANXIETY_PROPENSITY", "LUST_PROPENSITY", "STRESS_VULNERABILITY", "GREED", "IMMODERATION", "VIOLENT", "PERSEVERANCE",
                                                                "WASTEFULNESS", "DISCORD", "FRIENDLINESS", "POLITENESS", "DISDAIN_ADVICE", "BRAVERY", "CONFIDENCE", "VANITY", "AMBITION",
                                                                "GRATITUDE", "IMMODESTY", "HUMOR", "VENGEFUL", "PRIDE", "CRUELTY", "SINGLEMINDED", "HOPEFUL", "CURIOUS", "BASHFUL", "PRIVACY",
                                                                "PERFECTIONIST", "CLOSEMINDED", "TOLERANT", "EMOTIONALLY_OBSESSIVE", "SWAYED_BY_EMOTIONS", "ALTRUISM", "DUTIFULNESS",
                                                                "THOUGHTLESSNESS", "ORDERLINESS", "TRUST", "GREGARIOIUSNESS", "ASSERTIVENESS", "ACTIVITY_LEVEL", "EXCITEMENT_SEEKING",
                                                                "IMAGINATION", "ABSTRACT_INCLINED", "ART_INCLINED"};

        public Personality()
        {
            facets = new List<Tuple<string, int>>();

            foreach(var facet_type in facet_names)
            {
                // Get 10 random values from 0 to 10, and add them
                // This allows us to simulate a normal distribution from 0 to 100
                var facet_value = 0;

                for (int i = 0; i < 10; i++)
                {
                    facet_value += Random.Integer(0, 11);
                }


                facets.Add(Tuple.Create<string, int>(facet_type, facet_value));
            }

        }
    }


    public Occupation workStatus;


    /// <summary>
    /// The Occupation of the person which corresponds to their job within the town.
    /// </summary>
    public class Occupation
    {
        public Institution workplace; //Presently only supports a single job at a time.
        public List<Institution> former_workplaces;
        public float wage;
        public bool looking_for_job;
        public bool working;
        public bool retired;
        public float money { get; set; }

        //Constructor to be used for children / new people who are not yet looking to enter the workforce
        public Occupation(bool lookingForJob){
            workplace = null;
            money = 0;
            looking_for_job = lookingForJob;
            working = false;
            retired = false;
        }

        //Default constructor to be used for adults in the workforce
        public Occupation(Institution wp, Institution[] fwp, bool lfj, bool w, bool r, float m){
            workplace = wp;
            former_workplaces = fwp.ToList();
            looking_for_job = lfj;
            working = w;
            retired = r;
            money = m;
        }

        public void updateJobSearchStatus(bool looking){
            looking_for_job = looking;
        }

        //On obtaining a new job, updates the Occupation fields accordingly.  Presently only supports a single job at a time.
        public void getNewJob(Institution newWorkplace, float newWage){
            working = true;
            if(workplace != null)
                former_workplaces.Add(workplace);
            workplace = newWorkplace;
            wage = newWage;
        }

        //Removal of present workplace due to being fired or laid off or quitting. 
        public void loseJob(){
            working = false;
            former_workplaces.Add(workplace);
            workplace = null;
            wage = 0;
        }

        public void retire(){
            working = false;
            former_workplaces.Add(workplace);
            workplace = null;
            wage = 0;
            looking_for_job = false;
            retired = true;
        }
    }

    private Education personalEducation;
    class Education{
        public bool student {get; set;}
        public bool high_school_graduate {get; set;}
        public bool college_graduate {get; set;}
        public Education(bool s, bool hsg, bool cg){
            student = s;
            high_school_graduate = hsg;
            college_graduate = cg;
        }
    }


    /// -----------------------------------------------------------------------------------------------------------------------------///
    /// -----------------------------------------------------------------------------------------------------------------------------///
    /// <summary>
    /// Interface function to generate a person with randomly properties
    /// </summary>
    public static Person generateRandomPerson()
    {
        Person p = new Person("",null,null);
        p.biologicalSex = (Random.Integer(0, 2) == 1);
        p.DateOfBirth = Simulator.CurrentTime;
        p.firstName = NameManager.getFirstname(p.biologicalSex ? NameManager.sex.male : NameManager.sex.female);
        p.lastName = NameManager.getSurname(null);
        return p;
    }
    
    /// <summary>
    /// Basic Constructor for newborns, takes in string nameAtBirth, list of current siblings, and array of parents
    /// string nameAtBirth can be an empty string. the constructor will assign a randomly generated name.
    /// </summary>
    public Person(string nameAtBirth, List<Person> currSiblings, Person[] parentsParam)
    {
        DateOfBirth = Simulator.CurrentTime;
        sigOther = null;
        siblings = new List<Person>();
        children = null;
        parents = parentsParam;
        individualPersonality = new Personality();
        id = Guid.NewGuid();
        currentInstitution = parents[0].currentInstitution;  // Location right now set to being in the insitution of the first parent
        if(Random.Integer(0, 2) == 1)
        {
            biologicalSex = true;
        }
        else
        {
            biologicalSex = false;
        }
        
        if(currSiblings != null)
        {
            foreach (Person p in currSiblings)
            {
                siblings.Add(p);
            }
        }
        string[] nameSplit = nameAtBirth.Split(' ');
        this.firstName = (nameAtBirth.Length != 0) ? nameSplit[0] : GenerateRandomFirstName();
        this.lastName = (nameSplit.Length > 1) ? nameSplit[1] : GenerateRandomLastName();
    }
    
    /// <summary>
    /// Basic Constructor for newborns, takes in string nameAtBirth, and list of current siblings
    /// string nameAtBirth can be an empty string. the constructor will assign a randomly generated name.
    /// </summary>
    public Person(string nameAtBirth, List<Person> currSiblings)
    {
        DateOfBirth = Simulator.CurrentTime;
        age = 0;
        sigOther = null;
        siblings = new List<Person>();
        children = null;
        parents = new Person[2];
        id = Guid.NewGuid();
        individualPersonality = new Personality();

        //Likely location for a baby should be a home, but this is temp.  It should likely be the home location of the parents in the future.
        currentInstitution = InstitutionManager.RandomInstitutionIfAny();  // Unfinalized method name for random institution
        if(Random.Integer(0, 2) == 1)
        {
            biologicalSex = true;
        }
        else
        {
            biologicalSex = false;
        }
        
        if(currSiblings != null)
        {
            foreach (Person p in currSiblings)
            {
                siblings.Add(p);
            }
        }
        string[] nameSplit = nameAtBirth.Split(' ');
        this.firstName = (nameAtBirth.Length != 0) ? nameSplit[0] : GenerateRandomFirstName();
        this.lastName = (nameSplit.Length > 1) ? nameSplit[1] : GenerateRandomLastName();
    }
    /// <summary>
    /// Constructor for adults, those who may enter the town or are settlers
    /// string nameAtBirth can be an empty string. the constructor will assign a randomly generated name.
    /// </summary>
    public Person (string name, List<Person> currSiblings, int age, Person sigOther, List<Person> children, Person[] parents, bool biologicalSex){
        DateOfBirth = Simulator.CurrentTime.AddYears(-age);
        this.age = age;
        this.sigOther = sigOther;
        this.siblings = currSiblings;
        this.children = children;
        this.parents = parents;
        this.biologicalSex = biologicalSex;
        this.individualPersonality = new Personality();
        this.id = Guid.NewGuid();
        currentInstitution = InstitutionManager.RandomInstitution();  // random institution
        string[] nameSplit = name.Split(' ');
        
        this.firstName = (name.Length != 0) ? nameSplit[0] : GenerateRandomFirstName();
        this.lastName = (nameSplit.Length > 1) ? nameSplit[1] : GenerateRandomLastName();
    }
    
    /// <summary>
    /// Interface function for creating a child
    /// </summary>
    public static Person createChild(Person p1, params Person[] otherParents)
    {
        // createChild(p1, p1.sigOther)
        Person p2 = otherParents[0];
        if(otherParents.Length > 1)
        {
            // Humans should only have two biological parents
            return null;
        }


        int minAge = 18; // Change this to like 18 when we actually have age incrementing properly
        if(p1 == null || p2 == null)
        {
            return null;
        }

        if(p2.age >= minAge && p1.age >= minAge)
        {
            // if both individuals are above the minimum age, then they may have a child
            // Debug.LogFormat("Adam and Eve Results: {0}, {1}, {2}", p1.isFemale(), p2.isMale(), p1.isFemale() && p2.isMale());
            // Debug.LogFormat("Other Result: {0}, {1}, {2}", p1.isMale(), p2.isFemale(), p1.isMale() && p2.isFemale());
            if((p1.isFemale() && p2.isMale()) || (p1.isMale() && p2.isFemale()))
            {
                float birthChance = Random.Float(0,1);
                // Debug.LogFormat("Chance of birth: {0}/{1}", birthChance, conceptionRate);
           
                if(birthChance <= conceptionRate)
                {
                    
                    // string tempNameGen = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8);
                    List<Person> currSiblings = null;
                    if(p1.children != null)
                    {
                        if(p2.children != null)
                        {
                            var siblingsCollection =
                                from child in p1.children
                                where !(p2.children.Contains(child))
                                select child;

                            siblingsCollection = siblingsCollection.Union(from child in p2.children select child);

                            currSiblings = siblingsCollection.ToList<Person>();
                        }

                        currSiblings = p1.children;
                    }
                    else if(p2.children != null)
                    {
                        currSiblings = p2.children;
                    }
                    
                    Person bornChild = new Person("", currSiblings, new Person[] { p1, p2 });
                    if (p1.children == null)
                        p1.children = new List<Person>() {bornChild};
                    else
                        p1.children.Add(bornChild);
                    if (p2.children == null)
                        p2.children = new List<Person>() {bornChild};
                    else
                        p2.children.Add(bornChild);
                    return bornChild;
                }
            }

        }
        return null;  
    }

    public string getNamesFromListOfPersons(List<Person> listOfPersons)
    {
        if (listOfPersons == null || listOfPersons.Count == 0) return "None";
        string names = "";
        for (int i = 0; i < listOfPersons.Count; i++)
        {
            names += listOfPersons[i].name;
            names += ", ";
        }
        return names;
    }
    public override string ToString()
    {
        string parentNames = "";
        if ((parents == null) || (parents.Length == 0))
            parentNames = "None";
        else
        {
            for (int i = 0; i < parents.Length; i++)
            {
                parentNames += parents[i].name;
                parentNames += ", ";
            }
        }
        return string.Format("Person Name: {0}, Age: {1}, Sex: {2}, Significant Other: {3}, Parents: {4}Siblings: {5}Children: {6}",
            name, age, biologicalSex ? "M" : "F", sigOther == null ? "N/A" : sigOther.name, 
            parentNames, getNamesFromListOfPersons(siblings), getNamesFromListOfPersons(children));
    }
}
