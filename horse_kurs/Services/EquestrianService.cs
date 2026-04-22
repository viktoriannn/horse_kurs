using horse_kurs.Models;
using horse_kurs.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace horse_kurs.Services
{
    public interface IEquestrianService
    {
        Task<List<CoachScheduleDto>> GetCoachSchedule(int coachId);
        Task<ClientProfileDto> GetClientProfile(int clientId);
        Task<List<CompetitionDto>> GetAvailableCompetitions();
        Task<bool> RegisterForLesson(RegisterLessonDto dto);
        Task<bool> RegisterForCompetition(RegisterCompetitionDto dto);
        Task<byte[]> PrintSchedule(int clientId);
        Task<List<AdminUserDto>> GetAllUsers();
        Task<bool> UpdateUserRole(int userId, string newRole);
        Task<byte[]> GetAllUsersReport();

        Task<Employee> CreateEmployee(CreateEmployeeDto dto);
        Task<Client> CreateClient(CreateClientDto dto);
        Task<Horse> CreateHorse(CreateHorseDto dto);
        Task<BookingDataDto> GetBookingDataAsync();
        Task<bool> CreateBookingAsync(CreateBookingDto dto);
        Task<List<HorseInfoDto>> GetAvailableHorsesAsync(DateTime date, string lessonType);
        Task<List<ClientLessonDto>> GetClientSchedule(int clientId, DateTime? startDate = null, DateTime? endDate = null);
    }

    public class EquestrianService : IEquestrianService
    {
        private readonly EquestrianClubContext _context;

        public EquestrianService(EquestrianClubContext context)
        {
            _context = context;
        }

        public async Task<List<CoachScheduleDto>> GetCoachSchedule(int coachId)
        {
            var schedule = await _context.Lessons
                .Include(l => l.IdClientNavigation)
                .Include(l => l.IdArenaNavigation)
                .Include(l => l.IdCoachNavigation)
                    .ThenInclude(c => c.IdEmployeeNavigation)
                .Include(l => l.LessonHorses)
                    .ThenInclude(lh => lh.IdHorseNavigation)
                .Include(l => l.ScheduleArenas)
                .Where(l => l.IdCoach == coachId && l.Date >= DateOnly.FromDateTime(DateTime.Now))
                .OrderBy(l => l.Date)
                .Select(l => new CoachScheduleDto
                {
                    IdLesson = l.IdLesson,
                    Date = l.Date.ToDateTime(TimeOnly.MinValue),
                    LessonType = l.Type,
                    ArenaName = l.IdArenaNavigation != null ? l.IdArenaNavigation.Name : "Не указана",
                    ClientFullName = $"{l.IdClientNavigation.Surname} {l.IdClientNavigation.Name} {l.IdClientNavigation.Patronymic}",
                    ClientLevel = l.IdClientNavigation.LevelOfTraining,
                    StartTime = l.ScheduleArenas.FirstOrDefault() != null ? l.ScheduleArenas.First().StartTime.ToString() : "Не указано",
                    EndTime = l.ScheduleArenas.FirstOrDefault() != null ? l.ScheduleArenas.First().EndTime.ToString() : "Не указано",
                    HorseNames = l.LessonHorses.Select(lh => lh.IdHorseNavigation.Name).ToList()
                })
                .ToListAsync();

            return schedule;
        }
        public async Task<List<ClientLessonDto>> GetClientSchedule(int clientId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Lessons
                .Include(l => l.IdArenaNavigation)
                .Include(l => l.IdCoachNavigation)
                    .ThenInclude(c => c.IdEmployeeNavigation)
                .Include(l => l.LessonHorses)
                    .ThenInclude(lh => lh.IdHorseNavigation)
                .Include(l => l.ScheduleArenas)
                .Where(l => l.IdClient == clientId)
                .OrderBy(l => l.Date)
                .AsQueryable();  // Добавьте это - преобразуем в IQueryable

            if (startDate.HasValue)
                query = query.Where(l => l.Date >= DateOnly.FromDateTime(startDate.Value));

            if (endDate.HasValue)
                query = query.Where(l => l.Date <= DateOnly.FromDateTime(endDate.Value));

            var lessons = await query.Select(l => new ClientLessonDto
            {
                IdLesson = l.IdLesson,
                Date = l.Date.ToDateTime(TimeOnly.MinValue),
                LessonType = l.Type,
                ArenaName = l.IdArenaNavigation != null ? l.IdArenaNavigation.Name : "Не указана",
                CoachName = l.IdCoachNavigation != null && l.IdCoachNavigation.IdEmployeeNavigation != null
                    ? $"{l.IdCoachNavigation.IdEmployeeNavigation.Surname} {l.IdCoachNavigation.IdEmployeeNavigation.Name}"
                    : "Не указан",
                HorseNames = l.LessonHorses.Select(lh => lh.IdHorseNavigation.Name).ToList(),
                StartTime = l.ScheduleArenas.FirstOrDefault() != null
                    ? l.ScheduleArenas.First().StartTime.ToString()
                    : "Не указано",
                EndTime = l.ScheduleArenas.FirstOrDefault() != null
                    ? l.ScheduleArenas.First().EndTime.ToString()
                    : "Не указано"
            }).ToListAsync();

            return lessons;
        }
        public async Task<ClientProfileDto> GetClientProfile(int clientId)
        {
            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.IdClient == clientId);

            if (client == null)
                throw new Exception("Клиент не найден");

            var upcomingLessons = await _context.Lessons
                .Include(l => l.IdArenaNavigation)
                .Include(l => l.IdCoachNavigation)
                    .ThenInclude(c => c.IdEmployeeNavigation)
                .Include(l => l.LessonHorses)
                    .ThenInclude(lh => lh.IdHorseNavigation)
                .Include(l => l.ScheduleArenas)
                .Where(l => l.IdClient == clientId && l.Date >= DateOnly.FromDateTime(DateTime.Now))
                .OrderBy(l => l.Date)
                .Select(l => new ClientLessonDto
                {
                    IdLesson = l.IdLesson,
                    Date = l.Date.ToDateTime(TimeOnly.MinValue),
                    LessonType = l.Type,
                    ArenaName = l.IdArenaNavigation != null ? l.IdArenaNavigation.Name : "Не указана",
                    CoachName = l.IdCoachNavigation != null && l.IdCoachNavigation.IdEmployeeNavigation != null
                        ? $"{l.IdCoachNavigation.IdEmployeeNavigation.Surname} {l.IdCoachNavigation.IdEmployeeNavigation.Name}"
                        : "Не указан",
                    HorseNames = l.LessonHorses.Select(lh => lh.IdHorseNavigation.Name).ToList(),
                    StartTime = l.ScheduleArenas.FirstOrDefault() != null ? l.ScheduleArenas.First().StartTime.ToString() : "Не указано",
                    EndTime = l.ScheduleArenas.FirstOrDefault() != null ? l.ScheduleArenas.First().EndTime.ToString() : "Не указано"
                })
                .ToListAsync();

            var totalLessons = await _context.Lessons.CountAsync(l => l.IdClient == clientId);
            var activeMemberships = await _context.Memberships.CountAsync(m => m.IdClient == clientId && m.Status == "Активен");

            return new ClientProfileDto
            {
                IdClient = client.IdClient,
                FullName = $"{client.Surname} {client.Name} {client.Patronymic}",
                Phone = client.Phone,
                LevelOfTraining = client.LevelOfTraining,
                Balance = client.Balance,
                DateOfRegistration = client.DateOfRegistration.ToDateTime(TimeOnly.MinValue),
                TotalLessons = totalLessons,
                ActiveMemberships = client.Memberships
                    .Where(m => m.Status == "Активен" && m.ValidUntil >= DateOnly.FromDateTime(DateTime.Now))
                    .Select(m => new MembershipDto
                    {
                        Id = m.IdMembership,
                        Type = m.Type,
                        RemainingLessons = m.LessonsTotal,
                        ValidUntil = m.ValidUntil,
                        Status = m.Status
                    }).ToList(),
                UpcomingLessons = upcomingLessons,
                AvailableCompetitions = await GetAvailableCompetitions()
            };
        }

        public async Task<List<CompetitionDto>> GetAvailableCompetitions()
        {
            return await _context.Competitions
                .Where(c => c.Date >= DateOnly.FromDateTime(DateTime.Now) && c.Status == "Запланировано")
                .Select(c => new CompetitionDto
                {
                    IdCompetition = c.IdCompetition,
                    Name = c.Name,
                    Date = c.Date.ToDateTime(TimeOnly.MinValue),
                    Type = c.Type,
                    Level = c.Level,
                    Status = c.Status,
                    RegistrationFee = 1000
                })
                .ToListAsync();
        }

        public async Task<bool> RegisterForLesson(RegisterLessonDto dto)
        {
            var client = await _context.Clients.FindAsync(dto.IdClient);
            if (client == null || client.Balance < 1000)
                return false;

            var lesson = new Lesson
            {
                Date = DateOnly.FromDateTime(dto.Date),
                Type = dto.LessonType,
                IdClient = dto.IdClient,
                IdCoach = dto.IdCoach,
                IdArena = dto.IdArena
            };

            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();

            foreach (var horseId in dto.HorseIds)
            {
                _context.LessonHorses.Add(new LessonHorse
                {
                    IdLesson = lesson.IdLesson,
                    IdHorse = horseId
                });
            }

            _context.ScheduleArenas.Add(new ScheduleArena
            {
                IdArena = dto.IdArena,
                Date = DateOnly.FromDateTime(dto.Date),
                StartTime = new TimeOnly(10, 0),
                EndTime = new TimeOnly(11, 0),
                Status = "Запланировано",
                IdLesson = lesson.IdLesson
            });

            client.Balance -= 1000;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RegisterForCompetition(RegisterCompetitionDto dto)
        {
            var participation = new Participation
            {
                RegistrationDate = DateOnly.FromDateTime(DateTime.Now),
                IdCompetition = dto.IdCompetition,
                IdClient = dto.IdClient,
                IdHorse = dto.IdHorse
            };

            _context.Participations.Add(participation);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<byte[]> PrintSchedule(int clientId)
        {
            var profile = await GetClientProfile(clientId);
            var scheduleText = GenerateScheduleText(profile);
            return Encoding.UTF8.GetBytes(scheduleText);
        }

        public async Task<List<AdminUserDto>> GetAllUsers()
        {
            return await _context.AppUsers
                .Include(u => u.Client)
                .Select(u => new AdminUserDto
                {
                    IdUser = u.IdUser,
                    Login = u.Login,
                    Role = u.Role,
                    Client = u.Client != null ? new ClientInfoDto
                    {
                        IdClient = u.Client.IdClient,
                        Surname = u.Client.Surname,
                        Name = u.Client.Name,
                        Patronymic = u.Client.Patronymic,
                        Phone = u.Client.Phone,
                        Passport = u.Client.Passport,
                        City = u.Client.City,
                        Address = $"{u.Client.Street}, {u.Client.House}{(u.Client.Flat != null ? $", {u.Client.Flat}" : "")}",
                        Balance = u.Client.Balance,
                        DateOfRegistration = u.Client.DateOfRegistration.ToDateTime(TimeOnly.MinValue),
                        LevelOfTraining = u.Client.LevelOfTraining,
                        TotalLessons = _context.Lessons.Count(l => l.IdClient == u.Client.IdClient),
                        TotalSpent = _context.Payments
                            .Where(p => p.IdMembership != null && _context.Memberships.Any(m => m.IdMembership == p.IdMembership && m.IdClient == u.Client.IdClient))
                            .Sum(p => p.Summa)
                    } : null
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateUserRole(int userId, string newRole)
        {
            var user = await _context.AppUsers.FindAsync(userId);
            if (user == null) return false;

            user.Role = newRole;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<byte[]> GetAllUsersReport()
        {
            var users = await GetAllUsers();
            var report = GenerateUsersReport(users);
            return Encoding.UTF8.GetBytes(report);
        }

        private string GenerateScheduleText(ClientProfileDto profile)
        {
            var sb = new StringBuilder();
            sb.AppendLine("РАСПИСАНИЕ ЗАНЯТИЙ");
            sb.AppendLine($"Клиент: {profile.FullName}");
            sb.AppendLine($"Дата: {DateTime.Now:dd.MM.yyyy}");
            sb.AppendLine(new string('-', 50));

            foreach (var lesson in profile.UpcomingLessons)
            {
                sb.AppendLine($"Дата: {lesson.Date:dd.MM.yyyy}");
                sb.AppendLine($"Тип: {lesson.LessonType}");
                sb.AppendLine($"Арена: {lesson.ArenaName}");
                sb.AppendLine($"Тренер: {lesson.CoachName}");
                sb.AppendLine($"Время: {lesson.StartTime} - {lesson.EndTime}");
                sb.AppendLine($"Лошади: {string.Join(", ", lesson.HorseNames)}");
                sb.AppendLine(new string('-', 50));
            }

            return sb.ToString();
        }

        private string GenerateUsersReport(List<AdminUserDto> users)
        {
            var sb = new StringBuilder();
            sb.AppendLine("ОТЧЕТ ПО ПОЛЬЗОВАТЕЛЯМ");
            sb.AppendLine($"Дата: {DateTime.Now:dd.MM.yyyy HH:mm}");
            sb.AppendLine(new string('=', 100));

            foreach (var user in users)
            {
                sb.AppendLine($"Логин: {user.Login}");
                sb.AppendLine($"Роль: {user.Role}");
                if (user.Client != null)
                {
                    sb.AppendLine($"Клиент: {user.Client.Surname} {user.Client.Name} {user.Client.Patronymic}");
                    sb.AppendLine($"Телефон: {user.Client.Phone}");
                    sb.AppendLine($"Уровень: {user.Client.LevelOfTraining}");
                    sb.AppendLine($"Баланс: {user.Client.Balance} руб.");
                    sb.AppendLine($"Занятий: {user.Client.TotalLessons}");
                    sb.AppendLine($"Потрачено: {user.Client.TotalSpent} руб.");
                }
                sb.AppendLine(new string('-', 100));
            }

            return sb.ToString();
        }

        public async Task<Employee> CreateEmployee(CreateEmployeeDto dto)
        {
            var employee = new Employee
            {
                Surname = dto.Surname,
                Name = dto.Name,
                Patronymic = dto.Patronymic,
                DateOfBirth = DateOnly.FromDateTime(dto.DateOfBirth),
                City = dto.City,
                Street = dto.Street,
                HouseNumber = dto.HouseNumber,
                FlatNumber = dto.FlatNumber,
                Post = dto.Post,
                Phone = dto.Phone
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Client> CreateClient(CreateClientDto dto)
        {
            var client = new Client
            {
                Surname = dto.Surname,
                Name = dto.Name,
                Patronymic = dto.Patronymic,
                DateOfBirth = DateOnly.FromDateTime(dto.DateOfBirth),
                Phone = dto.Phone,
                LevelOfTraining = dto.LevelOfTraining,
                Passport = dto.Passport,
                City = dto.City,
                Street = dto.Street,
                House = dto.House,
                Flat = dto.Flat,
                Balance = 0,
                DateOfRegistration = DateOnly.FromDateTime(DateTime.Now)
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            var user = new AppUser
            {
                Login = dto.Login,
                Password = dto.Password,
                Role = "Client",
                IdClient = client.IdClient
            };

            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();

            return client;
        }

        public async Task<Horse> CreateHorse(CreateHorseDto dto)
        {
            var horse = new Horse
            {
                Name = dto.Name,
                Gender = dto.Gender,
                Breed = dto.Breed,
                DateOfBirth = DateOnly.FromDateTime(dto.DateOfBirth),
                StateOfHealth = dto.StateOfHealth,
                LevelOfTraining = dto.LevelOfTraining,
                Passport = dto.Passport,
                Status = dto.Status,
                IdStall = dto.IdStall
            };

            _context.Horses.Add(horse);
            await _context.SaveChangesAsync();
            return horse;
        }

        public async Task<BookingDataDto> GetBookingDataAsync()
        {
            var coaches = await _context.Coaches
                .Include(c => c.IdEmployeeNavigation)
                .Select(c => new CoachInfoDto
                {
                    IdCoach = c.IdCoach,
                    FullName = $"{c.IdEmployeeNavigation.Surname} {c.IdEmployeeNavigation.Name} {c.IdEmployeeNavigation.Patronymic}",
                    Specialization = c.Specialization
                })
                .ToListAsync();

            var arenas = await _context.Arenas
                .Where(a => a.Status == "Доступен")
                .Select(a => new ArenaInfoDto
                {
                    IdArena = a.IdArena,
                    Name = a.Name,
                    Type = a.Type
                })
                .ToListAsync();

            var horses = await _context.Horses
                .Where(h => h.Status == "В работе" && h.StateOfHealth == "Здорова")
                .Select(h => new HorseInfoDto
                {
                    IdHorse = h.IdHorse,
                    Name = h.Name,
                    Breed = h.Breed,
                    LevelOfTraining = h.LevelOfTraining
                })
                .ToListAsync();

            return new BookingDataDto
            {
                Coaches = coaches,
                Arenas = arenas,
                AvailableHorses = horses
            };
        }

        public async Task<List<HorseInfoDto>> GetAvailableHorsesAsync(DateTime date, string lessonType)
        {
            var query = _context.Horses
                .Where(h => h.Status == "В работе" && h.StateOfHealth == "Здорова");

            if (lessonType == "Индивидуальное" || lessonType == "Тренировка")
            {
                query = query.Where(h => h.LevelOfTraining == "Спортивный" || h.LevelOfTraining == "Профессиональный");
            }
            else if (lessonType == "Прогулка")
            {
                query = query.Where(h => h.LevelOfTraining == "Начинающий" || h.LevelOfTraining == "Продвинутый");
            }

            return await query
                .Select(h => new HorseInfoDto
                {
                    IdHorse = h.IdHorse,
                    Name = h.Name,
                    Breed = h.Breed,
                    LevelOfTraining = h.LevelOfTraining
                })
                .ToListAsync();
        }

        public async Task<bool> CreateBookingAsync(CreateBookingDto dto)
        {
            var client = await _context.Clients.FindAsync(dto.IdClient);
            if (client == null || client.Balance < 1500)
                return false;

            var existingLesson = await _context.Lessons
                .FirstOrDefaultAsync(l => l.IdCoach == dto.IdCoach && l.Date == DateOnly.FromDateTime(dto.Date));

            if (existingLesson != null)
                return false;

            var lesson = new Lesson
            {
                Date = DateOnly.FromDateTime(dto.Date),
                Type = dto.LessonType,
                IdClient = dto.IdClient,
                IdCoach = dto.IdCoach,
                IdArena = dto.IdArena
            };

            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();

            int horseId = dto.IdHorse ?? await GetBestHorseForLesson(dto.LessonType);

            _context.LessonHorses.Add(new LessonHorse
            {
                IdLesson = lesson.IdLesson,
                IdHorse = horseId
            });

            _context.ScheduleArenas.Add(new ScheduleArena
            {
                IdArena = dto.IdArena,
                Date = DateOnly.FromDateTime(dto.Date),
                StartTime = new TimeOnly(10, 0),
                EndTime = new TimeOnly(11, 0),
                Status = "Запланировано",
                IdLesson = lesson.IdLesson
            });

            client.Balance -= 1500;
            await _context.SaveChangesAsync();

            return true;
        }

        private async Task<int> GetBestHorseForLesson(string lessonType)
        {
            var horse = await _context.Horses
                .Where(h => h.Status == "В работе" && h.StateOfHealth == "Здорова")
                .FirstOrDefaultAsync();

            return horse?.IdHorse ?? 0;
        }
    }
}