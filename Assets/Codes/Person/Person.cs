using System;
using System.Collections.Generic;
using System.Linq;

public class Person
{
    public DateTime DateOfBirth;
    public bool dead;
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

    public class Relationship
    {
        public int Charge;
        public int Spark;
        public int Compatibility;

        public Relationship(int c, int s, Person p1, Person p2)
        {
            Charge = c;
            Spark = s;
            Compatibility = getCompatibility(p1, p2);
        }

        public static int getCompatibility(Person p1, Person p2)
        {
            double personalityDiff = 0;
            // these facets are chosen to mimic James' compatibility calculation using O/E/A difference 
            List<string> compatibility_facets = new List<string> { "CURIOUS", "EXCITEMENT_SEEKING", "POLITENESS",
                                                                  "FRIENDLINESS", "ASSERTIVENESS", "CHEER_PROPENSITY" };
            foreach(string compatibility_facet in compatibility_facets)
            {
                int p1_facet_value = p1.individualPersonality.facets[compatibility_facet];
                int p2_facet_value = p2.individualPersonality.facets[compatibility_facet];
                personalityDiff += Math.Pow(Math.Abs(p1_facet_value - p2_facet_value), 1.2);
            }

            // average difference across facets and flip for compatibility
            int personalityDiffNorm = (int) (2.5 * personalityDiff / compatibility_facets.Count);
            return Math.Max(100 - personalityDiffNorm, 0);
        }
    }

    public static bool inLoveTriangle(Person p1, Person p2, Person p3)
    {


        // each variable is an indicator for whether a romantic connection exists 
        // between any of the pairs in p1, p2, and p3
        int p1p2Connected = p1.romanticallyInterestedIn.Contains(p2) || p2.romanticallyInterestedIn.Contains(p1) ? 1 : 0;
        int p1p3Connected = p1.romanticallyInterestedIn.Contains(p3) || p3.romanticallyInterestedIn.Contains(p1) ? 1 : 0;
        int p2p3Connected = p2.romanticallyInterestedIn.Contains(p3) || p3.romanticallyInterestedIn.Contains(p2) ? 1 : 0;

        // if there are at least two connections in the triangle, we have a love triangle!
        return p1p2Connected + p1p3Connected + p2p3Connected >= 2;
    }

    private Dictionary<Person, Relationship> relationshipDict = new Dictionary<Person, Relationship>();
    public void updateRelationshipSpark(Person p, int amount)
    {
        // don't increase spark, if not attracted to p's sex
        if ((p.IsMale && !attractedToMen) || (!p.IsFemale && !attractedToWomen))
        {
            return;
        }
        if (!relationshipDict.ContainsKey(p))
        {
            relationshipDict.Add(p,new Relationship(0, 0, this, p));
        }
        else
        {
            // if person is quite love prone, boost positive spark updates
            if (amount > 0 && p.individualPersonality.facets["LOVE_PROPENSITY"] > 70)
            {
                amount = (int)Math.Ceiling(amount * 1.25f);
            }
            relationshipDict[p].Spark += amount;
        }
    }
    public void updateRelationshipCharge(Person p, int amount)
    {
        if (!relationshipDict.ContainsKey(p))
        {
            relationshipDict.Add(p, new Relationship(0, 0, this, p));
        }
        else
        {
            // if person is quite friendly, boost positive charge updates
            if (amount > 0 && p.individualPersonality.facets["FRIENDLINESS"] > 70)
            {
                amount = (int)Math.Ceiling(amount * 1.25f);
            }
            // if person is quite hate prone, boost negative charge updates 
            else if (amount < 0 && p.individualPersonality.facets["HATE_PROPENSITY"] < 30)
            {
                amount = (int)Math.Floor(amount * 1.25f);
            }
            relationshipDict[p].Charge += amount;
        }
    }

    public int getRelationshipSpark(Person p)
    {
        return relationshipDict[p].Spark;
    }
    public int getRelationshipCharge(Person p)
    {
        return relationshipDict[p].Charge;
    }
    


