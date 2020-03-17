using System.Net.Http;
using System.Threading.Tasks;
using BacklogApiBridge.Backlog.Models;
using Refit;

namespace BacklogApiBridge.Backlog
{
    public interface IBacklogClient
    {
        #region SpaceAPI
        [Get("/api/v2/space")]
        Task<Space> GetSpaceAsync([Query] string apiKey);

        [Get("/api/v2/space/activities")]
        Task<Activity[]> GetSpaceActivitiesAsync(
            [Query] string apiKey,
            [AliasAs("activityTypeId[]")] [Query(CollectionFormat = CollectionFormat.Multi)]
            int[] activityTypeId,
            [Query] int? minId,
            [Query] int? maxId,
            [Query] int? count = 20,
            [Query] string order = "desc"
        );
        #endregion

        #region UsersAPI
        [Get("/api/v2/users")]
        Task<User[]> GetUsersAsync(
            [Query] string apiKey
        );

        [Get("/api/v2/users/{userId}")]
        Task<User> GetUserAsync(
            [Query] string apiKey,
            int userId
        );
        #endregion

        #region ProjectAPI
        [Get("/api/v2/projects")]
        Task<Project[]> GetProjectsAsync(
            [Query] string apiKey
        );

        [Get("/api/v2/projects/{projectIdOrKey}")]
        Task<Project> GetProjectAsync(
            [Query] string apiKey,
            [Query] string projectIdOrKey
        );

        [Get("/api/v2/projects/{projectIdOrKey}/image")]
        Task<HttpContent> GetProjectImageAsync(
            [Query] string apiKey,
            [Query] string projectIdOrKey
        );

        [Get("/api/v2/projects/{projectIdOrKey}/users")]
        Task<User[]> GetProjectUsersAsync(
            [Query] string apiKey,
            [Query] string projectIdOrKey,
            [Query] bool excludeGroupMembers
        );

        [Get("/api/v2/projects/{projectIdOrKey}/statuses")]
        Task<Status[]> GetProjectStatusesAsync(
            [Query] string apiKey,
            [Query] string projectIdOrKey
        );

        [Get("/api/v2/projects/{projectIdOrKey}/issueTypes")]
        Task<IssueType[]> GetProjectIssueTypesAsync(
            [Query] string apiKey,
            [Query] string projectIdOrKey
        );

        [Get("/api/v2/projects/{projectIdOrKey}/issueTypes")]
        Task<Category[]> GetProjectCategoriesAsync(
            [Query] string apiKey,
            [Query] string projectIdOrKey
        );

        [Get("/api/v2/projects/{projectIdOrKey}/customFields ")]
        Task<CustomField[]> GetProjectCustomFieldsAsync(
            [Query] string apiKey,
            [Query] string projectIdOrKey
        );

        [Get("/api/v2/projects/{projectIdOrKey}/administrators ")]
        Task<User[]> GetProjectAdministratorsAsync(
            [Query] string apiKey,
            [Query] string projectIdOrKey
        );

        [Get("/api/v2/projects/{projectIdOrKey}/versions ")]
        Task<BacklogVersion[]> GetProjectVersionsAsync(
            [Query] string apiKey,
            [Query] string projectIdOrKey
        );
        #endregion

        #region IssueAPI

