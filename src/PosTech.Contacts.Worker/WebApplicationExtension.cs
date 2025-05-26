using PosTech.Contacts.Worker.Consumers;
using Quartz;

namespace PosTech.Contacts.Worker
{
    public static class WebApplicationExtension
    {
        public async static Task<WebApplication> AddQuartzJobsAsync(this WebApplication app, IConfiguration configuration)
        {
            var schedulerFactory = app.Services.GetRequiredService<ISchedulerFactory>();
            var scheduler = await schedulerFactory.GetScheduler();

            #region Add ContactCreatedMessageConsumer

            var contactCreatedMessageConsumerJob = JobBuilder.Create<ContactCreatedMessageConsumer>()
                .WithIdentity(nameof(ContactCreatedMessageConsumer))
                .Build();

            var contactCreatedMessageConsumerTrigger = TriggerBuilder.Create()
                .WithIdentity($"Trigger-{nameof(ContactCreatedMessageConsumer)}")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .RepeatForever()
                    .WithInterval(TimeSpan.FromMicroseconds(20)))
                .Build();

            await scheduler.ScheduleJob(contactCreatedMessageConsumerJob, contactCreatedMessageConsumerTrigger);

            #endregion

            #region Add ContactUpdatedMessageConsumer

            var contactUpdatedMessageConsumerJobDetail = JobBuilder.Create<ContactUpdatedMessageConsumer>()
                .WithIdentity(nameof(ContactUpdatedMessageConsumer))
                .Build();

            var contactUpdatedMessageConsumerTrigger = TriggerBuilder.Create()
                .WithIdentity($"Trigger-{nameof(ContactUpdatedMessageConsumer)}")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .RepeatForever()
                    .WithInterval(TimeSpan.FromMilliseconds(20)))
                .Build();

            await scheduler.ScheduleJob(contactUpdatedMessageConsumerJobDetail, contactUpdatedMessageConsumerTrigger);

            #endregion

            #region Add ContactDeletedMessageConsumer

            var contactDeletedMessageConsumerJobDetail = JobBuilder.Create<ContactDeletedMessageConsumer>()
                .WithIdentity(nameof(ContactDeletedMessageConsumer))
                .Build();

            var contactDeletedMessageConsumerTrigger = TriggerBuilder.Create()
                .WithIdentity($"Trigger-{nameof(ContactDeletedMessageConsumer)}")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .RepeatForever()
                    .WithInterval(TimeSpan.FromMilliseconds(1)))
                .Build();

            await scheduler.ScheduleJob(contactDeletedMessageConsumerJobDetail, contactDeletedMessageConsumerTrigger);

            #endregion

            return app;
        }
    }
}
