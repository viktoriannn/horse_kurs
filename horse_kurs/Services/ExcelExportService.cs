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

        public async Task<byte[]> GetExcelAsync()
        {
            var employees = await _context.Employees.ToListAsync();
            var clients = await _context.Clients.ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var empSheet = workbook.Worksheets.Add("Сотрудники");
                empSheet.Cell(1, 1).Value = "Фамилия";
                empSheet.Cell(1, 2).Value = "Имя";
                empSheet.Cell(1, 3).Value = "Должность";

                for (int i = 0; i < employees.Count; i++)
                {
                    empSheet.Cell(i + 2, 1).Value = employees[i].Surname;
                    empSheet.Cell(i + 2, 2).Value = employees[i].Name;
                    empSheet.Cell(i + 2, 3).Value = employees[i].Post;
                }

                var clientSheet = workbook.Worksheets.Add("Клиенты");
                clientSheet.Cell(1, 1).Value = "Фамилия";
                clientSheet.Cell(1, 2).Value = "Имя";
                clientSheet.Cell(1, 3).Value = "Баланс";

                for (int i = 0; i < clients.Count; i++)
                {
                    clientSheet.Cell(i + 2, 1).Value = clients[i].Surname;
                    clientSheet.Cell(i + 2, 2).Value = clients[i].Name;
                    clientSheet.Cell(i + 2, 3).Value = (double)clients[i].Balance;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}