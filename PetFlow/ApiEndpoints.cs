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
    }
    
    public static class MedicalNotes
    {
        private const string Base = $"{ApiBase}/pets/{{petId}}/medical-notes";

        public const string Create = Base;
        public const string Get = $"{Base}/{{id}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id}}";
        public const string Delete = $"{Base}/{{id}}";
        public const string GetSummary = $"{Base}/{{id}}/summary";
    }
    
    public static class Notes
    {
        private const string Base = $"{ApiBase}/pets/{{petId}}/notes";

        public const string Create = Base;
        public const string Get = $"{Base}/{{id}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id}}";
        public const string Delete = $"{Base}/{{id}}";
        public const string GetSummary = $"{Base}/{{id}}/summary";
    }
    
    public static class Events
    {
        private const string Base = $"{ApiBase}/pets/{{petId}}/events";

        public const string Create = Base;
        public const string Get = $"{Base}/{{id}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id}}";
        public const string Delete = $"{Base}/{{id}}";
    }
}