using SkillHub.Application.DTOs.Enrollments;

namespace SkillHub.Application.Interface;

public interface ICertificatePdfGenerator
{
    byte[] GenerateCertificatePdf(CertificateDataResponseDto data);
}
