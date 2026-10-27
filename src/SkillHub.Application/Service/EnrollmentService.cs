using SkillHub.Application.DTOs.Enrollments;
using SkillHub.Application.Interface;
using SkillHub.Domain.Entities;
using SkillHub.Domain.Enums;
using SkillHub.Domain.Errors;
using SkillHub.Domain.Exceptions;
using SkillHub.Domain.ValueObjects;

namespace SkillHub.Application.Service;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IWorkshopRepository _workshopRepository;
    private readonly IParticipantRepository _participantRepository;
    private readonly ICertificatePdfGenerator _certificatePdfGenerator;
    private readonly IExcelImportParser _excelImportParser;

    public EnrollmentService(
        IEnrollmentRepository enrollmentRepository,
        IWorkshopRepository workshopRepository,
        IParticipantRepository participantRepository,
        ICertificatePdfGenerator certificatePdfGenerator,
        IExcelImportParser excelImportParser)
    {
        _enrollmentRepository = enrollmentRepository;
        _workshopRepository = workshopRepository;
        _participantRepository = participantRepository;
        _certificatePdfGenerator = certificatePdfGenerator;
        _excelImportParser = excelImportParser;
    }

    public async Task<EnrollmentResponseDto> EnrollParticipantAsync(
        string identityUserId,
        CreateEnrollmentRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var participant = await _participantRepository.GetByIdentityUserIdAsync(identityUserId, cancellationToken)
            ?? throw new ParticipantNotFoundException();

        var workshop = await _workshopRepository.GetByIdWithDetailsAsync(request.WorkshopId, cancellationToken)
            ?? throw new WorkshopNotFoundException();

        var enrollment = await ExecuteEnrollmentAsync(participant, workshop, cancellationToken);
        return MapToResponseDto(enrollment, workshop, participant);
    }

    public async Task<IReadOnlyList<EnrollmentResponseDto>> GetMyEnrollmentsAsync(
        string identityUserId,
        CancellationToken cancellationToken = default)
    {
        var participant = await _participantRepository.GetByIdentityUserIdAsync(identityUserId, cancellationToken)
            ?? throw new ParticipantNotFoundException();

        var enrollments = await _enrollmentRepository.GetByParticipantIdAsync(participant.Id, cancellationToken);
        return enrollments.Select(e => MapToResponseDto(e, e.Workshop, participant)).ToList();
    }

    public async Task<EnrollmentResponseDto> CancelEnrollmentAsync(
        Guid enrollmentId,
        string currentUserId,
        bool isEmployee,
        CancellationToken cancellationToken = default)
    {
        var enrollment = await _enrollmentRepository.GetByIdWithDetailsAsync(enrollmentId, cancellationToken)
            ?? throw new EnrollmentNotFoundException();

        if (isEmployee)
        {
            enrollment.CancelByEmployee(currentUserId, enrollment.Workshop.Schedule.StartDate);
        }
        else
        {
            if (enrollment.Participant.IdentityUserId != currentUserId)
                throw new UnauthorizedEnrollmentAccessException();

            enrollment.CancelByParticipant(currentUserId, enrollment.Workshop.Schedule.StartDate);
        }

        await _enrollmentRepository.SaveChangesAsync(cancellationToken);
        return MapToResponseDto(enrollment, enrollment.Workshop, enrollment.Participant);
    }

    public async Task<ImportEnrollmentResponseDto> ImportFromExcelAsync(
        Stream excelStream,
        CancellationToken cancellationToken = default)
    {
        var rows = _excelImportParser.ParseEnrollmentRows(excelStream);
        if (rows.Count == 0)
            throw new DomainValidationException(EnrollmentErrors.EmptyImportFile);

        var totalEnrolled = 0;
        var rejectedRows = new List<RejectedEnrollmentRowResponseDto>();

        foreach (var row in rows)
        {
            try
            {
                var email = new Email(row.ParticipantEmail);
                var code = new WorkshopCode(row.WorkshopCode);

                var participant = await _participantRepository.GetByEmailAsync(email.Value, cancellationToken)
                    ?? throw new ParticipantNotFoundException();

                var workshop = await _workshopRepository.GetByCodeWithDetailsAsync(code.Value, cancellationToken)
                    ?? throw new WorkshopNotFoundException();

                await ExecuteEnrollmentAsync(participant, workshop, cancellationToken);
                totalEnrolled++;
            }
            catch (DomainException ex)
            {
                rejectedRows.Add(new RejectedEnrollmentRowResponseDto(
                    row.RowNumber,
                    row.ParticipantEmail,
                    row.WorkshopCode,
                    ex.Message));
            }
        }

        return new ImportEnrollmentResponseDto(totalEnrolled, rejectedRows);
    }

    public async Task<byte[]> GenerateCertificatePdfAsync(
        Guid enrollmentId,
        string currentUserId,
        bool isEmployee,
        CancellationToken cancellationToken = default)
    {
        var enrollment = await _enrollmentRepository.GetByIdWithDetailsAsync(enrollmentId, cancellationToken)
            ?? throw new EnrollmentNotFoundException();

        if (!isEmployee && enrollment.Participant.IdentityUserId != currentUserId)
            throw new UnauthorizedEnrollmentAccessException();

        enrollment.EnsureCanGenerateCertificate();

        var certificateData = new CertificateDataResponseDto(
            OrganizationName: "SkillHub.Api",
            DocumentTitle: "Constancia de inscripción",
            ParticipantName: enrollment.Participant.FullName,
            ParticipantEmail: enrollment.Participant.Email.Value,
            WorkshopCode: enrollment.Workshop.Code.Value,
            WorkshopTitle: enrollment.Workshop.Title,
            InstructorName: enrollment.Workshop.Instructor?.FullName ?? string.Empty,
            StartDate: enrollment.Workshop.Schedule.StartDate,
            EndDate: enrollment.Workshop.Schedule.EndDate,
            EnrollmentDate: enrollment.EnrollmentDate,
            GeneratedAt: DateTime.UtcNow);

        return _certificatePdfGenerator.GenerateCertificatePdf(certificateData);
    }

    private async Task<Enrollment> ExecuteEnrollmentAsync(
        Participant participant,
        Workshop workshop,
        CancellationToken cancellationToken)
    {
        var existingCancelled = workshop.Enrollments
            .Any(e => e.ParticipantId == participant.Id && e.Status == EnrollmentStatus.Cancelled);

        var activeEnrollments = await _enrollmentRepository.GetActiveByParticipantIdAsync(participant.Id, cancellationToken);
        var activeSchedules = activeEnrollments
            .Where(e => e.WorkshopId != workshop.Id && e.Workshop != null)
            .Select(e => e.Workshop.Schedule)
            .ToList();

        var enrollment = workshop.EnrollParticipant(participant.Id, activeSchedules);

        if (!existingCancelled)
            await _enrollmentRepository.AddAsync(enrollment, cancellationToken);

        await _enrollmentRepository.SaveChangesAsync(cancellationToken);
        return enrollment;
    }

    private static EnrollmentResponseDto MapToResponseDto(
        Enrollment enrollment,
        Workshop workshop,
        Participant participant) =>
        new(
            enrollment.Id,
            workshop.Id,
            workshop.Code.Value,
            workshop.Title,
            workshop.Instructor?.FullName ?? string.Empty,
            workshop.Schedule.StartDate,
            workshop.Schedule.EndDate,
            participant.Id,
            participant.FullName,
            participant.Email.Value,
            enrollment.EnrollmentDate,
            enrollment.Status.ToString(),
            enrollment.CancelledAt,
            enrollment.CancelledByUserId);
}