    // See https://github.com/james-owen-ryan/talktown/blob/master/config/social_sim_config.py
    int sparkThresholdForCaptivating = 10;
    int chargeThresholdForRelationship = 15;
    public List<Person> captivatedBy = new List<Person>();

    public void getCaptivatedIndividuals()
    {
        captivatedBy = new List<Person>();

        foreach (var item in relationshipDict)
        {
            if (item.Value.Spark >= sparkThresholdForCaptivating)
            {
                captivatedBy.Add(item.Key);
            }
        }

        captivatedBy.RemoveAll(item => item == null);
        if (this.siblings!= null)
        {
            captivatedBy.RemoveAll(item => this.siblings.Contains(item));
        }
        if (this.children != null)
        { 
            captivatedBy.RemoveAll(item => this.children.Contains(item));
        }

        captivatedBy = captivatedBy.Distinct().ToList();
    }

    public List<Person> romanticallyInterestedIn = new List<Person>();
    public void getRomanticInterests()
    {
        romanticallyInterestedIn = new List<Person>();

        foreach(var p in captivatedBy)
        {
            var relation = relationshipDict[p];
            if(relation.Charge >= chargeThresholdForRelationship)
            {
                romanticallyInterestedIn.Add(p);
            }
        }

        romanticallyInterestedIn.RemoveAll(item => item == null);
        romanticallyInterestedIn = romanticallyInterestedIn.Distinct().ToList();

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
    
    public bool attractedToMen;
    public bool attractedToWomen;
    private static double homosexualityIncidence = 0.1;
    private static double bisexualityIncidence = 0.1;
    public void setSexuality(bool biologicalSex, double sexualityNum)
    {
        if (sexualityNum < homosexualityIncidence)
        {
            attractedToMen = IsMale; // men like men, women don't like men
            attractedToWomen = IsFemale;
        }
        else
        {
            sexualityNum -= homosexualityIncidence;
            if (sexualityNum < bisexualityIncidence)
            {

                attractedToMen = true; // men like men, women don't like men
                attractedToWomen = true;
            }
            else
            {
                attractedToMen = IsFemale; // men like men, women don't like men
                attractedToWomen = IsMale;
            }
        }
    }

    public bool IsMale => biologicalSex;

    public bool IsFemale => !biologicalSex;
    

    public Personality individualPersonality;
    public class Personality
    {
        public Dictionary<string, int> facets;

        // See http://dwarffortresswiki.org/index.php/DF2014:Personality_trait#Facets
        private List<string> facet_names = new List<string>() { "LOVE_PROPENSITY", "HATE_PROPENSITY", "ENVY_PROPENSITY", "CHEER_PROPENSITY", "DEPRESSION_PROPENSITY", "ANGER_PROPENSITY",
                                                                "ANXIETY_PROPENSITY", "LUST_PROPENSITY", "STRESS_VULNERABILITY", "GREED", "IMMODERATION", "VIOLENT", "PERSEVERANCE",
                                                                "WASTEFULNESS", "DISCORD", "FRIENDLINESS", "POLITENESS", "DISDAIN_ADVICE", "BRAVERY", "CONFIDENCE", "VANITY", "AMBITION",
                                                                "GRATITUDE", "IMMODESTY", "HUMOR", "VENGEFUL", "PRIDE", "CRUELTY", "SINGLEMINDED", "HOPEFUL", "CURIOUS", "BASHFUL", "PRIVACY",
                                                                "PERFECTIONIST", "CLOSEMINDED", "TOLERANT", "EMOTIONALLY_OBSESSIVE", "SWAYED_BY_EMOTIONS", "ALTRUISM", "DUTIFULNESS",
                                                                "THOUGHTLESSNESS", "ORDERLINESS", "TRUST", "GREGARIOIUSNESS", "ASSERTIVENESS", "ACTIVITY_LEVEL", "EXCITEMENT_SEEKING",
                                                                "IMAGINATION", "ABSTRACT_INCLINED", "ART_INCLINED"}; 

        public Personality()
        {
            facets = new Dictionary<string, int>();

            foreach(var facet_type in facet_names)
            {
                // Get 10 random values from 0 to 10, and add them
                // This allows us to simulate a normal distribution from 0 to 100
                var facet_value = 0;

                for (int i = 0; i < 10; i++)
                {
                    facet_value += Random.Integer(0, 11);
                }


                facets.Add(facet_type, facet_value);
            }

        }
    }


    public Occupation workStatus = new Occupation(false);


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
            former_workplaces = new List<Institution>();
            money = 0;
            looking_for_job = lookingForJob;
            working = false;
            retired = false;
            former_workplaces = new List<Institution>();
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

    public Education personalEducation = new Education();
    public class Education{
        public bool is_student {get; set;}
        public bool is_high_school_graduate {get; set;}
        public bool is_college_graduate {get; set;}
        public Education(bool s, bool hsg, bool cg){
            is_student = s;
            is_high_school_graduate = hsg;
            is_college_graduate = cg;
        }
        public Education(){
            is_student = false;
            is_high_school_graduate = false;
            is_college_graduate = false;
        }
    }


    /// -----------------------------------------------------------------------------------------------------------------------------///
    /// -----------------------------------------------------------------------------------------------------------------------------///
    /// <summary>
    /// Interface function to generate a person with randomly properties
    /// </summary>
    public static Person generateRandomPerson()
    {
        Person p = new Person("",null,new Person[2]);
        p.biologicalSex = (Random.Integer(0, 2) == 1);
        p.setSexuality(p.biologicalSex, Random.Float(0, 1));
        p.age = Random.Integer(0, 70);
        p.DateOfBirth = Simulator.CurrentTime.AddYears(-p.age);
        p.firstName = NameManager.getFirstname(p.biologicalSex ? NameManager.sex.male : NameManager.sex.female);
        p.lastName = NameManager.getSurname(null);
        p.currentInstitution = InstitutionManager.RandomInstitutionIfAny();
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

        currentInstitution = InstitutionManager.RandomInstitutionIfAny();
        if(Random.Integer(0, 2) == 1)
        {
            biologicalSex = true;
        }
        else
        {
            biologicalSex = false;
        }
        setSexuality(biologicalSex, Random.Float(0, 1));

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
        setSexuality(biologicalSex, Random.Float(0, 1));


        if (currSiblings != null)
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
    public Person (string name, List<Person> currSiblings, int age, Person sigOther, List<Person> children, Person[] parents, bool biologicalSex, 
                   bool attractedToMen, bool attractedToWomen)
    {
        this.DateOfBirth = Simulator.CurrentTime.AddYears(-age);
        this.age = age;
        this.sigOther = sigOther;
        this.siblings = currSiblings;
        this.children = children;
        this.parents = parents;
        this.biologicalSex = biologicalSex;
        this.attractedToMen = attractedToMen;
        this.attractedToWomen = attractedToWomen;
        this.individualPersonality = new Personality();
        this.id = Guid.NewGuid();
        this.currentInstitution = InstitutionManager.RandomInstitution();  // random institution
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

    // helper function for the filter of biologicalMother in roleLibrary
    public bool readyForNextChild()
    {
        if (children == null || children.Count == 0)
            return true;
        foreach (Person child in children)
        {
            if (child.age < 1)
            {
                return false;
            }
        }
        return true;
    }
    // now the only constraint is that siblings cannot marry each other
    public bool CanMarry(Person p)
    {
        if (parents == null || p.parents == null) 
            return true;
        foreach (Person parent in parents)
        {
            if (parent != null && p.parents.Contains(parent))
            {
                return false;
            }
        }
        return true;
    }

    public bool haveAffair()
    {
        foreach (Person p in romanticallyInterestedIn)
        {
            if (p != sigOther && (getRelationshipSpark(p) > getRelationshipSpark(sigOther)))
                return true;
        }
        return false;
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

    //For the time being, making ToString only return the name to make it less verbose
    public override string ToString()
    {
        return string.Format("Person Name: {0}", name);
    }

    //Original ToString 
    public string ToStringMore(){
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
