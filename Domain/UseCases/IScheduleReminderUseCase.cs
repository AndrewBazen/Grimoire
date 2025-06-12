// // ---------- Domain/UseCases/IScheduleReminderUseCase.cs ----------
// using Grimoire.Domain.Models;

// namespace Grimoire.Domain.UseCases;

// /// <summary>
// /// A request for scheduling a reminder.
// /// </summary>
// public record ScheduleReminderRequest(string TomeId, string EntryId, DateTimeOffset TriggerAt);

// /// <summary>
// /// A response for scheduling a reminder.
// /// </summary>
// public record ScheduleReminderResponse(Reminder Reminder);

// /// <summary>
// /// A use case for scheduling a reminder.
// /// </summary>
// public interface IScheduleReminderUseCase
// {
//     Task<ScheduleReminderResponse> ExecuteAsync(ScheduleReminderRequest request, CancellationToken ct = default);
// }
// TODO: Implement this
