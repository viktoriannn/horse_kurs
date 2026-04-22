using ClosedXML.Excel;
using horse_kurs.Models;
using Microsoft.EntityFrameworkCore;

namespace horse_kurs.Services
{
    public class ExcelExportService
    {
        private readonly EquestrianClubContext _context;

        public ExcelExportService(EquestrianClubContext context)
        {
            _context = context;
        }

        public async Task<byte[]> GetFullReportExcelAsync()
        {
            using (var workbook = new XLWorkbook())
            {
                // 1. Лист с пользователями
                await CreateUsersSheet(workbook);

                // 2. Лист с клиентами
                await CreateClientsSheet(workbook);

                // 3. Лист с сотрудниками
                await CreateEmployeesSheet(workbook);

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        private async Task CreateUsersSheet(XLWorkbook workbook)
        {
            var users = await _context.AppUsers
                .Include(u => u.Client)
                .ToListAsync();

            var sheet = workbook.Worksheets.Add("Пользователи");

            sheet.Cell(1, 1).Value = "ID";
            sheet.Cell(1, 2).Value = "Логин";
            sheet.Cell(1, 3).Value = "Роль";
            sheet.Cell(1, 4).Value = "Фамилия";
            sheet.Cell(1, 5).Value = "Имя";
            sheet.Cell(1, 6).Value = "Телефон";
            sheet.Cell(1, 7).Value = "Баланс";

            var headerRange = sheet.Range(1, 1, 1, 7);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

            int row = 2;
            foreach (var user in users)
            {
                sheet.Cell(row, 1).Value = user.IdUser;
                sheet.Cell(row, 2).Value = user.Login;
                sheet.Cell(row, 3).Value = user.Role;

                if (user.Client != null)
                {
                    sheet.Cell(row, 4).Value = user.Client.Surname;
                    sheet.Cell(row, 5).Value = user.Client.Name;
                    sheet.Cell(row, 6).Value = user.Client.Phone;
                    sheet.Cell(row, 7).Value = (double)user.Client.Balance;
                }
                else
                {
                    sheet.Cell(row, 4).Value = "—";
                    sheet.Cell(row, 5).Value = "—";
                    sheet.Cell(row, 6).Value = "—";
                    sheet.Cell(row, 7).Value = 0;
                }
                row++;
            }

            sheet.Columns().AdjustToContents();
        }

        private async Task CreateClientsSheet(XLWorkbook workbook)
        {
            var clients = await _context.Clients.ToListAsync();

            var sheet = workbook.Worksheets.Add("Клиенты");

            sheet.Cell(1, 1).Value = "ID";
            sheet.Cell(1, 2).Value = "Фамилия";
            sheet.Cell(1, 3).Value = "Имя";
            sheet.Cell(1, 4).Value = "Телефон";
            sheet.Cell(1, 5).Value = "Уровень подготовки";
            sheet.Cell(1, 6).Value = "Баланс";
            sheet.Cell(1, 7).Value = "Город";

            var headerRange = sheet.Range(1, 1, 1, 7);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

            int row = 2;
            foreach (var client in clients)
            {
                sheet.Cell(row, 1).Value = client.IdClient;
                sheet.Cell(row, 2).Value = client.Surname;
                sheet.Cell(row, 3).Value = client.Name;
                sheet.Cell(row, 4).Value = client.Phone;
                sheet.Cell(row, 5).Value = client.LevelOfTraining;
                sheet.Cell(row, 6).Value = (double)client.Balance;
                sheet.Cell(row, 7).Value = client.City;
                row++;
            }

            sheet.Columns().AdjustToContents();
        }

        private async Task CreateEmployeesSheet(XLWorkbook workbook)
        {
            var employees = await _context.Employees.ToListAsync();

            var sheet = workbook.Worksheets.Add("Сотрудники");

            sheet.Cell(1, 1).Value = "ID";
            sheet.Cell(1, 2).Value = "Фамилия";
            sheet.Cell(1, 3).Value = "Имя";
            sheet.Cell(1, 4).Value = "Должность";
            sheet.Cell(1, 5).Value = "Телефон";

            var headerRange = sheet.Range(1, 1, 1, 5);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

            int row = 2;
            foreach (var emp in employees)
            {
                sheet.Cell(row, 1).Value = emp.IdEmployee;
                sheet.Cell(row, 2).Value = emp.Surname;
                sheet.Cell(row, 3).Value = emp.Name;
                sheet.Cell(row, 4).Value = emp.Post;
                sheet.Cell(row, 5).Value = emp.Phone ?? "";
                row++;
            }

            sheet.Columns().AdjustToContents();
        }
    }
}