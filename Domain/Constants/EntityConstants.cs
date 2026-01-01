namespace Domain.Constants;

public static class EntityConstants
{
    public static class Pet
    {
        public const int MaxNameLength = 50;
        public const int MaxSpeciesLength = 30;
        public const int MaxBreedLength = 30;
        public const int MaxPhotoUrlLength = 200;
    }

    public static class Event
    {
        public const int MaxTitleLength = 50;
        public const int MaxDescriptionLength = 500;
    }

    public static class Note
    {
        public const int MaxTypeLength = 50;
        public const int MaxContentLength = 2000;
    }

    public static class MedicalNote
    {
        public const int MaxTitleLength = 100;
        public const int MaxDescriptionLength = 2000;
    }
}