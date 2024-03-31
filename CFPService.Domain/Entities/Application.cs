using CFPService.Domain.Entities.Enums;

namespace CFPService.Domain.Entities
{
    public class Application
    {
        public Guid Id { get; init; }
        public Guid AuthorId { get; init; }
        public Activity? Activity { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime? SubmittedDate { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public string? Outline { get; init; }

        private Application(Guid id, Guid authorId, Activity? activity, DateTime createdDate, DateTime? submittedDate, string? name, string? description, string? outline)
        {
            Id = id;
            AuthorId = authorId;
            Activity = activity;
            CreatedDate = createdDate;
            SubmittedDate = submittedDate;
            Name = name;
            Description = description;
            Outline = outline;
        }

        public static Application Create(Guid id, Guid authorId, Activity? activity, DateTime createdDate, DateTime? submittedDate, string? name, string? description, string? outline)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id must be specified.", nameof(id));

            if (authorId == Guid.Empty)
                throw new ArgumentException("Author Id must be specified.", nameof(authorId));

            if (!string.IsNullOrWhiteSpace(name) && name.Length > 100)
                throw new ArgumentException("Name length must not exceed 100 characters.", nameof(name));

            if (!string.IsNullOrWhiteSpace(outline) && outline.Length > 1000)
                throw new ArgumentException("Outline length must not exceed 1000 characters.", nameof(outline));

            if (activity is not null && !Enum.IsDefined(typeof(Activity), activity))
                throw new ArgumentException("Invalid activity.", nameof(activity));

            return new Application(id, authorId, activity, createdDate, submittedDate, name, description, outline);
        }
    }
}
