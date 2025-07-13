using Microsoft.EntityFrameworkCore;
using Hub.Shared;
using Hub.Shared.Model;
using Hub.Shared.Voice;
using Hub.Shared.Voice.ReservationHandler;

namespace Hub.Server.Models;

public partial class XpVoiceDbContext : DbContext
{
    
    public virtual DbSet<GroupMaster> groupMasters { get; set; }
    public virtual DbSet<VoiceBroadCastHistory> voiceBroadCastHistorys { get; set; }
    public virtual DbSet<VoiceBroadCast> voiceBroadCasts { get; set; }
    public virtual DbSet<GeneralReservation> generalReservations { get; set; }
    public virtual DbSet<ScheduledReservation> schuledReservations { get; set; }
    public virtual DbSet<PeriodReservation> periodReservations { get; set; }

    public XpVoiceDbContext(DbContextOptions<XpVoiceDbContext> options)
        : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        => optionsBuilder.UseSqlServer();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {        
        CreateVoiceBroadcast(modelBuilder);
        CreateVoiceBroadcastHistory(modelBuilder);
        CreateVoiceBroadcastGroupMaster(modelBuilder);
        CreateReservationVoiceBroadcast(modelBuilder);
        OnModelCreatingPartial(modelBuilder);
    }

    private void CreateReservationVoiceBroadcast(ModelBuilder modelBuilder)
    {
        CreateGeneralReservationTable(modelBuilder);
        CreateScheduledReservationTable(modelBuilder);
        CreatePeriodReservationTable(modelBuilder);
    }

