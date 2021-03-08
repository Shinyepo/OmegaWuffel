﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OWuffel.Extensions.Database;

namespace OWuffel.Migrations
{
    [DbContext(typeof(WuffelDBContext))]
    [Migration("20210216105550_logEmotesCUD")]
    partial class logEmotesCUD
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("OWuffel.Extensions.Database.Settings", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("botActive")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("botAdminRole")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("botClearMessage")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("botClearMessagesForBot")
                        .HasColumnType("INTEGER");

                    b.Property<string>("botDisabledCommands")
                        .HasColumnType("TEXT");

                    b.Property<string>("botLanguage")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("botModRole")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("botMuteRole")
                        .HasColumnType("INTEGER");

                    b.Property<string>("botPrefix")
                        .HasColumnType("TEXT");

                    b.Property<bool>("botSystemNotice")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("guild_id")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("logEmoteCreated")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("logEmoteDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("logEmoteUpdated")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("logMessageDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("logMessageUpdated")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("logRoleCreated")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("logRoleDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("logRoleUpdated")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("logUserBanned")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("logUserJoined")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("logUserLeft")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("logUserUnbanned")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("logUserUpdated")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("logVoiceStateUpdated")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("reactServerChannel")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("reactServerMessage")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("reactServerParent")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("welcomeChannel")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("welcomeMessageAutoDelete")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.ToTable("Settings");
                });
#pragma warning restore 612, 618
        }
    }
}
