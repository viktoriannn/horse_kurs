using horse_kurs.DTOs;
using horse_kurs.Models;
using horse_kurs.Services;
using Microsoft.EntityFrameworkCore;

namespace TUnitTest
{
    public class UnitTest1
    {
        // Вспомогательный метод для создания тестового клиента
        private Client CreateTestClient(int id, string surname, string name, string patronymic, decimal balance)
        {
            return new Client
            {
                IdClient = id,
                Surname = surname,
                Name = name,
                Patronymic = patronymic,
                DateOfBirth = new DateOnly(1990, 1, 1),
                Phone = "+7 (999) 111-22-33",
                LevelOfTraining = "Новичок",
                Passport = "1234 567890",
                City = "Москва",
                Street = "Тестовая",
                House = "1",
                Flat = "1",
                Balance = balance,
                DateOfRegistration = DateOnly.FromDateTime(DateTime.Now)
            };
        }

        [Fact]
        public void GetClientProfile_ValidId_ReturnsCorrectFullName()
        {
            // Arrange
            string expectedFullName = "Смирнов Александр Игоревич";
            var options = new DbContextOptionsBuilder<EquestrianClubContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Profile")
                .Options;

            using (var context = new EquestrianClubContext(options))
            {
                var client = CreateTestClient(1, "Смирнов", "Александр", "Игоревич", 5000);
                context.Clients.Add(client);
                context.SaveChanges();
            }

            using (var context = new EquestrianClubContext(options))
            {
                var service = new EquestrianService(context);

                // Act
                var profile = service.GetClientProfile(1).Result;

                // Assert
                Assert.Equal(expectedFullName, profile.FullName);
            }
        }

        [Fact]
        public void Login_ValidAdminCredentials_ReturnsAdminRole()
        {
            // Arrange
            string expectedRole = "Admin";
            var options = new DbContextOptionsBuilder<EquestrianClubContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Login")
                .Options;

            using (var context = new EquestrianClubContext(options))
            {
                var admin = new AppUser
                {
                    IdUser = 1,
                    Login = "admin",
                    Password = "admin123",
                    Role = "Admin"
                };
                context.AppUsers.Add(admin);
                context.SaveChanges();
            }

            using (var context = new EquestrianClubContext(options))
            {
                var service = new AuthService(context);
                var loginDto = new LoginDto { Login = "admin", Password = "admin123" };

                // Act
                var result = service.Login(loginDto).Result;

                // Assert
                Assert.NotNull(result);
                Assert.Equal(expectedRole, result.Role);
            }
        }

        [Fact]
        public void Login_InvalidCredentials_ReturnsNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EquestrianClubContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_InvalidLogin")
                .Options;

            using (var context = new EquestrianClubContext(options))
            {
                var service = new AuthService(context);
                var loginDto = new LoginDto { Login = "wrong", Password = "wrong" };

                // Act
                var result = service.Login(loginDto).Result;

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public void GetCoachSchedule_ValidCoachId_ReturnsSchedule()
        {
            // Arrange
            int coachId = 1;
            var options = new DbContextOptionsBuilder<EquestrianClubContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Schedule")
                .Options;

            using (var context = new EquestrianClubContext(options))
            {
                var client = CreateTestClient(1, "Тестов", "Клиент", "Тестович", 5000);
                var coach = new Coach { IdCoach = coachId, Qualification = "Тренер", Specialization = "Конкур" };
                var arena = new Arena { IdArena = 1, Name = "Тестовая арена", Type = "Конкурный", Coverage = "Песок", Length = 40, Width = 20, Status = "Доступен" };
                var employee = new Employee
                {
                    IdEmployee = 1,
                    Surname = "Иванов",
                    Name = "Иван",
                    Post = "Тренер",
                    City = "Москва",
                    Street = "Ленина",
                    HouseNumber = "1",
                    DateOfBirth = new DateOnly(1980, 1, 1)
                };

                context.Employees.Add(employee);
                context.Clients.Add(client);
                context.Coaches.Add(coach);
                context.Arenas.Add(arena);

                var lesson = new Lesson
                {
                    IdLesson = 1,
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    Type = "Индивидуальное",
                    IdClient = 1,
                    IdCoach = coachId,
                    IdArena = 1
                };
                context.Lessons.Add(lesson);
                context.SaveChanges();
            }

            using (var context = new EquestrianClubContext(options))
            {
                var service = new EquestrianService(context);

                // Act
                var schedule = service.GetCoachSchedule(coachId).Result;

                // Assert
                Assert.NotNull(schedule);
                Assert.True(schedule.Count > 0);
            }
        }

        [Fact]
        public void CreateBooking_ValidData_ReturnsTrue()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EquestrianClubContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Booking")
                .Options;

            using (var context = new EquestrianClubContext(options))
            {
                var client = CreateTestClient(1, "Тестов", "Клиент", "Тестович", 5000);
                var coach = new Coach { IdCoach = 1, Qualification = "Тренер", Specialization = "Конкур" };
                var arena = new Arena { IdArena = 1, Name = "Тестовая арена", Type = "Конкурный", Coverage = "Песок", Length = 40, Width = 20, Status = "Доступен" };
                var employee = new Employee
                {
                    IdEmployee = 1,
                    Surname = "Иванов",
                    Name = "Иван",
                    Post = "Тренер",
                    City = "Москва",
                    Street = "Ленина",
                    HouseNumber = "1",
                    DateOfBirth = new DateOnly(1980, 1, 1)
                };
                var horse = new Horse
                {
                    IdHorse = 1,
                    Name = "Тестовая",
                    Gender = "Кобыла",
                    Breed = "Тракененская",
                    DateOfBirth = new DateOnly(2015, 1, 1),
                    StateOfHealth = "Здорова",
                    LevelOfTraining = "Начинающий",
                    Passport = "H12345",
                    Status = "В работе"
                };

                context.Employees.Add(employee);
                context.Clients.Add(client);
                context.Coaches.Add(coach);
                context.Arenas.Add(arena);
                context.Horses.Add(horse);
                context.SaveChanges();
            }

            using (var context = new EquestrianClubContext(options))
            {
                var service = new EquestrianService(context);
                var bookingDto = new CreateBookingDto
                {
                    IdClient = 1,
                    IdCoach = 1,
                    IdArena = 1,
                    Date = DateTime.Now.AddDays(1),
                    LessonType = "Индивидуальное"
                };

                // Act
                var result = service.CreateBookingAsync(bookingDto).Result;

                // Assert
                Assert.True(result);
            }
        }

