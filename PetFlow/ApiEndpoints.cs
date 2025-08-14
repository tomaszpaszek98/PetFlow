namespace PetFlow;

public static class ApiEndpoints
{
    private const string ApiBase = "api/v1";

    public static class Pets
    {
        private const string Base = $"{ApiBase}/pets";

        public const string Create = Base;
        public const string Get = $"{Base}/{{id}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id}}";
        public const string Delete = $"{Base}/{{id}}";

        public static class Photo
        {
            private const string PhotoBase = Base + "/{{id}}/photo";
            
            public const string Upload = PhotoBase;
            public const string Delete = $"{PhotoBase}/{{photoId}}";
        }

        public static class Events
        {
            public const string GetAll = $"{Base}/{{id}}/events";
        }
        
        public static class MedicalNotes
        {
            private const string MedicalNotesBase = $"{Base}/{{petId}}/medical-notes";

            public const string Create = MedicalNotesBase;
            public const string Get = $"{MedicalNotesBase}/{{id}}";
            public const string GetAll = MedicalNotesBase;
            public const string Update = $"{MedicalNotesBase}/{{id}}";
            public const string Delete = $"{MedicalNotesBase}/{{id}}";

            public static class Summary
            {
                public const string GetForTimeRange = $"{MedicalNotesBase}/summary";
                public const string GetForNote = $"{MedicalNotesBase}/{{id}}/summary";
            }
        }
    
        public static class Notes
        {
            private const string NotesBase = $"{Base}/{{petId}}/notes";

            public const string Create = NotesBase;
            public const string Get = $"{NotesBase}/{{id}}";
            public const string GetAll = NotesBase;
            public const string Update = $"{NotesBase}/{{id}}";
            public const string Delete = $"{NotesBase}/{{id}}";
            
            public static class Summary
            {
                public const string GetForTimeRange = $"{NotesBase}/summary";
                public const string GetForNote = $"{NotesBase}/{{id}}/summary";
            }
        }
    }
    
    public static class Events
    {
        private const string Base = $"{ApiBase}/events";

        public const string Create = Base;
        public const string Get = $"{Base}/{{id}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id}}";
        public const string Delete = $"{Base}/{{id}}";

        public static class Pets
        {
            private const string PetsBase = $"{Base}/{{eventId}}/pets";
            public const string Add = PetsBase;
            public const string Delete = $"{PetsBase}/{{petId}}";
        }
    }
}