    private void CreatePeriodReservationTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PeriodReservation>(entity =>
        {
            entity.HasKey(e => e.seq).HasName("PK_PERIOD_RESERVATION");
            entity.ToTable("PERIOD_RESERVATION");

            entity.Property(e => e.seq).HasColumnName("SEQ");
            entity.Property(e => e.reservationTime).HasColumnName("RESERVATION_TIME");
            entity.Property(e => e.aptCd).HasColumnName("APT_CD").HasMaxLength(10);
            entity.Property(e => e.reservationType).HasColumnName("RESERVATION_TYPE");
            entity.Property(e => e.startDate).HasColumnName("START_DATE");
            entity.Property(e => e.endDate).HasColumnName("END_DATE");
        });
    }

    private void CreateScheduledReservationTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ScheduledReservation>(entity =>
        {
            entity.HasKey(e => e.seq).HasName("PK_SCHEDULED_RESERVATION");
            entity.ToTable("SCHEDULED_RESERVATION");

            entity.Property(e => e.seq).HasColumnName("SEQ");
            entity.Property(e => e.reservationTime).HasColumnName("RESERVATION_TIME");
            entity.Property(e => e.aptCd).HasColumnName("APT_CD").HasMaxLength(10);
            entity.Property(e => e.reservationType).HasColumnName("RESERVATION_TYPE");

            // dayOfWeeks는 List<DayOfWeek>로 저장되므로, 이를 문자열로 변환하여 저장
            entity.Property(e => e.dayOfWeeks)
                .HasConversion(
                    v => string.Join(",", v),
                    v => v.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(e => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), e)).ToList())
                .HasColumnName("DAY_OF_WEEKS");
        });
    }

    private void CreateGeneralReservationTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GeneralReservation>(entity =>
        {
            entity.HasKey(e => e.seq).HasName("PK_GENERAL_RESERVATION");
            entity.ToTable("GENERAL_RESERVATION");

            entity.Property(e => e.seq).HasColumnName("SEQ");
            entity.Property(e => e.reservationTime).HasColumnName("RESERVATION_TIME");
            entity.Property(e => e.aptCd).HasColumnName("APT_CD").HasMaxLength(10);
            entity.Property(e => e.reservationType).HasColumnName("RESERVATION_TYPE");
        });
    }

    private void CreateVoiceBroadcastGroupMaster(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VoiceBroadcastGroup>(entity =>
        {
            // 복합 키 설정 (groupSeq, aptCd, seq)
            entity.HasKey(e => new { e.groupSeq, e.aptCd, e.seq })
                .HasName("PK_H_VOICE_BROADCAST_GROUP");

            entity.ToTable("H_VOICE_BROADCAST_GROUP");

            // groupMaster에서 복합 키 분리하여 설정
            entity.Property(e => e.groupSeq)
                .HasColumnName("GROUP_SEQ");
            entity.Property(e => e.aptCd)
                .HasColumnName("APT_CD")
                .HasMaxLength(10);
            entity.Property(e => e.seq)
                .HasColumnName("SEQ");

            // 외래 키 설정 (GroupMaster와 관계)
            entity.HasOne(e => e.groupMaster) // GroupMaster와의 관계 설정
                .WithMany() // GroupMaster가 여러 VoiceBroadcastGroup을 가질 수 있음
                //.HasForeignKey(e => new { e.groupSeq, e.aptCd }) // 외래 키 설정
                .OnDelete(DeleteBehavior.Cascade); // GroupMaster 삭제 시 관련된 VoiceBroadcastGroup도 삭제
        });

        modelBuilder.Entity<GroupMaster>(entity =>
        {
            // GroupMaster의 복합 키 설정
            entity.HasKey(e => new { e.groupSeq, e.aptCd })
                .HasName("PK_H_VOICE_BROADCAST_GROUPMASTER");

            entity.ToTable("H_VOICE_BROADCAST_GROUPMASTER");

            // 필드 설정
            entity.Property(e => e.groupSeq)
                .HasColumnName("GROUP_SEQ")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.aptCd)
                .HasColumnName("APT_CD");
            entity.Property(e => e.groupName)
                .HasColumnName("GROUP_NAME");
            entity.Property(e => e.yn)
            .HasColumnName("YN");
           
        });
    }

    private void CreateVoiceBroadcastHistory(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<VoiceBroadCastHistory>(entity =>
        {
            entity.HasKey(e => new { e.historySeq });
            entity.ToTable("H_VOICE_BROADCAST_HISTORY");
            entity.Property(e => e.historySeq)
             .HasColumnName("HISTORY_SEQ")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.seq)
                  .HasColumnName("SEQ");

            entity.Property(e => e.aptCd)
                  .HasColumnName("APT_CD")
                  .HasMaxLength(10)
                  .IsRequired();


            entity.Property(e => e.broadcastType)
                  .HasColumnName("BROADCAST_TYPE")
                  .HasMaxLength(20);

            entity.Property(e => e.title)
                  .HasColumnName("TITLE")
                  .HasMaxLength(200);

            entity.Property(e => e.body)
                  .HasColumnName("BODY")
                  .HasMaxLength(4000);

            entity.Property(e => e.speaker)
                  .HasColumnName("SPEAKER")
                  .HasMaxLength(20);

            entity.Property(e => e.historySeq)
                  .HasColumnName("HISTORY_SEQ")
                  .IsRequired();

            entity.Property(e => e.aptI)
                  .HasColumnName("APTI");

        });
    }

    private void CreateVoiceBroadcast(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VoiceBroadCast>().ToTable("VOICE_BROADCAST");


        modelBuilder.Entity<VoiceBroadCast>(entity =>
        {
            entity.HasKey(e => new { e.seq, e.aptCd })
                  .HasName("PK_H_VOICE_BROADCAST");

            entity.ToTable("H_VOICE_BROADCAST");

            entity.Property(e => e.seq)
                  .HasColumnName("SEQ")
                  .ValueGeneratedOnAdd();

            entity.Property(e => e.aptCd)
                  .HasColumnName("APT_CD")
                  .HasMaxLength(10)
                  .IsRequired();


            entity.Property(e => e.broadcastType)
                  .HasColumnName("BROADCAST_TYPE")
                  .HasMaxLength(20);

            entity.Property(e => e.title)
                  .HasColumnName("TITLE")
                  .HasMaxLength(200);

            entity.Property(e => e.body)
                  .HasColumnName("BODY")
                  .HasMaxLength(4000);

            entity.Property(e => e.speaker)
                  .HasColumnName("SPEAKER")
                  .HasMaxLength(20);


            entity.Property(e => e.inputDate)
                  .HasColumnName("INPUT_DATE")
                  .IsRequired();

            entity.Property(e => e.modifyDate)
                  .HasColumnName("MODIFY_DATE");

            entity.Property(e => e.use)
                  .HasColumnName("USE")
                  .HasDefaultValue('Y')
                  .HasMaxLength(1);

            entity.Property(e => e.bookMark)
                  .HasColumnName("BOOK_MARK")
                  .HasDefaultValue('N')
                  .HasMaxLength(1);

            entity.Property(e => e.voiceSpeed)
                  .HasColumnName("VOICE_SPEED");

            entity.Property(e => e.id)
                  .HasColumnName("ID")
                  .HasMaxLength(100);

            entity.HasOne(e => e.voiceGroup)
                   .WithMany()
                   .HasForeignKey(e => new { e.seq, e.aptCd, e.groupSeq })
                   .HasConstraintName("FK_VoiceBroadcast_VoiceBroadcastGroup");
        });


    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
