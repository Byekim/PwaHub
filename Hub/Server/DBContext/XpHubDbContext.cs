using Microsoft.EntityFrameworkCore;

using Hub.Shared.Model.Hub.Login;
using Hub.Shared.Model.Hub;
using Radzen;

namespace Hub.Server.Models;

public partial class XpHubDbContext : DbContext
{
    public DbSet<XpHubUserGroup> HubUserGroups { get; set; }
    public virtual DbSet<ResponseXperpToken> tokenDatas{ get; set; }
    public DbSet<UserToken> userTokens { get; set; }

    public XpHubDbContext()
    {
    }

    public XpHubDbContext(DbContextOptions<XpHubDbContext> options)
        : base(options)
    {
    }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        CreateXperpToken(modelBuilder);
        CreateHubUserGroup(modelBuilder);
        OnModelCreatingPartial(modelBuilder);
    }

    private void CreateHubUserGroup(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<XpHubUserGroup>(entity =>
        {
            entity.HasKey(e => new { e.groupSeq, e.groupId, e.seq, e.id });
            entity.ToTable("HUB_USER_GROUP", "dbo");
            entity.Property(e => e.groupSeq).HasColumnName("GROUP_SEQ").HasMaxLength(50).IsRequired();
            entity.Property(e => e.groupId).HasColumnName("GROUP_ID").HasMaxLength(50).IsRequired();
            entity.Property(e => e.seq).HasColumnName("SEQ").HasMaxLength(50).IsRequired();
            entity.Property(e => e.id).HasColumnName("ID").HasMaxLength(50).IsRequired();
            entity.Property(e => e.inputDate).HasColumnName("INPUT_DATE");
            entity.Property(e => e.inputId).HasColumnName("INPUT_ID").HasMaxLength(50);
            entity.Property(e => e.modifyDate).HasColumnName("MODIFY_DATE");
            entity.Property(e => e.modifyId).HasColumnName("MODIFY_ID").HasMaxLength(50);
        });
          
    }

    private void CreateXperpToken(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<ResponseXperpToken>(entity =>
        {
            entity.HasKey(e => new { e.access_token })
                  .HasName("PK_H_XPERP_TOKEN");

            entity.ToTable("H_VOICE_BROADCAST");

            entity.Property(e => e.access_token)
                  .HasColumnName("ACCESS_TOKEN")
                  .IsRequired();

            entity.Property(e => e.token_type)
                  .HasColumnName("TOKEN_TYPE");

            entity.Property(e => e.expires_in)
                  .HasColumnName("EXPIREAS_IN");
            
        });

    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
