﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OWuffel.Extensions.Database;

namespace OWuffel.Migrations
{
    [DbContext(typeof(WuffelDBContext))]
    [Migration("20210117132811_RebuildPart1")]
    partial class RebuildPart1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("OWuffel.Extensions.Database.Settings", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("botActive")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("botAdminRole")
                        .HasColumnType("INTEGER");

                    b.Property<int>("botClearBotMessagesDelay")
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

                    b.Property<ulong>("logBlockedByUser")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("logChannelBanModule")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("logChannelMemberRemoveModule")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("logChannelMessageIngored")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("logChannelMessageModule")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("logChannelReact")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("logGuildEvents")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("logUserEvents")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("logVoiceStateUpdate")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("reactServerChannel")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("reactServerMessage")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("reactServerParent")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("welcomeChannel")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("welcomeEnabled")
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