﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OWuffel.Extensions.Database;

namespace OWuffel.Migrations
{
    [DbContext(typeof(WuffelDBContext))]
    [Migration("20210518164615_IconHash")]
    partial class IconHash
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("OWuffel.Extensions.Database.ChannelCheckModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<decimal>("AuthorId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("ChannelId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Channels")
                        .HasColumnType("text");

                    b.Property<string>("Found")
                        .HasColumnType("text");

                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("MessageId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("Target")
                        .HasColumnType("text");

                    b.Property<string>("Timestamp")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ChannelChecks");
                });

            modelBuilder.Entity("OWuffel.Extensions.Database.DailyRanking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<decimal>("ChannelId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("IgnoreId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("DailyRankings");
                });

            modelBuilder.Entity("OWuffel.Extensions.Database.Settings", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("botActive")
                        .HasColumnType("integer");

                    b.Property<decimal>("botAdminRole")
                        .HasColumnType("numeric(20,0)");

                    b.Property<bool>("botClearMessage")
                        .HasColumnType("boolean");

                    b.Property<bool>("botClearMessagesForBot")
                        .HasColumnType("boolean");

                    b.Property<string>("botDisabledCommands")
                        .HasColumnType("text");

                    b.Property<string>("botLanguage")
                        .HasColumnType("text");

                    b.Property<decimal>("botModRole")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("botMuteRole")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("botPrefix")
                        .HasColumnType("text");

                    b.Property<bool>("botSystemNotice")
                        .HasColumnType("boolean");

                    b.Property<string>("guild_icon_hash")
                        .HasColumnType("text");

                    b.Property<decimal>("guild_id")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("guild_name")
                        .HasColumnType("text");

                    b.Property<decimal>("logChannelCreated")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("logChannelDeleted")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("logChannelUpdated")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("logEmoteCreated")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("logEmoteDeleted")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("logEmoteUpdated")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("logGuildUpdated")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("logIgnoreMessageDeleted")
                        .HasColumnType("text");

                    b.Property<string>("logIgnoreMessageUpdated")
                        .HasColumnType("text");

                    b.Property<string>("logIgnoreVoiceStateUpdated")
                        .HasColumnType("text");

                    b.Property<decimal>("logMessageDeleted")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("logMessageUpdated")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("logRoleCreated")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("logRoleDeleted")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("logRoleUpdated")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("logUserBanned")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("logUserJoined")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("logUserLeft")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("logUserUnbanned")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("logUserUpdated")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("logVoiceStateUpdated")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("reactServerChannel")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("reactServerMessage")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("reactServerParent")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("suggestionChannel")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("suggestionLogChannel")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("welcomeChannel")
                        .HasColumnType("numeric(20,0)");

                    b.Property<bool>("welcomeMessageAutoDelete")
                        .HasColumnType("boolean");

                    b.HasKey("id");

                    b.HasIndex("guild_id")
                        .IsUnique();

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("OWuffel.Extensions.Database.Suggestions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Author")
                        .HasColumnType("text");

                    b.Property<decimal>("AuthorId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("ChannelId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("MessageId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Moderator")
                        .HasColumnType("text");

                    b.Property<decimal>("ModeratorId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("Timestamp")
                        .HasColumnType("text");

                    b.Property<int>("VoteDislike")
                        .HasColumnType("integer");

                    b.Property<int>("VoteLike")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Suggestions");
                });

            modelBuilder.Entity("OWuffel.Extensions.Database.SupportConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<decimal>("ActiveTicketsId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("MessageId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<int>("Notify")
                        .HasColumnType("integer");

                    b.Property<decimal>("ParentId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<int>("PremiumStatus")
                        .HasColumnType("integer");

                    b.Property<decimal>("TicketInfoId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("TicketMessage")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("SupportConfiguration");
                });

            modelBuilder.Entity("OWuffel.Extensions.Database.Tickets", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<decimal>("ChannelId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("ModeratorId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("Timestamp")
                        .HasColumnType("text");

                    b.Property<decimal>("UserId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("WhoClosedTicket")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("Id");

                    b.ToTable("Tickets");
                });
#pragma warning restore 612, 618
        }
    }
}