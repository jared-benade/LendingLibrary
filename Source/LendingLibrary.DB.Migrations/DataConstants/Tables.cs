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

        public static class ItemType
        {
            public const string NAME = "ItemTypes";

            public class Columns
            {
                public const string ID = "Id";
                public const string DESCRIPTION = "Description";
                public const string ISACTIVE = "IsActive";
            }
        }

        public static class Item
        {
            public const string NAME = "Items";

            public class Columns
            {
                public const string ID = "Id";
                public const string TITLE = "Title";
                public const string AUTHOR = "Author";
                public const string EDITION = "Edition";
                public const string YEAR = "Year";
                public const string ARTIST = "Artist";
                public const string AVAILABLE = "Available";
                public const string ITEMTYPEID = "ItemTypeId";
                public const string ISACTIVE = "IsActive";
            }
        }
    }
}