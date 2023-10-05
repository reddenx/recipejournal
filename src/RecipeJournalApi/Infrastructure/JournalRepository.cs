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

    public class JournalRepository : IJournalRepository
    {
        private readonly string _connectionString;

        public JournalRepository(IDbConfig config)
        {
            _connectionString = config.ConnectionString;
        }

        public RecipeJournalListEntryDto[] GetEntriesForRecipe(Guid userId, Guid recipeId)
        {
            var sql = @"
            select 
                j.Id,
                j.RecipeId,
                j.RecipeScale,
                j.SuccessRating,
                j.EntryDate,
                j.StickyNext,
                j.NextDismissed
            from recipe_journal_entry j
            where j.UserId = @UserId and j.RecipeId = @RecipeId";

            using (var conn = new MySqlConnection(_connectionString))
            {
                var results = conn.Query<RecipeJournalListData>(sql, new
                {
                    UserId = userId.ToString("N"),
                    RecipeId = recipeId.ToString("N")
                });

                return results.Select(r => new RecipeJournalListEntryDto
                {
                    Id = Guid.Parse(r.Id),
                    RecipeId = Guid.Parse(r.RecipeId),
                    RecipeScale = r.RecipeScale,
                    SuccessRating = r.SuccessRating,
                    Date = r.EntryDate,
                    StickyNext = r.StickyNext,
                    NextDismissed = r.NextDismissed
                }).ToArray();
            }
        }

        public RecipeJournalEntryDto GetEntry(Guid userId, Guid entryId)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                return GetEntryConn(conn, userId, entryId);
            }
        }
        private RecipeJournalEntryDto GetEntryConn(MySqlConnection conn, Guid userId, Guid entryId)
        {
            var sql = @"
            select 
                j.Id,
                j.UserId,
                j.RecipeId,
                j.DateCreated,
                j.DateModified,
                j.EntryDate,
                j.SuccessRating,
                j.RecipeScale,
                j.AttemptNotes,
                j.GeneralNotes,
                j.NextNotes,
                j.StickyNext,
                j.NextDismissed
            from recipe_journal_entry j
            where j.UserId = @UserId and j.Id = @Id
                ";
            var result = conn.Query<RecipeJournalData>(sql, new
            {
                UserId = userId.ToString("N"),
                Id = entryId.ToString("N")
            }).FirstOrDefault();

            if (result == null)
                return null;

            return new RecipeJournalEntryDto
            {
                Id = Guid.Parse(result.Id),
                RecipeId = Guid.Parse(result.RecipeId),
                Date = result.EntryDate,
                SuccessRating = result.SuccessRating,
                RecipeScale = result.RecipeScale,
                AttemptNotes = result.AttemptNotes,
                GeneralNotes = result.GeneralNotes,
                NextNotes = result.NextNotes,
                NextDismissed = result.NextDismissed,
                StickyNext = result.StickyNext,
            };
        }

        public RecipeJournalEntryDto UpdateEntry(Guid userId, RecipeJournalEntryDto entry)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                if (!entry.Id.HasValue)
                {
                    var insertSql = @"
                    insert into recipe_journal_entry (Id, UserId, RecipeId, DateCreated, DateModified, EntryDate, SuccessRating, RecipeScale, AttemptNotes, GeneralNotes, NextNotes, StickyNext, NextDismissed)
                    values (@Id, @UserId, @RecipeId, @DateCreated, @DateModified, @EntryDate, @SuccessRating, @RecipeScale, @AttemptNotes, @GeneralNotes, @NextNotes, @StickyNext, @NextDismissed)";

                    var id = Guid.NewGuid();

                    var success = conn.Execute(insertSql, new
                    {
                        Id = id.ToString("N"),
                        UserId = userId.ToString("N"),
                        RecipeId = entry.RecipeId.ToString("N"),
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        EntryDate = entry.Date ?? DateTime.Now,
                        SuccessRating = entry.SuccessRating,
                        RecipeScale = entry.RecipeScale,
                        AttemptNotes = entry.AttemptNotes,
                        GeneralNotes = entry.GeneralNotes,
                        NextNotes = entry.NextNotes,
                        StickyNext = entry.StickyNext,
                        NextDismissed = entry.NextDismissed
                    }) > 0;
                    if (!success)
                        return null;

                    return GetEntryConn(conn, userId, id);
                }
                else
                {
                    var updateSql = @"
                    update recip_journal_entry
                    set
                        DateModified = @DateModified,
                        EntryDate = @EntryDate,
                        SuccessRating = @SuccessRating,
                        RecipeScale = @RecipeScale,
                        AttemptNotes = @AttemptNotes,
                        GeneralNotes = @GeneralNotes,
                        NextNotes = @NextNotes,
                        StickyNext = @StickyNest,
                        NextDismissed = @NextDismissed
                    where Id = @Id";
                    var success = conn.Execute(updateSql, new 
                    {
                        Id = entry.Id.Value.ToString("N"),
                        DateModified = DateTime.Now,
                        EntryDate = entry.Date ?? DateTime.Now,
                        SuccessRating = entry.SuccessRating,
                        RecipeScale = entry.RecipeScale,
                        AttemptNotes = entry.AttemptNotes,
                        GeneralNotes = entry.GeneralNotes,
                        NextNotes = entry.NextNotes,
                        StickyNext = entry.StickyNext,
                        NextDismissed = entry.NextDismissed
                    }) > 0;
                    if(!success)
                        return null;

                    return entry;
                }
            }
        }

        class RecipeJournalListData
        {
            public string Id { get; set; }
            public string RecipeId { get; set; }
            public float RecipeScale { get; set; }
            public float SuccessRating { get; set; }
            public DateTime EntryDate { get; set; }
            public bool StickyNext { get; set; }
            public bool NextDismissed { get; set; }
        }

        class RecipeJournalData
        {
            public string Id { get; set; }
            public string UserId { get; set; }
            public string RecipeId { get; set; }
            public DateTime DateCreated { get; set; }
            public DateTime DateModified { get; set; }
            public DateTime EntryDate { get; set; }
            public float SuccessRating { get; set; }
            public float RecipeScale { get; set; }
            public string AttemptNotes { get; set; }
            public string GeneralNotes { get; set; }
            public string NextNotes { get; set; }
            public bool StickyNext { get; set; }
            public bool NextDismissed { get; set; }
        }
    }

    public class MockJournalRepository : IJournalRepository
    {
        public RecipeJournalListEntryDto[] GetEntriesForRecipe(Guid userId, Guid recipeId)
        {
            if (!_mockDb.ContainsKey(userId)) _mockDb.Add(userId, new List<RecipeJournalEntryDto>());

            var items = _mockDb[userId].Where(e => e.RecipeId == recipeId);
            if (!items.Any())
            {
                // _mockDb[userId].Add(new RecipeJournalEntryDto
                // {
                //     Id = Guid.NewGuid(),
                //     AttemptNotes = "wanted to try that new thingy",
                //     Date = new DateTime(2023, 10, 4),
                //     GeneralNotes = "this recipe is meh",
                //     NextDismissed = false,
                //     NextNotes = "next time try more anger",
                //     RecipeId = recipeId,
                //     RecipeScale = 1,
                //     StickyNext = true,
                //     SuccessRating = .5f
                // });
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