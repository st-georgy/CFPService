namespace CFPService.Domain.Entities
{
    public class Draft
    {
        public Guid ApplicationId { get; init; }
        public Guid AuthorId { get; init; }

        private Draft(Guid applicationId, Guid authorId)
        {
            ApplicationId = applicationId;
            AuthorId = authorId;
        }

        public static Draft Create(Guid applicationId, Guid authorId)
        {
            if (applicationId == Guid.Empty)
                throw new ArgumentException("Application Id must be specified.", nameof(applicationId));

            if (authorId == Guid.Empty)
                throw new ArgumentException("Author Id must be specified.", nameof(authorId));

            return new Draft(applicationId, authorId);
        }
    }
}
