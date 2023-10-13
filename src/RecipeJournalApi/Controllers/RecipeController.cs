using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeJournalApi.Infrastructure;

namespace RecipeJournalApi.Controllers
{
    [ApiController]
    [Route("api/v1/recipes")]
    public class RecipeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRecipeRepository _recipeRepo;
        private readonly IJournalRepository _journalRepository;

        public RecipeController(ILogger<HomeController> logger, IRecipeRepository recipeRepo, IJournalRepository journalRepository)
        {
            _logger = logger;
            _recipeRepo = recipeRepo;
            _journalRepository = journalRepository;
        }

        [HttpGet("")]
        public ActionResult<RecipeListItemDto[]> GetRecipeList()
        {
            var user = UserInfo.FromClaimsPrincipal(this.User);
            var recipes = _recipeRepo.GetRecipesForUser(user?.Id);

            return recipes.Select(r => new RecipeListItemDto
            {
                Id = r.Id,
                DurationMinutes = r.DurationMinutes,
                Servings = r.Servings,
                Title = r.Title
            }).ToArray();
        }
        public class RecipeListItemDto
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public int? DurationMinutes { get; set; }
            public int? Servings { get; set; }

            public float Rating { get; set; }
            public string[] Tags { get; set; }
            public int TotalJournalCount { get; set; }
            public DateTime DateCreated { get; set; }
            public string Author { get; set; }
            public int Version { get; set; }
            public DateTime LastModified { get; set; }
            public bool IsPublic { get; set; }
            public bool IsDraft { get; set; }
            public LoggedInInfoDto LoggedInInfo { get; set; }

            public class LoggedInInfoDto
            {
                public float? PersonalBest { get; set; }
                public int? PersonalJournalCount { get; set; }
                public DateTime? PersonalLastJournalDate { get; set; }
                public int? PersonalGoalCount { get; set; }
                public int? PersonalNoteCount { get; set; }
            }

