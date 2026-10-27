using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SkillHub.Application.DTOs.Enrollments;
using SkillHub.Application.DTOs.Workshops;
using SkillHub.Application.Interface;

namespace SkillHub.Infrastructure.Persistence.Repositories;

public class EpplusExcelImportParser : IExcelImportParser
{
    public IReadOnlyList<ImportEnrollmentRowRequestDto> ParseEnrollmentRows(Stream excelStream)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage(excelStream);
        var worksheet = package.Workbook.Worksheets["Enrollments"]
            ?? package.Workbook.Worksheets.FirstOrDefault();

        if (worksheet?.Dimension is null)
            return Array.Empty<ImportEnrollmentRowRequestDto>();

        var rows = new List<ImportEnrollmentRowRequestDto>();
        var rowCount = worksheet.Dimension.Rows;

        for (var row = 2; row <= rowCount; row++)
        {
            var participantEmail = worksheet.Cells[row, 1].Text?.Trim() ?? string.Empty;
            var workshopCode = worksheet.Cells[row, 2].Text?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(participantEmail) && string.IsNullOrWhiteSpace(workshopCode))
                continue;

            rows.Add(new ImportEnrollmentRowRequestDto(row, participantEmail, workshopCode));
        }

        return rows;
    }

    public byte[] ExportWorkshopEnrollmentsToExcel(
        WorkshopResponseDto workshop,
        IReadOnlyList<EnrollmentResponseDto> enrollments)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var sheet = package.Workbook.Worksheets.Add($"Inscritos_{workshop.Code}");

        // Workshop Header Info
        sheet.Cells["A1"].Value = $"SkillHub — Listado Oficial de Inscritos ({workshop.Code})";
        sheet.Cells["A1:E1"].Merge = true;
        sheet.Cells["A1"].Style.Font.Bold = true;
        sheet.Cells["A1"].Style.Font.Size = 14;
        sheet.Cells["A1"].Style.Font.Color.SetColor(Color.White);
        sheet.Cells["A1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
        sheet.Cells["A1"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#00324D"));

        sheet.Cells["A2"].Value = $"Taller: {workshop.Title} | Instructor: {workshop.InstructorName} ({workshop.InstructorSpecialty})";
        sheet.Cells["A2:E2"].Merge = true;
        sheet.Cells["A2"].Style.Font.Bold = true;

        sheet.Cells["A3"].Value = $"Horario: {workshop.StartDate:yyyy-MM-dd HH:mm} a {workshop.EndDate:yyyy-MM-dd HH:mm} UTC | Inscritos Activos: {workshop.ActiveEnrollmentsCount} / {workshop.MaxCapacity}";
        sheet.Cells["A3:E3"].Merge = true;

        // Table Headers
        string[] headers = ["#", "Participant Full Name", "Participant Email", "Status", "Enrollment Date (UTC)"];
        for (var col = 0; col < headers.Length; col++)
        {
            var cell = sheet.Cells[5, col + 1];
            cell.Value = headers[col];
            cell.Style.Font.Bold = true;
            cell.Style.Font.Color.SetColor(Color.White);
            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#39A900"));
        }

        var rowIdx = 6;
        var index = 1;
        foreach (var item in enrollments)
        {
            sheet.Cells[rowIdx, 1].Value = index++;
            sheet.Cells[rowIdx, 2].Value = item.ParticipantName;
            sheet.Cells[rowIdx, 3].Value = item.ParticipantEmail;
            sheet.Cells[rowIdx, 4].Value = item.Status;
            sheet.Cells[rowIdx, 5].Value = item.EnrollmentDate.ToString("yyyy-MM-dd HH:mm");
            rowIdx++;
        }

        if (sheet.Dimension is not null)
            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

        return package.GetAsByteArray();
    }
}
