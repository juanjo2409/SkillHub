using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkillHub.Domain.Entities;
using SkillHub.Domain.ValueObjects;

namespace SkillHub.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Instructor> Instructors => Set<Instructor>();
    public DbSet<Participant> Participants => Set<Participant>();
    public DbSet<Workshop> Workshops => Set<Workshop>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        SeedInitialData(builder);
    }

    // Seeds the 3 initial sheets from SkillHub_Data.xlsx: Instructors (6), Participants (30), and Workshops (15).
    // The Enrollments sheet is intentionally left unseeded to be processed via POST /api/v1/enrollments/import.
    private static void SeedInitialData(ModelBuilder builder)
    {
        const string employeeRoleId = "11111111-1111-1111-1111-111111111111";
        const string participantRoleId = "22222222-2222-2222-2222-222222222222";

        builder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = employeeRoleId,
                Name = "Employee",
                NormalizedName = "EMPLOYEE",
                ConcurrencyStamp = employeeRoleId
            },
            new IdentityRole
            {
                Id = participantRoleId,
                Name = "Participant",
                NormalizedName = "PARTICIPANT",
                ConcurrencyStamp = participantRoleId
            });

        var hasher = new PasswordHasher<IdentityUser>();
        var identityUsers = new List<IdentityUser>();
        var userRoles = new List<IdentityUserRole<string>>();

        // 1. Sheet: Instructors (6 employees)
        var instructorsSeed = new[]
        {
            (Idx: 1, FullName: "Andrés Felipe Mejía Arango", Email: "andres.mejia@correo.com", Specialty: "Fotografía"),
            (Idx: 2, FullName: "Rosa Elena Barrios Polo", Email: "rosa.barrios@correo.com", Specialty: "Cocina"),
            (Idx: 3, FullName: "Mauricio Jiménez Cárdenas", Email: "mauricio.jimenez@correo.com", Specialty: "Oratoria"),
            (Idx: 4, FullName: "Natalia Herrera Castaño", Email: "natalia.herrera@correo.com", Specialty: "Diseño gráfico"),
            (Idx: 5, FullName: "Hernán Darío Quintero Gil", Email: "hernan.quintero@correo.com", Specialty: "Música"),
            (Idx: 6, FullName: "Paola Andrea Vargas Mora", Email: "paola.vargas@correo.com", Specialty: "Marketing digital")
        };

        var instructorEntities = new List<object>();
        var instructorIdByEmail = new Dictionary<string, Guid>();

        foreach (var item in instructorsSeed)
        {
            var userId = $"33333333-3333-3333-3333-{item.Idx:D12}";
            var instructorId = Guid.Parse($"44444444-4444-4444-4444-{item.Idx:D12}");
            instructorIdByEmail[item.Email] = instructorId;

            var user = new IdentityUser
            {
                Id = userId,
                UserName = item.Email,
                NormalizedUserName = item.Email.ToUpperInvariant(),
                Email = item.Email,
                NormalizedEmail = item.Email.ToUpperInvariant(),
                EmailConfirmed = true,
                SecurityStamp = userId,
                ConcurrencyStamp = userId
            };
            user.PasswordHash = hasher.HashPassword(user, "Instructor2026*");

            identityUsers.Add(user);
            userRoles.Add(new IdentityUserRole<string> { RoleId = employeeRoleId, UserId = userId });

            instructorEntities.Add(new
            {
                Id = instructorId,
                FullName = item.FullName,
                Email = new Email(item.Email),
                Specialty = item.Specialty,
                IdentityUserId = userId
            });
        }

        // 2. Sheet: Participants (30 participants)
        var participantsSeed = new[]
        {
            (Idx: 1, FullName: "Jorge Andrés Pérez Rojas", Email: "jorge.perez@correo.com"),
            (Idx: 2, FullName: "Luz Marina Ospina Ríos", Email: "luz.ospina@correo.com"),
            (Idx: 3, FullName: "Carlos Eduardo Rincón Díaz", Email: "carlos.rincon@correo.com"),
            (Idx: 4, FullName: "Valentina Gómez Ruiz", Email: "valentina.gomez@correo.com"),
            (Idx: 5, FullName: "Juan Camilo Restrepo Vélez", Email: "juan.restrepo@correo.com"),
            (Idx: 6, FullName: "María Fernanda Torres León", Email: "maria.torres@correo.com"),
            (Idx: 7, FullName: "Santiago Ramírez Henao", Email: "santiago.ramirez@correo.com"),
            (Idx: 8, FullName: "Daniela Morales Pineda", Email: "daniela.morales@correo.com"),
            (Idx: 9, FullName: "Sebastián Castro Duque", Email: "sebastian.castro@correo.com"),
            (Idx: 10, FullName: "Laura Sofía Martínez Suárez", Email: "laura.martinez@correo.com"),
            (Idx: 11, FullName: "Andrés Mauricio Salazar Ortiz", Email: "andres.salazar@correo.com"),
            (Idx: 12, FullName: "Camila Andrea Cárdenas Ruiz", Email: "camila.cardenas@correo.com"),
            (Idx: 13, FullName: "Diego Alejandro Muñoz Peña", Email: "diego.munoz@correo.com"),
            (Idx: 14, FullName: "Juliana Ortega Sánchez", Email: "juliana.ortega@correo.com"),
            (Idx: 15, FullName: "Felipe Agudelo Marín", Email: "felipe.agudelo@correo.com"),
            (Idx: 16, FullName: "Natalia Montoya Correa", Email: "natalia.montoya@correo.com"),
            (Idx: 17, FullName: "Óscar Iván Guerrero Lozano", Email: "oscar.guerrero@correo.com"),
            (Idx: 18, FullName: "Paula Andrea Zapata Ríos", Email: "paula.zapata@correo.com"),
            (Idx: 19, FullName: "Cristian David Becerra Mejía", Email: "cristian.becerra@correo.com"),
            (Idx: 20, FullName: "Mariana Londoño Álvarez", Email: "mariana.londono@correo.com"),
            (Idx: 21, FullName: "Javier Alonso Cuéllar Pardo", Email: "javier.cuellar@correo.com"),
            (Idx: 22, FullName: "Isabella Navarro Fuentes", Email: "isabella.navarro@correo.com"),
            (Idx: 23, FullName: "Kevin Stiven Mosquera Palacios", Email: "kevin.mosquera@correo.com"),
            (Idx: 24, FullName: "Ana María Jaramillo Gaviria", Email: "ana.jaramillo@correo.com"),
            (Idx: 25, FullName: "Luis Fernando Arias Cano", Email: "luis.arias@correo.com"),
            (Idx: 26, FullName: "Sara Valencia Bermúdez", Email: "sara.valencia@correo.com"),
            (Idx: 27, FullName: "Esteban Rodríguez Molina", Email: "esteban.rodriguez@correo.com"),
            (Idx: 28, FullName: "Manuela Escobar Toro", Email: "manuela.escobar@correo.com"),
            (Idx: 29, FullName: "Wilmer Alfonso Benítez Cruz", Email: "wilmer.benitez@correo.com"),
            (Idx: 30, FullName: "Yuliana Patiño Osorio", Email: "yuliana.patino@correo.com")
        };

        var participantEntities = new List<object>();

        foreach (var item in participantsSeed)
        {
            var userId = $"55555555-5555-5555-5555-{item.Idx:D12}";
            var participantId = Guid.Parse($"66666666-6666-6666-6666-{item.Idx:D12}");

            var user = new IdentityUser
            {
                Id = userId,
                UserName = item.Email,
                NormalizedUserName = item.Email.ToUpperInvariant(),
                Email = item.Email,
                NormalizedEmail = item.Email.ToUpperInvariant(),
                EmailConfirmed = true,
                SecurityStamp = userId,
                ConcurrencyStamp = userId
            };
            user.PasswordHash = hasher.HashPassword(user, "Skillhub2026*");

            identityUsers.Add(user);
            userRoles.Add(new IdentityUserRole<string> { RoleId = participantRoleId, UserId = userId });

            participantEntities.Add(new
            {
                Id = participantId,
                FullName = item.FullName,
                Email = new Email(item.Email),
                IdentityUserId = userId
            });
        }

        // 3. Sheet: Workshops (15 workshops)
        var workshopsSeed = new[]
        {
            (Idx: 1, Code: "FOT-001", Title: "Fotografía con celular", Description: "Composición, luz y edición básica desde el celular", InstructorEmail: "andres.mejia@correo.com", Start: new DateTime(2026, 12, 5, 8, 0, 0, DateTimeKind.Utc), End: new DateTime(2026, 12, 5, 12, 0, 0, DateTimeKind.Utc), Capacity: 15),
            (Idx: 2, Code: "FOT-002", Title: "Retrato en exteriores", Description: "Manejo de luz natural para retratos al aire libre", InstructorEmail: "andres.mejia@correo.com", Start: new DateTime(2027, 1, 16, 8, 0, 0, DateTimeKind.Utc), End: new DateTime(2027, 1, 16, 12, 0, 0, DateTimeKind.Utc), Capacity: 10),
            (Idx: 3, Code: "FOT-003", Title: "Edición en Lightroom", Description: "Flujo de revelado y ajustes de color", InstructorEmail: "andres.mejia@correo.com", Start: new DateTime(2027, 2, 6, 14, 0, 0, DateTimeKind.Utc), End: new DateTime(2027, 2, 6, 18, 0, 0, DateTimeKind.Utc), Capacity: 12),
            (Idx: 4, Code: "COC-001", Title: "Cocina costeña", Description: "Arroz con coco, patacones y mojarra frita", InstructorEmail: "rosa.barrios@correo.com", Start: new DateTime(2026, 12, 5, 14, 0, 0, DateTimeKind.Utc), End: new DateTime(2026, 12, 5, 18, 0, 0, DateTimeKind.Utc), Capacity: 12),
            (Idx: 5, Code: "COC-002", Title: "Repostería básica", Description: "Tortas, galletas y postres de nevera", InstructorEmail: "rosa.barrios@correo.com", Start: new DateTime(2027, 1, 9, 9, 0, 0, DateTimeKind.Utc), End: new DateTime(2027, 1, 9, 13, 0, 0, DateTimeKind.Utc), Capacity: 10),
            (Idx: 6, Code: "COC-003", Title: "Cocina saludable", Description: "Platos balanceados para la semana", InstructorEmail: "rosa.barrios@correo.com", Start: new DateTime(2027, 2, 13, 9, 0, 0, DateTimeKind.Utc), End: new DateTime(2027, 2, 13, 12, 0, 0, DateTimeKind.Utc), Capacity: 14),
            (Idx: 7, Code: "ORA-001", Title: "Oratoria para principiantes", Description: "Técnicas para hablar en público con seguridad", InstructorEmail: "mauricio.jimenez@correo.com", Start: new DateTime(2026, 12, 12, 9, 0, 0, DateTimeKind.Utc), End: new DateTime(2026, 12, 12, 12, 0, 0, DateTimeKind.Utc), Capacity: 20),
            (Idx: 8, Code: "ORA-002", Title: "Presentaciones efectivas", Description: "Cómo estructurar y presentar una idea en 10 minutos", InstructorEmail: "mauricio.jimenez@correo.com", Start: new DateTime(2027, 1, 23, 9, 0, 0, DateTimeKind.Utc), End: new DateTime(2027, 1, 23, 12, 0, 0, DateTimeKind.Utc), Capacity: 18),
            (Idx: 9, Code: "DIS-001", Title: "Diseño en Canva", Description: "Piezas gráficas para redes sociales", InstructorEmail: "natalia.herrera@correo.com", Start: new DateTime(2026, 12, 19, 14, 0, 0, DateTimeKind.Utc), End: new DateTime(2026, 12, 19, 17, 0, 0, DateTimeKind.Utc), Capacity: 18),
            (Idx: 10, Code: "DIS-002", Title: "Fundamentos de color y tipografía", Description: "Principios visuales para piezas profesionales", InstructorEmail: "natalia.herrera@correo.com", Start: new DateTime(2027, 1, 30, 14, 0, 0, DateTimeKind.Utc), End: new DateTime(2027, 1, 30, 17, 0, 0, DateTimeKind.Utc), Capacity: 15),
            (Idx: 11, Code: "MUS-001", Title: "Guitarra desde cero", Description: "Acordes básicos y primeras canciones", InstructorEmail: "hernan.quintero@correo.com", Start: new DateTime(2026, 12, 12, 14, 0, 0, DateTimeKind.Utc), End: new DateTime(2026, 12, 12, 17, 0, 0, DateTimeKind.Utc), Capacity: 10),
            (Idx: 12, Code: "MUS-002", Title: "Producción musical en casa", Description: "Grabación y mezcla con herramientas gratuitas", InstructorEmail: "hernan.quintero@correo.com", Start: new DateTime(2027, 2, 20, 14, 0, 0, DateTimeKind.Utc), End: new DateTime(2027, 2, 20, 18, 0, 0, DateTimeKind.Utc), Capacity: 12),
            (Idx: 13, Code: "MKT-001", Title: "Marketing en redes sociales", Description: "Estrategia de contenido para pequeños negocios", InstructorEmail: "paola.vargas@correo.com", Start: new DateTime(2026, 12, 19, 8, 0, 0, DateTimeKind.Utc), End: new DateTime(2026, 12, 19, 12, 0, 0, DateTimeKind.Utc), Capacity: 20),
            (Idx: 14, Code: "MKT-002", Title: "Publicidad en Meta e Instagram", Description: "Configuración y medición de campañas", InstructorEmail: "paola.vargas@correo.com", Start: new DateTime(2027, 1, 16, 14, 0, 0, DateTimeKind.Utc), End: new DateTime(2027, 1, 16, 18, 0, 0, DateTimeKind.Utc), Capacity: 16),
            (Idx: 15, Code: "MKT-003", Title: "Emprendimiento digital", Description: "Validar y lanzar una idea de negocio en línea", InstructorEmail: "paola.vargas@correo.com", Start: new DateTime(2027, 2, 27, 9, 0, 0, DateTimeKind.Utc), End: new DateTime(2027, 2, 27, 12, 0, 0, DateTimeKind.Utc), Capacity: 20)
        };

        var workshopEntities = new List<object>();
        var scheduleOwnedEntities = new List<object>();

        foreach (var w in workshopsSeed)
        {
            var workshopId = Guid.Parse($"77777777-7777-7777-7777-{w.Idx:D12}");
            workshopEntities.Add(new
            {
                Id = workshopId,
                Code = new WorkshopCode(w.Code),
                Title = w.Title,
                Description = w.Description,
                MaxCapacity = w.Capacity,
                InstructorId = instructorIdByEmail[w.InstructorEmail]
            });

            scheduleOwnedEntities.Add(new
            {
                WorkshopId = workshopId,
                StartDate = w.Start,
                EndDate = w.End
            });
        }

        builder.Entity<IdentityUser>().HasData(identityUsers);
        builder.Entity<IdentityUserRole<string>>().HasData(userRoles);
        builder.Entity<Instructor>().HasData(instructorEntities);
        builder.Entity<Participant>().HasData(participantEntities);
        builder.Entity<Workshop>().HasData(workshopEntities);
        builder.Entity<Workshop>().OwnsOne(w => w.Schedule).HasData(scheduleOwnedEntities);
    }
}
