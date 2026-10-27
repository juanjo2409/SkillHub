using SkillHub.Application.DTOs.Enrollments;
using SkillHub.Application.DTOs.Workshops;

namespace SkillHub.Application.Interface;

public interface IExcelImportParser
{
    IReadOnlyList<ImportEnrollmentRowRequestDto> ParseEnrollmentRows(Stream excelStream);
    byte[] ExportWorkshopEnrollmentsToExcel(WorkshopResponseDto workshop, IReadOnlyList<EnrollmentResponseDto> enrollments);
}