        [Get("/api/v2/issues")]
        Task<Issue[]> FindIssiesByAsync(
            [Query] string apiKey,
            [AliasAs("projectId[]")][Query(CollectionFormat.Multi)] int[] projectIds,
            [AliasAs("issueTypeId[]")][Query(CollectionFormat.Multi)] int[] issueTypeIds,
            [AliasAs("categoryId[]")][Query(CollectionFormat.Multi)] int[] categoryIds,
            [AliasAs("versionId[]")][Query(CollectionFormat.Multi)] int[] versionIds,
            [AliasAs("milestoneId[]")][Query(CollectionFormat.Multi)] int[] milestoneIds,
            [AliasAs("statusId[]")][Query(CollectionFormat.Multi)] int[] statusIds,
            [AliasAs("priorityId[]")][Query(CollectionFormat.Multi)] int[] priorityIds,
            [AliasAs("assigneeId[]")][Query(CollectionFormat.Multi)] int[] assigneeIds,
            [AliasAs("createdUserId[]")][Query(CollectionFormat.Multi)] int[] createdUserIds,
            [AliasAs("resolutionId[]")][Query(CollectionFormat.Multi)] int[] resolutionIds,
            int parentChild,
            string attachment,
            string sharedFile,
            string sort,
            string order,
            int offset,
            int count,
            string createdSince,
            string createdUntil,
            string updatedSince,
            string updatedUntil,
            string startDateSince,
            string startdateUntil,
            string dueDateSince,
            string dueDateUntil,
            [AliasAs("id[]")][Query(CollectionFormat.Multi)] int[] ids,
            [AliasAs("parentIssueId[]")][Query(CollectionFormat.Multi)] int[] parentIssueIds,
            string keyword
        );

        [Get("/api/v2/issues/{issueIdOrKey}/comments")]
        Task<Comment[]> GetCommentsAsync(
            [Query] string apiKey,
            [Query] string issueIdOrKey,
            int? minId,
            int? maxId,
            int? count,
            string order);
        #endregion

        #region WebhookAPI
        [Get("/api/v2/projects/{projectIdOrKey}/webhooks")]
        Task<Webhook[]> GetWebhooksAsync(
            [Query] string apiKey,
            [Query] string projectIdOrKey
        );
        [Get("/api/v2/projects/{projectIdOrKey}/webhooks/{webhookId}")]
        Task<Webhook> GetWebhookAsync(
            [Query] string apiKey,
            [Query] string projectIdOrKey,
            [Query] string webhookId
        );
        [Post("/api/v2/projects/{projectIdOrKey}/webhooks")]
        Task<Webhook> CreateWebhookAsync(
            [Query] string apiKey,
            [Query] string projectIdOrKey
        );
        [Patch("/api/v2/projects/{projectIdOrKey}/webhooks/{webhookId}")]
        Task<Webhook> UpdateWebhookAsync(
            [Query] string apiKey,
            [Query] string projectIdOrKey,
            [Query] string webhookId
        );
        [Delete("/api/v2/projects/{projectIdOrKey}/webhooks/{webhookId}")]
        Task<Webhook> DeleteWebhookAsync(
            [Query] string apiKey,
            [Query] string projectIdOrKey,
            [Query] string webhookId
        );
        #endregion

        #region GitPullRequest
        [Post("/api/v2/projects/{projectIdOrKey}/git/repositories/{repoIdOrName}/pullRequests")]
        Task<PullRequest> CreatePullRequestAsync(
            [Query] string apiKey,
            [Query] string projectIdOrKey,
            [Query] string repoIdOrName,
            [Query] string summary,
            [Query] string description,
            [Query] string @base,
            [Query] string branch,
            [Query] int? issueId,
            [Query] int? assigneeId,
            [AliasAs("notifiedUserId[]")][Query(CollectionFormat.Multi)] int[] notifiedUserId,
            [AliasAs("attachmentId[]")][Query(CollectionFormat.Multi)] int[] attachmentId
        );

        [Get("/api/v2/projects/{projectIdOrKey}/git/repositories/{repoIdOrName}/pullRequests/{number}")]
        Task<PullRequest> GetPullRequestAsync(
            [Query] string apiKey,
            [Query] string projectIdOrKey,
            [Query] string repoIdOrName,
            [Query] int number
        );

        [Patch("/api/v2/projects/{projectIdOrKey}/git/repositories/{repoIdOrName}/pullRequests/{number}")]
        Task<PullRequest> UpdatePullRequestAsync(
            [Query] string apiKey,
            [Query] string projectIdOrKey,
            [Query] string repoIdOrName,
            [Query] int number,
            [Query] string summary,
            [Query] string description,
            [Query] int? issueId,
            [Query] int? assigneeId,
            [AliasAs("notifiedUserId[]")][Query(CollectionFormat.Multi)] int[] notifiedUserId,
            [Query] string comment = null
        );
        #endregion
    }
}
