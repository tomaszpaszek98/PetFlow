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
            SeedPetEvents(modelBuilder);
            SeedNotes(modelBuilder);
            SeedMedicalNotes(modelBuilder);
        }

        private static void SeedPets(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<Pet>().HasData(
                new Pet
                {
                    Id = 1,
                    UserId = 1,
                    Name = "Max",
                    Species = "Dog",
                    Breed = "Golden Retriever",
                    DateOfBirth = new DateTime(2020, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                    PhotoUrl = "https://example.com/max.jpg",
                    StatusId = 1,
                    Created = seedDate,
                    CreatedBy = "system"
                },
                new Pet
                {
                    Id = 2,
                    UserId = 1,
                    Name = "Whiskers",
                    Species = "Cat",
                    Breed = "British Shorthair",
                    DateOfBirth = new DateTime(2019, 7, 22, 0, 0, 0, DateTimeKind.Utc),
                    PhotoUrl = "https://example.com/whiskers.jpg",
                    StatusId = 1,
                    Created = seedDate,
                    CreatedBy = "system"
                },
                new Pet
                {
                    Id = 3,
                    UserId = 1,
                    Name = "Buddy",
                    Species = "Dog",
                    Breed = "Labrador Retriever",
                    DateOfBirth = new DateTime(2021, 1, 10, 0, 0, 0, DateTimeKind.Utc),
                    PhotoUrl = null,
                    StatusId = 1,
                    Created = seedDate,
                    CreatedBy = "system"
                }
            );
        }

        private static void SeedEvents(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var upcomingDate = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc);
            
            modelBuilder.Entity<Event>().HasData(
                new Event
                {
                    Id = 1,
                    Title = "Vet Appointment",
                    Description = "Annual checkup for Max",
                    DateOfEvent = upcomingDate.AddDays(7),
                    Reminder = true,
                    StatusId = 1,
                    Created = seedDate,
                    CreatedBy = "system"
                },
                new Event
                {
                    Id = 2,
                    Title = "Grooming Session",
                    Description = "Bath and nail trimming for Whiskers",
                    DateOfEvent = upcomingDate.AddDays(3),
                    Reminder = false,
                    StatusId = 1,
                    Created = seedDate,
                    CreatedBy = "system"
                },
                new Event
                {
                    Id = 3,
                    Title = "Vaccination",
                    Description = "Rabies vaccination for Buddy",
                    DateOfEvent = upcomingDate.AddDays(14),
                    Reminder = true,
                    StatusId = 1,
                    Created = seedDate,
                    CreatedBy = "system"
                }
            );
        }

        private static void SeedPetEvents(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            
            modelBuilder.Entity<PetEvent>().HasData(
                new PetEvent
                {
                    Id = 1,
                    PetId = 1, // Max
                    EventId = 1, // Vet Appointment
                    StatusId = 1,
                    Created = seedDate,
                    CreatedBy = "system"
                },
                new PetEvent
                {
                    Id = 2,
                    PetId = 2, // Whiskers
                    EventId = 2, // Grooming Session
                    StatusId = 1,
                    Created = seedDate,
                    CreatedBy = "system"
                },
                new PetEvent
                {
                    Id = 3,
                    PetId = 3, // Buddy
                    EventId = 3, // Vaccination
                    StatusId = 1,
                    Created = seedDate,
                    CreatedBy = "system"
                }
            );
        }

        private static void SeedNotes(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<Note>().HasData(
                new Note
                {
                    Id = 1,
                    Content = "Max seems to have good energy today",
                    Type = NoteType.Mood,
                    PetId = 1,
                    StatusId = 1,
                    Created = seedDate,
                    CreatedBy = "system"
                },
                new Note
                {
                    Id = 2,
                    Content = "Whiskers was very playful during the afternoon",
                    Type = NoteType.Behaviour,
                    PetId = 2,
                    StatusId = 1,
                    Created = seedDate,
                    CreatedBy = "system"
                },
                new Note
                {
                    Id = 3,
                    Content = "Buddy ate all his meals today",
                    Type = NoteType.General,
                    PetId = 3,
                    StatusId = 1,
                    Created = seedDate,
                    CreatedBy = "system"
                }
            );
        }

        private static void SeedMedicalNotes(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<MedicalNote>().HasData(
                new MedicalNote
                {
                    Id = 1,
                    Title = "Ear Infection Treatment",
                    Description = "Started antibiotic treatment for Max's ear infection",
                    PetId = 1,
                    StatusId = 1,
                    Created = seedDate,
                    CreatedBy = "system"
                },
                new MedicalNote
                {
                    Id = 2,
                    Title = "Dental Cleaning",
                    Description = "Professional dental cleaning performed for Whiskers",
                    PetId = 2,
                    StatusId = 1,
                    Created = seedDate,
                    CreatedBy = "system"
                },
                new MedicalNote
                {
                    Id = 3,
                    Title = "Weight Management",
                    Description = "Buddy needs to reduce weight by 2kg - started new diet plan",
                    PetId = 3,
                    StatusId = 1,
                    Created = seedDate,
                    CreatedBy = "system"
                }
            );
        }
    }
}
