using SkillHub.Application.DTOs.Enrollments;
using SkillHub.Application.DTOs.Workshops;
using SkillHub.Application.Interface;
using SkillHub.Domain.Entities;
using SkillHub.Domain.Exceptions;
using SkillHub.Domain.ValueObjects;

namespace SkillHub.Application.Service;

public class WorkshopService : IWorkshopService
{
    private readonly IWorkshopRepository _workshopRepository;
    private readonly IInstructorRepository _instructorRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IExcelImportParser _excelImportParser;

    public WorkshopService(
        IWorkshopRepository workshopRepository,
        IInstructorRepository instructorRepository,
        IEnrollmentRepository enrollmentRepository,
        IExcelImportParser excelImportParser)
    {
        _workshopRepository = workshopRepository;
        _instructorRepository = instructorRepository;
        _enrollmentRepository = enrollmentRepository;
        _excelImportParser = excelImportParser;
    }

    public async Task<IReadOnlyList<WorkshopResponseDto>> GetUpcomingCatalogAsync(CancellationToken cancellationToken = default)
    {
        var workshops = await _workshopRepository.GetUpcomingCatalogAsync(cancellationToken);

        return workshops
            .Where(w => !w.HasStarted())
            .Select(MapToResponseDto)
            .ToList();
    }

    public async Task<WorkshopResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var workshop = await _workshopRepository.GetByIdWithDetailsAsync(id, cancellationToken)
            ?? throw new WorkshopNotFoundException();

        return MapToResponseDto(workshop);
    }

    public async Task<WorkshopResponseDto> CreateAsync(CreateWorkshopRequestDto request, CancellationToken cancellationToken = default)
    {
        var instructorEmail = new Email(request.InstructorEmail);
        var instructor = await _instructorRepository.GetByEmailAsync(instructorEmail.Value, cancellationToken)
            ?? throw new InstructorNotFoundException();

        var workshopCode = new WorkshopCode(request.Code);
        if (await _workshopRepository.ExistsByCodeAsync(workshopCode.Value, cancellationToken))
            throw new DuplicateWorkshopCodeException();

        var schedule = new DateTimeRange(request.StartDate, request.EndDate);
        var workshop = new Workshop(
            workshopCode,
            request.Title,
            request.Description,
            schedule,
            request.MaxCapacity,
            instructor.Id);

        await _workshopRepository.AddAsync(workshop, cancellationToken);
        await _workshopRepository.SaveChangesAsync(cancellationToken);

        var created = await _workshopRepository.GetByIdWithDetailsAsync(workshop.Id, cancellationToken) ?? workshop;
        return MapToResponseDto(created, instructor);
    }

    public async Task<IReadOnlyList<EnrollmentResponseDto>> GetWorkshopEnrollmentsAsync(Guid workshopId, CancellationToken cancellationToken = default)
    {
        var workshop = await _workshopRepository.GetByIdWithDetailsAsync(workshopId, cancellationToken)
            ?? throw new WorkshopNotFoundException();

        var enrollments = await _enrollmentRepository.GetByWorkshopIdAsync(workshop.Id, cancellationToken);

        return enrollments.Select(e => new EnrollmentResponseDto(
            e.Id,
            workshop.Id,
            workshop.Code.Value,
            workshop.Title,
            workshop.Instructor?.FullName ?? string.Empty,
            workshop.Schedule.StartDate,
            workshop.Schedule.EndDate,
            e.ParticipantId,
            e.Participant?.FullName ?? string.Empty,
            e.Participant?.Email.Value ?? string.Empty,
            e.EnrollmentDate,
            e.Status.ToString(),
            e.CancelledAt,
            e.CancelledByUserId)).ToList();
    }

    public async Task<(byte[] FileBytes, string FileName)> ExportWorkshopEnrollmentsExcelAsync(
        Guid workshopId,
        CancellationToken cancellationToken = default)
    {
        var workshopDto = await GetByIdAsync(workshopId, cancellationToken);
        var enrollments = await GetWorkshopEnrollmentsAsync(workshopId, cancellationToken);

        var bytes = _excelImportParser.ExportWorkshopEnrollmentsToExcel(workshopDto, enrollments);
        var fileName = $"Inscritos_{workshopDto.Code}_{DateTime.UtcNow:yyyyMMdd}.xlsx";

        return (bytes, fileName);
    }

    public async Task<IReadOnlyList<TopWorkshopResponseDto>> GetTopWorkshopsAsync(CancellationToken cancellationToken = default)
    {
        var workshops = await _workshopRepository.GetTopByActiveEnrollmentsAsync(cancellationToken);

        return workshops
            .OrderByDescending(w => w.GetActiveEnrollmentsCount())
            .Select(w => new TopWorkshopResponseDto(
                w.Id,
                w.Code.Value,
                w.Title,
                w.Instructor?.FullName ?? string.Empty,
                w.Schedule.StartDate,
                w.Schedule.EndDate,
                w.MaxCapacity,
                w.GetActiveEnrollmentsCount(),
                w.GetAvailableSpots()))
            .ToList();
    }

    private static WorkshopResponseDto MapToResponseDto(Workshop workshop) =>
        MapToResponseDto(workshop, workshop.Instructor);

    private static WorkshopResponseDto MapToResponseDto(Workshop workshop, Instructor? instructor) =>
        new(
            workshop.Id,
            workshop.Code.Value,
            workshop.Title,
            workshop.Description,
            instructor?.FullName ?? string.Empty,
            instructor?.Email.Value ?? string.Empty,
            instructor?.Specialty ?? string.Empty,
            workshop.Schedule.StartDate,
            workshop.Schedule.EndDate,
            workshop.MaxCapacity,
            workshop.GetActiveEnrollmentsCount(),
            workshop.GetAvailableSpots());
}
