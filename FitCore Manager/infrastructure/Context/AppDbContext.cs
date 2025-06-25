using Domain.Model;
using FitCore_Manager.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { } 
            public DbSet<User>Users { get; set; }
            public DbSet<UserMembership> UserMembership { get; set; }
            public DbSet<MembershipPlans> membershipPlans {  get; set; }
            public DbSet<WorkoutPlan> workoutPlans { get; set; }
            public DbSet<UserWorkoutPlan> userWorkoutPlans{  get; set; }
            public DbSet<WorkoutPlanDayDetails>workoutPlanDayDetails { get; set; }
        //public DbSet<Trainer> Trainers {  get; set; }
        //public DbSet<TimeSlot> TimeSlots {  get; set; }
        //public DbSet<TrainerTimeSlot> TrainerTimeSlots {  get; set; }
        //public DbSet<TrainerAvailability> TrainerAvailabilities { get; set; }
            public DbSet<TimeSlot> TimeSlots { get; set; }
            public DbSet<TrainerTimeSlot> TrainerTimeSlots { get; set; }
            public DbSet<TrainerTimeSlotAttendance> TrainerTimeSlotAttendances { get; set; }

            public DbSet<Feedback> feedbacks {  get; set; }
            public DbSet<Notification> Notifications { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<User>()
            //    .Property(e => e.Role)
            //    .HasDefaultValue("User");

            modelBuilder.Entity<User>()
                .Property(x => x.isBlocked)
                .HasDefaultValue(false);

            modelBuilder.Entity<User>()
               .Property(x => x.CreatedAt)
               .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<User>()
                .HasOne(s=>s.UserMembership)
                .WithOne(s => s.User)
                .HasForeignKey<UserMembership>(s=>s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<UserMembership>()
            //    .HasOne(s=>s.MembershipPlans)
            //    .WithMany(s=>s.UserMembership)
            //    .HasForeignKey(s=>s.PlanId)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MembershipPlans>()
                .HasMany(S => S.UserMembership)
                .WithOne(s => s.MembershipPlans)
                .HasForeignKey(s => s.PlanId);

            modelBuilder.Entity<MembershipPlans>()
                .Property(p => p.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<UserWorkoutPlan>()
                .HasOne(a => a.User)
                .WithMany(a => a.UserWorkoutPlans)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<UserWorkoutPlan>()
                .HasOne(a => a.WorkoutPlan)
                .WithMany(a => a.UserWorkoutPlans)
                .HasForeignKey(a => a.WorkoutPlanId);

            modelBuilder.Entity<WorkoutPlan>()
                .HasMany(a => a.WorkoutPlanDayDetails)
                .WithOne(b => b.WorkoutPlan)
                .HasForeignKey(c => c.WorkoutPlanId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkoutPlan>()
                .HasMany(a => a.UserWorkoutPlans)
                .WithOne(b => b.WorkoutPlan)
                .HasForeignKey(c => c.WorkoutPlanId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkoutPlanDayDetails>()
                .Property(a=>a.WeekNumber)
                .IsRequired();

            modelBuilder.Entity<WorkoutPlanDayDetails>()
                .Property(p => p.DayNumber)
                .IsRequired();

            modelBuilder.Entity<WorkoutPlanDayDetails>()
    .HasIndex(p => new { p.WorkoutPlanId, p.WeekNumber, p.DayNumber });

            

            
            //.........................................

            modelBuilder.Entity<TrainerTimeSlot>()
                  .HasOne(t => t.User)
                  .WithMany(u => u.TrainerBookings)
                  .HasForeignKey(t => t.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TrainerTimeSlot>()
                .HasOne(t => t.Trainer)
                .WithMany()
                .HasForeignKey(t => t.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TrainerTimeSlot>()
                .HasOne(t => t.TimeSlot)
                .WithMany(ts => ts.TrainerTimeSlots)
                .HasForeignKey(t => t.TimeSlotId);

            modelBuilder.Entity<Feedback>()
                .HasKey(x => x.Id);


            modelBuilder.Entity<TrainerTimeSlot>()
                .Property(x => x.UserId)
                .HasDefaultValue(0);
        }
    }
}
