using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SkillHub.Application.DTOs.Enrollments;
using SkillHub.Application.Interface;

namespace SkillHub.Infrastructure.Persistence.Repositories;

public class QuestPdfCertificateGenerator : ICertificatePdfGenerator
{
    private const string AcademicLogoSvg = """
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100">
            <rect width="100" height="100" rx="20" fill="#00324D"/>
            <rect x="6" y="6" width="88" height="88" rx="16" fill="none" stroke="#39A900" stroke-width="3"/>
            <polygon points="50,22 16,38 50,54 84,38" fill="#39A900"/>
            <path d="M28,46 L28,66 C28,74 72,74 72,66 L72,46 L50,57 Z" fill="#FFFFFF"/>
            <rect x="80" y="38" width="4" height="24" rx="2" fill="#39A900"/>
            <circle cx="82" cy="64" r="4" fill="#39A900"/>
        </svg>
        """;

    public byte[] GenerateCertificatePdf(CertificateDataResponseDto data)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(28);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(11).FontColor("#1E293B"));

                page.Content().Border(2.5f).BorderColor("#00324D").Padding(6)
                    .Border(1f).BorderColor("#39A900").Padding(28)
                    .Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.ConstantItem(64).Height(64).Svg(AcademicLogoSvg);

                            row.RelativeItem().PaddingLeft(16).Column(headerCol =>
                            {
                                headerCol.Item().Text(data.OrganizationName.ToUpperInvariant())
                                    .FontSize(22).Bold().FontColor("#00324D").LetterSpacing(0.08f);

                                headerCol.Item().Text("CENTRO DE FORMACIÓN Y CAPACITACIÓN PRESENCIAL")
                                    .FontSize(9).Bold().FontColor("#39A900").LetterSpacing(0.06f);

                                headerCol.Item().PaddingTop(2).Text("Registro Oficial de Oferta Educativa")
                                    .FontSize(9).FontColor("#64748B");
                            });

                            row.ConstantItem(135).AlignRight().Column(badgeCol =>
                            {
                                badgeCol.Item().Background("#EBF7E5").Border(1).BorderColor("#39A900")
                                    .PaddingHorizontal(10).PaddingVertical(6).AlignCenter()
                                    .Text("ESTADO: ACTIVA").FontSize(9).Bold().FontColor("#2D8600");

                                badgeCol.Item().PaddingTop(4).AlignRight()
                                    .Text($"Código: {data.WorkshopCode}")
                                    .FontSize(9).SemiBold().FontColor("#00324D");
                            });
                        });

                        col.Item().PaddingVertical(16).LineHorizontal(2f).LineColor("#39A900");

                        col.Item().PaddingBottom(16).AlignCenter().Column(titleCol =>
                        {
                            titleCol.Item().AlignCenter().Text(data.DocumentTitle.ToUpperInvariant())
                                .FontSize(20).Bold().FontColor("#00324D").LetterSpacing(0.05f);

                            titleCol.Item().PaddingTop(4).AlignCenter()
                                .Text("Documento oficial que acredita la reserva de cupo y matrícula activa")
                                .FontSize(10).Italic().FontColor("#64748B");
                        });

                        col.Item().PaddingBottom(18).Background("#F4F7F9").Border(1).BorderColor("#CBD5E1")
                            .Padding(18).Column(studentCol =>
                            {
                                studentCol.Item().AlignCenter()
                                    .Text("El Centro de Formación SkillHub hace constar que el(la) participante:")
                                    .FontSize(10.5f).FontColor("#475569");

                                studentCol.Item().PaddingTop(8).AlignCenter()
                                    .Text(data.ParticipantName.ToUpperInvariant())
                                    .FontSize(18).Bold().FontColor("#00324D");

                                studentCol.Item().PaddingTop(4).AlignCenter()
                                    .Text($"Correo electrónico registrado: {data.ParticipantEmail}")
                                    .FontSize(10.5f).Bold().FontColor("#2D8600");
                            });

                        col.Item().PaddingBottom(8).Text("INFORMACIÓN DEL TALLER PRESENCIAL INSCRITO")
                            .FontSize(10).Bold().FontColor("#00324D").LetterSpacing(0.05f);

                        col.Item().Border(1).BorderColor("#CBD5E1").Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(165);
                                columns.RelativeColumn();
                            });

                            static IContainer LabelCell(IContainer c) =>
                                c.Background("#00324D").BorderBottom(0.5f).BorderColor("#FFFFFF")
                                 .PaddingVertical(9).PaddingHorizontal(12);

                            static IContainer ValueCell(IContainer c) =>
                                c.Background("#FFFFFF").BorderBottom(0.5f).BorderColor("#CBD5E1")
                                 .PaddingVertical(9).PaddingHorizontal(12);

                            table.Cell().Element(LabelCell).Text("Centro de Formación").FontSize(10).SemiBold().FontColor("#FFFFFF");
                            table.Cell().Element(ValueCell).Text(data.OrganizationName).FontSize(10).Bold().FontColor("#00324D");

                            table.Cell().Element(LabelCell).Text("Código del Taller").FontSize(10).SemiBold().FontColor("#FFFFFF");
                            table.Cell().Element(ValueCell).Text(data.WorkshopCode).FontSize(10).Bold().FontColor("#2D8600");

                            table.Cell().Element(LabelCell).Text("Título del Taller").FontSize(10).SemiBold().FontColor("#FFFFFF");
                            table.Cell().Element(ValueCell).Text(data.WorkshopTitle).FontSize(10).Bold().FontColor("#00324D");

                            table.Cell().Element(LabelCell).Text("Instructor Asignado").FontSize(10).SemiBold().FontColor("#FFFFFF");
                            table.Cell().Element(ValueCell).Text(data.InstructorName).FontSize(10).FontColor("#1E293B");

                            table.Cell().Element(LabelCell).Text("Fecha del Taller").FontSize(10).SemiBold().FontColor("#FFFFFF");
                            table.Cell().Element(ValueCell).Text(data.StartDate.ToString("yyyy-MM-dd")).FontSize(10).FontColor("#1E293B");

                            table.Cell().Element(LabelCell).Text("Horario de la Sesión").FontSize(10).SemiBold().FontColor("#FFFFFF");
                            table.Cell().Element(ValueCell).Text($"{data.StartDate:HH:mm} a {data.EndDate:HH:mm} UTC").FontSize(10).FontColor("#1E293B");

                            table.Cell().Element(LabelCell).Text("Fecha de Inscripción").FontSize(10).SemiBold().FontColor("#FFFFFF");
                            table.Cell().Element(ValueCell).Text($"{data.EnrollmentDate:yyyy-MM-dd HH:mm} UTC").FontSize(10).FontColor("#1E293B");
                        });

                        col.Item().PaddingTop(14).Background("#EBF7E5").Border(1).BorderColor("#39A900")
                            .Padding(12).Text(text =>
                            {
                                text.Span("Verificación Oficial: ").FontSize(9.5f).Bold().FontColor("#00324D");
                                text.Span("Esta constancia certifica que el participante cuenta con un cupo activo y validado en la plataforma SkillHub. Presente este documento al ingreso del taller presencial.")
                                    .FontSize(9.5f).FontColor("#00324D");
                            });

                        col.Item().ExtendVertical();

                        col.Item().PaddingTop(24).Row(sigRow =>
                        {
                            sigRow.RelativeItem().Column(leftSig =>
                            {
                                leftSig.Item().PaddingHorizontal(16).LineHorizontal(1).LineColor("#00324D");
                                leftSig.Item().PaddingTop(4).AlignCenter().Text("Coordinación Académica SkillHub")
                                    .FontSize(9.5f).Bold().FontColor("#00324D");
                                leftSig.Item().AlignCenter().Text("Control y Registro de Matrículas")
                                    .FontSize(8.5f).FontColor("#64748B");
                            });

                            sigRow.ConstantItem(30);

                            sigRow.RelativeItem().Column(rightSig =>
                            {
                                rightSig.Item().PaddingHorizontal(16).LineHorizontal(1).LineColor("#00324D");
                                rightSig.Item().PaddingTop(4).AlignCenter().Text(data.InstructorName)
                                    .FontSize(9.5f).Bold().FontColor("#00324D");
                                rightSig.Item().AlignCenter().Text("Instructor Titular del Taller")
                                    .FontSize(8.5f).FontColor("#64748B");
                            });
                        });

                        col.Item().PaddingTop(18).BorderTop(1.5f).BorderColor("#00324D").PaddingTop(8)
                            .Row(footerRow =>
                            {
                                footerRow.RelativeItem().Text($"{data.OrganizationName} — {data.DocumentTitle}")
                                    .FontSize(8.5f).Bold().FontColor("#00324D");

                                footerRow.RelativeItem().AlignRight()
                                    .Text($"Fecha de generación del documento: {data.GeneratedAt:yyyy-MM-dd HH:mm:ss} UTC")
                                    .FontSize(8.5f).SemiBold().FontColor("#2D8600");
                            });
                    });
            });
        });

        return document.GeneratePdf();
    }
}
