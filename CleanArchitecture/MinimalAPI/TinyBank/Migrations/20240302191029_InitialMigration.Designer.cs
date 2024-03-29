﻿// <auto-generated />
using System;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TinyBank.Migrations
{
    [DbContext(typeof(BankContext))]
    [Migration("20240302191029_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TinyBank.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Balance")
                        .HasColumnType("integer")
                        .HasColumnName("saldo");

                    b.Property<int>("Limit")
                        .HasColumnType("integer")
                        .HasColumnName("limite");

                    b.HasKey("Id");

                    b.ToTable("clientes", (string)null);
                });

            modelBuilder.Entity("TinyBank.Models.Transaction.BankTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ClientId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("descricao");

                    b.Property<DateTime>("DoneAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("realizada_em");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("tipo");

                    b.Property<int>("Value")
                        .HasColumnType("integer")
                        .HasColumnName("valor");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("transacoes", (string)null);
                });

            modelBuilder.Entity("TinyBank.Models.Transaction.BankTransaction", b =>
                {
                    b.HasOne("TinyBank.Models.Client", "Client")
                        .WithMany("Transactions")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("TinyBank.Models.Client", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
