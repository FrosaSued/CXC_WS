using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CXC_WS.Data.Models
{
    public partial class cxc_newContext : DbContext
    {
        public cxc_newContext()
        {
        }

        public cxc_newContext(DbContextOptions<cxc_newContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Alerta> Alertas { get; set; } = null!;
        public virtual DbSet<Cliente> Clientes { get; set; } = null!;
        public virtual DbSet<Concepto> Conceptos { get; set; } = null!;
        public virtual DbSet<Condicione> Condiciones { get; set; } = null!;
        public virtual DbSet<Documento> Documentos { get; set; } = null!;
        public virtual DbSet<DocumentosDetalle> DocumentosDetalles { get; set; } = null!;
        public virtual DbSet<Estado> Estados { get; set; } = null!;
        public virtual DbSet<Pago> Pagos { get; set; } = null!;
        public virtual DbSet<ConceptosDescripcion> ConceptosDescripcions { get; set; } = null!;
        public virtual DbSet<TipoPago> TipoPagos { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;
        public virtual DbSet<VwDocumentoCliente> VwDocumentoClientes { get; set; } = null!;
        public virtual DbSet<Logs> Logs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //optionsBuilder.UseSqlServer("Server=10.1.0.7;Database= cxc_new;User Id= cxcrw;Password=er3NxhlJiw7540SB;Trust Server Certificate = true;");
                optionsBuilder.UseSqlServer("Server=10.2.0.9;Database= cxc_new;User Id= cxcrw;Password=L3fteaHmbaPA;Trust Server Certificate = true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Alerta>(entity =>
            {
                entity.Property(e => e.FhCreacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdTipoDocumento)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioCreacion)
                    .HasMaxLength(128)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("Clientes");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

               // entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Ciudad).HasMaxLength(50);

                entity.Property(e => e.Condiciones).HasMaxLength(64);

                entity.Property(e => e.CuentaBanco).HasMaxLength(768);

                entity.Property(e => e.EstadoPais)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Moneda)
                    .HasMaxLength(120)
                    .IsUnicode(false);

                entity.Property(e => e.Nif)
                    .HasMaxLength(24)
                    .HasColumnName("NIF");

                entity.Property(e => e.Nombre).HasMaxLength(50);

                entity.Property(e => e.Pais).HasMaxLength(50);

                entity.Property(e => e.Telefono).HasMaxLength(150);
            });
            
            modelBuilder.Entity<Logs>(entity =>
            {
                entity.ToTable("Logs");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
              //  entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Usuario).HasMaxLength(50);

            });
            
            modelBuilder.Entity<Concepto>(entity =>
            {
                entity.ToTable("Conceptos");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Concepto1).HasColumnName("Concepto");

                entity.Property(e => e.FechaDoc).HasColumnType("datetime");

                entity.Property(e => e.Monto).HasColumnType("money");

            });


            modelBuilder.Entity<Condicione>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Documento>(entity =>
            {
                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FechaDoc).HasColumnType("datetime");

                entity.Property(e => e.Saldado).HasDefaultValueSql("((0))");

               /* entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Documentos)
                    .HasForeignKey(d => d.IdCliente)
                    .HasConstraintName("FK_Documentos_Clientes");*/
                
            });

            modelBuilder.Entity<DocumentosDetalle>(entity =>
            {
                entity.ToTable("Documentos_Detalles");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.Monto).HasColumnType("money");

                entity.Property(e => e.MontoPagar).HasColumnType("money");

                entity.Property(e => e.MontoPendiente).HasColumnType("money");
            });

            modelBuilder.Entity<Estado>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Pago>(entity =>
            {
                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Creacion");

                entity.Property(e => e.FechaPago).HasColumnType("datetime");

                entity.Property(e => e.TipoPagoId).HasColumnName("TipoPagoId");

                entity.Property(e => e.Monto).HasColumnType("money");

               /* entity.HasOne(d => d.IdDocumentoNavigation)
                    .WithMany(p => p.Pagos)
                    .HasForeignKey(d => d.IdDocumento)
                    .HasConstraintName("FK_Pagos_Documentos");

                entity.HasOne(d => d.IdtipoPagoNavigation)
                    .WithMany(p => p.Pagos)
                    .HasForeignKey(d => d.IdtipoPago)
                    .HasConstraintName("FK_Pagos_TipoPago");*/
            });

            modelBuilder.Entity<TipoPago>(entity =>
            {
                entity.ToTable("TipoPago");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Descripcion).HasMaxLength(50);
            });      
            
            modelBuilder.Entity<ConceptosDescripcion>(entity =>
            {
                entity.ToTable("ConceptosDescripcion");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Descripcion).HasMaxLength(50);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID");

                entity.Property(e => e.Nombre).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Salt)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Usuario1)
                    .HasMaxLength(50)
                    .HasColumnName("Usuario");
            });

            modelBuilder.Entity<VwDocumentoCliente>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VwDocumentoClientes");

                entity.Property(e => e.ClienteNombre).HasMaxLength(50);

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaDoc).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