        [Fact]
        public void CreateBooking_InsufficientBalance_ReturnsFalse()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EquestrianClubContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_NoMoney")
                .Options;

            using (var context = new EquestrianClubContext(options))
            {
                var client = CreateTestClient(1, "Тестов", "Клиент", "Тестович", 500);
                var coach = new Coach { IdCoach = 1, Qualification = "Тренер", Specialization = "Конкур" };
                var arena = new Arena { IdArena = 1, Name = "Тестовая арена", Type = "Конкурный", Coverage = "Песок", Length = 40, Width = 20, Status = "Доступен" };
                var employee = new Employee
                {
                    IdEmployee = 1,
                    Surname = "Иванов",
                    Name = "Иван",
                    Post = "Тренер",
                    City = "Москва",
                    Street = "Ленина",
                    HouseNumber = "1",
                    DateOfBirth = new DateOnly(1980, 1, 1)
                };
                var horse = new Horse
                {
                    IdHorse = 1,
                    Name = "Тестовая",
                    Gender = "Кобыла",
                    Breed = "Тракененская",
                    DateOfBirth = new DateOnly(2015, 1, 1),
                    StateOfHealth = "Здорова",
                    LevelOfTraining = "Начинающий",
                    Passport = "H12345",
                    Status = "В работе"
                };

                context.Employees.Add(employee);
                context.Clients.Add(client);
                context.Coaches.Add(coach);
                context.Arenas.Add(arena);
                context.Horses.Add(horse);
                context.SaveChanges();
            }

            using (var context = new EquestrianClubContext(options))
            {
                var service = new EquestrianService(context);
                var bookingDto = new CreateBookingDto
                {
                    IdClient = 1,
                    IdCoach = 1,
                    IdArena = 1,
                    Date = DateTime.Now.AddDays(1),
                    LessonType = "Индивидуальное"
                };

                // Act
                var result = service.CreateBookingAsync(bookingDto).Result;

                // Assert
                Assert.False(result);
            }
        }

        [Fact]
        public void GetAvailableCompetitions_ReturnsOnlyFutureCompetitions()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EquestrianClubContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Competitions")
                .Options;

            using (var context = new EquestrianClubContext(options))
            {
                var arena = new Arena
                {
                    IdArena = 1,
                    Name = "Тестовая арена",
                    Type = "Конкурный",
                    Coverage = "Песок",
                    Length = 40,
                    Width = 20,
                    Status = "Доступен"
                };
                context.Arenas.Add(arena);

                var futureComp = new Competition
                {
                    IdCompetition = 1,
                    Name = "Будущее соревнование",
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(10)),
                    Type = "Конкур",
                    Level = "Региональные",
                    Status = "Запланировано",
                    IdArena = 1
                };
                var pastComp = new Competition
                {
                    IdCompetition = 2,
                    Name = "Прошедшее соревнование",
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)),
                    Type = "Конкур",
                    Level = "Региональные",
                    Status = "Завершено",
                    IdArena = 1
                };

                context.Competitions.Add(futureComp);
                context.Competitions.Add(pastComp);
                context.SaveChanges();
            }

            using (var context = new EquestrianClubContext(options))
            {
                var service = new EquestrianService(context);

                // Act
                var competitions = service.GetAvailableCompetitions().Result;

                // Assert
                Assert.NotNull(competitions);
                Assert.Single(competitions);
                Assert.Equal("Будущее соревнование", competitions[0].Name);
            }
        }

        [Fact]
        public void UpdateUserRole_ValidData_ReturnsTrue()
        {
            // Arrange
            string expectedRole = "Coach";
            var options = new DbContextOptionsBuilder<EquestrianClubContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_UpdateRole")
                .Options;

            using (var context = new EquestrianClubContext(options))
            {
                var user = new AppUser
                {
                    IdUser = 1,
                    Login = "test",
                    Password = "123",
                    Role = "Client"
                };
                context.AppUsers.Add(user);
                context.SaveChanges();
            }

            using (var context = new EquestrianClubContext(options))
            {
                var service = new EquestrianService(context);

                // Act
                var result = service.UpdateUserRole(1, "Coach").Result;

                // Assert
                Assert.True(result);

                var updatedUser = context.AppUsers.Find(1);
                Assert.Equal(expectedRole, updatedUser.Role);
            }
        }
    }
}