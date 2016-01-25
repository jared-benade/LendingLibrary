namespace LendingLibrary.DB.Migrations.DataConstants
{
    public class Tables
    {
        public static class Person
        {
            public const string NAME = "Person";

            public class Columns
            {
                public const string PERSONID = "PersonId";
                public const string FIRSTNAME = "FirstName";
                public const string LASTNAME = "LastName";
                public const string CONTACTNUMBER = "ContactNumber";
                public const string EMAIL = "Email";
            }
        }
    }
}