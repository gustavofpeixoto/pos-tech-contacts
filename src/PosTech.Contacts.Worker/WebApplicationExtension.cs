using PosTech.Contacts.Worker.Consumers;
using Quartz;

namespace PosTech.Contacts.Worker
{
    public static class WebApplicationExtension
    {
        public async static Task<WebApplication> AddQuartzJobsAsync(this WebApplication app, IConfiguration configuration)
        {
            var cronSchedule = configuration["CronSchedule"];
            var schedulerFactory = app.Services.GetRequiredService<ISchedulerFactory>();
            var scheduler = await schedulerFactory.GetScheduler();
            var contactCreatedMessageConsumerJob = JobBuilder.Create<ContactCreatedMessageConsumer>()
                .WithIdentity(nameof(ContactCreatedMessageConsumer))
                .Build();
            var contactDeletedMessageConsumerJob = JobBuilder.Create<ContactDeletedMessageConsumer>()
                .WithIdentity(nameof(ContactDeletedMessageConsumer))
                .Build();
            var contactUpdatedMessageConsumerJob = JobBuilder.Create<ContactUpdatedMessageConsumer>()
                .WithIdentity(nameof(ContactUpdatedMessageConsumer))
                .Build();
            var triggerCreated = TriggerBuilder.Create()
               .WithIdentity(string.Concat("Trigger-",nameof(ContactCreatedMessageConsumer)))
               .WithCronSchedule(cronSchedule)
               .Build();
            var triggerDeleted = TriggerBuilder.Create()
               .WithIdentity(string.Concat("Trigger-", nameof(ContactDeletedMessageConsumer)))
                .WithCronSchedule(cronSchedule)
                .Build();
            var triggerUpdated = TriggerBuilder.Create()
               .WithIdentity(string.Concat("Trigger-", nameof(ContactUpdatedMessageConsumer)))
                .WithCronSchedule(cronSchedule)
                .Build();

            await scheduler.ScheduleJob(contactCreatedMessageConsumerJob, triggerCreated);
            await scheduler.ScheduleJob(contactDeletedMessageConsumerJob, triggerDeleted);
            await scheduler.ScheduleJob(contactUpdatedMessageConsumerJob, triggerUpdated);

            return app;
        }
    }
}
