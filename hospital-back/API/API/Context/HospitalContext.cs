using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API.Context;

public partial class HospitalContext : DbContext
{
    public HospitalContext()
    {
    }

    public HospitalContext(DbContextOptions<HospitalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bitacora> Bitacoras { get; set; }

    public virtual DbSet<Cita> Citas { get; set; }

    public virtual DbSet<Horario> Horarios { get; set; }

    public virtual DbSet<HorariosMedico> HorariosMedicos { get; set; }

    public virtual DbSet<HorariosMedico1> HorariosMedicos1 { get; set; }

    public virtual DbSet<Insumo> Insumos { get; set; }

    public virtual DbSet<LoginLog> LoginLogs { get; set; }

    public virtual DbSet<Medico> Medicos { get; set; }

    public virtual DbSet<MedicosEspecialidad> MedicosEspecialidads { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<RecetaMedica> RecetaMedicas { get; set; }

    public virtual DbSet<RecetaMedicamento> RecetaMedicamentos { get; set; }

    public virtual DbSet<RecuperacionContrasena> RecuperacionContrasenas { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<ServiciosTicket> ServiciosTickets { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TicketsInsumo> TicketsInsumos { get; set; }

    public virtual DbSet<TipoInsumo> TipoInsumos { get; set; }

    public virtual DbSet<TipoUsuario> TipoUsuarios { get; set; }

    public virtual DbSet<Trabajador> Trabajadors { get; set; }

    public virtual DbSet<TrabajadorRol> TrabajadorRols { get; set; }

    public virtual DbSet<TrabajadorServicio> TrabajadorServicios { get; set; }

    public virtual DbSet<TrabajadorServicio1> TrabajadorServicios1 { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<VistaHorariosMedico> VistaHorariosMedicos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=servidorprueba15.database.windows.net;Database=Hospital;User Id=adminsql;Password=Leonardo15!;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bitacora>(entity =>
        {
            entity.HasKey(e => e.IdBitacora).HasName("PK__Bitacora__896F993D17C8B908");

            entity.ToTable("Bitacora");

            entity.Property(e => e.IdBitacora).HasColumnName("Id_Bitacora");
            entity.Property(e => e.Diagnostico).IsUnicode(false);
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.IdCita).HasColumnName("Id_Cita");
            entity.Property(e => e.IdRecetaMedica).HasColumnName("Id_Receta_Medica");

            entity.HasOne(d => d.IdCitaNavigation).WithMany(p => p.Bitacoras)
                .HasForeignKey(d => d.IdCita)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bitacora__Id_Cit__58D1301D");

            entity.HasOne(d => d.IdRecetaMedicaNavigation).WithMany(p => p.Bitacoras)
                .HasForeignKey(d => d.IdRecetaMedica)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bitacora__Id_Rec__57DD0BE4");
        });

        modelBuilder.Entity<Cita>(entity =>
        {
            entity.HasKey(e => e.IdCita).HasName("PK__Citas__A95AFC070F58C71B");

            entity.Property(e => e.IdCita).HasColumnName("Id_Cita");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.IdMedico).HasColumnName("Id_Medico");
            entity.Property(e => e.IdPaciente).HasColumnName("Id_Paciente");
            entity.Property(e => e.IdServicio).HasColumnName("Id_Servicio");
            entity.Property(e => e.IdStatus).HasColumnName("Id_Status");

            entity.HasOne(d => d.IdMedicoNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdMedico)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Citas__Id_Medico__489AC854");

            entity.HasOne(d => d.IdPacienteNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdPaciente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Citas__Id_Pacien__498EEC8D");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Citas__Id_Servic__4A8310C6");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Citas_Id_Status");
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => e.IdHorario).HasName("PK__Horarios__AD7A4DD399652B80");

            entity.Property(e => e.IdHorario).HasColumnName("Id_Horario");
            entity.Property(e => e.HoraFin)
                .HasPrecision(4)
                .HasColumnName("Hora_Fin");
            entity.Property(e => e.HoraInicio)
                .HasPrecision(4)
                .HasColumnName("Hora_Inicio");
            entity.Property(e => e.Turno)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HorariosMedico>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Horarios_Medicos");

            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Apellido_Materno");
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Apellido_Paterno");
            entity.Property(e => e.HoraFin).HasColumnName("Hora_Fin");
            entity.Property(e => e.HoraInicio).HasColumnName("Hora_Inicio");
            entity.Property(e => e.IdMedico).HasColumnName("Id_Medico");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Turno)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HorariosMedico1>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("HorariosMedicos");

            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Apellido_Materno");
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Apellido_Paterno");
            entity.Property(e => e.HoraFin).HasColumnName("Hora_Fin");
            entity.Property(e => e.HoraInicio).HasColumnName("Hora_Inicio");
            entity.Property(e => e.IdMedico).HasColumnName("Id_Medico");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Turno)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Insumo>(entity =>
        {
            entity.HasKey(e => e.IdInsumo).HasName("PK__Insumos__02514E86D4856A85");

            entity.Property(e => e.IdInsumo).HasColumnName("Id_Insumo");
            entity.Property(e => e.IdTipoInsumo).HasColumnName("Id_Tipo_Insumo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdTipoInsumoNavigation).WithMany(p => p.Insumos)
                .HasForeignKey(d => d.IdTipoInsumo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Insumos__Id_Tipo__395884C4");
        });

        modelBuilder.Entity<LoginLog>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__LoginLog__EF59F7624D17F685");

            entity.ToTable("LoginLog");

            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.Mensaje).IsUnicode(false);
        });

        modelBuilder.Entity<Medico>(entity =>
        {
            entity.HasKey(e => e.IdMedico).HasName("PK__Medicos__7BA5D810BD01BCCB");

            entity.Property(e => e.IdMedico).HasColumnName("Id_Medico");
            entity.Property(e => e.Cedula)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Consultorio)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Especialidad)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IdTrabajador).HasColumnName("Id_Trabajador");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdTrabajadorNavigation).WithMany(p => p.Medicos)
                .HasForeignKey(d => d.IdTrabajador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Medicos__Id_Trab__29221CFB");
        });

        modelBuilder.Entity<MedicosEspecialidad>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("MedicosEspecialidad");

            entity.Property(e => e.Especialidad)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NumeroMedicos).HasColumnName("numero_medicos");
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.IdPaciente).HasName("PK__Paciente__032CD4A6B1193C3D");

            entity.Property(e => e.IdPaciente).HasColumnName("Id_Paciente");
            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Apellido_Materno");
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Apellido_Paterno");
            entity.Property(e => e.Curp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CURP");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.IdPersona).HasName("PK__Personas__C95634AF2D11A36F");

            entity.Property(e => e.IdPersona).HasColumnName("Id_Persona");
            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Apellido_Materno");
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Apellido_Paterno");
            entity.Property(e => e.Calle)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Colonia)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Cp)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CP");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaNacimiento)
                .HasColumnType("date")
                .HasColumnName("Fecha_Nacimiento");
            entity.Property(e => e.Municipio)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RecetaMedica>(entity =>
        {
            entity.HasKey(e => e.IdRecetaMedica).HasName("PK__Receta_M__009BD783EE49A668");

            entity.ToTable("Receta_Medica", tb =>
                {
                    tb.HasTrigger("ModificacionBitacoraReceta");
                    tb.HasTrigger("RegistroBitacoraReceta");
                });

            entity.Property(e => e.IdRecetaMedica).HasColumnName("Id_Receta_Medica");
            entity.Property(e => e.IdCita).HasColumnName("Id_Cita");
            entity.Property(e => e.Posologia).IsUnicode(false);

            entity.HasOne(d => d.IdCitaNavigation).WithMany(p => p.RecetaMedicas)
                .HasForeignKey(d => d.IdCita)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Receta_Me__Id_Ci__4D5F7D71");
        });

        modelBuilder.Entity<RecetaMedicamento>(entity =>
        {
            entity.HasKey(e => e.IdRecetaMedicamento).HasName("PK__Receta_M__EE185ABBF062940A");

            entity.ToTable("Receta_Medicamentos");

            entity.Property(e => e.IdRecetaMedicamento).HasColumnName("Id_Receta_Medicamento");
            entity.Property(e => e.IdInsumo).HasColumnName("Id_Insumo");
            entity.Property(e => e.IdRecetaMedica).HasColumnName("Id_Receta_Medica");

            entity.HasOne(d => d.IdInsumoNavigation).WithMany(p => p.RecetaMedicamentos)
                .HasForeignKey(d => d.IdInsumo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Receta_Me__Id_In__540C7B00");

            entity.HasOne(d => d.IdRecetaMedicaNavigation).WithMany(p => p.RecetaMedicamentos)
                .HasForeignKey(d => d.IdRecetaMedica)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Receta_Me__Id_Re__55009F39");
        });

        modelBuilder.Entity<RecuperacionContrasena>(entity =>
        {
            entity.HasKey(e => e.IdRecuperacionContrasena).HasName("PK__Recupera__90DF60174A685A7E");

            entity.ToTable("Recuperacion_Contrasena");

            entity.Property(e => e.IdRecuperacionContrasena).HasColumnName("Id_Recuperacion_Contrasena");
            entity.Property(e => e.FechaExpiracion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Expiracion");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");
            entity.Property(e => e.TokenRecuperacion)
                .IsUnicode(false)
                .HasColumnName("Token_Recuperacion");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.RecuperacionContrasenas)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Recuperac__Id_Us__245D67DE");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Roles__55932E8628388E63");

            entity.Property(e => e.IdRol).HasColumnName("Id_Rol");
            entity.Property(e => e.Rol)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.IdServicio).HasName("PK__Servicio__5B1345F0491A3170");

            entity.ToTable("Servicio");

            entity.Property(e => e.IdServicio).HasColumnName("Id_Servicio");
            entity.Property(e => e.Servicio1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Servicio");
        });

        modelBuilder.Entity<ServiciosTicket>(entity =>
        {
            entity.HasKey(e => e.IdServicioTicket).HasName("PK__Servicio__35E23319667DADF3");

            entity.ToTable("Servicios_Ticket");

            entity.Property(e => e.IdServicioTicket).HasColumnName("Id_Servicio_Ticket");
            entity.Property(e => e.IdServicio).HasColumnName("Id_Servicio");
            entity.Property(e => e.IdTicket).HasColumnName("Id_Ticket");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.ServiciosTickets)
                .HasForeignKey(d => d.IdServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Servicios__Id_Se__3587F3E0");

            entity.HasOne(d => d.IdTicketNavigation).WithMany(p => p.ServiciosTickets)
                .HasForeignKey(d => d.IdTicket)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Servicios__Id_Ti__367C1819");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.IdStatus);

            entity.ToTable("Status");

            entity.Property(e => e.IdStatus).HasColumnName("Id_Status");
            entity.Property(e => e.Status1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Status");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.IdTicket).HasName("PK__Tickets__2CE8F3C724812B54");

            entity.Property(e => e.IdTicket).HasColumnName("Id_Ticket");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.IdTrabajador).HasColumnName("Id_Trabajador");

            entity.HasOne(d => d.IdTrabajadorNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.IdTrabajador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tickets__Id_Trab__31B762FC");
        });

        modelBuilder.Entity<TicketsInsumo>(entity =>
        {
            entity.HasKey(e => e.IdTicketInsumo).HasName("PK__Tickets___E9D345FC02DEE394");

            entity.ToTable("Tickets_Insumos");

            entity.Property(e => e.IdTicketInsumo).HasColumnName("Id_Ticket_Insumo");
            entity.Property(e => e.IdInsumo).HasColumnName("Id_Insumo");
            entity.Property(e => e.IdTicket).HasColumnName("Id_Ticket");

            entity.HasOne(d => d.IdInsumoNavigation).WithMany(p => p.TicketsInsumos)
                .HasForeignKey(d => d.IdInsumo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tickets_I__Id_In__3F115E1A");

            entity.HasOne(d => d.IdTicketNavigation).WithMany(p => p.TicketsInsumos)
                .HasForeignKey(d => d.IdTicket)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tickets_I__Id_Ti__3E1D39E1");
        });

        modelBuilder.Entity<TipoInsumo>(entity =>
        {
            entity.HasKey(e => e.IdTipoInsumo).HasName("PK__Tipo_Ins__322701A6EAB11F03");

            entity.ToTable("Tipo_Insumo");

            entity.Property(e => e.IdTipoInsumo).HasColumnName("Id_Tipo_Insumo");
            entity.Property(e => e.Tipo)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoUsuario>(entity =>
        {
            entity.HasKey(e => e.IdTipoUsuario).HasName("PK__Tipo_Usu__A1DC3E90A70F6666");

            entity.ToTable("Tipo_Usuario");

            entity.Property(e => e.IdTipoUsuario).HasColumnName("Id_Tipo_usuario");
            entity.Property(e => e.TipoUsuario1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Tipo_Usuario");
        });

        modelBuilder.Entity<Trabajador>(entity =>
        {
            entity.HasKey(e => e.IdTrabajador).HasName("PK__Trabajad__D3644B95A1C67372");

            entity.ToTable("Trabajador");

            entity.Property(e => e.IdTrabajador).HasColumnName("Id_Trabajador");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("date")
                .HasColumnName("Fecha_Inicio");
            entity.Property(e => e.IdHorario).HasColumnName("Id_Horario");
            entity.Property(e => e.IdPersona).HasColumnName("Id_Persona");
            entity.Property(e => e.IdRol).HasColumnName("Id_Rol");

            entity.HasOne(d => d.IdHorarioNavigation).WithMany(p => p.Trabajadors)
                .HasForeignKey(d => d.IdHorario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Trabajado__Id_Ho__151B244E");

            entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.Trabajadors)
                .HasForeignKey(d => d.IdPersona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Trabajado__Id_Pe__160F4887");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Trabajadors)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Trabajado__Id_Ro__14270015");
        });

        modelBuilder.Entity<TrabajadorRol>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("TrabajadorRol");

            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Apellido_Materno");
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Apellido_Paterno");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Rol)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TrabajadorServicio>(entity =>
        {
            entity.HasKey(e => e.IdTrabajadorServicio).HasName("PK__Trabajad__07BD7BF310B743D2");

            entity.ToTable("Trabajador_Servicio");

            entity.Property(e => e.IdTrabajadorServicio).HasColumnName("Id_Trabajador_Servicio");
            entity.Property(e => e.IdServicio).HasColumnName("Id_Servicio");
            entity.Property(e => e.IdTrabajador).HasColumnName("Id_Trabajador");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.TrabajadorServicios)
                .HasForeignKey(d => d.IdServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Trabajado__Id_Se__2EDAF651");

            entity.HasOne(d => d.IdTrabajadorNavigation).WithMany(p => p.TrabajadorServicios)
                .HasForeignKey(d => d.IdTrabajador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Trabajado__Id_Tr__2DE6D218");
        });

        modelBuilder.Entity<TrabajadorServicio1>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("TrabajadorServicio");

            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Apellido_Materno");
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Apellido_Paterno");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Servicio)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__EF59F7628C6D8E8D");

            entity.ToTable("Usuario");

            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IdTrabajador).HasColumnName("Id_Trabajador");
            entity.Property(e => e.Salt)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.Usuario1)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("Usuario");

            entity.HasOne(d => d.IdTrabajadorNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdTrabajador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuario__Id_Trab__1EA48E88");
        });

        modelBuilder.Entity<VistaHorariosMedico>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vista_Horarios_Medicos");

            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Apellido_Materno");
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Apellido_Paterno");
            entity.Property(e => e.HoraFin).HasColumnName("Hora_Fin");
            entity.Property(e => e.HoraInicio).HasColumnName("Hora_Inicio");
            entity.Property(e => e.IdMedico).HasColumnName("Id_Medico");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Turno)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
