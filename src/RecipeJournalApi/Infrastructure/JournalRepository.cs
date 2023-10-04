using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;
using static RecipeJournalApi.Controllers.RecipeController; //yeah yeah should make my own type, still prototyping

namespace RecipeJournalApi.Infrastructure
{
    public interface IJournalRepository
    {
        RecipeJournalListEntryDto[] GetEntriesForRecipe(Guid userId, Guid recipeId);
        RecipeJournalEntryDto GetEntry(Guid userId, Guid entryId);
        RecipeJournalEntryDto UpdateEntry(Guid userId, RecipeJournalEntryDto entry);
    }

    public class MockJournalRepository : IJournalRepository
    {
        public RecipeJournalListEntryDto[] GetEntriesForRecipe(Guid userId, Guid recipeId)
        {
            if (!_mockDb.ContainsKey(userId)) _mockDb.Add(userId, new List<RecipeJournalEntryDto>());

            var items = _mockDb[userId].Where(e => e.RecipeId == recipeId);
            if (!items.Any())
            {
                _mockDb[userId].Add(new RecipeJournalEntryDto
                {
                    Id = Guid.NewGuid(),
                    AttemptNotes = "wanted to try that new thingy",
                    Date = new DateTime(2023, 10, 4),
                    GeneralNotes = "this recipe is meh",
                    NextDismissed = false,
                    NextNotes = "next time try more anger",
                    RecipeId = recipeId,
                    RecipeScale = 1,
                    StickyNext = true,
                    SuccessRating = .5f
                });
            }

            return _mockDb[userId].Select(e => new RecipeJournalListEntryDto
            {
                Id = e.Id.Value,
                Date = e.Date.Value,
                NextDismissed = e.NextDismissed,
                RecipeId = e.RecipeId,
                RecipeScale = e.RecipeScale,
                StickyNext = e.StickyNext,
                SuccessRating = e.SuccessRating
            }).ToArray();
        }

        public RecipeJournalEntryDto GetEntry(Guid userId, Guid entryId)
        {
            if (!_mockDb.ContainsKey(userId)) _mockDb.Add(userId, new List<RecipeJournalEntryDto>());

            return _mockDb[userId].FirstOrDefault(e => e.Id == entryId);
        }

        public RecipeJournalEntryDto UpdateEntry(Guid userId, RecipeJournalEntryDto entry)
        {
            if (!_mockDb.ContainsKey(userId)) _mockDb.Add(userId, new List<RecipeJournalEntryDto>());

            if (!entry.Id.HasValue)
            {
                entry.Id = Guid.NewGuid();
                entry.Date = DateTime.Now;
            }

            _mockDb[userId].RemoveAll(e => e.Id == entry.Id);
            _mockDb[userId].Add(entry);

            return entry;
        }

        private static readonly Dictionary<Guid, List<RecipeJournalEntryDto>> _mockDb = new Dictionary<Guid, List<RecipeJournalEntryDto>>();
    }
}