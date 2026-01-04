using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace PetFlow.Persistence
{
    public static class Seed
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            SeedPets(modelBuilder);
            SeedEvents(modelBuilder);
            SeedNotes(modelBuilder);
            SeedMedicalNotes(modelBuilder);
        }

        private static void SeedPets(ModelBuilder modelBuilder)
        {
            var now = DateTime.UtcNow;
            modelBuilder.Entity<Pet>().HasData(
                new Pet
                {
                    Id = 1,
                    UserId = 1,
                    Name = "Max",
                    Species = "Dog",
                    Breed = "Golden Retriever",
                    DateOfBirth = new DateTime(2020, 3, 15),
                    PhotoUrl = "https://example.com/max.jpg",
                    StatusId = 1,
                    Created = now,
                    CreatedBy = now.ToString()
                },
                new Pet
                {
                    Id = 2,
                    UserId = 1,
                    Name = "Whiskers",
                    Species = "Cat",
                    Breed = "British Shorthair",
                    DateOfBirth = new DateTime(2019, 7, 22),
                    PhotoUrl = "https://example.com/whiskers.jpg",
                    StatusId = 1,
                    Created = now,
                    CreatedBy = now.ToString()
                },
                new Pet
                {
                    Id = 3,
                    UserId = 1,
                    Name = "Buddy",
                    Species = "Dog",
                    Breed = "Labrador Retriever",
                    DateOfBirth = new DateTime(2021, 1, 10),
                    PhotoUrl = null,
                    StatusId = 1,
                    Created = now,
                    CreatedBy = now.ToString()
                }
            );
        }

        private static void SeedEvents(ModelBuilder modelBuilder)
        {
            var now = DateTime.UtcNow;
            modelBuilder.Entity<Event>().HasData(
                new Event
                {
                    Id = 1,
                    Title = "Vet Appointment",
                    Description = "Annual checkup for Max",
                    DateOfEvent = DateTime.UtcNow.AddDays(7),
                    Reminder = true,
                    StatusId = 1,
                    Created = now,
                    CreatedBy = now.ToString()
                },
                new Event
                {
                    Id = 2,
                    Title = "Grooming Session",
                    Description = "Bath and nail trimming for Whiskers",
                    DateOfEvent = DateTime.UtcNow.AddDays(3),
                    Reminder = false,
                    StatusId = 1,
                    Created = now,
                    CreatedBy = now.ToString()
                },
                new Event
                {
                    Id = 3,
                    Title = "Vaccination",
                    Description = "Rabies vaccination for Buddy",
                    DateOfEvent = DateTime.UtcNow.AddDays(14),
                    Reminder = true,
                    StatusId = 1,
                    Created = now,
                    CreatedBy = now.ToString()
                }
            );
        }

        private static void SeedNotes(ModelBuilder modelBuilder)
        {
            var now = DateTime.UtcNow;
            modelBuilder.Entity<Note>().HasData(
                new Note
                {
                    Id = 1,
                    Content = "Max seems to have good energy today",
                    Type = NoteType.Mood,
                    PetId = 1,
                    StatusId = 1,
                    Created = now,
                    CreatedBy = now.ToString()
                },
                new Note
                {
                    Id = 2,
                    Content = "Whiskers was very playful during the afternoon",
                    Type = NoteType.Behaviour,
                    PetId = 2,
                    StatusId = 1,
                    Created = now,
                    CreatedBy = now.ToString()
                },
                new Note
                {
                    Id = 3,
                    Content = "Buddy ate all his meals today",
                    Type = NoteType.General,
                    PetId = 3,
                    StatusId = 1,
                    Created = now,
                    CreatedBy = now.ToString()
                }
            );
        }

        private static void SeedMedicalNotes(ModelBuilder modelBuilder)
        {
            var now = DateTime.UtcNow;
            modelBuilder.Entity<MedicalNote>().HasData(
                new MedicalNote
                {
                    Id = 1,
                    Title = "Ear Infection Treatment",
                    Description = "Started antibiotic treatment for Max's ear infection",
                    PetId = 1,
                    StatusId = 1,
                    Created = now,
                    CreatedBy = now.ToString()
                },
                new MedicalNote
                {
                    Id = 2,
                    Title = "Dental Cleaning",
                    Description = "Professional dental cleaning performed for Whiskers",
                    PetId = 2,
                    StatusId = 1,
                    Created = now,
                    CreatedBy = now.ToString()
                },
                new MedicalNote
                {
                    Id = 3,
                    Title = "Weight Management",
                    Description = "Buddy needs to reduce weight by 2kg - started new diet plan",
                    PetId = 3,
                    StatusId = 1,
                    Created = now,
                    CreatedBy = now.ToString()
                }
            );
        }
    }
}
