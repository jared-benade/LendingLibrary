namespace LendingLibrary.DB.Migrations.DataConstants
{
    public class Tables
    {
        public static class Person
        {
            public const string NAME = "People";

            public class Columns
            {
                public const string ID = "Id";
                public const string FIRSTNAME = "FirstName";
                public const string LASTNAME = "LastName";
                public const string CONTACTNUMBER = "ContactNumber";
                public const string EMAIL = "Email";
                public const string ISACTIVE = "IsActive";
            }
        }
    }
}