            public RecipeListItemDto FromService(Guid id, string title, int? durationMinutes, int? servings, float rating, string[] tags, int totalJournalCount, DateTime dateCreated, string author, int version, DateTime lastModified, bool isPublic, bool isDraft, bool isLoggedIn, float? personalBest, int? personalJournalCount, DateTime? personalLastJournalDate, int? personalGoalCount, int? personalNoteCount)
            {
                return new RecipeListItemDto
                {
                    Id = id,
                    Title = title,
                    DurationMinutes = durationMinutes,
                    Servings = servings,
                    Rating = rating,
                    Tags = tags,
                    TotalJournalCount = totalJournalCount,
                    DateCreated = dateCreated,
                    Author = author,
                    Version = version,
                    LastModified = lastModified,
                    IsPublic = isPublic,
                    IsDraft = isDraft,
                    LoggedInInfo = isLoggedIn ? new LoggedInInfoDto
                    {
                        PersonalBest = personalBest,
                        PersonalJournalCount = personalJournalCount,
                        PersonalLastJournalDate = personalLastJournalDate,
                        PersonalGoalCount = personalGoalCount,
                        PersonalNoteCount = personalNoteCount,
                    } : null,
                };
            }
        }

        [HttpGet("{id}")]
        public ActionResult<RecipeDto> GetRecipe([FromRoute] Guid id)
        {
            var recipe = _recipeRepo.GetRecipe(id);
            if (recipe == null)
                return StatusCode(404);
            return RecipeDto.FromDataType(recipe);
        }
        public class RecipeDto
        {
            public static RecipeDto FromDataType(Recipe recipe)
            {
                return new RecipeDto
                {
                    Id = recipe.Id,
                    Title = recipe.Title,
                    Author = recipe.Author,
                    Description = recipe.Description,
                    DurationMinutes = recipe.DurationMinutes,
                    IsDraft = recipe.IsDraft,
                    IsPublic = recipe.IsPublic,
                    Servings = recipe.Servings,
                    Components = recipe.Components.OrderBy(c => c.Number).Select(c => new RecipeDto.RecipeComponentDto
                    {
                        Id = c.Id,
                        Number = c.Number,
                        Description = c.Description,
                        Title = c.Title,
                        Steps = c.Steps.OrderBy(s => s.Number).Select(s => new RecipeDto.RecipeStepDto
                        {
                            Id = s.Id,
                            Number = s.Number,
                            Title = s.Title,
                            Body = s.Body,
                            Ingredients = s.Ingredients.OrderBy(i => i.Number).Select(i => new RecipeDto.RecipeIngredientDto
                            {
                                Id = i.Id,
                                Number = i.Number,
                                Name = i.Name,
                                Amount = i.Amount,
                                Unit = i.Unit
                            }).ToArray()
                        }).ToArray()
                    }).ToArray()
                };
            }

            public Guid? Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Author { get; set; }
            public int? DurationMinutes { get; set; }
            public int? Servings { get; set; }
            public bool IsDraft { get; set; }
            public bool IsPublic { get; set; }
            public RecipeComponentDto[] Components { get; set; }

            public float Rating { get; set; }
            public string[] Tags { get; set; }
            public int TotalJournalCount { get; set; }
            public DateTime DateCreated { get; set; }
            public DateTime LastModified { get; set; }
            public int Version { get; set; }
            public LoggedInInfoDto LoggedInInfo { get; set; }

            public class LoggedInInfoDto
            {
                public float? PersonalBest { get; set; }
                public int? PersonalJournalCount { get; set; }
                public DateTime? PersonalLastJournalDate { get; set; }
                public int? PersonalGoalCount { get; set; }
                public int? PersonalNoteCount { get; set; }
            }

            public class RecipeComponentDto
            {
                public Guid? Id { get; set; }
                public int Number { get; set; }
                public string Title { get; set; }
                public string Description { get; set; }
                public RecipeStepDto[] Steps { get; set; }
            }
            public class RecipeStepDto
            {
                public Guid? Id { get; set; }
                public int Number { get; set; }
                public string Title { get; set; }
                public string Body { get; set; }
                public RecipeIngredientDto[] Ingredients { get; set; }
            }
            public class RecipeIngredientDto
            {
                public Guid? Id { get; set; }
                public int Number { get; set; }
                public string Name { get; set; }
                public string Unit { get; set; }
                public float Amount { get; set; }
                public string Description { get; set; }
            }
        }

        [Authorize]
        [HttpPut("")]
        public ActionResult<RecipeDto> UpdateRecipe(RecipeInputDto recipeDto)
        {
            var user = UserInfo.FromClaimsPrincipal(this.User);
            var updated = _recipeRepo.UpdateRecipe(recipeDto, user);
            var dto = RecipeDto.FromDataType(updated);
            return dto;
        }
        public class RecipeInputDto
        {
            public Guid? Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int? DurationMinutes { get; set; }
            public int? Servings { get; set; }
            public bool IsDraft { get; set; }
            public bool IsPublic { get; set; }
            public RecipeDto.RecipeComponentDto[] Components { get; set; }
        }

        [Authorize]
        [HttpGet("{recipeId}/journal")]
        public ActionResult<RecipeJournalListEntryDto[]> GetJournalForRecipe(Guid recipeId)
        {
            var user = UserInfo.FromClaimsPrincipal(this.User);
            var entries = _journalRepository.GetEntriesForRecipe(user.Id, recipeId);
            return entries;
        }
        public class RecipeJournalListEntryDto
        {
            public Guid Id { get; set; }
            public Guid RecipeId { get; set; }
            public float RecipeScale { get; set; }
            public float SuccessRating { get; set; }
            public DateTime Date { get; set; }
            public bool StickyNext { get; set; }
            public bool NextDismissed { get; set; }
        }

        [Authorize]
        [HttpGet("{recipeId}/journal/{entryId}")]
        public ActionResult<RecipeJournalEntryDto> GetJournalEntry(Guid entryId)
        {
            var user = UserInfo.FromClaimsPrincipal(this.User);
            var entry = _journalRepository.GetEntry(user.Id, entryId);
            return entry;
        }
        public class RecipeJournalEntryDto
        {
            public Guid? Id { get; set; }
            public Guid RecipeId { get; set; }
            public float RecipeScale { get; set; }
            public float SuccessRating { get; set; }
            public DateTime? Date { get; set; }
            public string AttemptNotes { get; set; }
            public string ResultNotes { get; set; }
            public string GeneralNotes { get; set; }
            public string NextNotes { get; set; }
            public bool StickyNext { get; set; }
            public bool NextDismissed { get; set; }
        }

        [Authorize]
        [HttpPut("{recipeId}/journal")]
        public ActionResult<RecipeJournalEntryDto> UpdateJournalEntry(RecipeJournalEntryDto entry)
        {
            var user = UserInfo.FromClaimsPrincipal(this.User);
            var updated = _journalRepository.UpdateEntry(user.Id, entry);
            return updated;
        }
    }
}