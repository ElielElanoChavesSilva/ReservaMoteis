using BookMotelsDomain.Entities;
using BookMotelsInfra.Context;

namespace BookMotelsAPI
{
    public static class DbInitializer
    {
        public static void Seed(MainContext db)
        {
            SeedProfiles(db);
            SeedAdmin(db);
        }

        private static void SeedProfiles(MainContext db)
        {
            if (!db.Profiles.Any())
            {
                db.Profiles.AddRange(
                    new ProfileEntity { Id = 1, Name = "Admin" },
                    new ProfileEntity { Id = 2, Name = "User" }
                );
                db.SaveChanges();
            }
        }
        private static void SeedAdmin(MainContext db)
        {
            if (!db.Users.Any())
            {
                db.Users.Add(new UserEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Eliel",
                    Email = "eliel@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("eliel"),
                    ProfileId = 1
                });
                db.SaveChanges();

                db.Users.Add(new UserEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Silva",
                    Email = "silva@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("silva"),
                    ProfileId = 2
                });
                db.SaveChanges();
            }
        }
    }
}
